@echo off
SETLOCAL EnableDelayedExpansion
set EXTRACTDRIVERS=%2
set LOGFILE=%4

rem CREATE LOG FILE
SET ThisScriptsDirectory=%~dp0
SET PowerShellScriptPath=%ThisScriptsDirectory%\InstallScripts\createLogFile.ps1
PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%PowerShellScriptPath%' "%LOGFILE%@%~0 "%*""";
IF ERRORLEVEL 1 GOTO ERROR

if "%~2"=="" (
GOTO DETECT_PRV_DRV
)
IF NOT ("%~2"=="") (
SET ExtractDriversScriptPath=%ThisScriptsDirectory%\InstallScripts\extractDrivers.ps1
PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '!ExtractDriversScriptPath!' "%EXTRACTDRIVERS%"";
IF ERRORLEVEL 1 GOTO ERROR
ECHO Extract drivers finished successfully. >> %LOGFILE%
GOTO EXIT
)

:DETECT_PRV_DRV
call .\InstallScripts\runDetectPrvDrv.bat >> %LOGFILE%
set retVal=0
IF %ERRORLEVEL%==0 ( 
	rem No previous driver, new driver can be installed.
	GOTO INSTALL_DRIVERS
) ELSE IF %ERRORLEVEL%==1 (
	rem Installation driver version is greater than installed driver version, installation will proceed.
	GOTO INSTALL_DRIVERS
) ELSE IF %ERRORLEVEL%==2 (
	rem Installation driver version equals to installed driver version, driver installation will be skipped.
	ECHO Thunderbolt installation finished successfully. >> %LOGFILE%
	GOTO EXIT
) ELSE (
	rem Installed driver version is greater than Installation driver version. Driver installation will be skipped.
	set retVal=2
	ECHO Thunderbolt installation finished successfully. >> %LOGFILE%
	GOTO EXIT
)

:INSTALL_DRIVERS
echo Installation in progress... >> %LOGFILE%
pnputil -i -a .\Drivers\ThunderboltWindowsDchSetup\TbtHostController.inf >> %logFile%	
IF ERRORLEVEL 1 GOTO ERROR
pnputil -i -a .\Drivers\ThunderboltApplicationLauncherSetup\TbtHostControllerExtension.inf >> %logFile%
IF ERRORLEVEL 1 GOTO ERROR

:EXIT
rem ADD COMPLETION TIME TO THE LOG FILE
SET ThisScriptsDirectory=%~dp0
SET PowerShellScriptPath=%ThisScriptsDirectory%\InstallScripts\addCompletionDateToLogFile.ps1
PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%PowerShellScriptPath%' "%LOGFILE%@%~0 "%*""";
exit /b %retVal% 

:ERROR 
ECHO Installation failed. >> %LOGFILE%
ECHO Try uninstall and then install again. >> %LOGFILE%
rem ADD COMPLETION TIME TO THE LOG FILE
SET ThisScriptsDirectory=%~dp0
SET PowerShellScriptPath=%ThisScriptsDirectory%\InstallScripts\addCompletionDateToLogFile.ps1
PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%PowerShellScriptPath%' "%LOGFILE%@%~0 "%*""";
exit /b 1


