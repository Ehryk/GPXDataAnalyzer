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
            Console.WriteLine("GPX Data Analyzer Console Interface");
            Console.WriteLine();
            Console.Write("Initializing DataAnalyzer Object... ");

            DataAnalyzer da = GetDefaultDataAnalyzer();

            Console.WriteLine("Done.");
            Console.WriteLine("");

            //Enters the main UI Loop which asks for a command, does it, prints any error, and keeps repeating until told to stop);
            bool repeat = true;
            while (repeat)
            {
                try
                {
                    Console.Write("> ");
                    string entry = Console.ReadLine();

                    string command = entry.Split(' ')[0].ToUpper();

                    switch (command)
                    {
                        //Use the Get command to test the methods written
                        case "GET":
                        case "G":
                            Get(da, entry);
                            break;

                        case "ADD":
                        case "A":
                            AddSegment(da, entry);
                            Console.WriteLine("Segment Added.");
                            break;

                        case "ADDSAMPLE":
                        case "AS":
                            AddSampleSegments(da);
                            Console.WriteLine("Sample segments added.");
                            break;

                        case "GPXLOAD":
                        case "GPX":
                            TestGPXLoad();
                            break;

                        case "CLS":
                            Console.Clear();
                            break;

                        case "CLEAR":
                        case "C":
                            da = new DataAnalyzer(100);
                            Console.WriteLine("DataAnalyzer cleared, it now contains no segments.");
                            break;

                        case "DEFAULT":
                        case "D":
                            da = GetDefaultDataAnalyzer();
                            Console.WriteLine("DataAnalyzer reset to default.");
                            break;

                        case "HELP":
                        case "H":
                        case "?":
                            PrintHelp();
                            break;

                        case "QUIT":
                        case "EXIT":
                        case "Q":
                        case "E":
                            repeat = false;
                            break;

                        default:
                            Console.WriteLine("Command Not Recognized.  For a list of commands, type H or ? for help.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Application ended. Press any key to continue...");
            Console.ReadKey(true);
        }

        #region DataAnalyzer Testing

        public static void Get(DataAnalyzer pDA, string entry)
        {
            string[] segments = entry.Split(' ');
            string subcommand = segments[1].ToUpper();
            double returned = -1;

            switch (subcommand)
            {
                #region Internal

                case "VELOCITY":
                case "V":
                    if (segments.Count() < 4)
                    {
                        Console.Write("Distance and Time not entered.");
                    }
                    else
                    {
                        Console.Write("Velocity = distance/time = {0}/{1} = ", segments[2], segments[3]);
                        returned = pDA.Velocity(double.Parse(segments[2]), double.Parse(segments[3]));
                        Console.WriteLine(returned);
                    }
                    break;

                case "ACCELERATION":
                case "A":
                    if (segments.Count() < 5)
                    {
                        Console.Write("Distance and Time not entered.");
                    }
                    else if (segments.Count() == 5)
                    {
                        Console.Write("Acceleration = (v2 - v1)/time = ({1} - {0})/{2} = ", segments[2], segments[3], segments[4]);
                        returned = pDA.Acceleration(double.Parse(segments[2]), double.Parse(segments[3]), double.Parse(segments[4]));
                        Console.WriteLine(returned);
                    }
                    else if (segments.Count() > 5)
                    {
                        Console.Write("Acceleration = (v2 - v1)/time = ({2}/{3} - {0}/{1})/{3} = ", segments[2], segments[3], segments[4], segments[5]);
                        returned = pDA.Acceleration(double.Parse(segments[2]), double.Parse(segments[3]), double.Parse(segments[4]), double.Parse(segments[5]));
                        Console.WriteLine(returned);
                    }
                    break;

                #endregion

                #region Private

                case "I":
                    returned = pDA.GetI();
                    Console.WriteLine("i = " + returned);
                    break;

                case "N":
                    returned = pDA.GetN();
                    Console.WriteLine("n = " + returned);
                    break;

                case "MAX":
                    returned = pDA.GetMax();
                    Console.WriteLine("max = " + returned);
                    break;

                case "STARTELEVATION":
                case "SE":
                    returned = pDA.GetStartElevation();
                    Console.WriteLine("Start Elevation = " + returned);
                    break;

                case "STARTLATITUDE":
                case "SLAT":
                    returned = pDA.GetStartLatitude();
                    Console.WriteLine("Start Latitude = " + returned);
                    break;

                case "STARTLONGITUDE":
                case "SLON":
                    returned = pDA.GetStartLongitude();
                    Console.WriteLine("Start Longitude = " + returned);
                    break;

                case "STARTDATETIME":
                case "SD":
                    Console.WriteLine("Start Date Time = " + pDA.GetStartDateTime());
                    break;

                case "ENDELEVATION":
                case "EE":
                    returned = pDA.GetEndElevation();
                    Console.WriteLine("End Elevation = " + returned);
                    break;

                case "ENDLATITUDE":
                case "ELAT":
                    returned = pDA.GetEndLatitude();
                    Console.WriteLine("End Latitude = " + returned);
                    break;

                case "ENDLONGITUDE":
                case "ELON":
                    returned = pDA.GetEndLongitude();
                    Console.WriteLine("End Longitude = " + returned);
                    break;

                case "ENDDATETIME":
                case "ED":
                    Console.WriteLine("End Date Time = " + pDA.GetEndDateTime());
                    break;

                case "DISTANCES":
                case "DS":
                    Console.WriteLine("Distances:");
                    Console.WriteLine(pDA.GetDistances());
                    break;

                case "FLATDISTANCES":
                case "FDS":
                    Console.WriteLine("Flat Distances:");
                    Console.WriteLine(pDA.GetFlatDistances());
                    break;

                case "VERTICALDISTANCES":
                case "VDS":
                    Console.WriteLine("Vertical Distances:");
                    Console.WriteLine(pDA.GetVerticalDistances());
                    break;

                case "VELOCITIES":
                case "VS":
                    Console.WriteLine("Velocities:");
                    Console.WriteLine(pDA.GetVelocities());
                    break;

                case "FLATVELOCITIES":
                case "FVS":
                    Console.WriteLine("Flat Velocities:");
                    Console.WriteLine(pDA.GetFlatVelocities());
                    break;

                case "VERTICALVELOCITIES":
                case "VVS":
                    Console.WriteLine("Vertical Velocities:");
                    Console.WriteLine(pDA.GetVerticalVelocities());
                    break;

                case "COURSES":
                case "CS":
                    Console.WriteLine("Courses:");
                    Console.WriteLine(pDA.GetCourses());
                    break;

                case "TIMES":
                case "TS":
                    Console.WriteLine("Times:");
                    Console.WriteLine(pDA.GetTimes());
                    break;

                #endregion

                #region Common

                case "TOTALDISTANCE":
                case "TD":
                    returned = pDA.GetTotalDistance();
                    Console.WriteLine("Total Distance = " + returned);
                    break;

                case "AVERAGEDISTANCE":
                case "AD":
                    returned = pDA.GetAverageDistance();
                    Console.WriteLine("Average Distance = " + returned);
                    break;

                case "TOTALTIME":
                case "TT":
                    returned = pDA.GetTotalTime();
                    Console.WriteLine("Total Distance = " + returned);
                    break;

                case "AVERAGETIME":
                case "AT":
                    returned = pDA.GetAverageTime();
                    Console.WriteLine("Average Distance = " + returned);
                    break;

                case "AVERAGEVELOCITY":
                case "AV":
                    returned = pDA.GetAverageVelocity();
                    Console.WriteLine("Total Distance = " + returned);
                    break;

                case "MAXVELOCITY":
                case "MV":
                    returned = pDA.GetMaxVelocity();
                    Console.WriteLine("Average Distance = " + returned);
                    break;

                case "MINIMUMELEVATION":
                case "MINELEVATION":
                case "MINELE":
                case "MNE":
                    returned = pDA.GetMinElevation();
                    Console.WriteLine("Total Distance = " + returned);
                    break;

                case "MAXIMUMELEVATION":
                case "MAXELEVATION":
                case "MAXELE":
                case "MXE":
                    returned = pDA.GetMaxElevation();
                    Console.WriteLine("Average Distance = " + returned);
                    break;

                default:
                    Console.WriteLine("Function Not Recognized.");
                    break;

                #endregion
            }
        }

        public static DataAnalyzer GetDefaultDataAnalyzer()
        {
            DataAnalyzer analyzer = new DataAnalyzer(100, 220.27917, new DateTime(2010, 11, 18, 23, 52, 17), -150.17818630, 61.89438680);

            AddSampleSegments(analyzer);

            return analyzer;
        }

        public static void AddSampleSegments(DataAnalyzer pDA)
        {
            pDA.AddSegment(393.93, 8, 205.98, 9.61, 393.81);
            pDA.AddSegment(495.3, 10, 199.94, 10.09, 495.20);
            pDA.AddSegment(514.3, 10, 192.42, 4.33, 514.28);
            pDA.AddSegment(468.76, 9, 188.40, 3.85, 468.74);
            pDA.AddSegment(253.65, 5, 164.79, 5.77, 253.58);
            pDA.AddSegment(200.45, 4, 139.13, 4.33, 200.41);
            pDA.AddSegment(203.53, 4, 117.75, 2.40, 203.52);
            pDA.AddSegment(155.14, 3, 100.85, 0.96, 155.13);
            pDA.AddSegment(355.56, 7, 86.89, 4.81, 355.53);
            pDA.AddSegment(242.13, 5, 70.93, 11.54, 241.85);
        }

        public static void AddSegment(DataAnalyzer pDA, string entry)
        {
            string[] segments = entry.Split(' ');
            AddSegment(pDA, Double.Parse(segments[1]), Double.Parse(segments[2]), Double.Parse(segments[3]), Double.Parse(segments[4]), Double.Parse(segments[5]));
        }

        public static void AddSegment(DataAnalyzer pDA, double distance, double time, double course, double vertical, double flat)
        {
            pDA.AddSegment(distance, time, course, vertical, flat);
        }

        public static void PrintHelp()
        {
            Console.WriteLine("  --- Commands ---");
            Console.WriteLine("Default (C)   - Resets the DataAnalyzer to default sample segments");
            Console.WriteLine("CLS (C)       - Clears the Screen");
            Console.WriteLine("Clear (C)     - Clears the DataAnalyzer");
            Console.WriteLine("AddSample (AS)- Adds the default sample segments");
            Console.WriteLine("GPXLoad (GPX) - Reads in Sample.gpx and tests GPX file parsing");
            Console.WriteLine();
            Console.WriteLine("Add (A)       - Adds a segment: <Distance> <Time> <Course> <Vertical> <Flat>");
            Console.WriteLine("                Example: Add 393.93 8 180 9.61 393.81");
            Console.WriteLine();
            Console.WriteLine("Exit (E)      - Quits the Application");
            Console.WriteLine("Quit (Q)      - Quits the Application");
        }

        #endregion

        #region GPX Loading

        //Loads the Sample.gpx and tests the parsing function
        public static void TestGPXLoad()
        {
            GPXFile gpx = new GPXFile();
            if (!String.IsNullOrEmpty(gpx.Result))
            {
                Console.WriteLine(gpx.Result);

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);

                return;
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
                return;
            }

            for (int x = 1; x < trackPoints.Count(); x++)
            {
                distances.Add(GPX.DataAnalyzer.Distance(trackPoints[x - 1], trackPoints[x]));
                verticalDistances.Add(GPX.DataAnalyzer.VerticalDistance(trackPoints[x - 1], trackPoints[x]));
                flatEarthDistances.Add(GPX.DataAnalyzer.Distance(trackPoints[x - 1], trackPoints[x], true));

                velocities.Add(GPX.DataAnalyzer.Velocity(trackPoints[x - 1], trackPoints[x]));
                verticalVelocities.Add(GPX.DataAnalyzer.VerticalVelocity(trackPoints[x - 1], trackPoints[x]));
                flatEarthVelocities.Add(GPX.DataAnalyzer.Velocity(trackPoints[x - 1], trackPoints[x], true));
            }

            Console.WriteLine("{0, -20} {1, -25} {2, -25}", "-Between Points-", "-Distance-", "-Velocity-");
            for (int x = 0; x < distances.Count(); x++)
            {
                Console.WriteLine("{0, -20} {1, -25} {2, -25}", (x + 1) + " and " + (x + 2), distances[x] + " m", velocities[x] + " m/s");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            Console.WriteLine();
            Console.WriteLine("Total Distance: {0}", GPX.DataAnalyzer.Total(distances) + " m");
            Console.WriteLine("Total Vertical Distance: {0}", GPX.DataAnalyzer.Total(verticalDistances) + " m");
            Console.WriteLine("Total Flat Earth Distance: {0}", GPX.DataAnalyzer.Total(flatEarthDistances) + " m");
            Console.WriteLine();
            Console.WriteLine("Average Distance: {0}", GPX.DataAnalyzer.Average(distances) + " m");
            Console.WriteLine("Average Vertical Distance: {0}", GPX.DataAnalyzer.Average(verticalDistances) + " m");
            Console.WriteLine("Average Flat Earth Distance: {0}", GPX.DataAnalyzer.Average(flatEarthDistances) + " m");
            Console.WriteLine();
            Console.WriteLine("Average Velocity: {0}", GPX.DataAnalyzer.Average(velocities) + " m/s");
            Console.WriteLine("Average Vertical Velocity: {0}", GPX.DataAnalyzer.Average(verticalVelocities) + " m/s");
            Console.WriteLine("Average Flat Earth Velocity: {0}", GPX.DataAnalyzer.Average(flatEarthVelocities) + " m/s");
            Console.WriteLine();
        }

        #endregion
    }
}
