using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Export
{
    /// <summary>
    /// HTML to PDF exporter using WkHtmlToPdf.
    /// </summary>
    public sealed class PDFExporter : IExporter
    {
        public byte[] Export( string content )
        {
            var data = new wkhtmltopdfWrapper().Convert( content );
            return data;
        }

        public void Export( string content, Stream outputStream )
        {
            if (outputStream.CanWrite)
            {
                var data = Export( content );
                outputStream.Write( data, 0, data.Length );
            }
        }
    }
}
