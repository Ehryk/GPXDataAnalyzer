using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPX;
using WebApplication.App_Code;

namespace WebApplication
{
    public partial class Default : BasePage
    {
        #region Properties

        protected DataAnalyzer Analyzer
        {
            get { return Session["DataAnalyzer"] as DataAnalyzer; }
            set { Session["DataAnalyzer"] = value; }
        }

        protected bool ActivityPanelsVisible
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

        protected string FileStatus
        {
            get
            {
                return lblFileStatus.Text;
            }
            set
            {
                lblFileStatus.Text = value;
                lblFileStatus.Visible = !String.IsNullOrWhiteSpace(value);
            }
        }

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFileDDL();
            }

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnSave);
        }

        #endregion

        #region Events

        protected void BindFileDDL()
        {
            ddlFiles.Items.Clear();
            ddlFiles.Items.Add(new ListItem(""));
            foreach (string file in Directory.EnumerateFiles(Server.MapPath("~/Uploads/")))
                ddlFiles.Items.Add(new ListItem(Path.GetFileName(file), file));

            pnlTracks.Visible = false;
            pnlAnalysis.Visible = false;
            pnlResults.Visible = false;
        }

        protected void FileChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlFiles.SelectedValue))
            {
                btnSave.Visible = false;
                pnlTracks.Visible = false;
                pnlAnalysis.Visible = false;
                pnlActivity.Visible = false;
                ActivityPanelsVisible = false;
                pnlResults.Visible = false;
                FileStatus = "Select a file.";
                return;
            }
            if (!File.Exists(ddlFiles.SelectedValue))
            {
                pnlTracks.Visible = false;
                pnlAnalysis.Visible = false;
                pnlActivity.Visible = false;
                ActivityPanelsVisible = false;
                pnlResults.Visible = false;
                FileStatus = "File not found.";
                return;
            }

            FileStatus = "";
            btnSave.Visible = true;

            FilePath = ddlFiles.SelectedValue;

            LoadTracks();
        }

        protected void SaveSelectedFile(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(FilePath);

            //Response.Redirect("~/Uploads/" + fileName);

            Response.ContentType = "Application/gpx+xml";
            Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", fileName));
            Response.TransmitFile(FilePath);
            Response.End();
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

            Analyzer = new DataAnalyzer(trackPoints);

            gvTrackPoints.DataSource = Analyzer.TrackPoints;
            gvTrackPoints.DataBind();

            lblSegmentCount.Text = String.Format(" ({0})", Analyzer.Segments.Count());

            gvSegments.DataSource = Analyzer.Segments;
            gvSegments.DataBind();

            lblTotalDistance.Text = FormatDistance(Analyzer.TotalDistance());
            lblTotalVerticalDistance.Text = FormatDistance(Analyzer.TotalVerticalDistance());
            lblTotalFlatEarthDistance.Text = FormatDistance(Analyzer.TotalFlatEarthDistance());

            lblTotalTime.Text = FormatTime(Analyzer.TotalTime());

            lblAverageDistance.Text = FormatDistance(Analyzer.AverageDistance());
            lblAverageVerticalDistance.Text = FormatDistance(Analyzer.AverageVerticalDistance());
            lblAverageFlatEarthDistance.Text = FormatDistance(Analyzer.AverageFlatEarthDistance());

            lblAverageTime.Text = FormatTime(Analyzer.AverageTime());
            lblAverageCourse.Text = FormatCourse(Analyzer.AverageCourse());

            lblAverageVelocity.Text = FormatVelocity(Analyzer.AverageVelocity());
            lblAverageVerticalVelocity.Text = FormatVelocity(Analyzer.AverageVerticalVelocity());
            lblAverageFlatEarthVelocity.Text = FormatVelocity(Analyzer.AverageFlatEarthVelocity());

            pnlAnalysis.Visible = true;

            pnlActivity.Visible = true;
            ActivityChanged(ddlActivity, EventArgs.Empty);
        }

        protected void ActivityChanged(object sender, EventArgs e)
        {
            ActivityPanelsVisible = false;

            if (ddlActivity.SelectedValue == "NotSure")
            {
                lblResultsTitle.Text = "Results";
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

            pnlResults.Visible = true;
        }

        #endregion

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

        protected void ToggleResults(object sender, EventArgs e)
        {
            Toggle(cpResults, ibResults);
        }

            protected void ToggleSpecific(object sender, EventArgs e)
            {
                Toggle(cpSpecific, ibSpecific);
            }

            protected void ToggleBasic(object sender, EventArgs e)
            {
                Toggle(cpBasic, ibBasic);
            }

        #endregion

        #region Methods

        protected void Show(Panel panel)
        {
            //Show it
            panel.Visible = true;
            ((ImageButton)panel.Controls[0]).ImageUrl = "~/Images/collapse.png";
        }

        protected void Hide(Panel panel)
        {
            //Hide it
            panel.Visible = false;
            ((ImageButton)panel.Controls[0]).ImageUrl = "~/Images/expand.png";
        }

        protected void Toggle(Panel panel, ImageButton imageButton)
        {
            bool showing = panel.Visible;

            if (showing)
            {
                //Hide it
                panel.Visible = false;
                imageButton.ImageUrl = "~/Images/expand.png";
            }
            else
            {
                //Show it
                panel.Visible = true;
                imageButton.ImageUrl = "~/Images/collapse.png";
            }
        }

        protected void LoadSlowResults(string pTitle)
        {
            lblResultsTitle.Text = pTitle;

            lblAverageHikeSpeed.Text = FormatVelocity(Analyzer.HikingSpeed());
            lblTotalHikeTime.Text = FormatTime(Analyzer.HikingTime());

            lblMinElevation.Text = FormatDistance(Analyzer.MinimumElevation());
            lblMaxElevation.Text = FormatDistance(Analyzer.MaximumElevation());

            lblStartElevation.Text = FormatDistance(Analyzer.StartElevation());
            lblEndElevation.Text = FormatDistance(Analyzer.EndElevation());

            lblUphillHikeSpeed.Text = FormatVelocity(Analyzer.AverageUpSpeed());
            lblDownhillHikeSpeed.Text = FormatVelocity(Analyzer.AverageDownSpeed());

            lblNumberHikingRests.Text = Analyzer.NumberHikingRests().ToString();
            lblTotalHikeRestTime.Text = FormatTime(Analyzer.HikingRestTime());
        }

        protected void LoadDownhillResults(string pTitle)
        {
            lblResultsTitle.Text = pTitle;
            lblNumberOfRuns.Text = Analyzer.NumberRuns().ToString();
            lblNumberOfLifts.Text = Analyzer.NumberLifts().ToString();
            lblNumberOfFalls.Text = Analyzer.NumberFalls().ToString();

            lblTotalDownhillDistance.Text = FormatDistance(Analyzer.SkiDistance());
            lblVerticalDistance.Text = FormatDistance(Analyzer.MaximumElevation() - Analyzer.MinimumElevation());

            lblAverageLiftSpeed.Text = FormatVelocity(Analyzer.AverageLiftSpeed());
            lblAverageSkiSpeed.Text = FormatVelocity(Analyzer.AverageSkiSpeed());
        }

        protected void LoadFastResults(string pTitle)
        {
            lblResultsTitle.Text = pTitle;
            lblTotalFastTime.Text = FormatTime(Analyzer.TotalTime());
            lblTotalFastDistance.Text = FormatDistance(Analyzer.TotalDistance());

            lblNumberStops.Text = Analyzer.NumberStops().ToString();
            lblMaxAcceleration.Text = FormatAcceleration(Analyzer.MaximumAcceleration());
            lblMaxDeceleration.Text = FormatAcceleration(Analyzer.MaximumDeceleration());

            lblVehicleRestTime.Text = FormatTime(Analyzer.VehicleRestTime());
            lblCoastTime.Text = FormatTime(Analyzer.CoastTime());
            lblAccelerationTime.Text = FormatAcceleration(Analyzer.AcceleratingTime());
            lblDecelerationTime.Text = FormatAcceleration(Analyzer.DeceleratingTime());
        }

        protected void LoadFlightResults(string pTitle)
        {
            lblResultsTitle.Text = pTitle;
            lblTotalFlightTime.Text = FormatTime(Analyzer.TotalTime());
            lblTotalFlightDistance.Text = FormatDistance(Analyzer.TotalDistance());

            lvlAverageFlightVelocity.Text = FormatVelocity(Analyzer.AverageVelocity());
            lblAverageClimbingVelocity.Text = FormatVelocity(Analyzer.AverageUpSpeed());
            lblAverageDescentVelocity.Text = FormatVelocity(Analyzer.AverageDownSpeed());
            
            lblMaximumVelocity.Text = FormatVelocity(Analyzer.MaximumVelocity());
            lblMinimumVelocity.Text = FormatVelocity(Analyzer.MinimumVelocity());
            lblMaximumAcceleration.Text = FormatAcceleration(Analyzer.MaximumAcceleration());
            lblMaximumDeceleration.Text = FormatAcceleration(Analyzer.MaximumDeceleration());
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
            return String.Format("{0:N2} ft ({1:N2} m), {2:N2} miles ({3:N2} km)", MetersToFeet(meters), meters, MetersToMiles(meters), MetersToKM(meters));
        }

        public static string FormatVelocity(double metersPerSecond)
        {
            return String.Format("{0:N2} MPH ({1:N2} m/s, {2:N2} km/h)", MPStoMPH(metersPerSecond), metersPerSecond, MPStoKMPH(metersPerSecond));
        }

        public static string FormatAcceleration(double metersPerSecondPerSecond)
        {
            return String.Format("{0:N2} g ({1:N2} m/s^2)", MPSPStoG(metersPerSecondPerSecond), metersPerSecondPerSecond);
        }

        public static string FormatCourse(double course)
        {
            string[] directions = new [] {"N", "NE", "E", "SE", "S", "SW", "W", "NW", "N"};

            int index = Convert.ToInt32((course + 23) / 45);
            return String.Format("{0:N2}&deg; ({1})", course, directions[index]);
        }

        public static double MetersToFeet(double meters)
        {
            return meters*3.2808399;
        }

        public static double MetersToKM(double meters)
        {
            return meters/1000;
        }

        public static double FeetToMiles(double feet)
        {
            return feet/5280;
        }

        public static double MetersToMiles(double meters)
        {
            return meters/1609.344;
        }

        public static double MPStoMPH(double metersPerSecond)
        {
            return metersPerSecond * 2.23693629;
        }

        public static double MPStoKMPH(double metersPerSecond)
        {
            return metersPerSecond * 3.6;
        }

        public static double MPSPStoG(double metersPerSecondPerSecond)
        {
            return metersPerSecondPerSecond * 0.101971621;
        }

        #endregion
    }
}
