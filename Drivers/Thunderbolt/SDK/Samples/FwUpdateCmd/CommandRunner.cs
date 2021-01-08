/*******************************************************************************
* Copyright (C) 2015 - 2018 Intel Corp. All rights reserved.
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
using System.Globalization;
using System.Text.RegularExpressions;
using FwUpdateApiSample;

namespace FwUpdateCmd
{
    /// <summary>
    /// Base class for command handling. Implements the common commands.
    /// </summary>
    internal abstract class CommandRunner
    {
        /// <summary>
        /// Prints current NVM version
        /// </summary>
        /// <param name="args">The arguments as they come from the command line</param>
        internal void GetCurrentNvmVersion(string[] args)
        {
            Console.WriteLine(GetController(args[1]).GetCurrentFullNvmVersion());
        }

        /// <summary>
        /// Prints the content (bytes) of a given I2C register
        /// </summary>
        /// <param name="args">The arguments as they come from the command line</param>
        internal void I2CRead(string[] args)
        {
            string inController = args[1];
            string inPort = args[2];
            string inOffset = args[3];
            string inLenght = args[4];
            uint port;
            uint offset;
            uint length;
            if (!(uint.TryParse(inPort, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out port)
                  && uint.TryParse(inOffset, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out offset)
                  && uint.TryParse(inLenght, out length)))
            {
                throw new TbtException(TbtStatus.SDK_INVALID_ARGUMENT);
            }
            //the format that will be printed is: after each byte - space, after 4 bytes - 2 spaces, every 16 bytes - a new line
            //will have a space after every byte
            var read = BitConverter.ToString(GetController(inController).I2CRead(port, offset, length), 0)
                .Replace("-", " ");
            //every 4 bytes are 12 chars. Adding space after every 4 bytes, to have a dubble space after 4 bytes
            read = Regex.Replace(read, ".{12}", "$0 ");
            //every 16 bytes that are formatted already are 4*13 chars. Adding \n after every 16 bytes
            read = Regex.Replace(read, ".{" + 4*13 + "}", "$0\n");
            Console.WriteLine(read);
        }

        /// <summary>
        /// Writes the given data to an I2C register
        /// </summary>
        /// <param name="args">The arguments as they come from the command line</param>
        internal void I2CWrite(string[] args)
        {
            string inController = args[1];
            string inPort = args[2];
            string inOffset = args[3];
            string inData = args[4];
            uint port;
            uint offset;
            if (!(uint.TryParse(inPort, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out port) &&
                  uint.TryParse(inOffset, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out offset)) ||
                (inData.Length%2 == 1))
            {
                throw new TbtException(TbtStatus.SDK_INVALID_ARGUMENT);
            }
            var convertedData = new byte[inData.Length/2];
            try
            {
                for (var i = 0; i < inData.Length; i += 2)
                {
                    convertedData[i/2] = Convert.ToByte(inData.Substring(i, 2), 16);
                }
            }
            catch
            {
                throw new TbtException(TbtStatus.SDK_INVALID_ARGUMENT);
            }
            GetController(inController).I2CWrite(port, offset, convertedData);
            Console.WriteLine("Finished successfully");
        }

        /// <summary>
        /// Prints TI wanted registers
        /// </summary>
        /// <param name="args">The arguments as they come from the command line</param>
        internal void GetTIPdInfo(string[] args)
        {
            Console.WriteLine("TI PD version: " + Utilities.GetTIPdInfo(GetController(args[1])));
            Console.WriteLine("TI CUSTUSE (offset 0x6): " +
                              BitConverter.ToString(GetController(args[1]).I2CRead(1, 0x6, 8), 0).Replace("-", ""));
            Console.WriteLine("TI CustomerVersion (offset 0x2C): " +
                              BitConverter.ToString(GetController(args[1]).I2CRead(1, 0x2C, 1), 0));
        }

        /// <summary>
        /// Update FW of selected controller
        /// </summary>
        /// <param name="args">The arguments as they come from the command line</param>
        internal void FwUpdate(string[] args)
        {
            var controller = GetController(args[1]);
            var path = args[2];

            controller.ValidateImage(path);
            ValidationInSafeModeWarning(controller);

            var currentNvm = Utilities.SafeGetVersion(() => controller.GetCurrentFullNvmVersion());
            var imageNvm = Utilities.GetImageFullNvmVersion(path);
            var currentPd = Utilities.SafeGetVersion(() => Utilities.GetTIPdInfo(controller));
            var imagePd = Utilities.GetImageTIPdVersion(path);

            var needG3 = currentPd != imagePd;

            Console.WriteLine("Current NVM version = " + currentNvm);
            Console.WriteLine("Current PD version = " + currentPd);

            Console.WriteLine("File NVM version = " + imageNvm);
            Console.WriteLine("File PD version = " + imagePd);

            Console.WriteLine(Resources.CloseDuringUpdate);
            controller.UpdateFirmwareFromFile(path); // GP TODO create an abstract method which will update the firmware from file and pass it the controller as parameter.
            // then in the derived class implement it.
            //  GP TODO create a new protected static method that check if we are in G3.
            // then we will call a new method "UpdateFirmwareFinalizer" which  will check if it is not  G3, and in safemode , and if so, will print that a restart is required.

            Console.WriteLine(Resources.FWUpdateSuccessMessage);
            if (needG3)
            {
                Console.WriteLine(controller.GetNeedPowerDownMessage);
            }
            
        }

        /// <summary>
        /// Detailed information on selected controller
        /// </summary>
        /// <param name="args">The arguments as they come from the command line</param>
        internal void GetControllerInfo(string[] args)
        {
            //only on Controller not on Device
            var controller = GetController(args[1]) as SdkTbtController;
            if (controller == null)
                throw new TbtException(TbtStatus.SDK_COMMAND_IS_NOT_SUPPORTED_ON_DEVICE);

            var deviceId = controller.DeviceId;

            Console.WriteLine("Thunderbolt generation = " + controller.Generation);

            Console.WriteLine("Device ID = 0x{0:X4}", deviceId);

            var hwinfo = FwInfoSource.HwConfiguration(deviceId);
            var ports = hwinfo.Type.ToString();
            Console.WriteLine("Number of ports = " + ports[1]);

            Console.WriteLine("Vendor ID = " + controller.VendorID);

            Console.WriteLine("Model ID = " + controller.ModelId);

            Console.WriteLine("Model revision = " + controller.ModelRevision);

            Console.WriteLine("NVM revision = " + controller.NVMRevision);

            var currentNvm = Utilities.SafeGetVersion(() => controller.GetCurrentFullNvmVersion());
            Console.WriteLine("NVM Firmware version = " + currentNvm);

            Console.WriteLine("Security level = " + controller.SecurityLevel);

            Console.WriteLine("External GPUs supported = " + controller.SupportsExternalGpu);

            Console.WriteLine("OS native PCI enumeration (Native Express mode) = " +
                              (controller.IsInSafeMode ? Utilities.NA : controller.OsNativePciEnumeration.ToString()));

            Console.WriteLine("RTD3 Capable = " +
                              (controller.IsInSafeMode ? Utilities.NA : controller.Rtd3Capable.ToString()));

            if (!controller.IsInSafeMode)
            {
                Console.WriteLine("The following details are specific for TI PD vendor controller");
                GetTIPdInfo(args);
                Console.WriteLine("Customized TI version = " + controller.CustomizedTIVersion);
            }

            else
            {
                CmdUtilities.WriteWrappedLine(Resources.SafeModeWarning);
            }
        }

        protected abstract SdkTbtBase GetController(string id);

        private static void ValidationInSafeModeWarning(SdkTbtBase sdkTbtBase)
        {
            var controller = sdkTbtBase as SdkTbtController;
            if (controller != null && controller.IsInSafeMode)
            {
                CmdUtilities.WriteWrappedLine(Resources.MinimalValidationInSafeMode);
            }
        }
    }
}
