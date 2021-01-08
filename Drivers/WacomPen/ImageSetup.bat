@echo off 
echo Wacom Components Driver Image Install Script 
set RS23=0
set Win78=0

ver | find "Version 6." > nul  && set Win78=1
if %Win78%==1 (
	echo This script is for Win10 RS2 and above, use Setup.exe for previous Operating systems
	GOTO Exit
)
IF "%1"=="yetifix" (
sc config EinkSvr start=disabled
net stop EinkSvr
 ) 


ver | find "10.0.16" > nul  && set RS23=1
ver | find "10.0.15" > nul  && set RS23=1
reg Query "HKLM\Hardware\Description\System\CentralProcessor\0" | find /i "x86" > NUL && set OSbits=32 || set OSbits=64
IF "%1"=="apponly" GOTO AppOnly 

 echo Install Wacom Components Kernel Driver 
PnpUtil.exe /add-driver %~dp0%OSbits%\WacHIDRouterISDU.inf /install 

 echo Install Wacom Components Service 
PnpUtil.exe /add-driver %~dp0%OSbits%\WTabletServiceISD.inf /install 

IF "%1"=="yetifix" (
sc config EinkSvr start=auto
net start EinkSvr
) 


 :AppOnly
echo Installing Wacom Settings App Package
:: Install Wacom Components Settings App 
if %RS23%==1 ( 
	if %OSbits%==32 ( 
		Dism /Online /Add-ProvisionedAppxPackage /PackagePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9.appxbundle"  /DependencyPackagePath:"%~dp0App\Microsoft.VCLibs.140.00.UWPDesktop_14.0.27323.0_x86__8wekyb3d8bbwe.appx" /LicensePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9_License1.xml"
	) 
	if %OSbits%==64 ( 
		Dism /Online /Add-ProvisionedAppxPackage /PackagePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9.appxbundle"  /DependencyPackagePath:"%~dp0App\Microsoft.VCLibs.140.00.UWPDesktop_14.0.27323.0_x64__8wekyb3d8bbwe.appx" /LicensePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9_License1.xml"
	) 
) 
if %RS23%==0 ( 
	if %OSbits%==32 ( 
		Dism /Online /Add-ProvisionedAppxPackage /PackagePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9.appxbundle"  /DependencyPackagePath:"%~dp0App\Microsoft.VCLibs.140.00.UWPDesktop_14.0.27323.0_x86__8wekyb3d8bbwe.appx" /LicensePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9_License1.xml" /Region:All
	) 
	if %OSbits%==64 ( 
		Dism /Online /Add-ProvisionedAppxPackage /PackagePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9.appxbundle"  /DependencyPackagePath:"%~dp0App\Microsoft.VCLibs.140.00.UWPDesktop_14.0.27323.0_x64__8wekyb3d8bbwe.appx" /LicensePath:"%~dp0App\d0351f161e3c4990a40e14b3935ab8a9_License1.xml" /Region:All
	) 
) 


 :Exit
  
