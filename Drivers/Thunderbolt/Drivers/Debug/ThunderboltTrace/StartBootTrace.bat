@echo off
cls

SETLOCAL ENABLEDELAYEDEXPANSION

:: Check for Mandatory Label\High Mandatory Level
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( 
echo got admin
goto gotAdmin 
)

:UACPrompt
	echo prompting
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    set params = %*:"=""
    echo UAC.ShellExecute "%~s0", "%params%", "", "runas", 1 >> "%temp%\getadmin.vbs"
    "%temp%\getadmin.vbs"
    exit /B

:gotAdmin

CD /D %~dp0

For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c-%%a-%%b)
For /f "tokens=1-3 delims=/:" %%a in ("%TIME%") do (set mytime=%%a-%%b-%%c)

set SUFFIX_STRING=%1%
set SUFFIX_STRING=%SUFFIX_STRING: =_%

set ARGC=0
for %%x in (%*) do Set /A ARGC+=1

if not %ARGC%==0 set SUFFIX=_%SUFFIX_STRING%

set TRACE_DIR="TBT_BOOT_LOG_%mydate%_%mytime%%SUFFIX%"

if not exist %TRACE_DIR% mkdir %TRACE_DIR%

call DumpDriverInfo.bat %TRACE_DIR% begin_boot_

logman create trace autosession\TbtBootLogSession -o %TRACE_DIR%\TbtLog.etl -ets -pf wpp.guids

echo Don't forget to call StopBootTrace after the reboot (or the trace will continue running every boot).
