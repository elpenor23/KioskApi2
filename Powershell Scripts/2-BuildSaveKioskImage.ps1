net start com.docker.service
Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"

Set-Location C:\Users\Eric\Documents\KioskApi2
git pull

Start-Sleep -Seconds 10;

docker build -t kioskapi2 .
docker save kioskapi2:latest -o kiosk_api_image.tar

docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
docker rmi -f $(docker images -aq)
docker system prune -f

$isrunning = Get-Process "Docker Desktop" -ErrorAction SilentlyContinue
if ($isrunning) {
  $isrunning.CloseMainWindow()
  $isrunning | Stop-Process -Force
}

Set-Service -Name "com.docker.service" -Status running -StartupType manual
Stop-Service -Name "com.docker.service"

Set-Location "C:\Users\Eric\Documents\KioskApi2\Powershell Scripts"
Copy-Item "daemon.json" -Destination "C:\ProgramData\docker\config"

$service = Get-Service -Name "docker" -ErrorAction SilentlyContinue
if ($service.Length -eq 0) {
	New-Service -Name "docker" -BinaryPathName "C:\Program Files\Docker\Docker\resources\dockerd"
}

Restart-Computer