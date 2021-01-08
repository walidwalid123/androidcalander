/*******************************************************************************
* Copyright (C) 2014 - 2018 Intel Corp. All rights reserved.
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
using System.IO;
using System.Linq;
using System.Text;
using FwUpdateDriverApi;
namespace FwUpdateApiSample
{
    /// <summary>
    /// This class is the interface to a host controller
    ///
    /// This, with its base class, is the main interface of this API module for applications
    /// to interface with host controllers.
    /// The main role of this class is to wrap the interface to driver but it includes also the
    /// interface for validating image compatibility with the current controller.
    /// </summary>
    public class SdkTbtController : SdkTbtBase
    {
        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="mo">Driver interface to use for interfacing with the controller</param>
        public SdkTbtController(IDriverController controllerIf) : base(controllerIf)
        {
            _controllerIf = controllerIf;
            _controllerParams = controllerIf.getParams();
        }

        private Dictionary<Sections, FwInfo.SectionDetails> _sections;
        private string _vendorId;
        private string _modelId;
        private string _modelRevision;
        private string _nvmRevision;
        private string _customizedTiVersion;
        private IDriverController _controllerIf;
        private ControllerParams _controllerParams;

        /// <summary>
        /// Drom offset
        /// </summary>
        internal Dictionary<Sections, FwInfo.SectionDetails> Section
        {
            get
            {
                return _sections ?? (_sections = new HostFwInfo(new ControllerFwInfoSource(this)).GetSectionInfo());
            }
        }

        /// <summary>
        /// This property is used to differentiate between the power down messages
        /// given to user for host controller or device controller after update in
        /// cases where the PD version was updated. 
        /// </summary>
        /// <returns>Power down necessary for host controller message string</returns>
        public override string GetNeedPowerDownMessage
        {
            get { return Resources.HostNeedsPowerDownMessage; } 
        }

        /// <summary>
        /// Controller ID property as returned by the driver
        /// </summary>
        public string ControllerId
        {
            get { return _controllerParams.ControllerId; }
        }

        /// <summary>
        /// Controller generation as return by the driver
        /// </summary>
        public UInt32 Generation
        {
            get { return _controllerParams.Generation; }
        }

        /// <summary>
        /// Device ID property as returned by the driver
        /// </summary>
        public UInt16 DeviceId
        {
            get { return _controllerParams.DeviceId; }
        }

        /// <summary>
        /// Security Level property as returned by the driver
        /// If is in Safe Mode returns NA
        /// </summary>
        public string SecurityLevel
        {
            get
            {
                if (IsInSafeMode)
                    return Utilities.NA;
                return _controllerParams.SecurityLevel.ToString();
            }
        }

        /// <summary>
        /// Supports External GPU property as returned by the driver
        /// If is in Safe Mode returns NA
        /// </summary>
        public string SupportsExternalGpu
        {
            get
            {
                if (IsInSafeMode)
                    return Utilities.NA;
                return _controllerParams.SupportsExternalGpu.ToString();
            }
        }

        /// <summary>
        /// True if the controller is in safe-mode according to the information from the driver
        /// </summary>
        public bool IsInSafeMode
        {
            get { return _controllerParams.InSafeMode; }
        }

        /// <summary>
        /// Vendor id as read from the Drom, if is in Safe Mode or no drom returns NA
        /// </summary>
        public string VendorID
        {
            get
            {
                try
                {
                    return _vendorId ??
                           (_vendorId =
                               string.Format("0x{0:D4}",
                                   BitConverter.ToUInt16(ReadFirmware(Section[Sections.DROM].Offset + 0x10, 2), 0)
                                       .ToString("X")));
                }
                catch
                {
                    return Utilities.NA;
                }
            }
        }

        /// <summary>
        /// Model id as read from the Drom, if is in Safe Mode or no drom returns NA
        /// </summary>
        public string ModelId
        {
            get
            {
                try
                {
                    return _modelId ??
                           (_modelId =
                               string.Format("0x{0:D4}",
                                   BitConverter.ToUInt16(ReadFirmware(Section[Sections.DROM].Offset + 0x12, 2), 0)
                                       .ToString("X")));
                }
                catch
                {
                    return Utilities.NA;
                }
            }
        }

        /// <summary>
        /// Model revision as read from the Drom, if is in Safe Mode or no drom returns NA
        /// </summary>
        public string ModelRevision
        {
            get
            {
                try
                {
                    return _modelRevision ??
                           (_modelRevision = ReadFirmware(Section[Sections.DROM].Offset + 0x14, 1)[0].ToString());
                }
                catch
                {
                    return Utilities.NA;
                }
            }
        }

        /// <summary>
        /// NVM revision as read from the Drom, if is in Safe Mode or no drom returns NA
        /// </summary>
        public string NVMRevision
        {
            get
            {
                try
                {
                    return _nvmRevision ??
                           (_nvmRevision = ReadFirmware(Section[Sections.DROM].Offset + 0x15, 1)[0].ToString());
                }
                catch
                {
                    return Utilities.NA;
                }
            }
        }

        /// <summary>
        /// Customized TI Version as read from the Drom, if is in Safe Mode or no drom returns NA
        /// </summary>
        public string CustomizedTIVersion
        {
            get
            {
                try
                {
                    return _customizedTiVersion ??
                           (_customizedTiVersion =
                               BitConverter.ToUInt16(ReadFirmware(Section[Sections.Ee2TarDma].Offset + 0x3A, 2), 0)
                                   .ToString("X"));
                }
                catch
                {
                    return Utilities.NA;
                }
            }
        }

        public bool OsNativePciEnumeration
        {
            get
            {
                {
                    return _controllerParams.OsNativePciEnum;
                }
            }
        }

        public bool Rtd3Capable
        {
            get
            {
                {
                    return _controllerParams.Rtd3Capable;
                }
            }
        }

        /// <summary>
        /// Gets list of SdkTbtController objects for each of the host controllers exposed by the driver.
        /// </summary>
        /// <returns>
        /// Dictionary with objects for all the host controllers; the key is the controller ID
        /// </returns>
        public static Dictionary<string, SdkTbtController> GetControllers()
        {
            Dictionary<string, IDriverController> controllersInterfaces = null;
            Utilities.WrapWithConvertToTbtException(() => controllersInterfaces = DriverApiFactory.GetControllers(Logger.Instance));
            var controllers = new Dictionary<string, SdkTbtController>();
            foreach (var controller in controllersInterfaces)
            {
                var tbtController = new SdkTbtController(controller.Value);
                Logger.Instance.LogInfo(tbtController.ToString());
                controllers.Add(controller.Key, tbtController);
                
            }
            return controllers;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('*', 80).Append("\n");
            sb.AppendLine($"Controller with ID: {ControllerId}");
            sb.AppendLine($"Generation: {Generation}");
            sb.AppendLine($"DeviceId: {DeviceId}");
            sb.AppendLine($"SecurityLevel: {SecurityLevel}");
            sb.AppendLine($"SupportsExternalGpu: {SupportsExternalGpu}");
            sb.AppendLine($"IsInSafeMode: {IsInSafeMode}");
            sb.AppendLine($"VendorId: {VendorID}");
            sb.AppendLine($"ModelId: {ModelId}");
            sb.AppendLine($"ModelRevision: {ModelRevision}");
            sb.AppendLine($"NVMRevision: {NVMRevision}");
            sb.AppendLine($"CustomizedTIVersion: {CustomizedTIVersion}");
            sb.AppendLine($"OsNativePciEnumeration: {OsNativePciEnumeration}");
            sb.AppendLine($"Rtd3Capable: {Rtd3Capable}");
            return sb.ToString();
        }

        /// <summary>
        /// Validates FW image file compatibility with the current controller
        /// </summary>
        /// <param name="path">Path to FW image file to check</param>
        /// <remarks>
        /// When controller is in safe-mode, it's impossible to read its FW to gather needed info
        /// for comparison, so the function do only minimal image validation! User has to check 
        /// if the controller is in safe-mode to know that only minimal validation was done.
        /// </remarks>
        /// <exception>Throws exception in case of incompatibility error or other errors</exception>
        public override void ValidateImage(string path)
        {
            if (!Utilities.HostUpdateSupported(ControllerId))
            {
                throw new TbtException(TbtStatus.UNSUPPORTED_OPERATION);
            }
            if (!File.Exists(path))
            {
                throw new TbtException(TbtStatus.SDK_FILE_NOT_FOUND);
            }

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                var imageBuffer = reader.ReadBytes((int) fs.Length);
                var fileFwInfo = new HostFwInfo(new FileFwInfoSource(imageBuffer));
                
                // Only minimal validation done in safe-mode; applications are advised to warn the user about it
                if (IsInSafeMode)
                {
                    var FWInfo = FwInfoSource.HwConfiguration(DeviceId);
                    if (FWInfo.Generation != fileFwInfo.Info.Generation)
                    {
                        throw new TbtException(TbtStatus.SDK_HW_GENERATION_MISMATCH);
                    }
                    if (FWInfo.Type != fileFwInfo.Info.Type)
                    {
                        throw new TbtException(TbtStatus.SDK_PORT_COUNT_MISMATCH);
                    }

                    if (!Utilities.GetImageIsHost(path))
                    {
                        throw new TbtException(TbtStatus.SDK_IMAGE_FOR_DEVICE_ERROR);
                    }
                    return;
                }
                var controllerFwInfo = new HostFwInfo(new ControllerFwInfoSource(this));

                if (controllerFwInfo.Info.Generation != fileFwInfo.Info.Generation)
                {
                    throw new TbtException(TbtStatus.SDK_HW_GENERATION_MISMATCH);
                }
                if (controllerFwInfo.Info.Type != fileFwInfo.Info.Type)
                {
                    throw new TbtException(TbtStatus.SDK_PORT_COUNT_MISMATCH);
                }

                var validator = new HostImageValidator(this,
                    imageBuffer,
                    controllerFwInfo.GetSectionInfo(),
                    fileFwInfo.GetSectionInfo(),
                    controllerFwInfo.Info);
                validator.Validate();

                if (OsNativePciEnumeration != fileFwInfo.OsNativePciEnumeration)
                {
                    throw new TbtException(TbtStatus.SDK_NATIVE_MODE_MISMATCH);
                }
            }
        }

        /// <summary>
        /// See SdkTbtBase.GetCurrentNvmVersion().
        /// This function is here to throw in case of safe-mode
        /// </summary>
        public override UInt32 GetCurrentNvmVersion()
        {
            if (IsInSafeMode)
            {
                throw new TbtException(TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE);
            }
            return base.GetCurrentNvmVersion();
        }

        /// <summary>
        /// See SdkTbtBase.GetCurrentFullNvmVersion().
        /// This function is here to throw in case of safe-mode
        /// </summary>
        public override string GetCurrentFullNvmVersion()
        {
            if (IsInSafeMode)
            {
                throw new TbtException(TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE);
            }
            return base.GetCurrentFullNvmVersion();
        }

        /// <summary>
        /// See SdkTbtBase.I2CRead().
        /// This function is here to throw in case of safe-mode
        /// </summary>
        public override byte[] I2CRead(UInt32 port, UInt32 offset, UInt32 length)
        {
            if (IsInSafeMode)
            {
                throw new TbtException(TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE);
            }
            if (!Utilities.HostUpdateSupported(ControllerId))
            {
                throw new TbtException(TbtStatus.UNSUPPORTED_OPERATION);
            }
            return base.I2CRead(port, offset, length);
        }

        /// <summary>
        /// See SdkTbtBase.I2CWrite().
        /// This function is here to throw in case of safe-mode
        /// </summary>
        public override void I2CWrite(UInt32 port, UInt32 offset, byte[] data)
        {
            if (IsInSafeMode)
            {
                throw new TbtException(TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE);
            }
            if (!Utilities.HostUpdateSupported(ControllerId))
            {
                throw new TbtException(TbtStatus.UNSUPPORTED_OPERATION);
            }
            base.I2CWrite(port, offset, data);
        }

        /// <summary>
        /// GetCurrentPdVersion is deprecated
        /// This function is here to throw in case of safe-mode
        /// </summary>
        /// <returns>N/A</returns>
        [ObsoleteAttribute("This method is obsolete, use I2CRead", true)]
        public override string GetCurrentPdVersion()
        {
            if (IsInSafeMode)
            {
                throw new TbtException(TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE);
            }
            return Utilities.NA;
        }

        /// <summary>
        /// See SdkTbtBase.ReadFirmware().
        /// This function is here to throw in case of safe-mode
        /// </summary>
        public override byte[] ReadFirmware(UInt32 offset, UInt32 length)
        {
            if (IsInSafeMode)
            {
                throw new TbtException(TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE);
            }
            return base.ReadFirmware(offset, length);
        }

        /// <summary>
        /// Starts the NVM update process
        /// </summary>
        /// <param name="bufferSize">Size of the array passed in buffer argument</param>
        /// <param name="buffer">Buffer with the firmware image binary to use for the update</param>
        /// <exception cref="TbtException">
        /// Throws an exception with proper message and the returned error code in case of an error
        /// </exception>
        public override void UpdateFirmware(UInt32 bufferSize, byte[] buffer)
        {
            if (!Utilities.HostUpdateSupported(ControllerId))
            {
                throw new TbtException(TbtStatus.UNSUPPORTED_OPERATION);
            }
            base.UpdateFirmware(bufferSize, buffer);
        }
    }
}
