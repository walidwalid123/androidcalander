/*******************************************************************************
* Copyright (C) 2014 - 2017 Intel Corp. All rights reserved.
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
using FwUpdateDriverApi;
using System.Text;

namespace FwUpdateApiSample
{
    /// <summary>
    /// This class is the interface to a device controller
    ///
    /// This, with its base class, is the main interface of this API module for applications
    /// to interface with device controllers.
    /// The main role of this class is to wrap the interface to driver but it includes also the
    /// interface for validating image compatibility with the current controller.
    /// </summary>
    public class SdkTbtDevice : SdkTbtBase
    {
        private IDriverDevice _deviceIf;
        private DeviceParams _deviceParams;

        /// <summary>
        /// C-tor
        /// </summary>
        public SdkTbtDevice(IDriverDevice deviceIf) : base(deviceIf)
        {
            _deviceIf = deviceIf;
            _deviceParams = deviceIf.getParams();
        }

        /// <summary>
        /// This property is used to differentiate between the power down messages
        /// given to user for host controller or device controller after update in
        /// cases where the PD version was updated. 
        /// </summary>
        /// <returns>Power down necessary for device controller message string</returns>
        public override string GetNeedPowerDownMessage
        {
            get { return Resources.DeviceNeedsPowerDownMessage; }
        }

        /// <summary>
        /// Device controller UUID property as returned by the driver
        /// </summary>
        public string UUID
        {
            get { return _deviceParams.UUID; }
        }

        /// <summary>
        /// ID of the host controller the device is connected to as returned by the driver
        /// </summary>
        public string ControllerId
        {
            get { return _deviceParams.ControllerId; }
        }

        /// <summary>
        /// 0-based index of the port in the host controller the device is connected to as
        /// returned by the driver
        /// </summary>
        public UInt32 PortNum
        {
            get { return _deviceParams.PortNum; }
        }

        /// <summary>
        /// 1-based index of the device position in the port the device is connected to as
        /// returned by the driver
        /// </summary>
        public UInt32 PositionInChain
        {
            get { return _deviceParams.PositionInChain; }
        }

        /// <summary>
        /// Device vendor name as returned by the driver
        /// </summary>
        public string VendorName
        {
            get { return _deviceParams.VendorName; }
        }

        /// <summary>
        /// Device model name as returned by the driver
        /// </summary>
        public string ModelName
        {
            get { return _deviceParams.ModelName; }
        }

        /// <summary>
        /// Device vendor ID as returned by the driver
        /// </summary>
        public UInt16 VendorId
        {
            get { return _deviceParams.VendorId; }
        }

        /// <summary>
        /// Device model ID as returned by the driver
        /// </summary>
        public UInt16 ModelId
        {
            get { return _deviceParams.ModelId; }
        }

        /// <summary>
        /// Controller enumeration out of n controllers in the device (as marked in the controller's NVM)
        /// </summary>
        public byte ControllerNum
        {
            get { return _deviceParams.ControllerNum; }
        }

        /// <summary>
        /// Total amount of controllers in the device
        /// </summary>
        public byte NumOfControllers
        {
            get { return _deviceParams.NumOfControllers; }
        }

        /// <summary>
        /// True if the device is updatable according to the information from the driver
        /// </summary>
        /// <remarks>
        /// If the controller doesn't support device FW update, the device appears like not
        /// updatable even if it supports FW update.
        /// </remarks>
        public bool Updatable
        {
            get { return _deviceParams.Updatable; }
        }

        /// <summary>
        /// CIO link speed (in Gbps) of the device
        /// </summary>
        public byte LinkSpeed
        {
            get { return _deviceParams.LinkSpeed; }
        }

        public static Dictionary<string, SdkTbtDevice> GetDevices()
        {
            Dictionary<string, IDriverDevice> devicesInterfaces = null;
            Utilities.WrapWithConvertToTbtException(() => devicesInterfaces = DriverApiFactory.GetDevices(Logger.Instance));
            var devices = new Dictionary<string, SdkTbtDevice>();
            foreach (var device in devicesInterfaces)
            {
                var tbtDevice = new SdkTbtDevice(device.Value);
                Logger.Instance.LogInfo(tbtDevice.ToString());
                devices.Add(device.Key, tbtDevice);
            }
            return devices;
        }

        /// <summary>
        /// Validates FW image file compatibility with the current controller
        /// </summary>
        /// <param name="path">Path to FW image file to check</param>
        /// <exception>Throws exception in case of incompatibility error or other errors</exception>
        public override void ValidateImage(string path)
        {
            if (!File.Exists(path))
            {
                throw new TbtException(TbtStatus.SDK_FILE_NOT_FOUND);
            }
            if (!Updatable)
            {
                throw new TbtException(TbtStatus.SDK_DEVICE_NOT_SUPPORTED);
            }

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                var imageBuffer = reader.ReadBytes((int) fs.Length);
                var deviceFwInfo = new DeviceFwInfo(new ControllerFwInfoSource(this));
                var fileFwInfo = new DeviceFwInfo(new FileFwInfoSource(imageBuffer));

                if (deviceFwInfo.Info.Generation != fileFwInfo.Info.Generation)
                {
                    throw new TbtException(TbtStatus.SDK_HW_GENERATION_MISMATCH);
                }
                if (deviceFwInfo.Info.Type != fileFwInfo.Info.Type)
                {
                    throw new TbtException(TbtStatus.SDK_PORT_COUNT_MISMATCH);
                }

                var validator = new DeviceImageValidator(this,
                    imageBuffer,
                    deviceFwInfo.GetSectionInfo(),
                    fileFwInfo.GetSectionInfo(),
                    deviceFwInfo.Info);
                validator.Validate();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('-', 80).Append("\n");
            sb.AppendLine($"Device with UUID: {UUID}");
            sb.AppendLine($"Controller ID: {ControllerId}");
            sb.AppendLine($"PortNum: {PortNum}");
            sb.AppendLine($"PositionInChain: {PositionInChain}");
            sb.AppendLine($"VendorName: {VendorName}");
            sb.AppendLine($"ModelName: {ModelName}");
            sb.AppendLine($"ModelId: {ModelId}");
            sb.AppendLine($"VendorId: {VendorId}");
            sb.AppendLine($"ControllerNum: {ControllerNum}");
            sb.AppendLine($"NumOfControllers: {NumOfControllers}");
            sb.AppendLine($"Updatable: {Updatable}");
            sb.AppendLine($"LinkSpeed: {LinkSpeed}");
            return sb.ToString();
        }
    }
}
