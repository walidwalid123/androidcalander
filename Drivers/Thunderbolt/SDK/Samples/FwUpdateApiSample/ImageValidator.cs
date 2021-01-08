/*******************************************************************************
* Copyright (C) 2015-2017 Intel Corp. All rights reserved.
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

namespace FwUpdateApiSample
{
    /// <summary>
    /// This class, with its derived classes, is used by SdkTbtBase class tree to implement the
    /// FW image file validation
    /// </summary>
    internal abstract class ImageValidator
    {
        /// <summary>
        /// Returns check locations for validating FW image file compatibility with the controller
        /// </summary>
        /// <param name="generation">Controller generation</param>
        /// <returns>Container with check locations to compare</returns>
        protected abstract IEnumerable<CheckLocation> GetCheckLocations(HwGeneration generation);

        /// <summary>
        /// Helper function with common check location that needs error code customization in
        /// derived classes
        /// </summary>
        /// <returns>Check location in FW to know if it's host controller or device</returns>
        public static CheckLocation GetIsHostCheckLocation()
        {
            return new CheckLocation(offset: 0x10, length: 1, mask: Convert.ToByte("00000010", 2));
        }

        protected HwInfo HwInfo { get; private set; }

        private readonly Dictionary<Sections, FwInfo.SectionDetails> _fileSections;
        private readonly SdkTbtBase _controller;
        private readonly byte[] _image;
        private readonly Dictionary<Sections, FwInfo.SectionDetails> _controllerSections;

        protected ImageValidator(SdkTbtBase controller,
            byte[] image,
            Dictionary<Sections, FwInfo.SectionDetails> controllerSections,
            Dictionary<Sections, FwInfo.SectionDetails> fileSections,
            HwInfo hwInfo)
        {
            _controller = controller;
            _image = image;
            _controllerSections = controllerSections;
            _fileSections = fileSections;
            HwInfo = hwInfo;
        }

        /// <summary>
        /// Validates FW image file compatibility with the controller
        /// </summary>
        internal virtual void Validate()
        {
            ValidateChipSize();
            ComparePdExistence();
            CompareDROM();
            Compare(GetCheckLocations(HwInfo.Generation));
        }

        /// <summary>
        /// Validates that image file fits into HW chip size
        /// </summary>
        private void ValidateChipSize()
        {
            // 0x45[2:0] is flash chip size in FR and AR FW
            var flashSizeCheckLocation = new CheckLocation(offset: 0x45, length: 1,
                errorCode: TbtStatus.SDK_CHIP_SIZE_ERROR, mask: Convert.ToByte("00000111", 2));

            var chipSize = ReadFromFw(flashSizeCheckLocation)[0];
            var imageChipSize = ReadFromFile(flashSizeCheckLocation)[0];
            if (chipSize < imageChipSize)
            {
                throw new TbtException(flashSizeCheckLocation.ErrorCode);
            }
        }

        /// <summary>
        /// Validates that either both or neither controller and FW image file contain PD FW
        /// </summary>
        private void ComparePdExistence()
        {
            var pdPointer = new CheckLocation(offset: 0x10C, length: 4, section: Sections.ArcParams);

            var controllerHasPd
                = Utilities.ValidPointer(BitConverter.ToUInt32(ReadFromFw(pdPointer), 0), (int) pdPointer.Length);
            var imageHasPd
                = Utilities.ValidPointer(BitConverter.ToUInt32(ReadFromFile(pdPointer), 0), (int) pdPointer.Length);

            if (controllerHasPd != imageHasPd)
            {
                throw new TbtException(TbtStatus.SDK_PD_MISMATCH);
            }
        }

        /// <summary>
        /// Compares DROM information to validate FW image file compatibility with
        /// the controller
        /// </summary>
        private void CompareDROM()
        {
            var dromCheckLocations = new List<CheckLocation>
            {
                new CheckLocation(offset: 0x10, length: 2, errorCode: TbtStatus.SDK_VENDOR_MISMATCH, section: Sections.DROM),
                new CheckLocation(offset: 0x12, length: 2, errorCode: TbtStatus.SDK_MODEL_MISMATCH , section: Sections.DROM),
            };

            Compare(dromCheckLocations);
        }

        /// <summary>
        /// Compares each of the given check locations to validate FW image file compatibility with
        /// the controller
        /// </summary>
        /// <param name="checkLocations">List of FW locations to compare</param>
        private void Compare(IEnumerable<CheckLocation> checkLocations)
        {
            foreach (var val in checkLocations)
            {
                var valueFromFw = ReadFromFw(val);
                var valueFromFile = ReadFromFile(val);
                Logger.Instance.LogInfo($"Validating {val.Description}");
                Logger.Instance.LogInfo($"Value from firmware    : {System.Text.Encoding.Default.GetString(valueFromFw)}");
                Logger.Instance.LogInfo($"Value from given image : {System.Text.Encoding.Default.GetString(valueFromFile)}");
                if (!valueFromFw.SequenceEqual(valueFromFile))
                {
                    if (val.Description != null && val.Description.Any())
                    {
                        throw new TbtException(val.ErrorCode,
                            Resources.ValidationFailedPart1 + val.Description + "\n"
                            + Resources.ValidationFailedPart2);
                    }
                    throw new TbtException(val.ErrorCode);
                }
            }
        }

        protected byte[] ReadFromFw(CheckLocation loc)
        {
            Logger.Instance.LogInfo($"ReadFromFw. Offset: {_controllerSections[loc.Section].Offset + loc.Offset} Length: {loc.Length}");
            var val = _controller.ReadFirmware(_controllerSections[loc.Section].Offset + loc.Offset, loc.Length);
            return ApplyMask(val, loc.Mask);
        }

        protected byte[] ReadFromFile(CheckLocation loc)
        {
            Logger.Instance.LogInfo($"ReadFromFile. Offset: {_fileSections[loc.Section].Offset + loc.Offset} Length: {loc.Length}");
            var val =
                _image.Skip((int) (_fileSections[loc.Section].Offset + loc.Offset)).Take((int) loc.Length).ToArray();
            return ApplyMask(val, loc.Mask);
        }

        private static byte[] ApplyMask(byte[] val, byte mask)
        {
            if (mask != CheckLocation.FullMask)
            {
                val[0] &= mask;
            }
            return val;
        }
    }

    /// <summary>
    /// This class is used for passing information about FW locations to compare between the
    /// controller and the FW image file to validate compatibility.
    /// </summary>
    internal class CheckLocation
    {
        public Sections Section { get; private set; }
        public UInt32 Offset { get; private set; }
        public UInt32 Length { get; private set; }
        public byte Mask { get; private set; }

        /// <summary>
        /// Used in the exception thrown in case of a mismatch
        /// </summary>
        public TbtStatus ErrorCode { get; set; }

        /// <summary>
        /// Used in the error message in case of a mismatch
        /// </summary>
        public string Description { get; private set; }

        public const byte FullMask = byte.MaxValue;

        public CheckLocation(UInt32 offset,
            UInt32 length,
            byte mask = FullMask,
            Sections section = Sections.Digital,
            TbtStatus errorCode = TbtStatus.SDK_IMAGE_VALIDATION_ERROR,
            string description = null)
        {
            Section = section;
            Offset = offset;
            Length = length;
            if (mask != FullMask && length > 1) // We expect mask to be applied for 1 byte only
            {
                // If we find such cases in the future, other code must be changed to remove this assumption
                throw new TbtException(TbtStatus.SDK_INTERNAL_ERROR, Resources.MaskIsntSupported);
            }
            Mask = mask;
            ErrorCode = errorCode;
            Description = description;
        }
    }
}
