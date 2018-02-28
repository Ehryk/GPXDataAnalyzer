using System;

namespace WebApplication.App_Code
{
    public class FileResult
    {
        public int Number { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public long SizeKB { get { return Size / 1024; } }
        public DateTime Uploaded { get; set; }
    }
}
