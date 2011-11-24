using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPX;

namespace WebApplication
{
    public partial class _Default : System.Web.UI.Page
    {
        private string FilePath
        {
            get { return Session["FilePath"] as string; }
            set { Session["FilePath"] = value; }
        }

        private GPXFile GPX
        {
            get { return Session["GPX"] as GPXFile; }
            set { Session["GPX"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UploadClicked(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                //try
                //{
                    string filename = Path.GetFileName(fileUpload.FileName);

                    if (!fileUpload.FileName.ToUpper().EndsWith(".GPX"))
                    {
                        lblStatus.Text = "Upload status: Only .gpx files are accepted.";
                        pnlTracks.Visible = false;
                        pnlAnalysis.Visible = false;
                    }

                    FilePath = Server.MapPath("~/") + filename;
                    fileUpload.SaveAs(FilePath);
                    lblStatus.Text = "Upload status: File uploaded!";

                    LoadTracks();

                    pnlTracks.Visible = true;
                //}
                //catch (Exception ex)
                //{
                //    lblStatus.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                //    pnlTracks.Visible = false;
                //    pnlAnalysis.Visible = false;
                //}
            }
            else
            {
                lblStatus.Text = "Upload status: No file selected";
                pnlTracks.Visible = false;
                pnlAnalysis.Visible = false;
            }
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
