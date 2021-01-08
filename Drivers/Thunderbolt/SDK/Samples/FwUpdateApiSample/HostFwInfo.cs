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
    /// This class is in use by SdkTbtController to get information about sections in FW for hosts,
    /// both for a "live" FW (in a controller) and for files
    /// </summary>
    /// <remarks>For more info, see base class documentation</remarks>
    internal class HostFwInfo : FwInfo
    {
        public HostFwInfo(FwInfoSource source)
            : base(source)
        {
        }

        [Flags]
        private enum SectionReadBit
        {
            DPIN  = 1 << 0,
            DPOUT = 1 << 1,
            CP    = 1 << 2,
            ARC   = 1 << 3,
            LC    = 1 << 4,
            IRAM  = 1 << 5,
            DRAM  = 1 << 6,
        }

        public override Dictionary<Sections, SectionDetails> GetSectionInfo()
        {
            var dict = base.GetSectionInfo();

            switch (Source.Info.Generation)
            {
                case HwGeneration.DSL6540_6340:
                case HwGeneration.JHL6240:
                case HwGeneration.JHL6540_6340:
                case HwGeneration.JHL7540_7440_7340:
                {
                    /*
                     * Algorithm:
                     * To find the DRAM section, we have to jump from section to section in a
                     * chain of sections.
                     * readUcodeSections location tells what sections exist at all (with flags
                     * as defined in SectionReadBit enum).
                     * ee_ucode_start_addr location tells the offset of the first section in
                     * the list according to the digital section start.
                     * After having the offset of the first section, we have a loop over the
                     * section list. If the section exists, we read its length (2 bytes at
                     * section start) and add it to current offset to find the start of the
                     * next section. Otherwise, we already have the next section offset at hand...
                     */

                    var readUcodeSections = new FwLocation {Offset = dict[Sections.Digital].Offset + 0x2, Length = 1};
                    var sectionRead = (SectionReadBit) Source.Read(readUcodeSections.Offset, readUcodeSections.Length)[0];
                    Logger.Instance.LogInfo($"sectionRead : {sectionRead}");
                    if (sectionRead.HasFlag(SectionReadBit.DRAM))
                    {
                        var sectionList = new[]
                        {
                            new {name = "CP"    , flag = SectionReadBit.CP   },
                            new {name = "HDPOut", flag = SectionReadBit.DPOUT},
                            new {name = "HDPIn" , flag = SectionReadBit.DPIN },
                            new {name = "LC"    , flag = SectionReadBit.LC   },
                            new {name = "ARC"   , flag = SectionReadBit.ARC  },
                            new {name = "IRAM"  , flag = SectionReadBit.IRAM },
                        };

                        var ee_ucode_start_addr = new FwLocation
                        {
                            Offset = dict[Sections.Digital].Offset + 0x3,
                            Length = 2
                        };
                        var offset
                            = dict[Sections.Digital].Offset
                              + BitConverter.ToUInt16(Source.Read(ee_ucode_start_addr.Offset,
                                  ee_ucode_start_addr.Length),
                                  0);
                        Logger.Instance.LogInfo($"starting from offset {offset}");
                        foreach (var section in sectionList)
                        {
                            if (sectionRead.HasFlag(section.flag))
                            {
                                var length = SectionSize16BitDw(offset);
                                Logger.Instance.LogInfo($"adding length of {section.name} {length}. New offset is {offset + length}");
                                offset += length;
                            }
                        }

                        dict[Sections.DRAMUCode] = new SectionDetails {Offset = offset};
                        dict[Sections.DRAMUCode].Length = SectionSize16BitDw(dict[Sections.DRAMUCode].Offset);
                        Logger.Instance.LogInfo($"DRAMUCode offset {offset}, length {dict[Sections.DRAMUCode].Length}");
                        }
                }
                break;

                default:
                    throw new TbtException(TbtStatus.SDK_UNKNOWN_CHIP);
            }
            return dict;
        }

        public bool OsNativePciEnumeration
        {
            get { return (Source.Read(base.GetSectionInfo()[Sections.Digital].Offset + 0x7b, 1)[0] & (1 << 5)) != 0; }
        }
    }
}
