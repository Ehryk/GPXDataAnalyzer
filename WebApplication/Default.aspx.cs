using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPX;
using WebApplication.App_Code;

namespace WebApplication
{
    public partial class _Default : BasePage
    {
        #region Properties

        protected DataAnalyzer analyzer
        {
            get { return Session["DataAnalyzer"] as DataAnalyzer; }
            set { Session["DataAnalyzer"] = value; }
        }

        bool ActivityPanelsVisible
        {
            set
            {
                pnlNotSure.Visible = value;
                pnlSlow.Visible = value;
                pnlDownhill.Visible = value;
                pnlFast.Visible = value;
                pnlFlight.Visible = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindFileDDL();
        }

        protected void BindFileDDL()
        {
            ddlFiles.Items.Clear();
            ddlFiles.Items.Add(new ListItem(""));
            foreach (string file in Directory.EnumerateFiles(Server.MapPath("~/Uploads/")))
                ddlFiles.Items.Add(new ListItem(Path.GetFileName(file), file));

            pnlTracks.Visible = false;
            pnlAnalysis.Visible = false;
        }

        protected void FileChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlFiles.SelectedValue))
            {
                pnlTracks.Visible = false;
                pnlAnalysis.Visible = false;
                pnlActivity.Visible = false;
                ActivityPanelsVisible = false;
                return;
            }
            if (!File.Exists(ddlFiles.SelectedValue))
            {
                pnlTracks.Visible = false;
                pnlAnalysis.Visible = false;
                pnlActivity.Visible = false;
                ActivityPanelsVisible = false;
                //lblStatus.Text = "File Doesn't Exist";
                return;
            }

            FilePath = ddlFiles.SelectedValue;

            LoadTracks();
        }

        protected void LoadTracks()
        {
            if (String.IsNullOrWhiteSpace(FilePath) || !File.Exists(FilePath))
                return;

            GPX = new GPXFile(FilePath);

            List<trkType> tracks = GPX.GetTracks();
            tracks.Insert(0, new trkType{name = ""});

            ddlTracks.DataSource = tracks;
            ddlTracks.DataBind();

            TrackChanged(ddlTracks, EventArgs.Empty);

            pnlTracks.Visible = true;
        }

        protected void TrackChanged(object sender, EventArgs e)
        {
            if (GPX == null)
                return;
            if (String.IsNullOrEmpty(ddlTracks.SelectedValue))
            {
                pnlAnalysis.Visible = false;
                pnlActivity.Visible = false;
                ActivityPanelsVisible = false;
                return;
            }

            List<wptType> trackPoints = GPX.GetTrackFirstSegmentPoints(ddlTracks.SelectedValue);

            lblTrackPointCount.Text = String.Format(" ({0})", trackPoints.Count());

            TrackResults results = Loader.GetResults(trackPoints);

            gvTrackPoints.DataSource = results.TrackPoints;
            gvTrackPoints.DataBind();

            lblSegmentCount.Text = String.Format(" ({0})", results.Segments.Count());

            gvSegments.DataSource = results.Segments;
            gvSegments.DataBind();

            lblTotalDistance.Text = FormatDistance(results.TotalDistance);
            lblTotalVerticalDistance.Text = FormatDistance(results.TotalVerticalDistance);
            lblTotalFlatEarthDistance.Text = FormatDistance(results.TotalFlatEarthDistance);

            lblTotalTime.Text = FormatTime(results.TotalTime);

            lblAverageDistance.Text = FormatDistance(results.AverageDistance);
            lblAverageVerticalDistance.Text = FormatDistance(results.AverageVerticalDistance);
            lblAverageFlatEarthDistance.Text = FormatDistance(results.AverageFlatEarthDistance);

            lblAverageTime.Text = FormatTime(results.AverageTime);
            lblAverageCourse.Text = FormatCourse(results.AverageCourse);

            lblAverageVelocity.Text = FormatVelocity(results.AverageVelocity);
            lblAverageVerticalVelocity.Text = FormatVelocity(results.AverageVerticalVelocity);
            lblAverageFlatEarthVelocity.Text = FormatVelocity(results.AverageFlatEarthVelocity);

            pnlAnalysis.Visible = true;

            LoadDataAnalyzer(results);

            pnlActivity.Visible = true;
            ActivityChanged(ddlActivity, EventArgs.Empty);
        }

        #region Collabsible Panels

        protected void ToggleFiles(object sender, EventArgs e)
        {
            Toggle(cpFiles, ibFiles);
        }

        protected void ToggleTracks(object sender, EventArgs e)
        {
            Toggle(cpTracks, ibTracks);
        }

        protected void ToggleData(object sender, EventArgs e)
        {
            Toggle(cpData, ibData);
        }

        protected void ToggleTrackPoints(object sender, EventArgs e)
        {
            Toggle(cpTrackPoints, ibTrackPoints);
        }

        protected void ToggleSegments(object sender, EventArgs e)
        {
            Toggle(cpSegments, ibSegments);
        }

        protected void ToggleTotals(object sender, EventArgs e)
        {
            Toggle(cpTotals, ibTotals);
        }

        protected void ToggleGraphs(object sender, EventArgs e)
        {
            Toggle(cpGraphs, ibGraphs);
        }

        protected void ToggleVelocity(object sender, EventArgs e)
        {
            Toggle(cpVelocity, ibVelocity);
        }

        protected void ToggleActivity(object sender, EventArgs e)
        {
            Toggle(cpActivity, ibActivity);
        }

        #endregion

        protected void ActivityChanged(object sender, EventArgs e)
        {
            ActivityPanelsVisible = false;

            if (ddlActivity.SelectedValue == "NotSure")
            {
                pnlNotSure.Visible = true;
            }
            if (ddlActivity.SelectedValue == "Hiking" || ddlActivity.SelectedValue == "Jogging" || ddlActivity.SelectedValue == "CrossCountry")
            {
                LoadSlowResults(ddlActivity.SelectedItem.Text + " Results");
                pnlSlow.Visible = true;
            }
            else if (ddlActivity.SelectedValue == "Downhill" || ddlActivity.SelectedValue == "Snowboarding")
            {
                LoadDownhillResults(ddlActivity.SelectedItem.Text + " Results");
                pnlDownhill.Visible = true;
            }
            else if (ddlActivity.SelectedValue == "Snowmobiling" || ddlActivity.SelectedValue == "4Wheeling" || ddlActivity.SelectedValue == "Driving")
            {
                LoadFastResults(ddlActivity.SelectedItem.Text + " Results");
                pnlFast.Visible = true;
            }
            else if (ddlActivity.SelectedItem.Text == "Flying")
            {
                LoadFlightResults("Flight Results");
                pnlFlight.Visible = true;
            }
        }

        #endregion

        #region Methods

        protected void LoadDataAnalyzer(TrackResults pResults)
        {
            wptType start = pResults.TrackPoints.FirstOrDefault();

            if (start != null)
                analyzer = new DataAnalyzer(pResults.Segments.Count, (double)start.ele, start.time, (double)start.lat, (double)start.lon);
            else
                analyzer = new DataAnalyzer(pResults.Segments.Count);

            foreach (Segment segment in pResults.Segments)
            {
                analyzer.AddSegment(segment.Distance, segment.Time, segment.Course, segment.VerticalDistance, segment.FlatEarthDistance);
            }
        }

        protected void Toggle(Panel panel, ImageButton imageButton)
        {
            bool showing = panel.Visible;

            if (showing)
            {
                //Hide it
                panel.Visible = false;
                imageButton.ImageUrl = "~/Images/expand.jpg";
            }
            else
            {
                //Show it
                panel.Visible = true;
                imageButton.ImageUrl = "~/Images/collapse.jpg";
            }
        }

        protected void LoadSlowResults(string pTitle)
        {
            lblSlowTitle.Text = pTitle;

            lblAverageHikeSpeed.Text = FormatVelocity(analyzer.GetHikingSpeed());
            lblTotalHikeTime.Text = FormatTime(analyzer.GetHikingTime());

            lblMinElevation.Text = FormatDistance(analyzer.GetMinElevation());
            lblMaxElevation.Text = FormatDistance(analyzer.GetMaxElevation());

            lblStartElevation.Text = FormatDistance(analyzer.GetStartElevation());
            lblEndElevation.Text = FormatDistance(analyzer.GetEndElevation());

            lblUphillHikeSpeed.Text = FormatVelocity(analyzer.GetAverageUpSpeed());
            lblDownhillHikeSpeed.Text = FormatVelocity(analyzer.GetAverageDownSpeed());

            lblNumberHikingRests.Text = analyzer.GetNumberHikingRests().ToString();
            lblTotalHikeRestTime.Text = FormatTime(analyzer.GetHikingRestTime());
        }

        protected void LoadDownhillResults(string pTitle)
        {
            lblDownhillTitle.Text = pTitle;
            lblNumberOfRuns.Text = analyzer.GetNumberRuns().ToString();
            lblNumberOfLifts.Text = analyzer.GetNumberRuns().ToString();
            lblNumberOfFalls.Text = analyzer.GetNumberRuns().ToString();

            lblTotalDownhillDistance.Text = FormatDistance(analyzer.GetSkiDistance());
            lblVerticalDistance.Text = FormatDistance(analyzer.GetMaxElevation() - analyzer.GetEndElevation());

            lblAverageLiftSpeed.Text = FormatVelocity(analyzer.GetAverageLiftSpeed());
            lblAverageSkiSpeed.Text = FormatVelocity(analyzer.GetAverageSkiSpeed());
        }

        protected void LoadFastResults(string pTitle)
        {
            lblFastTitle.Text = pTitle;
            lblTotalFastTime.Text = FormatTime(analyzer.GetTotalTime());
            lblTotalFastDistance.Text = FormatDistance(analyzer.GetTotalDistance());

            lblNumberStops.Text = analyzer.GetNumberStops().ToString();
            lblMaxAcceleration.Text = FormatAcceleration(analyzer.GetMaximumAcceleration());
            lblMaxDeceleration.Text = FormatAcceleration(analyzer.GetMaximumDeceleration());

            lblVehicleRestTime.Text = FormatTime(analyzer.GetVehicleRestTime());
            lblCoastTime.Text = FormatTime(analyzer.GetCoastTime());
            lblAccelerationTime.Text = FormatAcceleration(analyzer.GetAcceleratingTime());
            lblDecelerationTime.Text = FormatAcceleration(analyzer.GetDeceleratingTime());
        }

        protected void LoadFlightResults(string pTitle)
        {
            lblFlightTitle.Text = pTitle;
            lblTotalFlightTime.Text = FormatTime(analyzer.GetTotalTime());
            lblTotalFlightDistance.Text = FormatDistance(analyzer.GetTotalDistance());

            lvlAverageFlightVelocity.Text = FormatVelocity(analyzer.GetAverageSkiSpeed());
            lblAverageClimbingVelocity.Text = FormatVelocity(analyzer.GetAverageUpSpeed());
            lblAverageDescentVelocity.Text = FormatVelocity(analyzer.GetAverageDownSpeed());

            lblMaximumVelocity.Text = FormatVelocity(analyzer.GetMaxVelocity());
            lblMaximumAcceleration.Text = FormatAcceleration(analyzer.GetMaximumAcceleration());
            lblMaximumDeceleration.Text = FormatAcceleration(analyzer.GetMaximumDeceleration());
        }

        public static string FormatTime(double seconds)
        {
            int s = Convert.ToInt32(seconds);
            if (s / 60 < 1)
                return String.Format("{0}s", seconds);
            if (s / 3600 < 1)
                return String.Format("{0}m {1}s", s / 60, s % 60);
            if (s / 3600 / 24 < 1)
                return String.Format("{0}h {1}m {2}s", s / 3600, (s / 60) % 60, s % 60);

            return String.Format("{0}d {1}h {2}m {3}s", (s / 3600 / 24) % 24, (s / 3600) % 60, (s / 60) % 60, s % 60);
        }

        public static string FormatDistance(double meters)
        {
            return String.Format("{0:N2} ft ({1:N2} m)", MetersToFeet(meters), meters);
        }

        public static string FormatVelocity(double metersPerSecond)
        {
            return String.Format("{0:N2} MPH ({1:N2} m/s)", MPStoMPH(metersPerSecond), metersPerSecond);
        }

        public static string FormatAcceleration(double metersPerSecondPerSecond)
        {
            return String.Format("{0:N2} g ({1:N2} m/s^2)", MPSStoG(metersPerSecondPerSecond), metersPerSecondPerSecond);
        }

        public static string FormatCourse(double course)
        {
            string[] directions = new [] {"N", "NE", "E", "SE", "S", "SW", "W", "NW", "N"};

            int index = Convert.ToInt32((course + 23) / 45);
            return String.Format("{0:N2}&deg; {1}", course, directions[index]);
        }

        public static double MetersToFeet(double meters)
        {
            return meters*3.2808399;
        }

        public static double MPStoMPH(double metersPerSecond)
        {
            return metersPerSecond * 2.23693629;
        }

        public static double MPSStoG(double metersPerSecondPerSecond)
        {
            return metersPerSecondPerSecond * 0.101971621;
        }

        #endregion
    }
}
