using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Export
{
    /// <summary>
    /// HTML to PDF exporter using WkHtmlToPdf.
    /// </summary>
    public class PDFExporter : IExporter
    {
        // I found a c# wrapper for WkHtmlToPdf by NReco
        // Main page here: https://www.nrecosite.com/pdf_generator_net.aspx

        public byte[] Export( string content )
        {
            var converter = new NReco.PdfGenerator.HtmlToPdfConverter();
            return converter.GeneratePdf( content );
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
