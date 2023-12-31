Set-Service -Name "docker" -Status running -StartupType manual
Stop-Service -Name "docker"
Restart-Computer