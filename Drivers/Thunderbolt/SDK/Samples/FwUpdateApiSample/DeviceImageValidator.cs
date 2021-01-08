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

namespace FwUpdateApiSample
{
    /// <summary>
    /// This class is used by SdkTbtDevice class to implement the FW image file validation.
    /// Includes validation information specific to device controller checks.
    /// </summary>
    internal class DeviceImageValidator : ImageValidator
    {
        internal DeviceImageValidator(SdkTbtBase controller,
            byte[] image,
            Dictionary<Sections, FwInfo.SectionDetails> controllerSections,
            Dictionary<Sections, FwInfo.SectionDetails> fileSections,
            HwInfo hwInfo)
            : base(controller, image, controllerSections, fileSections, hwInfo)
        {
        }

        protected override IEnumerable<CheckLocation> GetCheckLocations(HwGeneration generation)
        {
            var isHost = GetIsHostCheckLocation();
            isHost.ErrorCode = TbtStatus.SDK_IMAGE_FOR_HOST_ERROR;
            switch (generation)
            {
                case HwGeneration.DSL6540_6340:
                case HwGeneration.JHL6240:
                case HwGeneration.JHL6540_6340:
                    return new[]
                    {
                        isHost,
                        new CheckLocation(offset: 0x124, length: 1, description: "X of N", section: Sections.ArcParams)
                    };

                case HwGeneration.JHL7540_7440_7340:
                case HwGeneration.JHL8440:
                {
                    return new[]
                    {
                        isHost
                    };
                }
                default:
                    throw new TbtException(TbtStatus.SDK_UNKNOWN_CHIP);
            }
        }

        internal override void Validate()
        {
            base.Validate();
            if (HwInfo.Generation >= HwGeneration.JHL7540_7440_7340)
                CheckMultipleController();
        }

        private void CheckMultipleController()
        {
            var lengthLocation = new CheckLocation(offset: 0xE, length: 2, section: Sections.DROM);
            var fileLengthContainer = ReadFromFile(lengthLocation);
            var fwLengthContainer = ReadFromFw(lengthLocation);
            var fileDromLength = ExtractLength(fileLengthContainer);
            var fwDromLength = ExtractLength(fwLengthContainer);
            var fileDrom = ReadFromFile(new CheckLocation(offset: 0, length: fileDromLength, section: Sections.DROM));
            var fwDrom = ReadFromFw(new CheckLocation(offset: 0, length: fwDromLength, section: Sections.DROM));

            var fileMcData = GetMcData(fileDrom);
            var fwMcData = GetMcData(fwDrom);

            // Either not the same data or one is MC and one is not
            if (fileMcData != fwMcData)
                throw new TbtException(TbtStatus.SDK_IMAGE_VALIDATION_ERROR,
                    Resources.ValidationFailedPart1 + "X of N" + "\n" + Resources.ValidationFailedPart2);
        }

        /// <summary>
        /// Extracting the DROM length from the bytes
        /// </summary>
        /// <param name="lengthBytes">Bytes that include the length</param>
        /// <returns>Exact length</returns>
        private uint ExtractLength(byte[] lengthBytes)
        {
            //DROM length including the identification section(9) and the CRC32(4)
            return GetBitField(BitConverter.ToUInt16(lengthBytes, 0), 0, 12) + 13;
        }

        /// <summary>
        /// Getting the data of MC
        /// </summary>
        /// <param name="drom">The entire DROM</param>
        /// <returns>In case of multiple controller device - the data, otherwise - null</returns>
        private int? GetMcData(byte[] drom)
        {
            const int headerSize = 13;
            const int idSectionSize = 9;

            for (var mcOffset = idSectionSize + headerSize; mcOffset < drom.Length; mcOffset += drom[mcOffset])
            {
                byte entryDetails = drom[mcOffset + 1];
                var p = Convert.ToBoolean(GetBitField(entryDetails, 7, 1));
                if (!p) // If p is set to zero - generic entry
                {
                    var entryType = GetBitField(entryDetails, 0, 6);
                    if (entryType == 0x6) // If type = 0x6 - Multiple Controller entry
                    {
                        return drom[mcOffset + 2]; // The data is in the 3rd byte of the entry
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Getting specific bits
        /// </summary>
        /// <param name="field">The field to get the bits from</param>
        /// <param name="offset">The offset (from the LSB)</param>
        /// <param name="size">Number of bits wanted</param>
        /// <returns>Value that was retrieved from the bits field</returns>
        private uint GetBitField(uint field, int offset, int size)
        {
            field = field >> offset;
            var mask = (1 << size) - 1;
            return (uint) (field & mask);
        }
    }
}