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
    /// This class is used by SdkTbtController class to implement the FW image file validation.
    /// Includes validation information specific to host controller checks.
    /// </summary>
    internal class HostImageValidator : ImageValidator
    {
        internal HostImageValidator(SdkTbtBase controller,
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
            isHost.ErrorCode = TbtStatus.SDK_IMAGE_FOR_DEVICE_ERROR;
            var checkLocations = new List<CheckLocation> {isHost};

            // Add to the returned list only the check locations that are relevant for current
            // controller, according to its type
            checkLocations.AddRange(
                GetHostCheckLocations(generation).Where(val => val.Type == HwType._1Port || val.Type == HwInfo.Type));

            return checkLocations;
        }

        private class HostCheckLocation : CheckLocation
        {
            // In this context, 1Port type means that it's relevant for both 2C *and* 4C controllers
            // while 2Ports means that it's relevant only for 4C controllers.
            public HwType Type { get; private set; }

            public HostCheckLocation(HwType type,
                uint offset,
                uint length,
                string description,
                byte mask = FullMask,
                Sections section = Sections.Digital)
                : base(offset, length, mask, section, description: description)
            {
                Type = type;
            }
        }

        /// <summary>
        /// This function includes the tables with locations in FW to compare to make sure the FW image file
        /// is compatible with the controller.
        /// The list of this locations comes from the release notes of the FW (aka NVM) and it's compatible with the
        /// current latest revision for each controller generation.
        /// </summary>
        /// <param name="gen">The controller generation</param>
        /// <returns>Table with locations to check for image compatibility with the controller</returns>
        private static IEnumerable<HostCheckLocation> GetHostCheckLocations(HwGeneration gen)
        {
            switch (gen)
            {
                case HwGeneration.DSL6540_6340: // B0, release notes rev1, lane swap configuration rev2
                case HwGeneration.JHL6540_6340: // C0, release notes rev0, lane swap configuration rev3
                    return new[]
                    {
                        #region DSL6540_6340

                        new HostCheckLocation(type: HwType._1Port,  offset: 0x5,   length: 2, description: "Device ID"     ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x10,  length: 4, description: "PCIe Settings" ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x12,  length: 1, description: "PA",           mask: Convert.ToByte("11001100", 2), section: Sections.DRAMUCode),
                        new HostCheckLocation(type: HwType._2Ports, offset: 0x13,  length: 1, description: "PB",           mask: Convert.ToByte("11001100", 2), section: Sections.DRAMUCode),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x121, length: 1, description: "Snk0"          ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x129, length: 1, description: "Snk1"          ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x136, length: 1, description: "Src0",         mask: 0xF0),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0xB6,  length: 1, description: "PA/PB (USB2)", mask: Convert.ToByte("11000000", 2)),

                        #endregion
                    };
                case HwGeneration.JHL6240: // A0, release notes rev1, lane swap configuration rev1
                    return new[]
                    {
                        #region JHL6240

                        new HostCheckLocation(type: HwType._1Port, offset: 0x5,   length: 2, description: "Device ID"     ),
                        new HostCheckLocation(type: HwType._1Port, offset: 0x10,  length: 4, description: "PCIe Settings" ),
                        new HostCheckLocation(type: HwType._1Port, offset: 0x12,  length: 1, description: "PA",           mask: Convert.ToByte("11001100", 2), section: Sections.DRAMUCode),
                        new HostCheckLocation(type: HwType._1Port, offset: 0x13,  length: 1, description: "PB",           mask: Convert.ToByte("01000100", 2), section: Sections.DRAMUCode),
                        new HostCheckLocation(type: HwType._1Port, offset: 0x121, length: 1, description: "Snk0"          ),
                        new HostCheckLocation(type: HwType._1Port, offset: 0xB6,  length: 1, description: "PA/PB (USB2)", mask: Convert.ToByte("11000000", 2)),

                        #endregion
                    };
                case HwGeneration.JHL7540_7440_7340: // A0, release notes rev4, lane swap configuration rev2
                    return new[]
                    {
                        #region JHL7540_7440_7340

                        new HostCheckLocation(type: HwType._1Port,  offset: 0x5,   length: 2, description: "Device ID"     ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x10,  length: 4, description: "PCIe Settings" ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x12,  length: 1, description: "PA",           mask: Convert.ToByte("11001100", 2), section: Sections.DRAMUCode),
                        new HostCheckLocation(type: HwType._2Ports, offset: 0x13,  length: 1, description: "PB",           mask: Convert.ToByte("11001100", 2), section: Sections.DRAMUCode),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x121, length: 1, description: "Snk0"          ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x129, length: 1, description: "Snk1"          ),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x136, length: 1, description: "Src0",         mask: 0xF0),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0xB6,  length: 1, description: "PA/PB (USB2)", mask: Convert.ToByte("11000000", 2)),
                        new HostCheckLocation(type: HwType._1Port,  offset: 0x5E,  length: 1, description: "Aux",          mask: 0x0F),
                        new HostCheckLocation(type: HwType._2Ports, offset: 0x5E,  length: 1, description: "Aux (PB)",     mask: 0x10),

                        #endregion
                    };
                default:
                    throw new TbtException(TbtStatus.SDK_UNKNOWN_CHIP);
            }
        }
    }
}
