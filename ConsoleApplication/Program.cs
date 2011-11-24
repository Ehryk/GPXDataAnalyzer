using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPX;

namespace ConsoleApplication
{
    class Program
    {
        private const int PauseGroup = 20;

        static void Main(string[] args)
        {
            GPXFile gpx = new GPXFile();
            if (!String.IsNullOrEmpty(gpx.Result))
            {
                Console.WriteLine(gpx.Result);

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);

                Environment.Exit(0);
            }

            Console.Write("Type: ");

            Console.WriteLine(gpx.Get().ToString());

            Console.WriteLine("Waypoints: ");
            Console.WriteLine("{0,-4} {1,-4} {2,-20} {3,-15} {4,-15} {5,-15}", "", "", "-Name-", "-Latitude-", "-Longitude-", "-Elevation-");

            int i = 1;
            int total = gpx.GetWaypoints().Count();
            foreach (wptType waypoint in gpx.GetWaypoints())
            {
                Console.WriteLine("{0,4}/{1,-4} {2,-20} {3,-15} {4,-15} {5,-15}", i, total, waypoint.name, waypoint.lat, waypoint.lon, waypoint.ele);
                i++;

                if (i % PauseGroup == 0)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }
            }

            Console.WriteLine("Tracks: ");
            Console.WriteLine("{0,-4} {1,-4} {2,-20} {3,-10} {4,-10}", "", "", "-Name-", "-Segments-", "-Track Points- (first segment only)");

            List<trkType> tracks = gpx.GetTracks();
            i = 1;
            total = tracks.Count();
            foreach (trkType track in tracks)
            {
                int trackSegmentCount = track.trkseg.Count();
                int trackPointCount = track.trkseg[0].trkpt.Count();
                Console.WriteLine("{0,4}/{1,-4} {2,-20} {3,-10} {4,-10}", i, total, track.name, trackSegmentCount, trackPointCount);
                i++;

                if (i % PauseGroup == 0)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }
            }

            string selection = null;
            while (selection == null)
            {
                Console.Write("Enter a track name> ");
                string entry = Console.ReadLine();
                if (gpx.GetTrackNames().Any(n => n.ToUpper() == entry.ToUpper()))
                    selection = entry;
            }

            Console.WriteLine("Track Points (first segment only): ");
            Console.WriteLine("{0,-4} {1,-4} {2,-20} {3,-15} {4,-15} {5,-15}", "", "", "-Name-", "-Latitude-", "-Longitude-", "-Elevation-");

            List<wptType> trackPoints = gpx.GetTrackFirstSegmentPoints(selection);
            i = 1;
            total = trackPoints.Count();
            foreach (wptType trackpoint in trackPoints)
            {
                Console.WriteLine("{0,4}/{1,-4} {2,-20} {3,-15} {4,-15} {5,-15}", i, total, trackpoint.name, trackpoint.lat, trackpoint.lon, trackpoint.ele);
                i++;

                if (i % PauseGroup == 0)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }
            }

            List<double> distances = new List<double>();
            List<double> verticalDistances = new List<double>();
            List<double> flatEarthDistances = new List<double>();

            List<double> velocities = new List<double>();
            List<double> verticalVelocities = new List<double>();
            List<double> flatEarthVelocities = new List<double>();
            
            Console.WriteLine();

            if (trackPoints.Count < 2)
            {
                Console.WriteLine("Not enough trackpoints to calculate distances.");
                Console.WriteLine("Application ended. Press any key to continue...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }

            for (int x = 1; x < trackPoints.Count(); x++)
            {
                distances.Add(Loader.Distance(trackPoints[x - 1], trackPoints[x]));
                verticalDistances.Add(Loader.VerticalDistance(trackPoints[x - 1], trackPoints[x]));
                flatEarthDistances.Add(Loader.Distance(trackPoints[x - 1], trackPoints[x], true));

                velocities.Add(Loader.Velocity(trackPoints[x - 1], trackPoints[x]));
                verticalVelocities.Add(Loader.VerticalVelocity(trackPoints[x - 1], trackPoints[x]));
                flatEarthVelocities.Add(Loader.Velocity(trackPoints[x - 1], trackPoints[x], true));
            }

            Console.WriteLine("{0, -20} {1, -25} {2, -25}", "-Between Points-", "-Distance-", "-Velocity-");
            for (int x = 0; x < distances.Count(); x++)
            {
                Console.WriteLine("{0, -20} {1, -25} {2, -25}", (x + 1) + " and " + (x + 2), distances[x] + " m", velocities[x] + " m/s");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            Console.WriteLine();
            Console.WriteLine("Total Distance: {0}", Loader.Total(distances) + " m");
            Console.WriteLine("Total Vertical Distance: {0}", Loader.Total(verticalDistances) + " m");
            Console.WriteLine("Total Flat Earth Distance: {0}", Loader.Total(flatEarthDistances) + " m");
            Console.WriteLine();
            Console.WriteLine("Average Distance: {0}", Loader.Average(distances) + " m");
            Console.WriteLine("Average Vertical Distance: {0}", Loader.Average(verticalDistances) + " m");
            Console.WriteLine("Average Flat Earth Distance: {0}", Loader.Average(flatEarthDistances) + " m");
            Console.WriteLine();
            Console.WriteLine("Average Velocity: {0}", Loader.Average(velocities) + " m/s");
            Console.WriteLine("Average Vertical Velocity: {0}", Loader.Average(verticalVelocities) + " m/s");
            Console.WriteLine("Average Flat Earth Velocity: {0}", Loader.Average(flatEarthVelocities) + " m/s");
            Console.WriteLine();
            Console.WriteLine("Application ended. Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
