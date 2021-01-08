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
using System.Linq;
using FwUpdateApiSample;

namespace DeviceFWUTool
{
    /// <summary>
    /// This is the entry point of the tool. Most of the logic happens in DeviceUpdater
    /// class. The Main() controls only the high-level flow and printing errors
    /// and general messages.
    /// </summary>
    internal static class Program
    {
        private static int Main(string[] args)
        {
            FwUpdateParameters param = null;
            try
            {
                param = FwUpdateCMD.ParseArgs(args);
                if (param.HelpRequired)
                {
                    FwUpdateCMD.Help();
                }
                else
                {
                    var updater = new DeviceUpdater();
                    updater.FwUpdate(param);
                }
                return 0;
            }
            catch (TbtException e)
            {
                CmdUtilities.WriteDescription(TbtException.ErrorMessage(e.ErrorCode), e.TbtMessage);
                return (int) e.ErrorCode;
            }
            catch (Exception e)
            {
                const TbtStatus errorCode = TbtStatus.SDK_GENERAL_ERROR_CODE;
                CmdUtilities.WriteDescription(TbtException.ErrorMessage(errorCode),
                    e.Message.Any() ? e.Message : e.HResult.ToString());
                return (int) errorCode;
            }
            finally
            {
                if (param != null && !param.Quiet)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
