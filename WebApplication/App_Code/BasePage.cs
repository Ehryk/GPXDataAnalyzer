using System.Web.UI;
using GPX;

namespace WebApplication.App_Code
{
    public class BasePage : Page
    {
        protected string FilePath
        {
            get { return Session["FilePath"] as string; }
            set { Session["FilePath"] = value; }
        }

        protected GPXFile GPX
        {
            get { return Session["GPX"] as GPXFile; }
            set { Session["GPX"] = value; }
        }
    }
}
