﻿/*******************************************************************************
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

namespace FwUpdateCmd
{
    internal static class CmdUtilities
    {
        /// <summary>
        /// Helper function for writing long description in help to console
        /// </summary>
        /// <param name="key">String to print first, without indentation; assumed to be short</param>
        /// <param name="description">
        /// String (possible long one) to print after the key, with indentation and line wrapping
        /// </param>
        /// <param name="keyFieldWidth">
        /// Optional indentation width.
        /// Without it, the length of key argument + separator is used for indentation.
        /// </param>
        public static void WriteDescription(string key, string description, int? keyFieldWidth = null)
        {
            const string separator = "- ";
            var width = keyFieldWidth ?? key.Length + 1;
            Console.Write("{0," + -width + "}{1}", key, separator);
            var indentation = width + separator.Length;
            WriteWrappedLine(description, indentation);
        }

        /// <summary>
        /// Helper function for writing long lines, possibly with indentations
        /// Based on http://stackoverflow.com/a/29689349
        /// </summary>
        /// <param name="message">String to write</param>
        /// <param name="indentation">Indentation to use for each line besides the first one</param>
        public static void WriteWrappedLine(string message, int indentation = 0)
        {
            var maxWidth = Console.WindowWidth - indentation - 1;

            var words = message.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);
            var lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (list, word) =>
            {
                if (list.Last().Length + word.Length >= maxWidth)
                {
                    list.Add(word);
                }
                else
                {
                    list[list.Count - 1] += ' ' + word;
                }
                return list;
            });

            Console.WriteLine(lines[0]);
            var format = "{0," + indentation + "}{1}";
            foreach (var line in lines.Skip(1))
            {
                Console.WriteLine(format, null, line);
            }
        }
    }
}