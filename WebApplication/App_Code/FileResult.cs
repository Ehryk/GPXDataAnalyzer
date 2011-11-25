using System;
using System.Web.UI;
using GPX;

namespace WebApplication.App_Code
{
    public class FileResult
    {
        public int Number { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime Uploaded { get; set; }
    }
}
