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
    public partial class _Upload : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnUpload);

            if (!IsPostBack)
                BindFiles();

            lblStatus.Text = "Upload Status:";
        }

        protected void BindFiles()
        {
            List<FileResult> files = new List<FileResult>();

            int i = 1;
            foreach (string file in Directory.EnumerateFiles(Server.MapPath("~/Uploads/")))
            {
                FileInfo info = new FileInfo(file);
                files.Add(new FileResult { Name = Path.GetFileName(file), Path = file, Number = i, Size = info.Length, Uploaded = info.CreationTime });
                i++;
            }

            gvFiles.DataSource = files;
            gvFiles.DataBind();
        }

        protected void UploadClicked(object sender, EventArgs e)
        {
            if (fileUpload.FileName != string.Empty)
            {
                try
                {
                    string filename = Path.GetFileName(fileUpload.FileName);

                    if (fileUpload.FileName.ToUpper().EndsWith(".GPX"))
                    {
                        FilePath = Server.MapPath("~/Uploads/") + filename;
                        fileUpload.SaveAs(FilePath);
                        lblStatus.Text = "Upload status: File uploaded!";

                        BindFiles();
                    }
                    else
                    {
                        lblStatus.Text = "Upload status: Only .gpx files are accepted.";
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
            else
            {
                lblStatus.Text = "Upload status: No file selected";
            }
        }
    }
}
