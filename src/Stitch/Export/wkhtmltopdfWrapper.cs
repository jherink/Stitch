using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Export
{
    // This class uses wkhtmltopdf to convert html content into pdf.
    // For more detail see here: https://wkhtmltopdf.org/

    internal class wkhtmltopdfWrapper
    {
        private string wkhtmltopdfResource = string.Empty;
        private readonly Assembly WorkingAssembly;
        private readonly string WorkingRoot = string.Empty;

        private string Extractwkhtmltopdf()
        {
            var outputPath = Path.Combine( WorkingRoot, "wkhtmltopdf.exe" );
            if (File.Exists( outputPath )) return outputPath;
            using (var resourceStream = WorkingAssembly.GetManifestResourceStream( "Stitch.Export.wkhtmltopdf.exe" ))
            {
                var data = new BinaryReader( resourceStream ).ReadBytes( (int)resourceStream.Length );     
                File.WriteAllBytes( outputPath, data );
            }
            return outputPath;
        }    

        public wkhtmltopdfWrapper()
        {
            WorkingAssembly = Assembly.GetAssembly( typeof( wkhtmltopdfWrapper ) );
            WorkingRoot = Path.GetDirectoryName( WorkingAssembly.Location );
            wkhtmltopdfResource = Extractwkhtmltopdf();
        }

        public byte[] Convert( string content ) {
            var data = new byte[] { };
            //var root = Path.Combine( Path.GetTempPath(), "wkhtmltopdfSandbox" );
            var root = WorkingRoot;
            var writeTo = Path.Combine( root, $"in.html" );
            var outputTo = Path.Combine( root, $"out.pdf" );
            //if (!Directory.Exists( root )) Directory.CreateDirectory( root );
            File.WriteAllText( writeTo, content );
            using (var p = new Process())
            {
                // https://bsmadhu.wordpress.com/2012/03/19/embedding-c-libraryexe-inside-net-assembly/
                p.StartInfo = new ProcessStartInfo()
                {
                    Arguments = $"\"{writeTo}\" \"{outputTo}\"",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    //FileName = @"C:\Github Src\Stitch\src\ThirdParty\wkhtmltopdf.exe", // WIP
                    FileName = wkhtmltopdfResource
                };
                p.Start();
                if (p.WaitForExit( 10000 ))
                {
                    data = File.ReadAllBytes( outputTo );
                }
            }
            if (File.Exists( writeTo )) File.Delete( writeTo );
            if (File.Exists( outputTo )) File.Delete( outputTo );

            return data;
        }
    }
}
