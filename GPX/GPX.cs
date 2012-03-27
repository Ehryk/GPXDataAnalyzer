using System;
using System.Linq;
using System.Collections.Generic;

namespace GPX
{
    public class Segment
    {
        public string Name { get; set; }

        public double Distance { get; set; }
        public double VerticalDistance { get; set; }
        public double FlatEarthDistance { get; set; }

        public double Time { get; set; }

        public double Velocity { get; set; }
        public double VerticalVelocity { get; set; }
        public double FlatEarthVelocity { get; set; }
        
        public double StartElevation { get; set; }
        public double EndElevation { get; set; }

        public double Course { get; set; }
        
        public double AbsoluteVerticalVelocity { get { return Math.Abs(VerticalVelocity); } }
        public double AbsoluteVerticalDistance { get { return Math.Abs(VerticalDistance); } }
    }

    public class Segue
    {
        public string Name { get; set; }

        public double Acceleration { get; set; }
        public double VerticalAcceleration { get; set; }
        public double FlatEarthAcceleration { get; set; }
        
        public double AbsoluteAcceleration { get { return Math.Abs(Acceleration); } }
        public double AbsoluteVerticalAcceleration { get { return Math.Abs(VerticalAcceleration); } }
        public double AbsoluteFlatEarthAcceleration { get { return Math.Abs(FlatEarthAcceleration); } }
    }

    public class DataAnalyzer
    {
        #region Properties

        public List<wptType> TrackPoints;
        public List<Segment> Segments;
        public List<Segue> Segues;

        public List<double> Distances;
        public List<double> VerticalDistances;
        public List<double> FlatEarthDistances;
        
        public List<double> Times;
        public List<double> Elevations;
        public List<double> Courses;

        public List<double> Velocities;
        public List<double> VerticalVelocities;
        public List<double> FlatEarthVelocities;

        #endregion

        #region Constructors

        public DataAnalyzer(List<Segment> segments, wptType startPoint = null)
        {
            TrackPoints = BuildTrackPoints(segments, startPoint);

            Segments = segments;
            Segues = BuildSegues(Segments);
        }

        public DataAnalyzer(List<wptType> trackPoints)
        {
            TrackPoints = trackPoints;

            Segments = BuildSegments(TrackPoints);
            Segues = BuildSegues(Segments);
        }

        public DataAnalyzer(GPXFile file, trkType track)
        {
            TrackPoints = file.GetTrackFirstSegmentPoints(track.name);

            Segments = BuildSegments(TrackPoints);
            Segues = BuildSegues(Segments);
        }

        public DataAnalyzer(GPXFile file, string trackName)
        {
            TrackPoints = file.GetTrackFirstSegmentPoints(trackName);
            
            Segments = BuildSegments(TrackPoints);
            Segues = BuildSegues(Segments);
        }

        public List<Segment> BuildSegments(List<wptType> trackPoints, bool computeSublists = true)
        {
            if (computeSublists)
            {
                Distances = new List<double>();
                VerticalDistances = new List<double>();
                FlatEarthDistances = new List<double>();

                Times = new List<double>();
                Elevations = new List<double>();
                Courses = new List<double>();

                Velocities = new List<double>();
                VerticalVelocities = new List<double>();
                FlatEarthVelocities = new List<double>();

                foreach (wptType trackPoint in trackPoints)
                    Elevations.Add(ToDouble(trackPoint.ele));
            }

            List<Segment> segments = new List<Segment>();

            if (trackPoints.Count < 2)
            {
                //Not enough trackpoints to have any segments
                return segments;
            }

            for (int x = 1; x < trackPoints.Count(); x++)
            {
                double distance = Distance(trackPoints[x - 1], trackPoints[x]);
                double verticalDistance = VerticalDistance(trackPoints[x - 1], trackPoints[x]);
                double flatEarthDistance = Distance(trackPoints[x - 1], trackPoints[x], true);

                double velocity = Velocity(trackPoints[x - 1], trackPoints[x]);
                double verticalVelocity = VerticalVelocity(trackPoints[x - 1], trackPoints[x]);
                double flatEarthVelocity = Velocity(trackPoints[x - 1], trackPoints[x], true);
                
                double time = Time(trackPoints[x - 1], trackPoints[x]);
                double course = Course(trackPoints[x - 1], trackPoints[x]);

                if (computeSublists)
                {
                    Distances.Add(distance);
                    VerticalDistances.Add(verticalDistance);
                    FlatEarthDistances.Add(flatEarthDistance);

                    Times.Add(time);

                    Velocities.Add(velocity);
                    VerticalVelocities.Add(verticalVelocity);
                    FlatEarthVelocities.Add(flatEarthVelocity);

                    Courses.Add(course);
                }

                segments.Add(new Segment
                {
                    Name = String.Format("Point {0} to {1}", x, x + 1),

                    Distance = distance,
                    VerticalDistance = verticalDistance,
                    FlatEarthDistance = flatEarthDistance,

                    Time = time,

                    Velocity = velocity,
                    VerticalVelocity = verticalVelocity,
                    FlatEarthVelocity = flatEarthVelocity,
                    
                    StartElevation = ToDouble(trackPoints[x - 1].ele),
                    EndElevation = ToDouble(trackPoints[x].ele),

                    Course = course
                });
            }

            return segments;
        }

        public List<Segue> BuildSegues(List<Segment> segments, bool computeSublists = true)
        {
            List<Segue> segues = new List<Segue>();

            if (segments.Count < 2)
            {
                //Not enough segments to have any segues
                return segues;
            }

            for (int x = 1; x < segments.Count(); x++)
            {
                segues.Add(new Segue
                {
                    Name = String.Format("Segment {0} to {1}", x, x + 1),
                    
                    Acceleration = Acceleration(segments[x-1], segments[x]),
                    FlatEarthAcceleration = FlatEarthAcceleration(segments[x-1], segments[x]),
                    VerticalAcceleration = VerticalAcceleration(segments[x-1], segments[x])
                });
            }

            return segues;
        }

        public List<wptType> BuildTrackPoints(List<Segment> segments, wptType startPoint = null)
        {
            List<wptType> points = new List<wptType>();

            wptType start = startPoint ?? new wptType();
            start.name = "Point 1";
            points.Add(start);
            
            decimal lat = start.lat;
            decimal lon = start.lon;
            decimal ele = start.ele;
            DateTime time = start.time;

            foreach(Segment s in segments)
            {
                lat += ToDecimal(s.FlatEarthDistance * Math.Cos(s.Course));
                lon += ToDecimal(s.FlatEarthDistance * Math.Sin(s.Course));
                ele += ToDecimal(ele + ToDecimal(s.VerticalDistance));
                time += new TimeSpan(0, 0, 0, ToInt(Math.Floor(s.Time)), ToInt(1000 * (s.Time - ToInt(Math.Floor(s.Time)))));

                points.Add(new wptType { name = String.Format("Point {0}", points.Count() + 1), lat = lat, lon = lon, ele = ele, time = time });
            }

            return points;
        }

        #endregion

        #region Common - Non Activity Specific

        public double AverageDistance()
        {
            return Average(Distances);
        }

        public double AverageVerticalDistance()
        {
            return Average(VerticalDistances);
        }

        public double AverageFlatEarthDistance()
        {
            return Average(FlatEarthDistances);
        }

        public double TotalDistance()
        {
            return Total(Distances);
        }

        public double TotalVerticalDistance()
        {
            return Total(VerticalDistances);
        }

        public double TotalFlatEarthDistance()
        {
            return Total(FlatEarthDistances);
        }

        public double AverageVelocity()
        {
            return Average(Velocities);
        }

        public double AverageVerticalVelocity()
        {
            return Average(VerticalVelocities);
        }

        public double AverageFlatEarthVelocity()
        {
            return Average(FlatEarthVelocities);
        }

        public double TotalVelocity()
        {
            return Total(Velocities);
        }

        public double TotalVerticalVelocity()
        {
            return Total(VerticalVelocities);
        }

        public double TotalFlatEarthVelocity()
        {
            return Total(FlatEarthVelocities);
        }

        public double MaximumVelocity(bool flatEarth = false)
        {
            return Maximum(flatEarth ? FlatEarthVelocities : Velocities);
        }

        public double MinimumVelocity(bool flatEarth = false)
        {
            return Minimum(flatEarth ? FlatEarthVelocities : Velocities);
        }

        public double MaximumFlatEarthVelocity()
        {
            return Maximum(FlatEarthVelocities);
        }

        public double MinimumFlatEarthVelocity()
        {
            return Minimum(FlatEarthVelocities);
        }

        public double MaximumVerticalVelocity()
        {
            return Maximum(VerticalVelocities);
        }

        public double MinimumVerticalVelocity()
        {
            return Minimum(VerticalVelocities);
        }

        public double AverageTime()
        {
            return Average(Times);
        }

        public double TotalTime()
        {
            return Total(Times);
        }

        public double AverageElevationChange()
        {
            return Average(VerticalDistances);
        }

        public double TotalElevationChange()
        {
            return Total(VerticalDistances);
        }

        public double MaximumElevation()
        {
            return Maximum(Elevations);
        }

        public double MinimumElevation()
        {
            return Minimum(Elevations);
        }

        public double StartElevation()
        {
            return ToDouble((TrackPoints.FirstOrDefault() ?? new wptType()).ele);
        }

        public double EndElevation()
        {
            return ToDouble((TrackPoints.LastOrDefault() ?? new wptType()).ele);
        }

        public double AverageCourse()
        {
            return Average(Courses);
        }

        public double TotalCourse()
        {
            if (TrackPoints.Count() < 2)
                return 0;

            return Course(TrackPoints.First(), TrackPoints.Last());
        }

        public double AverageUpSpeed()
        {
	        double upSpeeds = 0;
	        int upSegments = 0;

	        for (int i = 0; i < Segments.Count(); i++)
	        {
		        if (Segments[i].VerticalDistance > 0)
		        {
			        upSpeeds += Segments[i].Velocity;
			        upSegments++;
		        }
	        }

	        if (upSegments == 0)
		        return 0;

	        return upSpeeds / upSegments;
        }

        public double AverageDownSpeed()
        {
	        double downSpeeds = 0;
	        int downSegments = 0;

	        for (int i = 0; i < Segments.Count(); i++)
	        {
		        if (Segments[i].VerticalDistance < 0)
		        {
			        downSpeeds += Segments[i].Velocity;
			        downSegments++;
		        }
	        }

	        if (downSegments == 0)
		        return 0;

	        return downSpeeds / downSegments;
        }

        #endregion

        #region Hiking / Jogging

        public int NumberHikingRests()
        {    
	        int numRests = 0;

	        for(int i = 0; i < Segments.Count; i++)
	        {
                if(Segments[i].Velocity < .2)
		        {
                    numRests++;

			        while (Segments[i].Velocity < .2 && i < Segments.Count)
			        {
				        i ++;
			        }
                }
	        }

	        return numRests;
        }

        public double HikingRestTime() 
        {
            double restTime = 0;

            for(int i = 0; i < Segments.Count(); i++)
	        {
                if(Segments[i].Velocity < .2)
		        {
                    restTime += Segments[i].Time;
                }
	        }

            return restTime;
        }

        public double HikingTime() 
        {
            double hikeTime = 0;

            for(int i = 0; i < Segments.Count(); i++)
	        {
                if(Segments[i].Velocity >= .2)
		        {
                    hikeTime += Segments[i].Time;
                }
	        }

            return hikeTime;
        }

        public double HikingSpeed()		
        {
	        double hikeSpeed = 0;
	        int hikeSegments = 0;

	        for(int i = 0; i < Segments.Count(); i++) 
	        {
		        if (Segments[i].Velocity > .2)
		        {
			        hikeSpeed += Segments[i].Velocity;
			        hikeSegments ++;
		        }
	        }

	        hikeSpeed = hikeSpeed / hikeSegments;
	        return hikeSpeed;
        }

        #endregion

        #region Skiing / Snowboarding

        public int NumberRuns()			
        {
	        int runs = 0;

	        for(int i = 0; i < Segments.Count(); i++)
            {
		        if(Segments[i].VerticalDistance < 0)
                {
			        //Noving downhill
			        runs++;
			
			        while(Segments[i].VerticalDistance < .2 && i < Segments.Count()) {
				        //Move counter forward until you start moving up again
				        i++;
			        }
		        }
	        }
	        return runs;
        }

        public int NumberLifts()				
        {
	        int runs = 0;

	        for(int i = 0; i < Segments.Count(); i++)
            {
		        if(Segments[i].VerticalDistance > 0)
                {
			        //Moving uphill
			        runs++;
			
			        while(Segments[i].VerticalDistance > -.2 && i < Segments.Count())
                    {
				        //Move counter forward until you start moving down again
				        i++;
			        }
		        }
	        }
	        return runs;
        }

        public int NumberFalls()			
        {
	        int falls = 0;

	        for(int i = 0; i < Segments.Count(); i++)
            {
		        if(Segments[i].VerticalDistance < 0 && Segments[i].Velocity < .2)
                {
			        //Moving downhill AND hit slow spot
			
			        while(Segments[i].Velocity < .2 && i < Segments.Count()) {
				        //Move counter forward until you start moving fast again
				        i++;
			        }

			        if (Segments[i].VerticalDistance < .2)
                        //If continuing downhill, it's a fall
				        falls++;
		        }
	        }

	        return falls;
        }

        public double AverageLiftSpeed()
        {
	        double upSpeeds = 0;
	        int upSegments = 0;

	        for (int i = 0; i < Segments.Count(); i++)
	        {
		        if (Segments[i].VerticalDistance > 0)
		        {
			        upSpeeds += Segments[i].Velocity;
			        upSegments++;
		        }
	        }

	        if (upSegments == 0)
		        return 0;

	        return upSpeeds / upSegments;
        }

        public double AverageSkiSpeed()
        {
	        double downSpeeds = 0;
	        int downSegments = 0;

	        for (int i = 0; i < Segments.Count(); i++)
	        {
		        if (Segments[i].VerticalDistance > 0)
		        {
			        downSpeeds += Segments[i].Velocity;
			        downSegments++;
		        }
	        }

	        if (downSegments == 0)
		        return 0;

	        return downSpeeds / downSegments;
        }

        public double AverageLiftWaitTime()		
        {
	        double waitTime = 0;
            int waitCount = 0;

	        for(int i = 0; i < Segments.Count(); i++)
            {
		        if(Segments[i].VerticalDistance < 0 && Segments[i].Velocity < .2)
                {
			        //Moving downhill AND hit slow spot
			        double stopTime = 0;
			
			        while(Segments[i].Velocity < .2 && i < Segments.Count()) {
				        //Move counter forward until you start moving fast again
				        stopTime += Segments[i].Time;
				        i++;
			        }

			        if (Segments[i].VerticalDistance > -.2)
			        {
			            waitTime += stopTime;
                        waitCount ++;
			        }
                }
	        }

            if (waitCount == 0)
                return 0;

	        return waitTime / waitCount;
        }

        public double TotalLiftWaitTime()		
        {
	        double waitTime = 0;

	        for(int i = 0; i < Segments.Count(); i++)
            {
		        if(Segments[i].VerticalDistance < 0 && Segments[i].Velocity < .2)
                {
			        //Moving downhill AND hit slow spot
			        double stopTime = 0;
			
			        while(Segments[i].Velocity < .2 && i < Segments.Count()) {
				        //Move counter forward until you start moving fast again
				        stopTime += Segments[i].Time;
				        i++;
			        }

			        if (Segments[i].VerticalDistance > -.2)
				        waitTime += stopTime;
		        }
	        }

	        return waitTime;		
        }
	
        public double AverageLiftTime()		
        {
	        return TotalLiftTime()/NumberLifts();
        }

        public double TotalLiftTime()		
        {
	        double total = 0;

	        for(int i = 1; i < Segments.Count(); i++)
            {
		        if(Segments[i].VerticalDistance > 0)
                {
			        total += Segments[i].Time;
		        }
	        }

	        return total;
        }

        public double AverageRunTime()		
        {
	        return TotalSkiTime()/NumberRuns();
        }

        public double TotalSkiTime()
        {
	        double total = 0;

	        for(int i = 1; i < Segments.Count(); i++)
            {
		        if(Segments[i].VerticalDistance < 0)
                {
			        total += Segments[i].Time;
		        }
	        }

	        return total;
        }

        public double AverageBindingTime()
        {
	        double restTime = 0;
	        int count = 0;

            for(int i = 1; i < Segments.Count(); i++)
	        {
               while(Segments[i-1].VerticalDistance > 0 && Segments[i].VerticalDistance < 0) 
	           {
			        if(Segments[i-1].Velocity < .2)
			        {
				        restTime += Segments[i-1].Time;
				        count++;
			        }
	           }
	        }

            if (count == 0)
                return 0;

            return restTime / count;
        }

        public double TotalBindingTime()
        {
	        double restTime = 0;

            for(int i = 1; i < Segments.Count(); i++)
	        {
               while(Segments[i-1].VerticalDistance > 0 && Segments[i].VerticalDistance < 0) 
	           {
			        if(Segments[i-1].VerticalDistance < .2)
			        {
				        restTime += Segments[i-1].Time;
			        }
	           }
	        }

            return restTime;
        }

        public double SkiDistance()
        {
	        double total = 0;

            for(int i = 0; i < Segments.Count(); i++)
	        {
		        if(Segments[i+1].VerticalDistance < Segments[i].VerticalDistance)
		        {
			        total += Segments[i].Distance;
		        }
            }

            return total;
        }

        #endregion

        #region Vehicle (Snowmobile/4-Wheeler/Car)

        public int NumberStops()
        {
	        int numRests = 0;

	        for(int i = 0; i < Segments.Count(); i++)
	        {
                if(Segments[i].Velocity < 2.5)
		        {
			        while (Segments[i].Velocity < 2.5 && i < Segments.Count())
				        i++;

                    numRests++;
                }
	        }

	        return numRests;
        }

        public double MaximumAcceleration(bool absolute = false)
        {
	        return Maximum(Segues.Select(s => absolute ? s.AbsoluteAcceleration : s.Acceleration).ToList());
        }

        public double MaximumDeceleration()
        {
	        return Minimum(Segues.Select(s => s.Acceleration).ToList());
        }

        public double VehicleRestTime()
        {
	        return Segments.Where(s => s.Velocity < 2.5).Select(s => s.Time).Sum();
        }

        public double CoastTime()
        {
	        double total = 0;

	        for(int i = 1; i < Segments.Count(); i++)
            {
		        double acceleration = Acceleration(Segments[i], Segments[i-1]);

		        if(-1 < acceleration && acceleration < 1 && Segments[i].Velocity > 10)
			        total += Segments[i].Time;
	        }

	        return total;
        }

        public double AcceleratingTime(double cutoff = 1)
        {
	        double total = 0;

	        for(int i = 1; i < Segments.Count(); i++)
	        {
		        if(Acceleration(Segments[i], Segments[i-1]) > cutoff)
			        total += Segments[i].Time;
	        }

	        return total;
        }

        public double DeceleratingTime(double cutoff = 1)
        {
	        double total = 0;

	        for(int i = 1; i < Segments.Count(); i++)
	        {
		        if(Acceleration(Segments[i], Segments[i-1]) < cutoff)
			        total += Segments[i].Time;
	        }

	        return total;
        }

        #endregion

        #region Distance

        public static double Distance(wptType pt1, wptType pt2, bool flatEarth = false)
        {
            // convert latitude and longitude to radians
            double lat1 = DegreesToRadians((double)pt1.lat);
            double lon1 = DegreesToRadians((double)pt1.lon);
            double lat2 = DegreesToRadians((double)pt2.lat);
            double lon2 = DegreesToRadians((double)pt2.lon);

            // compute latitude and longitude differences
            double dlat = lat2 - lat1;
            double dlon = lon2 - lon1;

            double distanceNorth = dlat;
            double distanceEast = dlon * Math.Cos(lat1);

            // and convert the radians to meters
            distanceNorth = RadiansToMeters(distanceNorth);
            distanceEast = RadiansToMeters(distanceEast);

            double distance = Distance(distanceNorth, distanceEast);

            if (!flatEarth)
            {
                // add the elevation difference to the calculation
                double vertical = (double)pt2.ele - (double)pt1.ele;
                distance = Distance(distanceNorth, distanceEast, vertical);
            }

            return distance;
        }

        public static double VerticalDistance(wptType point1, wptType point2)
        {
            return (double)(point2.ele - point1.ele);
        }

        #endregion

        #region Velocity

        public static double Velocity(wptType point1, wptType point2, bool flatEarth = false)
        {
            double distance = Distance(point1, point2, flatEarth);

            TimeSpan time = point2.time - point1.time;

            double seconds = time.TotalMilliseconds / 1000;

            return distance / seconds;
        }

        public static double VerticalVelocity(wptType point1, wptType point2)
        {
            double distance = VerticalDistance(point1, point2);

            TimeSpan time = point2.time - point1.time;

            double seconds = time.TotalMilliseconds / 1000;

            return distance / seconds;
        }

        #endregion

        #region Acceleration

        public static double Acceleration(double velocity1, double velocity2, double time)
        {
            return (velocity2 - velocity1) / time;
        }

        public static double Acceleration(Segment s1, Segment s2, bool flatEarth = false)
        {
            if (flatEarth)
                return Acceleration(s1.FlatEarthVelocity, s2.FlatEarthVelocity, s2.Time);

            return Acceleration(s1.Velocity, s2.Velocity, s2.Time);
        }

        public static double FlatEarthAcceleration(Segment s1, Segment s2)
        {
            return Acceleration(s1.FlatEarthVelocity, s2.FlatEarthVelocity, s2.Time);
        }

        public static double VerticalAcceleration(Segment s1, Segment s2)
        {
            return Acceleration(s1.VerticalVelocity, s2.VerticalVelocity, s2.Time);
        }

        #endregion

        #region Course

        public static double Course(wptType pt1, wptType pt2)
        {
            // convert latitude and longitude to radians
            double lat1 = DegreesToRadians((double)pt1.lat);
            double lon1 = DegreesToRadians((double)pt1.lon);
            double lat2 = DegreesToRadians((double)pt2.lat);
            double lon2 = DegreesToRadians((double)pt2.lon);

            // compute latitude and longitude differences
            double dlat = lat2 - lat1;
            double dlon = lon2 - lon1;

            double distanceNorth = dlat;
            double distanceEast = dlon * Math.Cos(lat1);

            // compute the course
            double course = Math.Atan2(distanceEast, distanceNorth) % (2 * Math.PI);
            course = RadiansToDegrees(course);
            if (course < 0)
                course += 360;

            return course;
        }

        #endregion

        #region Time

        public static double Time(wptType point1, wptType point2)
        {
            TimeSpan time = point2.time - point1.time;

            double seconds = time.TotalMilliseconds / 1000;

            return seconds;
        }

        #endregion

        #region Helper Methods

        public static double Distance(decimal d1, decimal d2 = 0, decimal d3 = 0)
        {
            return Math.Sqrt(Math.Pow(Decimal.ToDouble(d1), 2) + Math.Pow(Decimal.ToDouble(d2), 2) + Math.Pow(Decimal.ToDouble(d3), 2));
        }

        public static double Distance(double d1, double d2 = 0, double d3 = 0)
        {
            return Math.Sqrt(Math.Pow(d1, 2) + Math.Pow(d2, 2) + Math.Pow(d3, 2));
        }

        public static double Average(List<double> values)
        {
            double total = values.Sum();
            return total / values.Count;
        }

        public static double Total(List<double> values)
        {
            return values.Sum();
        }

        public static double Maximum(List<double> values)
        {
            return values.Max();
        }

        public static double Minimum(List<double> values)
        {
            return values.Min();
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        public static double RadiansToNauticalMiles(double radians)
        {
            // There are 60 nautical miles for each degree
            return radians * 60 * 180 / Math.PI;
        }

        public static double RadiansToMiles(double radians)
        {
            // There are 1.15077945 miles in a nautical mile
            return RadiansToNauticalMiles(radians) / 1.15077945;
        }

        public static double RadiansToMeters(double radians)
        {
            // there are 1852 meters in a nautical mile
            return 1852 * RadiansToNauticalMiles(radians);
        }

        public static double AbsoluteValue(double d)
        {
            return Math.Abs(d);
        }

        public static double ToDouble(object o)
        {
            return Convert.ToDouble(o);
        }

        public static decimal ToDecimal(object o)
        {
            return Convert.ToDecimal(o);
        }

        public static int ToInt(object o)
        {
            return Convert.ToInt32(o);
        }

        #endregion
    }
}