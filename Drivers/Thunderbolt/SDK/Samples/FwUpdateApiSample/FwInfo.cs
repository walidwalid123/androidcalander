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
    public enum Sections
    {
        Digital, // (Active region)
        DROM,
        DRAMUCode,
        ArcParams,
        Ee2TarDma,
        Pd
    }

    public class FwLocation
    {
        public UInt32 Offset { get; set; }
        public UInt32 Length { get; set; }
    }

    /// <summary>
    /// This class, with its derived classes, is in use by SdkTbtBase to get information about
    /// sections in FW for hosts and devices, both for a "live" FW (in a controller) and for files
    /// </summary>
    /// <remarks>
    /// This base class includes the logic for info which is common to host and device controllers.
    /// For actually getting the info, use HostFwInfo or DeviceFwInfo classes.
    ///
    /// For abstracting the actual info extracting (reading), which can come from a controller or
    /// from an image file, we use Strategy design pattern, using FwInfoSource class tree.
    /// </remarks>
    internal abstract class FwInfo
    {
        protected FwInfoSource Source { get; private set; }

        protected FwInfo(FwInfoSource source)
        {
            Source = source;
        }

        public HwInfo Info
        {
            get { return Source.Info; }
        }

        public class SectionDetails
        {
            public uint Offset { get; set; }
            public uint Length { get; set; }
        }

        private const uint DW = sizeof (UInt32);

        /// <summary>
        /// This function is the main interface of this class and returns the section info according
        /// to the HW generation
        /// </summary>
        /// <exception>Throws in case of null or unknown HW info or when no DROM is found</exception>
        /// <returns>
        /// Dictionary of section info about all the sections we need for image file validation
        /// </returns>
        public virtual Dictionary<Sections, SectionDetails> GetSectionInfo()
        {
            if (Source.Info == null)
            {
                throw new TbtException(TbtStatus.SDK_INTERNAL_ERROR, Resources.NoHWInfo);
            }

            var dict = new Dictionary<Sections, SectionDetails>();

            dict[Sections.Digital] = new SectionDetails {Offset = Source.DigitalSectionOffset()};
            dict[Sections.Digital].Length = SectionSize16Bit(dict[Sections.Digital].Offset);

            var arc_params_offset = new FwLocation {Offset = 0x0075, Length = 4};
            var arcStartOffset = BitConverter.ToUInt32(
                Source.Read(dict[Sections.Digital].Offset + arc_params_offset.Offset, arc_params_offset.Length), 0);

            dict[Sections.ArcParams] = new SectionDetails {Offset = dict[Sections.Digital].Offset + arcStartOffset};
            dict[Sections.ArcParams].Length = BitConverter.ToUInt32(Source.Read(dict[Sections.ArcParams].Offset, 4), 0)*
                                              DW;

            var eeDmaPointer = new FwLocation {Offset = 0x00C7, Length = 3};
            var eeDmaInfo = Source.Read(dict[Sections.Digital].Offset + eeDmaPointer.Offset, eeDmaPointer.Length);
            Array.Resize(ref eeDmaInfo, sizeof(UInt32));
            var eeDmaStrartOffset = BitConverter.ToUInt32(eeDmaInfo, 0);

            dict[Sections.Ee2TarDma] = new SectionDetails {Offset = dict[Sections.Digital].Offset + eeDmaStrartOffset};

            var pdPointer =
                BitConverter.ToUInt32(
                    Source.Read(
                        (uint)
                        (dict[Sections.ArcParams].Offset + (Info.Generation < HwGeneration.JHL7540_7440_7340 ? 0x10c : 0x14c)),
                        4), 0);
            if (Utilities.ValidPointer(pdPointer, 4))
            {
                dict[Sections.Pd] = new SectionDetails {Offset = pdPointer + dict[Sections.Digital].Offset};
            }

            var dromOffset = new FwLocation {Offset = 0x010E, Length = 4};
            var dromStartOffset = BitConverter.ToUInt32(
                Source.Read(dict[Sections.Digital].Offset + dromOffset.Offset, dromOffset.Length), 0);

            if (dromStartOffset == 0)
            {
                throw new TbtException(TbtStatus.SDK_NO_DROM_FOUND);
            }

            dict[Sections.DROM] = new SectionDetails {Offset = dict[Sections.Digital].Offset + dromStartOffset};

            // DROM length isn't implemented yet and should be ignored for now
            dict[Sections.DROM].Length = BitConverter.ToUInt32(Source.Read(dict[Sections.DROM].Offset, 4), 0)*DW;

            return dict;
        }

        private const uint SizeOfSizeField = sizeof (UInt16);

        /// <summary>
        /// Gets section size by reading from given offset, assuming the size field is 16-bit long,
        /// doesn't include the size field itself and the size field is in bytes
        /// </summary>
        /// <param name="sectionOffset">Offset to read from</param>
        /// <returns>Section size (in bytes) as read from FW + 2 for the size field itself</returns>
        private uint SectionSize16Bit(uint sectionOffset)
        {
            var buffer = Source.Read(sectionOffset, SizeOfSizeField);
            // The "+2" part is because the size doesn't include the size field itself
            return BitConverter.ToUInt16(buffer, 0) + SizeOfSizeField;
        }

        /// <summary>
        /// Gets section size by reading from given offset, assuming the size field is 16-bit long,
        /// doesn't include the size field itself and the size field is in DWORD (4-bytes) units
        /// </summary>
        /// <param name="sectionOffset">Offset to read from</param>
        /// <returns>Section size (in bytes) as read from FW + 2 for the size field itself</returns>
        protected uint SectionSize16BitDw(uint sectionOffset)
        {
            return (SectionSize16Bit(sectionOffset) - SizeOfSizeField)*DW + SizeOfSizeField;
        }
    }
}
