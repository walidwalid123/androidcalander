try{
$ErrorActionPreference = "Stop"; #Make all errors terminating
Start-Transcript -path TBT_Install.log -force

$targetdirectory=$args[0] + "\production\Windows10-x64"

$sourcedirectory1 = ".\Drivers\ThunderboltWindowsDchSetup"
$sourcedirectory2 = ".\Drivers\ThunderboltApplicationLauncherSetup"
 
if(Test-Path -Path $targetdirectory ){
Remove-Item $targetdirectory -Force -Recurse
}
New-Item $targetdirectory -Type Directory
Copy-Item -Path $sourcedirectory1\*.* -Destination $targetdirectory
Copy-Item -Path $sourcedirectory2\*.* -Destination $targetdirectory
Stop-Transcript
} catch {
exit 1
}
exit 0