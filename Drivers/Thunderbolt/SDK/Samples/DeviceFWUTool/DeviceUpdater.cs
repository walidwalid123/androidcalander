/*******************************************************************************
* Copyright (C) 2016 - 2017 Intel Corp. All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*  - Redistributions of source code must retain the above copyright notice,
*    this list of conditions and the following disclaimer.
*
*  - Redistributions in binary form must reproduce the above copyright notice,
*    this list of conditions and the following disclaimer in the documentation
*    and/or other materials provided with the distribution.
*
*  - Neither the name of Intel Corp. nor the names of its
*    contributors may be used to endorse or promote products derived from this
*    software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ``AS IS''
* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED. IN NO EVENT SHALL Intel Corp. OR THE CONTRIBUTORS
* BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
* CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
* SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
* CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
* ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
* POSSIBILITY OF SUCH DAMAGE.
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FwUpdateApiSample;

namespace DeviceFWUTool
{
    /// <summary>
    /// This is the main logic of the tool when FW update is requested.
    ///
    /// The general procedure is as follows:
    /// 1. Load the device list,
    /// 2. Find a FW image file,
    /// 3. For each device in the list, if it compatibles with the image file,
    ///    update it and report results.
    /// </summary>
    internal class DeviceUpdater
    {
        private readonly Dictionary<string, SdkTbtDevice> _devices;
        private uint _matchingDeviceFound;
        private uint _updatedDevices;
        private string _imageNvm;


        internal DeviceUpdater()
        {
            _devices = LoadDevices();
        }

        private static Dictionary<string, SdkTbtDevice> LoadDevices()
        {
            Dictionary<string, SdkTbtDevice> devices;
            try
            {
                devices = SdkTbtDevice.GetDevices();
            }
            catch
            {
                throw new TbtException(TbtStatus.SDK_LOAD_DEVICES_ERROR);
            }
            if (!devices.Any())
            {
                throw new TbtException(TbtStatus.SDK_NO_DEVICES,
                    Resources.NoEPStringPart0 + "\n"
                    + Resources.NoDeviceStringPart1 + "\n"
                    + Resources.NoDeviceStringPart2CMD);
            }
            return devices;
        }

        public void FwUpdate(FwUpdateParameters param)
        {
            string path;

            if (param.IsImageGiven)
            {
                path = param.ImagePath;
            }
            else
            {
                var di = new DirectoryInfo(Directory.GetCurrentDirectory());
                var binFiles = di.GetFiles("*.bin");

                switch (binFiles.Length)
                {
                    case 0:
                        throw new TbtException(TbtStatus.SDK_FILE_NOT_FOUND);
                    case 1:
                        path = binFiles[0].ToString();
                        break;
                    default:
                        throw new TbtException(TbtStatus.SDK_MULTIPLE_IMAGES_FOUND);
                }
            }

            if (!File.Exists(path))
            {
                throw new TbtException(TbtStatus.SDK_FILE_NOT_FOUND);
            }

            Console.WriteLine("Image loaded: " + path);

            var imageDeviceInfo = Utilities.GetImageDeviceInformation(path);
            _imageNvm = Utilities.GetImageFullNvmVersion(path);

            Console.WriteLine("File device information: Vendor ID {0:X}, Model ID {1:X}", imageDeviceInfo.VendorId,
                imageDeviceInfo.ModelId);
            Console.WriteLine("File NVM version = " + _imageNvm);

            // We want to skip all devices in unsupported controller at once, so
            // we use grouping by controller. We also want to update devices from
            // the farthest in the chain to the closest, so we order them by port
            // and then by position (descending)
            var deviceTree = _devices.OrderBy(device => device.Value.PortNum)
                .ThenByDescending(device => device.Value.PositionInChain)
                .GroupBy(device => device.Value.ControllerId);

            foreach (var controller in deviceTree)
            {
                Console.WriteLine("Controller {0}", controller.Key);

                foreach (var device in controller.Select(device => device.Value))
                {
                    try
                    {
                        UpdateDeviceFwFromFile(device, path, param);
                    }
                    catch (TbtException e)
                    {
                        CmdUtilities.WriteDescription(TbtException.ErrorMessage(e.ErrorCode), e.TbtMessage);
                    }
                }
            }

            if (_matchingDeviceFound == 0)
            {
                throw new TbtException(TbtStatus.SDK_NO_MATCHING_DEVICE_FOUND);
            }

            Console.WriteLine((_matchingDeviceFound == _updatedDevices ? "All" : "Not all") +
                              " matching devices were updated");
        }

        private static bool UserDecision(SdkTbtDevice device)
        {
            ConsoleKeyInfo input;
            do
            {
                Console.Write("Would you like to update " + device.VendorName + " " + device.ModelName + " " +
                              device.ControllerNum + "/" + device.NumOfControllers + " (located under port " +
                              (device.PortNum + 1) + " at depth " + device.PositionInChain + ") ? (y/n)   ");
                input = Console.ReadKey();
                Console.WriteLine();
            } while (input.Key != ConsoleKey.Y && input.Key != ConsoleKey.N);

            return input.Key == ConsoleKey.Y;
        }

        private void UpdateDeviceFwFromFile(SdkTbtDevice device, string path, FwUpdateParameters param)
        {
            if (!device.Updatable)
            {
                Console.WriteLine("The device located under port " + (device.PortNum + 1) + " at depth " +
                                  device.PositionInChain + " is not supported for FW update.");
                return;
            }

            Console.WriteLine("Validating image file for device located under port " + (device.PortNum + 1) +
                              " at depth " + device.PositionInChain + "...");
            try
            {
                device.ValidateImage(path);
            }
            catch (TbtException e)
            {
                CmdUtilities.WriteDescription("The device is incompatible with the image file", e.TbtMessage);
                return;
            }
            catch (Exception e)
            {
                CmdUtilities.WriteDescription("The device is incompatible with the image file", e.Message);
                return;
            }
            _matchingDeviceFound++;
            Console.WriteLine("Matching device found.");

            var currentNvm = device.GetCurrentFullNvmVersion();

            Console.WriteLine("Current NVM version = " + currentNvm);
            Console.WriteLine("File NVM version = " + _imageNvm);

            if (!param.Quiet && !UserDecision(device)) return;

            try
            {
                Console.WriteLine(Resources.CloseDuringUpdate);
                device.UpdateFirmwareFromFile(path);
                Console.WriteLine(Resources.FWUpdateSuccessMessage);
                _updatedDevices++;
            }
            catch (TbtException e)
            {
                CmdUtilities.WriteDescription(TbtException.ErrorMessage(e.ErrorCode), e.TbtMessage);
            }
            System.Threading.Thread.Sleep(5000);
        }
    }
}
