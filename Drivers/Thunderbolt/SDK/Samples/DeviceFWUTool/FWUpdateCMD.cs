/*******************************************************************************
* Copyright (C) 2016 Intel Corp. All rights reserved.
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
using System.IO;
using FwUpdateApiSample;

namespace DeviceFWUTool
{
    /// <summary>
    /// This is a utility class for command-line handling, e.g. argument parsing
    /// and help printing
    /// </summary>
    internal static class FwUpdateCMD
    {
        internal static void Help()
        {
            Console.WriteLine("Device FW Update Tool - allows user to update the device firmware.");
            Console.WriteLine("Usage:");
            Console.WriteLine("DeviceFWUTool.exe [image_path] [options]");
            Console.WriteLine("    options:");
            Console.WriteLine("         /q - quiet (update all compatible devices without asking)");
            Console.WriteLine("         /h - displays help page ");
        }

        internal static FwUpdateParameters ParseArgs(string[] args)
        {
            var param = new FwUpdateParameters();
            try
            {
                foreach (var arg in args)
                {
                    switch (arg)
                    {
                        case "/h":
                            param.HelpRequired = true;
                            break;
                        case "/q":
                            param.Quiet = true;
                            break;
                        default:
                            var extension = Path.GetExtension(arg);
                            if (extension != null && extension.Equals(".bin", StringComparison.OrdinalIgnoreCase))
                            {
                                if (param.IsImageGiven)
                                {
                                    throw new TbtException(TbtStatus.SDK_MULTIPLE_IMAGES_FOUND);
                                }
                                param.IsImageGiven = true;
                                param.ImagePath = arg;
                            }
                            else
                            {
                                throw new TbtException(arg.StartsWith("/")
                                    ? TbtStatus.SDK_COMMAND_NOT_FOUND
                                    : TbtStatus.SDK_FILE_NOT_FOUND);
                            }
                            break;
                    }
                }
                return param;
            }
            catch
            {
                Help();
                throw;
            }
        }
    }

    internal class FwUpdateParameters
    {
        public bool Quiet { get; set; }
        public bool IsImageGiven { get; set; }
        public string ImagePath { get; set; }
        public bool HelpRequired { get; set; }
    }
}
