/*******************************************************************************
* Copyright (C) 2015 Intel Corp. All rights reserved.
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

namespace FwUpdateApiSample
{
    /// <summary>
    /// See base class documentation
    /// </summary>
    internal class FileFwInfoSource : FwInfoSource
    {
        private readonly byte[] _image;

        public FileFwInfoSource(byte[] image)
        {
            _image = image;
            Info = GetHwConfiguration();
        }

        public override byte[] Read(uint offset, uint length)
        {
            return new ArraySegment<byte>(_image, (int) offset, (int) length).ToArray();
        }

        private const int FarbSize = 3;

        public override uint DigitalSectionOffset()
        {
            var farbAddresses = new uint[] {0, 0x1000};
            foreach (var farbAddress in farbAddresses)
            {
                var farb = GetFarb(farbAddress);
                if (Utilities.ValidPointer(farb, FarbSize))
                {
                    Logger.Instance.LogInfo($"Digital section offset is {farb}");
                    return farb;
                }
            }
            throw new TbtException(TbtStatus.SDK_INVALID_IMAGE_FILE);
        }

        private uint GetFarb(uint offset)
        {
            var farb = Read(offset, FarbSize);
            Array.Resize(ref farb, sizeof (UInt32));
            return BitConverter.ToUInt32(farb, 0);
        }
    }
}
