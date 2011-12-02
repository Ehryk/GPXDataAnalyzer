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
                pnlHiking.Visible = value;
                pnlJogging.Visible = value;
                pnlDownhill.Visible = value;
                pnlCrossCountry.Visible = value;
                pnlVehicle.Visible = value;
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

            lblSegmentCount.Text = String.Format(" ({0})", results.BetweenList.Count());

            gvSegments.DataSource = results.BetweenList;
            gvSegments.DataBind();

            lblTotalDistance.Text = results.TotalDistance.ToString();
            lblTotalVerticalDistance.Text = results.TotalVerticalDistance.ToString();
            lblTotalFlatEarthDistance.Text = results.TotalFlatEarthDistance.ToString();

            lblTotalTime.Text = results.TotalTime.ToString();

            lblAverageDistance.Text = results.AverageDistance.ToString();
            lblAverageVerticalDistance.Text = results.AverageVerticalDistance.ToString();
            lblAverageFlatEarthDistance.Text = results.AverageFlatEarthDistance.ToString();

            lblAverageTime.Text = results.AverageTime.ToString();
            lblAverageCourse.Text = results.AverageCourse.ToString();

            lblAverageVelocity.Text = results.AverageVelocity.ToString();
            lblAverageVerticalVelocity.Text = results.AverageVerticalVelocity.ToString();
            lblAverageFlatEarthVelocity.Text = results.AverageFlatEarthVelocity.ToString();

            pnlAnalysis.Visible = true;

            LoadDataAnalyzer(results.BetweenList);

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

        protected void ToggleDVA(object sender, EventArgs e)
        {
            Toggle(cpDVA, ibDVA);
        }

        protected void ToggleActivity(object sender, EventArgs e)
        {
            Toggle(cpActivity, ibActivity);
        }

        #endregion

        protected void ActivityChanged(object sender, EventArgs e)
        {
            pnlNotSure.Visible      = ddlActivity.SelectedValue == "NotSure";
            pnlHiking.Visible       = ddlActivity.SelectedValue == "Hiking";
            pnlJogging.Visible      = ddlActivity.SelectedValue == "Jogging";
            pnlDownhill.Visible     = ddlActivity.SelectedValue == "Downhill";
            pnlCrossCountry.Visible = ddlActivity.SelectedValue == "CrossCountry";
            pnlVehicle.Visible      = ddlActivity.SelectedValue == "Vehicle";
            pnlFlight.Visible       = ddlActivity.SelectedValue == "Flying";

            if (ddlActivity.SelectedValue == "Hiking") LoadHikingResults();
        }

        #endregion

        #region Methods

        protected void LoadDataAnalyzer(List<Between> pSegments)
        {
            analyzer = new DataAnalyzer(pSegments.Count);

            foreach (Between segment in pSegments)
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

        protected void LoadHikingResults()
        {
            lblHikingTotalTime.Text = analyzer.GetTotalTime().ToString();
        }

        #endregion
    }
}
