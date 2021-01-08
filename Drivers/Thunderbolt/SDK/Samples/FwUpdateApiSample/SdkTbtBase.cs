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

namespace FwUpdateApiSample
{
    /// <summary>
    /// This class is a base class for the interfacing with both controllers and devices.
    ///
    /// This, with the derived classes, is the main interface of this API module for applications
    /// to use.
    /// The main role of this class is to wrap the interface to the driver but it includes also the
    /// interface for validating image compatibility with the current controller.
    /// </summary>
    public abstract class SdkTbtBase
    {
        /// <summary>
        /// C-tor. For use by derived classes.
        /// </summary>
        protected SdkTbtBase(IDriverBase driverIf)
        {
            _driverIf = driverIf;
        }

        /// <summary>
        /// Abstract base property to get the proper message when a controller needs powering down.
        /// </summary>
        /// <returns>Child classes return appropriate message string</returns>
        public abstract string GetNeedPowerDownMessage { get; }

        /// <summary>
        /// Validates FW image file compatibility with the current controller
        /// </summary>
        /// <param name="path">Path to FW image file to check</param>
        /// <remarks>
        /// When controller is in safe-mode, it's impossible to read its FW to gather needed info
        /// for comparison, so the function is no-op! User has to check if the controller is in
        /// safe-mode to know if validation had any effect.
        /// </remarks>
        /// <exception>Throws exception in case of incompatibility error</exception>
        public abstract void ValidateImage(string path);

        /// <summary>
        /// Driver interface to use for interfacing with the controller/device
        /// </summary>
        private IDriverBase _driverIf;

        /// <summary>
        /// Starts the NVM update process
        /// </summary>
        /// <param name="bufferSize">Size of the array passed in buffer argument</param>
        /// <param name="buffer">Buffer with the firmware image binary to use for the update</param>
        /// <exception cref="TbtException">
        /// Throws an exception with proper message and the returned error code in case of an error
        /// </exception>
        public virtual void UpdateFirmware(UInt32 bufferSize, byte[] buffer)
        {
            Logger.Instance.LogInfo($"Start update firmware, buffer size is {bufferSize}");
            Utilities.WrapWithConvertToTbtException(() => _driverIf.UpdateFirmare(bufferSize, buffer));
        }

        /// <summary>
        /// Reads the current NVM version from the controller/device. The method is deprecated
        /// </summary>
        /// <returns>NVM version as returned from the firmware</returns>
        /// <remarks>
        /// For printing the version, refer to Utilities.NvmVersionToString()
        /// </remarks>
        /// <exception cref="TbtException">
        /// Throws an exception with proper message and the returned error code in case of an error
        /// </exception>
        public virtual UInt32 GetCurrentNvmVersion()
        {
            NvmVersion ver = null;
            Utilities.WrapWithConvertToTbtException(() => ver = _driverIf.GetNvmVersion());
            return ver.Major;
        }

        /// <summary>
        /// Reads the current full NVM version (major.minor) from the controller/device
        /// </summary>
        /// <returns>NVM version that was read from the firmware (in major.minor format)</returns>
        /// <exception cref="TbtException">
        /// Throws an exception with proper message and the returned error code in case of an error
        /// </exception>
        public virtual string GetCurrentFullNvmVersion()
        {
            string nvmVersion = null;
            Utilities.WrapWithConvertToTbtException(() => nvmVersion = _driverIf.GetNvmVersion().ToString());
            return nvmVersion;
        }

        /// <summary>
        /// Reads the content of I2C register
        /// </summary>
        /// <param name="port">port number</param>
        /// <param name="offset">offsett in I2C</param>
        /// <param name="length">length to read</param>
        /// <returns>byte array with the content of the register</returns>
        /// <exception cref="TbtException">
        /// Throws an exception with proper message and the returned error code in case of an error
        /// </exception>
        public virtual byte[] I2CRead(UInt32 port, UInt32 offset, UInt32 length)
        {
            byte[] readData = null;
            Logger.Instance.LogInfo($"Performing I2CRead, port={port}, offset={offset}, length={length}");
            Utilities.WrapWithConvertToTbtException(() => readData = _driverIf.I2CRead(port, offset, length));
            return readData;
        }

        /// <summary>
        /// Writes data to I2C register
        /// </summary>
        /// <param name="port">port number</param>
        /// <param name="offset">offset in I2C</param>
        /// <param name="data">data to write</param>
        /// <exception cref="TbtException">
        /// Throws an exception with proper message and the returned error code in case of an error
        /// </exception>
        public virtual void I2CWrite(UInt32 port, UInt32 offset, byte[] data)
        {
            Logger.Instance.LogInfo($"Performing I2CWrite, port={port}, offset={offset}, length={data.Length}");
            Utilities.WrapWithConvertToTbtException(() => _driverIf.I2CWrite(port, offset, data));
        }

        /// <summary>
        /// GetCurrentPdVersion is deprecated
        /// </summary>
        /// <returns>N/A</returns>
        [ObsoleteAttribute("This method is obsolete, use I2CRead", true)]
        public virtual string GetCurrentPdVersion()
        {
            return Utilities.NA;
        }

        /// <summary>
        /// Reads data from NVM (FW) of the controller/device
        /// </summary>
        /// <param name="offset">Offset in NVM</param>
        /// <param name="length">Length to read (in bytes)</param>
        /// <returns>Returns byte array with the information read from firmware</returns>
        /// <exception>Throws on failure</exception>
        /// <remarks>
        /// The service supports only DW aligned offsets and reads 1 DW (4 bytes) for each read.
        /// This function gives simpler interface to the rest of the code with support to any offset
        /// and length as needed by doing the alignment internally, reading in loop the needed data
        /// and then extracting only the requested data for returning
        /// </remarks>
        public virtual byte[] ReadFirmware(UInt32 offset, UInt32 length)
        {
            byte[] readData = null;
            Logger.Instance.LogInfo($"Performing ReadFirmware, offset={offset}, length={length}");
            Utilities.WrapWithConvertToTbtException(() => readData = _driverIf.ReadFirmware(offset, length));
            return readData;
        }

        /// <summary>Wrapper for SDK API to update from a file</summary>
        /// <param name="filename">Path to firmware image file</param>
        /// <exception>Throws an exception in case of an error</exception>
        public void UpdateFirmwareFromFile(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                var imageBuffer = reader.ReadBytes((int) fs.Length);
                UpdateFirmware((UInt32) imageBuffer.Length, imageBuffer);
            }
        }

    }
}
