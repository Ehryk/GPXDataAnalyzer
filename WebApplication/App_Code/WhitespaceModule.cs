using System;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
/// Removes whitespace from the webpage.
/// </summary>
public class WhitespaceModule : IHttpModule
{
    #region IHttpModule Members

    void IHttpModule.Dispose()
    {
        // Nothing to dispose; 
    }

    void IHttpModule.Init(HttpApplication context)
    {
        context.BeginRequest += new EventHandler(context_BeginRequest);
    }

    #endregion

    #region Events

    void context_BeginRequest(object sender, EventArgs e)
    {
        HttpApplication app = sender as HttpApplication;

        if (app.Request.RawUrl.Contains(".aspx") && app.Request.Headers["X-MicrosoftAjax"] != "Delta=true")
        {
            app.Response.Filter = new WhitespaceFilter(app.Response.Filter);
        } 
    }

    #endregion

    #region Stream filter

    private class WhitespaceFilter : Stream
    {
        #region Constructors

        public WhitespaceFilter(Stream sink)
        {
          _sink = sink;
        }

        #endregion
        
        //More Compression, has problems with AJAX Sometimes
        //private static Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");

        //95% of Compression, AJAX Safe
        private static Regex reg = new Regex(@"^\s+", RegexOptions.Multiline | RegexOptions.Compiled); 

        #region Properites

        private Stream _sink;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _sink.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position { get; set; }

        #endregion

        #region Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _sink.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _sink.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _sink.SetLength(value);
        }

        public override void Close()
        {
            _sink.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            string html = System.Text.Encoding.Default.GetString(buffer);

            html = reg.Replace(html, string.Empty);

            byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
            _sink.Write(outdata, 0, outdata.GetLength(0));
        }

        #endregion

    }

    #endregion
}
