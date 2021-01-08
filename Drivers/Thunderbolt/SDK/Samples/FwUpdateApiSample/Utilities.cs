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
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices.WindowsRuntime;

namespace FwUpdateApiSample
{
    /// <summary>
    /// This class includes various useful utility functions, some for internal use and some for
    /// applications to use
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Reads the NVM version from FW image file
        /// </summary>
        /// <param name="path">Path to FW image file to read from</param>
        /// <returns>NVM version as found in the file</returns>
        /// <remarks>
        /// For printing the version, refer to GetCurrentNvmVersionString() below or
        /// Utilities.NvmVersionToString()
        /// </remarks>
        public static UInt32 GetImageNvmVersion(string path)
        {
            const uint nvmVersionOffset = 0xA;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var size = (int) fs.Length;
                var reader = new BinaryReader(fs);

                var imageBuffer = reader.ReadBytes(size);
                var nvmStartOffset =
                    new DeviceFwInfo(new FileFwInfoSource(imageBuffer)).GetSectionInfo()[Sections.Digital].Offset;
                return imageBuffer[nvmVersionOffset + nvmStartOffset];
            }
        }

        /// <summary>
        /// Reads the full NVM version (major.minor) from FW image file
        /// </summary>
        /// <param name="path">Path to FW image file to read from</param>
        /// <returns>NVM version as found in the file</returns>
        public static string GetImageFullNvmVersion(string path)
        {
            const uint nvmMajorVersionOffset = 0xA;
            const uint nvmMinorVersionOffset = 0x9;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var size = (int) fs.Length;
                var reader = new BinaryReader(fs);

                var imageBuffer = reader.ReadBytes(size);
                var nvmStartOffset =
                    new DeviceFwInfo(new FileFwInfoSource(imageBuffer)).GetSectionInfo()[Sections.Digital].Offset;
                return string.Format("{0:X2}.{1:X2}",
                    imageBuffer[nvmStartOffset + nvmMajorVersionOffset],
                    imageBuffer[nvmStartOffset + nvmMinorVersionOffset]);
            }
        }

        /// <summary>
        /// Converts NVM version to string representation, assuming the version is valid
        /// </summary>
        /// <param name="version">NVM version to convert to string</param>
        /// <remarks>
        /// NVM number should be parsed like it's a hex number, but displayed like a decimal
        /// number, e.g. for rev 10 you get the value 16 (0x10) from the FW.
        /// </remarks>
        /// <returns>Strings representation of the NVM version</returns>
        public static string NvmVersionToString(UInt32 version)
        {
            return version.ToString("X");
        }

        /// <summary>
        /// The string used when version isn't available
        /// </summary>
        public const string NA = "N/A";

        /// <summary>
        /// Allows calling get current version functions in a safe manner which
        /// returns "N/A" when in safe mode
        /// </summary>
        /// <param name="func">Function object that reads current version and returns it as a string</param>
        /// <returns>Version string or "N/A" if the controller is safe-mode</returns>
        /// <exception>
        /// Rethrows any error thrown from func that isn't about safe-mode
        /// </exception>
        public static string SafeGetVersion(Func<string> func)
        {
            try
            {
                return func();
            }
            catch (TbtException e)
            {
                switch (e.ErrorCode)
                {
                    case TbtStatus.INVALID_OPERATION_IN_SAFE_MODE:
                    case TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE:
                        return NA;
                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// Reads the PD version from FW image file
        /// GetImagePdVersion is deprecated
        /// </summary>
        /// <param name="path">Path to FW image file to read from</param>
        /// <returns>PD version as found in the file</returns>
        [ObsoleteAttribute("This method is obsolete, for TI use GetImageTIPdVersion", true)]
        public static string GetImagePdVersion(string path)
        {
            return GetImageTIPdVersion(path);
        }

        /// <summary>
        /// Reads the PD version from FW image file
        /// </summary>
        /// <param name="path">Path to FW image file to read from</param>
        /// <returns>PD version as found in the file</returns>
        /// <remarks>
        /// Function tries to get the PD version by PD FW header. It looks for 'ACE1' identifier
        /// which should be located at offset 0x30. If that identifier is found, then the FW version
        /// will be at offset 0x34, otherwise it would try looking for the "TPS6598. HW.{5}FW" string.
        /// returns "N/A" if no PD version is found.
        /// This example works only with Texas Instrument (TI) chips TPS65982/3. 
        /// This should be implemented according to each
        /// specific PD solution.
        /// </remarks>
        public static string GetImageTIPdVersion(string path)
        {
            byte[] imageBuffer;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                imageBuffer = reader.ReadBytes((int) fs.Length);
            }

            var sections = new DeviceFwInfo(new FileFwInfoSource(imageBuffer)).GetSectionInfo();
            if (!sections.ContainsKey(Sections.Pd))
            {
                return NA;
            }

            string version;

            var identifier =
                Encoding.ASCII.GetString(
                    imageBuffer.Skip((int) (sections[Sections.Pd].Offset + 0x30)).Take(4).ToArray());
            if (identifier == "ACE1")
            {
                var buffer =
                    BitConverter.ToString(
                        imageBuffer.Skip((int) (sections[Sections.Pd].Offset + 0x34)).Take(3).ToArray(), 0)
                        .Split('-');

                version = string.Format("{0}.{1}.{2}", buffer[1], buffer[0], buffer[2]);
            }
            else
            {
                const int versionFromFileLength = 10;
                var imageString = Encoding.ASCII.GetString(imageBuffer);

                var match = Regex.Match(imageString, "TPS6598. HW.{5}FW", RegexOptions.Singleline);
                if (!match.Success)
                {
                    return NA;
                }

                version = imageString.Substring(match.Index + match.Length, versionFromFileLength);
            }

            // Removing leading 0's in major version so that instead of 
            // (for example) version being 0001.01.00, version will be 1.01.00
            // (if major version is 0, version will be (for example) 0.08.12).
            // (Side note: the reason we don't use the "Version" class here
            // for the parsing is because we want to preserve the leading
            // zeros for the components besides the major version.)
            version = version.TrimStart('0');
            if (version[0] == '.')
            {
                version = "0" + version;
            }

            return version;
        }

        /// <summary>
        /// This method demonstrate how to use the I2CRead method for a 
        /// specific TI  PD controller It would work with PD controller
        /// TPS65982 and TPS65983 (but not limited to) 
        /// </summary>
        /// <param name="controller">controller ID</param>
        /// <returns>TI pd version as returned from the driver</returns>
        public static string GetTIPdInfo(SdkTbtBase controller)
        {
            // On port 1, offset and length of TI Pd version
            var data = BitConverter.ToInt32(controller.I2CRead(1, 0xF, 4), 0);
            var version = data.ToString("X");
            version = version.Insert(version.Length - 2, ".");
            version = version.Insert(version.Length - 5, ".");
            return version;
        }

        public class DeviceInformation
        {
            public UInt16 VendorId { get; set; }
            public UInt16 ModelId { get; set; }
        }

        /// <summary>
        /// Reads device information (vendor and model IDs) from FW image file
        /// </summary>
        /// <param name="path">Path to FW image file to read from</param>
        /// <returns>Device information (vendor and model IDs) as found in the file</returns>
        public static DeviceInformation GetImageDeviceInformation(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                var imageBuffer = reader.ReadBytes((int) fs.Length);
                var fileFwInfo = new DeviceFwInfo(new FileFwInfoSource(imageBuffer));
                var fileSections = fileFwInfo.GetSectionInfo();
                var fileHasDROM = fileSections.ContainsKey(Sections.DROM);
                if (!fileHasDROM)
                {
                    throw new TbtException(TbtStatus.SDK_NO_DROM_IN_FILE_ERROR);
                }

                var vendorLocation = new CheckLocation(offset: 0x10, length: 2, section: Sections.DROM);
                var modelLocation = new CheckLocation(offset: 0x12, length: 2, section: Sections.DROM);

                Func<CheckLocation, byte[]> extracter =
                    loc =>
                        imageBuffer.Skip((int) (fileSections[loc.Section].Offset + loc.Offset))
                            .Take((int) loc.Length)
                            .ToArray();

                return new DeviceInformation
                {
                    VendorId = BitConverter.ToUInt16(extracter(vendorLocation), 0),
                    ModelId = BitConverter.ToUInt16(extracter(modelLocation), 0)
                };
            }
        }

        /// <summary>
        /// Reads from FW image file if the file is for host or device controller
        /// </summary>
        /// <param name="path">Path to FW image file to read from</param>
        /// <returns>Returns true when the image file is for host controller; otherwise - returns false</returns>
        public static bool GetImageIsHost(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                var imageBuffer = reader.ReadBytes((int) fs.Length);
                // Using DeviceFwInfo instead of HostFwInfo for getting only
                // the basic sections and not failing on non-existing ones
                var fileFwInfo = new DeviceFwInfo(new FileFwInfoSource(imageBuffer));
                var fileSections = fileFwInfo.GetSectionInfo();
                var isHostLocation = ImageValidator.GetIsHostCheckLocation();
                var val =
                    imageBuffer.Skip((int) (fileSections[isHostLocation.Section].Offset + isHostLocation.Offset))
                        .Take((int) isHostLocation.Length)
                        .ToArray();

                return System.Convert.ToBoolean(val[0] & isHostLocation.Mask);
            }
        }

        /// <summary>
        /// Reads "Native express" status from FW image file
        /// </summary>
        /// <param name="path">Path to FW image file to read from</param>
        /// <returns>"Native express" status as found in the file</returns>
        public static bool GetImageOsNativePciEnumerationStatus(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                var imageBuffer = reader.ReadBytes((int)fs.Length);

                var fileFwInfo = new HostFwInfo(new FileFwInfoSource(imageBuffer));
                return fileFwInfo.OsNativePciEnumeration;
            }
        }

        /// <summary>
        /// This method is deprecated as only some old (now unsupported) host
        /// controllers didn't support EP update.
        ///
        /// Checks if a host controller supports device FW update
        /// </summary>
        /// <param name="controllerId">Controller ID as comes from the driver</param>
        /// <returns>True if the controller supports device FW update; otherwise - false</returns>
        [Obsolete("All of the currently supported host controllers support EP update", true)]
        public static bool SupportsDeviceUpdate(string controllerId)
        {
            return true;
        }

        /// <summary>
        /// Checks if a host controller is supported for FW update
        /// </summary>
        /// <param name="controllerId">Controller ID as comes from the driver</param>
        /// <returns>True if the controller is supported for FW update; otherwise - false</returns>
        public static bool HostUpdateSupported(string controllerId)
        {
            var id = _GetDevIdFromControllerId(controllerId);
            switch (id)
            {
                case 0x8A0D:
                case 0x8A17:
                case 0x8A0F:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to make sure this SDK supports the the firmware update through the given host controller.
        /// </summary>
        /// <param name="controllerId">Controller ID as comes from the driver</param>
        /// <returns>true if the controller is supported by the SDK; otherwise - false</returns>
        public static bool IsSupported(string controllerId)
        {
            try
            {
                GetHwConfiguration(controllerId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get HW configuration information from controller ID; helper function
        /// </summary>
        /// <param name="controllerId">Controller ID as comes from the driver</param>
        /// <remarks>
        /// Controller ID format is something like:
        /// <![CDATA[
        /// PCI\VEN_8086&DEV_156A&SUBSYS_00000000&REV_00\6&1e803622&0&000000E4_0
        /// ]]>
        /// The 4 hex-digits after "DEV_" are the device ID
        /// </remarks>
        /// <returns>HWInfo object with HW configuration</returns>
        private static HwInfo GetHwConfiguration(string controllerId)
        {
            return FwInfoSource.HwConfiguration(_GetDevIdFromControllerId(controllerId));
        }

        private static ushort _GetDevIdFromControllerId(string controllerId)
        {
            const string idStartString = "DEV_";
            const int idLength = 4;

            var idLocation = controllerId.IndexOf(idStartString) + idStartString.Length;
            var idString = controllerId.Substring(idLocation, idLength);
            return UInt16.Parse(idString, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        /// <summary>
        /// Check if a pointer comes from FW is valid
        /// </summary>
        /// <param name="pointer">Pointer value as comes from FW</param>
        /// <param name="pointerSize">
        /// The real size of the pointer (to be able to handle 24-bit pointer)
        /// </param>
        /// <returns>True if the pointer is valid, otherwise - false</returns>
        /// <remarks>Invalid pointer is NULL pointer or -1 (0xFFFFFFFF in uint)</remarks>
        internal static bool ValidPointer(uint pointer, int pointerSize)
        {
            var invalidPointer = BitConverter.GetBytes(pointer).Take(pointerSize).All(b => b == byte.MaxValue);
            return pointer != 0 && !invalidPointer;
        }

        internal static void WrapWithConvertToTbtException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Logger.Instance.LogErr($"Got exception: {e}");
                if (e is FwUpdateDriverApi.FuDrvApiException)
                {
                    e = new TbtException(((FwUpdateDriverApi.FuDrvApiException)e).Code);
                }
                throw e;
            }
        }

    }
}
