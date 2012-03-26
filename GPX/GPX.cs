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
    }

    public class DataAnalyzer
    {
        #region Properties

        public List<wptType> TrackPoints;
        public List<Segment> Segments;

        public List<double> Distances;
        public List<double> VerticalDistances;
        public List<double> FlatEarthDistances;
        
        public List<double> Times;
        public List<double> Elevations;

        public List<double> Velocities;
        public List<double> VerticalVelocities;
        public List<double> FlatEarthVelocities;

        public List<double> Courses;

        #endregion

        #region Constructors

        public DataAnalyzer(List<wptType> trackPoints)
        {
            TrackPoints = trackPoints;

            Segments = BuildSegments(TrackPoints);
        }

        public DataAnalyzer(GPXFile file, trkType track)
        {
            TrackPoints = file.GetTrackFirstSegmentPoints(track.name);

            Segments = BuildSegments(TrackPoints);
        }

        public DataAnalyzer(GPXFile file, string trackName)
        {
            TrackPoints = file.GetTrackFirstSegmentPoints(trackName);

            Segments = BuildSegments(TrackPoints);
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

        #endregion

        #region Summary - Non Activity Specific

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