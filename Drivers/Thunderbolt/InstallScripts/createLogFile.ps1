$arr = $args[0] -split '@'
$logFileNameWithPath=$arr[0]
if (Test-Path $logFileNameWithPath -PathType Leaf) { Clear-Content $logFileNameWithPath}
$startTime = Get-Date
Add-Content $logFileNameWithPath -NoNewline "Time executed: "
$startTime | Add-Content  $logFileNameWithPath
Add-Content $logFileNameWithPath -NoNewline "Command line: "
Add-Content $logFileNameWithPath $arr[1]
Add-Content $logFileNameWithPath -NoNewline "Update version carried by package: "
Get-Content .\InstallScripts\version.txt | Add-Content $logFileNameWithPath
