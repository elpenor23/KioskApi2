# Docker Daemon and Docker Desktop conflict with each other
# So we need to stop the Daemon reboot first then we can start 
# Docker Desktop for building the windows image 
Set-Service -Name "docker" -Status running -StartupType manual
Stop-Service -Name "docker"
Restart-Computer