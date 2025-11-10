# DO NOT USE OLD AND OBSOLETE

# #Make sure that the docker daemon service is running and 
# #will automatically start 
# Start-Service -Name "docker"
# Set-Service -Name "docker" -Status running -StartupType automatic

# #Load out saved image into docker
# Set-Location C:\Users\Eric\Documents\KioskApi2
# docker load -i .\kiosk_api_image.tar

# #Create and run a container that will restart automatically
# #defaulting to using 8080 as the port. This can be configured differently
# docker run --restart unless-stopped  -d -p 8080:8080 kioskapi2

# #We are done. Bail.
# logoff