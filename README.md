# Animatronic Mouth with Eyes
## Connected to News and Weahter APIs 

### Implementing multi threading on the C# side to control the mouth.
* One thread to control the mouth
* One thread to control RGB values of neopixel eyes
* One thread to write data to arduino

### Uses an Arduino with 2 neopixles and a servo.
* The arduino is sent the data to move position and change eye color over the serial port.
* The sequence of instructions is split by he arduino.
* Serial commands are only sent when there is a change in instruction - to avoid overloading the serial buffer.

### Connected APIs
* The main code allows users to pick a full update of
- * News and Weahter
- * Or an update of News 
- * Or update on Weather
* The weather API is *OpenWeatherMap* , The News API is *NewsAPI*
