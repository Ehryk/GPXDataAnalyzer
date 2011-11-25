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
                return;
            }
            if (!File.Exists(ddlFiles.SelectedValue))
            {
                pnlTracks.Visible = false;
                pnlAnalysis.Visible = false;
                //lblStatus.Text = "File Doesn't Exist";
                return;
            }

            FilePath = ddlFiles.SelectedValue;

            LoadTracks();
            pnlTracks.Visible = true;
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
                return;
            }

            List<wptType> trackPoints = GPX.GetTrackFirstSegmentPoints(ddlTracks.SelectedValue);

            lblTrackPointCount.Text = String.Format(" ({0})", trackPoints.Count());

            TrackResults results = Loader.GetResults(trackPoints);

            gvTrackPoints.DataSource = results.TrackPoints;
            gvTrackPoints.DataBind();

            gvBetween.DataSource = results.BetweenList;
            gvBetween.DataBind();

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
        }
    }
}
