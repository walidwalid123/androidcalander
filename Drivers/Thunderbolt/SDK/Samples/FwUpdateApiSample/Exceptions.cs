/*******************************************************************************
* Copyright (C) 2016-2017 Intel Corp. All rights reserved.
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
using FwUpdateDriverApi;

namespace FwUpdateApiSample
{
    /// <summary>
    /// The exception type used almost everywhere in this SDK and samples. Supports uniform error
    /// message formating.
    /// </summary>
    /// <remarks>
    /// Message property returns both the "Error: {code} {enum entry string}" string and the long
    /// description given as a parameter to the c-tor or found in the message table below.
    ///
    /// To retrieve these parts separated, use ErrorMessage() with ErrorCode property to get the
    /// first component and TbtMessage property to get the second one.
    /// </remarks>
    public class TbtException : Exception
    {
        public TbtStatus ErrorCode { get; private set; }
        public string TbtMessage { get; private set; }

        public TbtException(TbtStatus code, string message)
            : base(ErrorMessage(code) + " " + message)
        {
            ErrorCode = code;
            TbtMessage = message;
        }

        public TbtException(TbtStatus code)
            : base(ErrorMessage(code) + (SdkStatusMessages.ContainsKey(code) ? " " + SdkStatusMessages[code] : ""))
        {
            ErrorCode = code;
            if (SdkStatusMessages.ContainsKey(code))
            {
                TbtMessage = SdkStatusMessages[code];
            }
        }

        internal TbtException(FwUpdateDriverApi.ErrorCode code) : this(MapFromDriverApiErrorCodeToTbtStatus[code])
        {
        }

        internal static readonly Dictionary<FwUpdateDriverApi.ErrorCode, TbtStatus> MapFromDriverApiErrorCodeToTbtStatus = new Dictionary<FwUpdateDriverApi.ErrorCode, TbtStatus>
        {
            { FwUpdateDriverApi.ErrorCode.SDK_DRIVER_COMMUNICATION_ERROR            , TbtStatus.SDK_DRIVER_COMMUNICATION_ERROR              },
            { FwUpdateDriverApi.ErrorCode.SDK_ERROR_DURING_IMAGE_UPDATE             , TbtStatus.SDK_ERROR_DURING_IMAGE_UPDATE               },
            { FwUpdateDriverApi.ErrorCode.SDK_NO_DRIVER                             , TbtStatus.SDK_NO_DRIVER                               },
            { FwUpdateDriverApi.ErrorCode.SDK_AUTHENTICATION_FAIL                   , TbtStatus.SDK_AUTHENTICATION_FAIL                     },
            { FwUpdateDriverApi.ErrorCode.SDK_FW_UPDATE_TIMEOUT                     , TbtStatus.SDK_FW_UPDATE_TIMEOUT                       },
            { FwUpdateDriverApi.ErrorCode.SDK_FW_UPDATE_MORE_THAN_ONE_HANDLE_OPEN   , TbtStatus.SDK_FW_UPDATE_MORE_THAN_ONE_HANDLE_OPEN     },
            { FwUpdateDriverApi.ErrorCode.SDK_DRIVER_API_UNKNOWN                    , TbtStatus.SDK_DRIVER_API_UNKNOWN                      },
        };

        private static readonly Dictionary<TbtStatus, string> SdkStatusMessages = new Dictionary<TbtStatus, string>
        {
            { TbtStatus.SDK_NO_COMMAND_SUPPLIED                     , Resources.NoCommandSupplied           },
            { TbtStatus.SDK_COMMAND_NOT_FOUND                       , Resources.CommandNotFound             },
            { TbtStatus.SDK_ARGUMENT_COUNT_MISMATCH                 , Resources.IncorrectArgumentCount      },
            { TbtStatus.SDK_INVALID_CONTROLLER_ID                   , Resources.InvalidControllerID         },
            { TbtStatus.SDK_INVALID_DEVICE_UUID                     , Resources.InvalidDeviceUUID           },
            { TbtStatus.SDK_FILE_NOT_FOUND                          , Resources.FileNotExists               },
            { TbtStatus.SDK_SERVICE_NOT_FOUND                       , Resources.ServiceDoesntExist          },
            { TbtStatus.SDK_LOAD_CONTROLLERS_ERROR                  , Resources.LoadControllersFailed       },
            { TbtStatus.SDK_LOAD_DEVICES_ERROR                      , Resources.LoadDevicesFailed           },
            { TbtStatus.SDK_INVALID_OPERATION_IN_SAFE_MODE          , Resources.SafeModeError               },
            { TbtStatus.SDK_DEVICE_NOT_SUPPORTED                    , Resources.DeviceNotSupported          },
            { TbtStatus.SDK_UNKNOWN_CHIP                            , Resources.UnknownChip                 },
            { TbtStatus.SDK_INVALID_IMAGE_FILE                      , Resources.InvalidImageFile            },
            { TbtStatus.SDK_HW_GENERATION_MISMATCH                  , Resources.IncompatibleHWGeneration    },
            { TbtStatus.SDK_PORT_COUNT_MISMATCH                     , Resources.IncompatiblePortCount       },
            { TbtStatus.SDK_CHIP_SIZE_ERROR                         , Resources.ChipSizeError               },
            { TbtStatus.SDK_IMAGE_FOR_HOST_ERROR                    , Resources.ImageForHostError           },
            { TbtStatus.SDK_IMAGE_FOR_DEVICE_ERROR                  , Resources.ImageForDeviceError         },
            { TbtStatus.SDK_PD_MISMATCH                             , Resources.PdMismatchError             },
            { TbtStatus.SDK_NO_DROM_IN_FILE_ERROR                   , Resources.NoDromInFileError           },
            { TbtStatus.SDK_VENDOR_MISMATCH                         , Resources.VendorMismatchError         },
            { TbtStatus.SDK_MODEL_MISMATCH                          , Resources.ModelMismatchError          },
            { TbtStatus.SDK_NO_MATCHING_DEVICE_FOUND                , Resources.NoMatchingDeviceFound       },
            { TbtStatus.SDK_MULTIPLE_IMAGES_FOUND                   , Resources.MultipleImagesFound         },
            { TbtStatus.SDK_COMMAND_IS_NOT_SUPPORTED_ON_DEVICE      , Resources.CommandNotSupportedOnDevice },
            { TbtStatus.SDK_DEPRECATED_METHOD                       , Resources.DeprecatedMethod            },
            { TbtStatus.SDK_INVALID_ARGUMENT                        , Resources.InvalidArgument             },
            { TbtStatus.SDK_NO_DROM_FOUND                           , Resources.DromNotFound                },
            { TbtStatus.SDK_NATIVE_MODE_MISMATCH                    , Resources.NativeModeMismatch          },
            { TbtStatus.SDK_DRIVER_COMMUNICATION_ERROR              , Resources.DriverCommunicationError    },
            { TbtStatus.SDK_ERROR_DURING_IMAGE_UPDATE               , Resources.ImageWriteError             },
            { TbtStatus.SDK_NO_DRIVER                               , Resources.SWNotInstalled              },
            { TbtStatus.SDK_AUTHENTICATION_FAIL                     , Resources.AuthFailed                  },
            { TbtStatus.SDK_FW_UPDATE_TIMEOUT                       , Resources.FWUpdateTimeout             },
            { TbtStatus.SDK_FW_UPDATE_MORE_THAN_ONE_HANDLE_OPEN     , Resources.MultiHandleOpenDuringFWUpdate},
            { TbtStatus.UNSUPPORTED_OPERATION                       , Resources.UnsuportedOperation},
            { TbtStatus.SDK_DRIVER_API_UNKNOWN                      , Resources.DriverAPIUnknown},
        };

        public static string ErrorMessage(TbtStatus code)
        {
            return string.Format("Error: 0x{0:X} {1}", (int) code, code);
        }
    }

    /// <summary>
    /// This file includes helper types for error reporting
    /// </summary>
    public enum TbtStatus : uint
    {
        SUCCESS_RESPONSE_CODE,

        // FW return codes
        AUTHENTICATION_FAILED_RESPONSE_CODE,
        ACCESS_TO_RESTRICTED_AREA_RESPONSE_CODE,
        GENERAL_ERROR_RESPONSE_CODE,
        AUTHENTICATION_IN_PROGRESS_RESPONSE_CODE,
        NO_KEY_FOR_THE_SPECIFIED_UID_RESPONSE_CODE,
        AUTHENTICATION_KEY_FAILED_RESPONSE_CODE,
        AUTHENTICATION_BONDED_UUID_FAILED_RESPONSE_CODE,
        SAFE_MODE_RESPONSE_CODE = 0x9,

        // SW return codes
        FW_RESPONSE_TIMEOUT_CODE = 0x100,
        WRONG_IMAGE_SIZE_CODE,
        SERVICE_INTERNAL_ERROR_CODE,
        POWER_CYCLE_FAILED_CODE,
        INVALID_OPERATION_IN_SAFE_MODE,
        NOT_SUPPORTED_PLATFORM,
        INVALID_ARGUMENTS,
        DEVICE_NOT_SUPPORTED,
        CONTROLLER_NOT_SUPPORTED,
        SDK_IN_USE,
        DEPRECATED_METHOD,
        I2C_ACCESS_NOT_SUPPORTED,
        I2C_ACCESS_UNSUPPORTED_LENGTH,
        UNKNOWN_HW,

        // SDK and samples return codes
        SDK_GENERAL_ERROR_CODE = 0x200,
        SDK_INTERNAL_ERROR,
        SDK_NO_COMMAND_SUPPLIED,
        SDK_COMMAND_NOT_FOUND,
        SDK_ARGUMENT_COUNT_MISMATCH,
        SDK_INVALID_CONTROLLER_ID,
        SDK_INVALID_DEVICE_UUID,
        SDK_FILE_NOT_FOUND,
        SDK_SERVICE_NOT_FOUND,
        SDK_LOAD_CONTROLLERS_ERROR,
        SDK_LOAD_DEVICES_ERROR,
        SDK_NO_CONTROLLERS,
        SDK_NO_DEVICES,
        SDK_INVALID_OPERATION_IN_SAFE_MODE,
        [Obsolete("Not used", true)] SDK_NO_EP_UPDATE_SUPPORT,
        SDK_DEVICE_NOT_SUPPORTED,
        [Obsolete("Not used", true)] SDK_REDWOOD_NOT_SUPPORTED,
        SDK_UNKNOWN_CHIP,
        SDK_INVALID_IMAGE_FILE,
        SDK_IMAGE_VALIDATION_ERROR,
        SDK_HW_GENERATION_MISMATCH,
        SDK_PORT_COUNT_MISMATCH,
        SDK_CHIP_SIZE_ERROR,
        SDK_IMAGE_FOR_HOST_ERROR,
        SDK_IMAGE_FOR_DEVICE_ERROR,
        SDK_PD_MISMATCH,
        SDK_NO_DROM_IN_FILE_ERROR,
        [Obsolete("Not used", true)] SDK_DROM_MISMATCH,
        SDK_VENDOR_MISMATCH,
        SDK_MODEL_MISMATCH,
        SDK_NO_MATCHING_DEVICE_FOUND,
        SDK_MULTIPLE_IMAGES_FOUND,
        SDK_COMMAND_IS_NOT_SUPPORTED_ON_DEVICE,
        SDK_DEPRECATED_METHOD,
        SDK_INVALID_ARGUMENT,
        SDK_NO_DROM_FOUND,
        SDK_NATIVE_MODE_MISMATCH,
        SDK_DRIVER_COMMUNICATION_ERROR,
        SDK_ERROR_DURING_IMAGE_UPDATE,
        SDK_NO_DRIVER,
        SDK_AUTHENTICATION_FAIL,
        SDK_FW_UPDATE_TIMEOUT,
        SDK_FW_UPDATE_MORE_THAN_ONE_HANDLE_OPEN,
        SDK_DRIVER_API_UNKNOWN,
        UNSUPPORTED_OPERATION
    };
}
