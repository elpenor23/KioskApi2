## Todo:
   - [ ] Error Handling  - add global error handling for controllers

   - [x] Organize configuration to make it sane.

   - [x] Remove swagger in favor of Scalar

   - [ ] Update documentation

   - [ ] More


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
