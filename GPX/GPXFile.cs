﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GPX
{
    public class GPXFile
    {
        private gpxType gpx;
        public string Result;

        public GPXFile(FileStream gpxFile)
        {
            Load(gpxFile);
        }

        public GPXFile(string path = "..\\..\\Sample.gpx")
        {
            Load(path);
        }

        public string Load(FileStream gpxFile)
        {
            string result = null;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(gpxType));
                gpx = (gpxType)ser.Deserialize(gpxFile);

                result = "";
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public string Load(string path = "..\\..\\Sample.gpx")
        {
            string result = null;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(gpxType));
                using (FileStream str = new FileStream(path, FileMode.Open))
                {
                    gpx = (gpxType)ser.Deserialize(str);
                }

                result = "";
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public gpxType Get()
        {
            return gpx;
        }

        public List<wptType> GetWaypoints()
        {
            return gpx.wpt.ToList();
        }

        public List<trkType> GetTracks()
        {
            return gpx.trk.ToList();
        }

        public List<string> GetTrackNames()
        {
            return gpx.trk.Select(t => t.name).ToList();
        }

        public static List<trksegType> GetTrackSegments(trkType track)
        {
            return track.trkseg.ToList();
        }

        public List<trksegType> GetTrackSegments(string trackName)
        {
            trkType track = gpx.trk.FirstOrDefault(t => t.name == trackName);
            if (track == null)
                return null;

            return track.trkseg.ToList();
        }

        public static List<wptType> GetTrackFirstSegmentPoints(trkType track)
        {
            if (track.trkseg.Count() < 1)
                return null;

            return track.trkseg[0].trkpt.ToList();
        }

        public List<wptType> GetTrackFirstSegmentPoints(string trackName)
        {
            trkType track = gpx.trk.FirstOrDefault(t => t.name.ToUpper() == trackName.ToUpper());
            if (track == null || track.trkseg.Count() < 1)
                return null;

            return track.trkseg[0].trkpt.ToList();
        }

        public static List<wptType> GetTrackPoints(trksegType trackSegment)
        {
            return trackSegment.trkpt.ToList();
        }

        public List<wptType> GetTrackPoints(string trackName, int segment)
        {
            trkType track = gpx.trk.FirstOrDefault(t => t.name.ToUpper() == trackName.ToUpper());
            if (track == null)
                return null;
            if (track.trkseg.Count() <= segment + 1)
                return null;

            return track.trkseg[segment + 1].trkpt.ToList();
        }
    }
}
