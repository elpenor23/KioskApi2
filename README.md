# API for Live Information Kiosk
This API implements the following
 - Loads weather from Open Weather Map and does some pre-formatting
 - returns the weather info
 - Calculates clothing needed to run based on passed in data and weather 
   and returns it in a list based on the list passed in.
 - Calculates the phase of the moon
 - Calculates the hours of daylight
 - Calculates time of sunrise and sunset
 - There is a version number but it is totally manual so means mostly nothing
   but is useful if you want to test to make sure your latest version is running
 - Images to be used with the API live in the [Live-Information-Kiosk/kiosk/assets](https://github.com/elpenor23/Live-Information-Kiosk/tree/master/kiosk/assets) folder.

# Docker Build
Included are scripts to build and load this on Docker on a windows computer with Docker Desktop installed.
 1) Read Powershell scripts carefully. They are well documented. 
    - **_They will reboot or log you off automatically!_**
    - if you dont want that comment out those lines!
 3) Update paths in powershell scripts
    - I commited mine because I am too lazy to get nant or something like that working.
 4) Run them in order. I found a reboot was required after certain steps and this 
    is why they are borken up into 3 files. Results may vary so you may be able to
    get them to work all in one go. More power to you!

If you want to run on linux or other docker setup the docker file and commands will work 
so you can yoink them out of the powershell scripts and use them as you deem necessary.
