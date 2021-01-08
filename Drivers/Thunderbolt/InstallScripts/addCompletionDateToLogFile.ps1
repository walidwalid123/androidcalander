$arr = $args[0] -split '@'
$logFileNameWithPath=$arr[0]
$logFilePath=Split-Path -Path $arr[0]
$completionTime = Get-Date
Add-Content $logFileNameWithPath -NoNewline "Time completed: "
$completionTime | Add-Content  $logFileNameWithPath
