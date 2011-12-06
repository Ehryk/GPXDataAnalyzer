using System;
using System.Linq;
using System.Collections.Generic;

namespace GPX
{
    public class TrackResults
    {
        public List<wptType> TrackPoints;

        public List<double> Distances;
        public List<double> VerticalDistances;
        public List<double> FlatEarthDistances;

        public List<double> Times;

        public List<double> Velocities;
        public List<double> VerticalVelocities;
        public List<double> FlatEarthVelocities;

        public List<double> Courses;

        public List<Segment> Segments;

        public double TotalDistance;
        public double TotalVerticalDistance;
        public double TotalFlatEarthDistance;

        public double TotalTime;

        public double AverageDistance;
        public double AverageVerticalDistance;
        public double AverageFlatEarthDistance;

        public double AverageTime;
        public double AverageCourse;

        public double AverageVelocity;
        public double AverageVerticalVelocity;
        public double AverageFlatEarthVelocity;
    }

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
        public double Course { get; set; }
    }

    public class Loader
    {
        public static TrackResults GetResults(List<wptType> trackPoints)
        {
            TrackResults results = new TrackResults();

            results.TrackPoints = trackPoints;

            results.Distances = new List<double>();
            results.VerticalDistances = new List<double>();
            results.FlatEarthDistances = new List<double>();

            results.Times = new List<double>();
            results.Courses = new List<double>();

            results.Velocities = new List<double>();
            results.VerticalVelocities = new List<double>();
            results.FlatEarthVelocities = new List<double>();

            results.Segments = new List<Segment>();

            if (trackPoints.Count < 2)
            {
                //Not enough trackpoints to calculate distances
                return results;
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

                results.Distances.Add(distance);
                results.VerticalDistances.Add(verticalDistance);
                results.FlatEarthDistances.Add(flatEarthDistance);

                results.Times.Add(time);

                results.Velocities.Add(velocity);
                results.VerticalVelocities.Add(verticalVelocity);
                results.FlatEarthVelocities.Add(flatEarthVelocity);

                results.Courses.Add(course);

                results.Segments.Add(new Segment
                                        {
                                            Name = String.Format("Point {0} to {1}", x, x + 1),
                                            Distance = distance,
                                            VerticalDistance = verticalDistance,
                                            FlatEarthDistance = flatEarthDistance,
                                            Time = time,
                                            Velocity = velocity,
                                            VerticalVelocity = verticalVelocity,
                                            FlatEarthVelocity = flatEarthVelocity,
                                            Course = course
                                        });
            }

            results.TotalDistance = Total(results.Distances);
            results.TotalVerticalDistance = Total(results.VerticalDistances);
            results.TotalFlatEarthDistance = Total(results.FlatEarthDistances);

            results.TotalTime = Total(results.Times);

            results.AverageDistance = Average(results.Distances);
            results.AverageVerticalDistance = Average(results.VerticalDistances);
            results.AverageFlatEarthDistance = Average(results.FlatEarthDistances);

            results.AverageTime = Average(results.Times);
            results.AverageCourse = Average(results.Courses);

            results.AverageVelocity = Average(results.Velocities);
            results.AverageVerticalVelocity = Average(results.VerticalVelocities);
            results.AverageFlatEarthVelocity = Average(results.FlatEarthVelocities);

            return results;
        }

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

        #region Generic

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

        public static double RadiansToMeters(double radians)
        {
            // there are 1852 meters in a nautical mile
            return 1852 * RadiansToNauticalMiles(radians);
        }

        #endregion
    }
}