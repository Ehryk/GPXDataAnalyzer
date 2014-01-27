# GPX Data Analyzer
This project allows you to upload GPX files and analyze track data contained within in various ways. Every track can see the 'Basic Analysis,' something most programs that work with GPX files can display:

* Total Distance
* Total Time
* Average Velocity

From there, you can then select an Activity to get more detailed analysis that only makes sense in the context of the activity:

* Flying: Time in flight, average climbing and descent velocities, maximum and minimum velocity, maximum and minimum acceleration
* Hiking/Jogging: Number of rests, average uphill and downhill speeds, highest and lowest elevation
* Downhill Skiing: Number of runs, number of falls, average lift speed, average skiing speed
* Driving: Number of stops, maximum and minimum acceleration, time spent accelerating, time spent decelerating
* ...and more!

Visit us at [gpxdataanalyzer.com](http://gpxdataanalyzer.com)

If you'd like to suggest additional analysis items, activities, or have any feedback open an issue here or [Send me an Email](mailto:ehryk42@gmail.com).

### General Requirements

 - Visual Studio and .NET

### Sample Analysis

#### Skiing
![Skiing](https://raw2.github.com/Ehryk/GPXDataAnalyzer/master/Documentation/SampleResults_Skiing.png)
#### Flying
![Flying](https://raw2.github.com/Ehryk/GPXDataAnalyzer/master/Documentation/SampleResults_Flying.png)
#### Hiking
![Hiking](https://raw2.github.com/Ehryk/GPXDataAnalyzer/master/Documentation/SampleResults_Hiking.png)
#### Driving
![Driving](https://raw2.github.com/Ehryk/GPXDataAnalyzer/master/Documentation/SampleResults_Driving.png)

### Setup

1. Modify the publish settings
1. Deploy
1. PROFIT

### Known issues / Future Goals

 - There are no users or any logging in, all uploads are public. Perhaps I should add this?
 - Silently fails on errors - add more robust error handling and notifications
 - The 'Graphs' item was planned for a future update - perhaps something with D3.js?
 - Add a map!
