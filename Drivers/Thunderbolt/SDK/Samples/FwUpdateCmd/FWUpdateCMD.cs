/*******************************************************************************
* Copyright (C) 2014 - 2017 Intel Corp. All rights reserved.
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
using System.Text;
using FwUpdateApiSample;

namespace FwUpdateCmd
{
    /// <summary>
    /// This is the main class for CLI logic.
    /// It parses the command and dispatches the appropriate class/function for handling it.
    /// </summary>
    internal static class FwUpdateCMD
    {
        /// <summary>
        /// Used also as the command name
        /// </summary>
        private enum Command
        {
            EnumControllers,
            EnumUpdatableDevices,
            GetTopology,
            GetCurrentNvmVersion,
            I2CRead,
            I2CWrite,
            GetTIPdInfo,
            GetControllerInfo,
            FWUpdate,
            Help,
        }

        /// <summary>
        /// Holds details for each command.
        /// </summary>
        private class CommandDetails
        {
            /// <summary>
            /// Function to run the command
            /// </summary>
            public Action<string[]> Handler { get; set; }

            /// <summary>
            /// Parameter list for the command.
            /// It's a list of lists to allow alternatives for a specific argument. See usage below
            /// for example.
            /// </summary>
            public List<List<Parameter>> Arguments { get; set; }
        }

        private enum Parameter
        {
            ControllerId,
            DeviceUUID,
            ImagePath,
            Port,
            Offset,
            Length,
            Data
        }

        private class ParameterDetails
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// This table is the main data structure that drives the CLI.
        /// It includes info for each command, what arguments it gets if at all, and how to run it.
        /// </summary>
        private static readonly Dictionary<Command, CommandDetails> Commands = new Dictionary<Command, CommandDetails>
        {
           { Command.EnumControllers     , new CommandDetails { Handler = args => new ControllerCommandRunner().EnumControllers() } },
           { Command.EnumUpdatableDevices, new CommandDetails { Handler = args => new DeviceCommandRunner().EnumUpdatableDevices() } },
           { Command.GetTopology         , new CommandDetails { Handler = args => new DeviceCommandRunner().GetTopology() } },
           { Command.GetCurrentNvmVersion, new CommandDetails { Handler = args => CreateCommandRunner(args[1]).GetCurrentNvmVersion(args), Arguments = new List<List<Parameter>> { new List<Parameter> { Parameter.ControllerId, Parameter.DeviceUUID } } } },
           { Command.I2CRead             , new CommandDetails { Handler = args => CreateCommandRunner(args[1]).I2CRead(args)             , Arguments = new List<List<Parameter>> { new List<Parameter> { Parameter.ControllerId, Parameter.DeviceUUID }, new List<Parameter> { Parameter.Port }, new List<Parameter> { Parameter.Offset }, new List<Parameter> { Parameter.Length } } } },
           { Command.I2CWrite            , new CommandDetails { Handler = args => CreateCommandRunner(args[1]).I2CWrite(args)            , Arguments = new List<List<Parameter>> { new List<Parameter> { Parameter.ControllerId, Parameter.DeviceUUID }, new List<Parameter> { Parameter.Port }, new List<Parameter> { Parameter.Offset }, new List<Parameter> { Parameter.Data } } } },
           { Command.GetTIPdInfo         , new CommandDetails { Handler = args => CreateCommandRunner(args[1]).GetTIPdInfo(args)         , Arguments = new List<List<Parameter>> { new List<Parameter> { Parameter.ControllerId, Parameter.DeviceUUID } } } },
           { Command.GetControllerInfo   , new CommandDetails { Handler = args => CreateCommandRunner(args[1]).GetControllerInfo(args)   , Arguments = new List<List<Parameter>> { new List<Parameter> { Parameter.ControllerId } } } },
           { Command.FWUpdate            , new CommandDetails { Handler = args => CreateCommandRunner(args[1]).FwUpdate(args)            , Arguments = new List<List<Parameter>> { new List<Parameter> { Parameter.ControllerId, Parameter.DeviceUUID }, new List<Parameter> { Parameter.ImagePath } } } },
           { Command.Help                , new CommandDetails { Handler = args => Help() } },
        };

        /// <summary>
        /// This table is a helper table for printing help about arguments (used in Help())
        /// </summary>
        private static readonly Dictionary<Parameter, ParameterDetails> Parameters = new Dictionary
            <Parameter, ParameterDetails>
        {
            { Parameter.ControllerId, new ParameterDetails { Name = "controller ID", Description = "controller ID as returned from " + Command.EnumControllers + " or " + Command.GetTopology + " commands"} },
            { Parameter.DeviceUUID  , new ParameterDetails { Name = "device UUID"  , Description = "device UUID as returned from " + Command.EnumUpdatableDevices + " or " + Command.GetTopology + " commands"} },
            { Parameter.ImagePath   , new ParameterDetails { Name = "imagePath"    , Description = "valid NVM image path" } },
            { Parameter.Port        , new ParameterDetails { Name = "port"         , Description = "port number (1 - based index)" } },
            { Parameter.Offset      , new ParameterDetails { Name = "offset"       , Description = "offset in I2C registers (in hex, with no prefix)" } },
            { Parameter.Length      , new ParameterDetails { Name = "length"       , Description = "length in bytes to read (decimal)" } },
            { Parameter.Data        , new ParameterDetails { Name = "data"         , Description = "data to write to an I2C register, in hex format without any delimiters or prefix e.g. AC0F0102. Number of digits must be even" } }
        };

        /// <summary>
        /// Prints user guide with help about the available commands, their arguments and the output
        /// format of informative commands
        /// </summary>
        private static void Help()
        {
            Console.WriteLine("FW Update CMD - allows user to update the controller firmware.");
            Console.WriteLine("                Prerequisites: Thunderbolt(TM) software should be fully ");
            Console.WriteLine("                installed and controller is powered.");
            Console.WriteLine();
            Console.WriteLine("This sample must run with Administrative privileges");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            foreach (var cmd in Enum.GetValues(typeof (Command)).Cast<Command>())
            {
                var builder = new StringBuilder("FwUpdateCmd ");
                builder.Append(cmd);

                if (Commands[cmd].Arguments != null)
                {
                    foreach (var argList in Commands[cmd].Arguments)
                    {
                        builder.Append(" <");
                        builder.Append(Parameters[argList.First()].Name);
                        foreach (var arg in argList.Skip(1))
                        {
                            builder.Append(" / ").Append(Parameters[arg].Name);
                        }
                        builder.Append(">");
                    }
                }

                Console.WriteLine(builder);
            }
            Console.WriteLine();
            Console.WriteLine("Parameters:");
            var parameterFieldSize = Parameters.Max(param => param.Value.Name.Length) + 1;
            foreach (var parameter in Parameters)
            {
                CmdUtilities.WriteDescription(parameter.Value.Name, parameter.Value.Description, parameterFieldSize);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Output formats:");
            Console.WriteLine();
            CmdUtilities.WriteDescription(Command.EnumControllers.ToString(),
                "Prints all the controller IDs line by line");
            Console.WriteLine();
            CmdUtilities.WriteDescription(Command.EnumUpdatableDevices.ToString(),
                "Prints each updatable device in a separated line in the following format: UUID VendorID ModelID ControllerNumber/NumberOfControllers");
            Console.WriteLine();
            CmdUtilities.WriteDescription(Command.GetTopology.ToString(),
                "Prints all connected devices in a hierarchical format grouped by controller and port, " +
                "ordered and numbered by position in the chain and includes UUID, vendor name, " +
                "model name, controller number and number of controllers in the device (formatted as X/N), " +
                "if it's updatable, NVM version and CIO link speed (in Gbps). Here is an example:");
            Console.WriteLine(
@"<ControllerID> <NVMVersion>
    Port 1:
        <position> <UUID> <VendorName> <ModelName> <ControllerNumber>/<NumberOfControllers> <Updatable> <NVMVersion> <LinkSpeed>
    Port 2:
        <position> <UUID> <VendorName> <ModelName> <ControllerNumber>/<NumberOfControllers> <Updatable> <NVMVersion> <LinkSpeed>
<ControllerID> <NVMVersion>
    Port 2:
        <position> <UUID> <VendorName> <ModelName> <ControllerNumber>/<NumberOfControllers> <Updatable> <NVMVersion> <LinkSpeed>");
            Console.WriteLine();
            CmdUtilities.WriteDescription(Command.I2CRead.ToString(),
                "Prints the content of an I2C register");
            Console.WriteLine();
            CmdUtilities.WriteDescription(Command.I2CWrite.ToString(),
                "Writes data into I2C register");
            Console.WriteLine();
            Console.WriteLine("The following is TI PD controller specific");
            CmdUtilities.WriteDescription(Command.GetTIPdInfo.ToString(),
                "Prints TI PD version, CUSTUSE and CustomerVersion registers");
            Console.WriteLine();
        }

        /// <summary>
        /// The main function of this class. Parses the command line arguments and runs the
        /// requested action.
        /// </summary>
        /// <param name="args">The arguments as comes from the command line</param>
        /// <exception>Throws an exception if something doesn't work</exception>
        public static void Start(string[] args)
        {
            var cmd = ValidateArgs(args);
            Commands[cmd].Handler(args);
        }

        /// <summary>
        /// Validates command line arguments including finding the supplied command.
        /// </summary>
        /// <param name="args">The arguments as comes from the command line</param>
        /// <returns>The found command</returns>
        /// <exception>
        /// Throws if command wasn't supplied or found or if wrong argument count were supplied
        /// </exception>
        private static Command ValidateArgs(string[] args)
        {
            try
            {
                if (!args.Any())
                {
                    throw new TbtException(TbtStatus.SDK_NO_COMMAND_SUPPLIED);
                }

                var cmd = FindCommand(args[0]);
                CheckArgCount(cmd, args);
                return cmd;
            }
            catch
            {
                Help();
                throw;
            }
        }

        /// <summary>
        /// Validates argument count given in command line vs. argument count for the specified
        /// command
        /// </summary>
        /// <param name="cmd">Command to check about</param>
        /// <param name="args">The arguments as comes from the command line</param>
        /// <exception>Throws in case of mismatch</exception>
        private static void CheckArgCount(Command cmd, string[] args)
        {
            var count = Commands[cmd].Arguments == null ? 0 : Commands[cmd].Arguments.Count;
            if (args.Length != (count + 1))
            {
                throw new TbtException(TbtStatus.SDK_ARGUMENT_COUNT_MISMATCH);
            }
        }

        /// <summary>
        /// Finds the command given in command line in the command table
        /// </summary>
        /// <param name="command">Command as comes from the command line</param>
        /// <returns>Command enum item to identify the command</returns>
        /// <exception>
        /// Throws if the command isn't found in the command table or there is an error in the
        /// table
        /// </exception>
        private static Command FindCommand(string command)
        {
            var c = from cmd in Commands
                where string.Equals(command, cmd.Key.ToString(), StringComparison.OrdinalIgnoreCase)
                select cmd.Key;

            var foundCommand = c.ToArray();

            if (!foundCommand.Any())
            {
                throw new TbtException(TbtStatus.SDK_COMMAND_NOT_FOUND);
            }

            // There must be only 1 entry, as we use the key for comparison and the key is unique
            // in a dictionary
            return foundCommand.First();
        }

        /// <summary>
        /// Factory function to create the correct command runner according to the argument given
        /// in command line
        /// </summary>
        /// <returns>The correct CommandRunner to run the command</returns>
        private static CommandRunner CreateCommandRunner(string argument)
        {
            if (IsUUID(argument))
            {
                return new DeviceCommandRunner();
            }
            return new ControllerCommandRunner();
        }

        /// <summary>
        /// Identify if a string represents a UUID (used here as device identifier)
        /// </summary>
        /// <param name="arg">String to check</param>
        /// <returns>True if the string represetns a UUID; otherwise - false</returns>
        private static bool IsUUID(string arg)
        {
            const string subpattern = @"[\da-fA-F]{8}";
            const string pattern = "^" + subpattern + ":" + subpattern + ":" + subpattern + ":" + subpattern + "$";
            return System.Text.RegularExpressions.Regex.IsMatch(arg, pattern);
        }
    }
}
