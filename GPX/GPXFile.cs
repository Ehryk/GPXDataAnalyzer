using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using WinUX;
namespace GPX
{
    public class GPXFile
    {
        private gpxType gpx;
        public string Result;

        public GPXFile(string path = "..\\..\\Sample.gpx")
        {
            Load(path);
        }

        public GPXFile() { }

        public void Save(string fileName, gpxType gpxData)
        {
            try
            {
                using (StreamWriter stream = new StreamWriter(fileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(gpxType));
                    ser.Serialize(stream, gpxData);
                }
            }
            catch (Exception e)
            {
            }
        }

        public List<rteType> GetRoutes()
        {
            if(gpx.rte == null)
            {
                return new List<rteType>();
            }
            return gpx.rte.ToList();
        }

        public string Name
        {
            get { return gpx.metadata?.name; }
        }

        public string Load(string path = "..\\..\\Sample.gpx")
        {
            string result = null;
            try
            {
                var xdocument = XDocument.Load(path);
                var defaultNS = xdocument.Root.GetDefaultNamespace();
                if (string.IsNullOrEmpty(defaultNS.NamespaceName))
                {
                    var xml = xdocument.ToString();
                    var indexOfGpx = xml.IndexOf("<gpx ");
                    var insertIndex = indexOfGpx + 5;
                    xml = xml.Insert(5, "xmlns = \"http://www.topografix.com/GPX/1/1\" ");
                    SetGpxWithMemoryStream(xml);
                }
                else
                {
                    SetGpxWithNormalFileStream(path);
                }
                result = "";
            }
            catch (Exception e)
            {
                result = e.Message;
                gpx = new gpxType();
            }
            return result;
        }

        private void SetGpxWithMemoryStream(string xml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(gpxType));
            using (var stream = xml.ToStream())
            {
                gpx = (gpxType)ser.Deserialize(stream);
            }
        }

        private void SetGpxWithNormalFileStream(string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(gpxType));
            using (FileStream str = new FileStream(path, FileMode.Open))
            {
                gpx = (gpxType)ser.Deserialize(str);
            }
        }

        public gpxType Get()
        {
            return gpx;
        }

        public List<wptType> GetWaypoints()
        {
            if(gpx.wpt == null)
            {
                return new List<wptType>();
            }
            return gpx.wpt.ToList();
        }

        public List<trkType> GetTracks()
        {
            if (gpx.trk == null)
            {
                return new List<trkType>();
            }
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
