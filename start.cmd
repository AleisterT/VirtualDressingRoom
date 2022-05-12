"C:\Windows/System32/ping.exe" 127.0.0.1 -n 6 > nul

::powershell -noexit "%~dp0\ChangeOrientation.ps1"

::powershell.exe -noexit "& '%~dp0ChangeOrientation.ps1'"

"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe" -ExecutionPolicy Bypass -File "%~dp0ChangeOrientation.ps1"

"C:\Windows/System32/ping.exe" 127.0.0.1 -n 6 > nul

"Build/Wirtualne lustro.exe"