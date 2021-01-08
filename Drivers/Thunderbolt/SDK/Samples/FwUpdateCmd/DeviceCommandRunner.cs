/*******************************************************************************
* Copyright (C) 2015 - 2017 Intel Corp. All rights reserved.
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
using FwUpdateApiSample;

namespace FwUpdateCmd
{
    /// <summary>
    /// Class for handling device controller specific commands
    /// </summary>
    internal class DeviceCommandRunner : CommandRunner
    {
        private readonly Dictionary<string, SdkTbtDevice> _devices;

        internal DeviceCommandRunner()
        {
            _devices = LoadDevices();
        }

        protected override SdkTbtBase GetController(string id)
        {
            if (_devices.ContainsKey(id))
            {
                return _devices[id];
            }
            throw new TbtException(TbtStatus.SDK_INVALID_DEVICE_UUID);
        }

        /// <summary>
        /// Loads the device list from API
        /// </summary>
        /// <returns>Device dictionary (key is device UUID)</returns>
        private static Dictionary<string, SdkTbtDevice> LoadDevices()
        {
            Dictionary<string, SdkTbtDevice> devices;
            devices = SdkTbtDevice.GetDevices();
            if (!devices.Any())
            {
                throw new TbtException(TbtStatus.SDK_NO_DEVICES,
                    Resources.NoEPStringPart0 + "\n"
                    + Resources.NoDeviceStringPart1 + "\n"
                    + Resources.NoDeviceStringPart2CMD);
            }
            return devices;
        }

        /// <summary>
        /// Prints list of updatable devices
        /// </summary>
        internal void EnumUpdatableDevices()
        {
            var updatableDevices = from device in _devices
                where device.Value.Updatable && Utilities.IsSupported(device.Value.ControllerId)
                select device;

            foreach (var details in updatableDevices.Select(device => device.Value))
            {
                Console.WriteLine(details.UUID + '\t' + details.VendorId.ToString("X") + '\t' +
                                  details.ModelId.ToString("X") + '\t' +
                                  details.ControllerNum + '/' + details.NumOfControllers);
            }
        }

        /// <summary>
        /// Prints a tree of all connected devices, showing the topology for easy identification
        /// </summary>
        internal void GetTopology()
        {
            var deviceTree =
                _devices.GroupBy(device => new {device.Value.ControllerId, device.Value.PortNum})
                    .GroupBy(port => port.Key.ControllerId);
            var controllers = SdkTbtController.GetControllers();

            foreach (var controller in deviceTree)
            {
                Console.WriteLine(controller.Key + '\t' + Utilities.SafeGetVersion(() => controllers[controller.Key].GetCurrentFullNvmVersion()));

                foreach (var port in controller)
                {
                    Console.WriteLine("\tPort #" + (port.Key.PortNum + 1) + ":");

                    foreach (var device in port.Select(device => device.Value))
                    {
                        Console.WriteLine("\t\t" + device.PositionInChain + '\t' + device.UUID + '\t'
                                          + device.VendorName + '\t' + device.ModelName + '\t'
                                          + device.ControllerNum + '/' + device.NumOfControllers + '\t'
                                          + device.Updatable + '\t' + device.GetCurrentFullNvmVersion() + '\t'
                                          + device.LinkSpeed );
                    }
                }
            }
        }
    }
}
