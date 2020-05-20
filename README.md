# Animatronic Mouth

A mouth that moves when talking, controlled by an Arduino over serial port. 
The software uses web APIs to fetch current and forecast weather data for a specific location.
It also fetches current news articles for a country or worldwide.
Using:
- Open weather map
    - Current Weather API
    - 5 day / 3 hour forecast
- NewsAPI.org
    - top headlines (Country based)

Written in C# using the microsoft SSAPI for TTS and JSON for the API data.
