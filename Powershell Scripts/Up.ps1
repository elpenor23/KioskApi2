# Docker Daemon and Docker Desktop conflict with each other
# So we need to stop the Daemon reboot first then we can start 
# Docker Desktop for building the windows image 
$service = Get-Service -Name "docker" -ErrorAction SilentlyContinue
if ($service.Length -eq 1) {
	Set-Service -Name "docker" -StartupType manual
	Stop-Service -Name "docker"
}

# Start the Docker Desktop service
net start com.docker.service

# Start Docker Desktop
# This is needed to build the windows image
Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"

#Set location to the root of the project
#This will change for different users/computers
Set-Location C:\Users\Eric\Documents\KioskApi2

#Make sure we have the latest code
git pull

#It takes Docker Desktop a hot minute to load 
#so we need to wait for it
Start-Sleep -Seconds 10;

#Build the image
docker build --rm -t kioskapi2 .

#Save the image to a tar file
docker save kioskapi2:latest -o kiosk_api_image.tar

#Clear out all the images and containers
#NOTE: If you have other containers running
#you are going to want to comment all/most of this out
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
docker rmi -f $(docker images -aq)
docker system prune -f

#Kill Docker Desktop
$isrunning = Get-Process "Docker Desktop" -ErrorAction SilentlyContinue
if ($isrunning) {
  $isrunning.CloseMainWindow()
  $isrunning | Stop-Process -Force
}

#stop the Docker Desktop Service
#and make sure it does not start automatically
Set-Service -Name "com.docker.service" -Status running -StartupType manual
Stop-Service -Name "com.docker.service"

#Docker Desktop adds some stuff to daemon.json
#that does not play well with the Docker Engine serviec
#So we copy a fresh copy of that over to remove those changes
#if you have different setting in your daemon.json file, make those 
#in the daemon.json file in the repo first so you do not lose them.
Set-Location "C:\Users\Eric\Documents\KioskApi2\Powershell Scripts"
Copy-Item "daemon.json" -Destination "C:\ProgramData\docker\config"

#Ensure that dockerd is a service
$service = Get-Service -Name "docker" -ErrorAction SilentlyContinue
if ($service.Length -eq 0) {
	New-Service -Name "docker" -BinaryPathName "C:\Program Files\Docker\Docker\resources\dockerd"
}

#Make sure that the docker daemon service is running and 
#will automatically start 
Start-Service -Name "docker"
Set-Service -Name "docker" -Status running -StartupType automatic

#Load saved image into docker
Set-Location C:\Users\Eric\Documents\KioskApi2
docker load -i .\kiosk_api_image.tar

#Create and run a container that will restart automatically
#defaulting to using 8080 as the port. This can be configured differently
docker run --restart unless-stopped  -d -p 8080:8080 kioskapi2

#We are done. Bail.
Write-Output "Peace!"
Start-Sleep -Seconds 2;
Restart-Computer