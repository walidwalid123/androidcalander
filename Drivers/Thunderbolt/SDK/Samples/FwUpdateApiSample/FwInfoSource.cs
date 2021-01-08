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

namespace FwUpdateApiSample
{
    // Be careful: Don't reorder the items here; some functions count on the
    // correct order here for comparing generations!
    public enum HwGeneration
    {
        /// <summary>Redwood Ridge - unsupported</summary>
        [Obsolete("Unsupported HW", true)] DSL4510_4410,

        /// <summary>Falcon Ridge - unsupported</summary>
        [Obsolete("Unsupported HW", true)] DSL5520_5320,

        /// <summary>Thunderbolt LP - Win Ridge - unsupported</summary>
        [Obsolete("Unsupported HW", true)] DSL5110,

        /// <summary>Alpine Ridge</summary>
        DSL6540_6340,

        /// <summary>Alpine Ridge LP</summary>
        JHL6240,

        /// <summary>Alpine Ridge C</summary>
        JHL6540_6340,

        /// <summary>Titan Ridge</summary>
        JHL7540_7440_7340,

        /// <summary>Goshen Ridge</summary>
        JHL8440,
    }

    public enum HwType
    {
        _2Ports,
        _1Port,
    }

    public class HwInfo
    {
        public HwGeneration Generation { get; set; }
        public HwType? Type { get; set; }
    }

    /// <summary>
    /// Strategy class tree for abstracting the actual info extracting (reading), which can come
    /// from a controller or from an image file, for FwInfo class tree
    ///
    /// See ControllerFwInfoSource and FileFwInfoSource classes for the concrete classes
    /// </summary>
    public abstract class FwInfoSource
    {
        public abstract byte[] Read(uint offset, uint length);
        public abstract uint DigitalSectionOffset();

        public HwInfo Info { get; protected set; }

        public static HwInfo HwConfiguration(UInt16 deviceId)
        {
            switch (deviceId)
            {
                // DSL6540_6340
                case 0x1577:
                case 0x1578:
                    return new HwInfo {Generation = HwGeneration.DSL6540_6340, Type = HwType._2Ports};
                case 0x1575:
                case 0x1576:
                    return new HwInfo {Generation = HwGeneration.DSL6540_6340, Type = HwType._1Port};
                case 0x15DD:
                    return new HwInfo {Generation = HwGeneration.DSL6540_6340};

                // JHL6240
                case 0x15BF:
                case 0x15C0:
                    return new HwInfo {Generation = HwGeneration.JHL6240, Type = HwType._1Port};
                case 0x15DC:
                    return new HwInfo {Generation = HwGeneration.JHL6240};

                // JHL6540_6340
                case 0x15D2:
                case 0x15D3:
                    return new HwInfo {Generation = HwGeneration.JHL6540_6340, Type = HwType._2Ports};
                case 0x15D9:
                case 0x15DA:
                    return new HwInfo {Generation = HwGeneration.JHL6540_6340, Type = HwType._1Port};
                case 0x15DE:
                    return new HwInfo {Generation = HwGeneration.JHL6540_6340};

                // JHL7540_7440_7340
                case 0x15EA:
                case 0x15EB:
                case 0x15EF:
                    return new HwInfo {Generation = HwGeneration.JHL7540_7440_7340, Type = HwType._2Ports};
                case 0x15E7:
                case 0x15E8:
                    return new HwInfo {Generation = HwGeneration.JHL7540_7440_7340, Type = HwType._1Port};
                
                // ICL
                case 0x8A0D:
                case 0x8A17:
                case 0x8A0F:
                    return new HwInfo { Generation = HwGeneration.JHL7540_7440_7340, Type = HwType._2Ports };   // TODO update generation for ICL if needed. Meantime use as for TR.

                case 0x0B26:
                    return new HwInfo { Generation = HwGeneration.JHL8440 };   

                default:
                    throw new TbtException(TbtStatus.SDK_UNKNOWN_CHIP);
            }
        }

        /// <summary>
        /// Reads the HW generation and type (port count) from FW (from controller or FW image)
        /// </summary>
        /// <returns>
        /// HWInfo object with generation and type (port count) info in regular case
        /// </returns>
        /// <exception>Throws in case of unknown device ID</exception>
        /// <remarks>
        /// We call it from each derived class c-tor, not from base class c-tor, because the use of
        /// abstract functions Read() and DigitalSectionOffset(), which may need the derived class
        /// initialized
        /// </remarks>
        protected HwInfo GetHwConfiguration()
        {
            var deviceId = new FwLocation {Offset = DigitalSectionOffset() + 0x5, Length = 2};
            var resultBuffer = Read(deviceId.Offset, deviceId.Length);
            return HwConfiguration(BitConverter.ToUInt16(resultBuffer, 0));
        }
    }
}
