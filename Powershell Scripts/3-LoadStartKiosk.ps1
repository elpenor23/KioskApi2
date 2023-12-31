Start-Service -Name "docker"
Set-Service -Name "docker" -Status running -StartupType automatic

Set-Location C:\Users\Eric\Documents\KioskApi2
docker load -i .\kiosk_api_image.tar
docker run --restart unless-stopped  -d -p 8080:8080 kioskapi2
logoff