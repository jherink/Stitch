using Stitch.CSS;
using System.IO;

namespace Stitch.Loaders
{
    internal abstract class CssResourceLoader
    {
        public StyleSheet LoadCssResource( string resource )
        {
            StyleSheet sheet;
            if (!resource.EndsWith( ".css" )) resource += ".css";
            using (var stream = GetType().Assembly.GetManifestResourceStream( resource ))
            {
                sheet = LoadCss( stream );
            }
            return sheet;
        }

        public StyleSheet LoadCss( string cssFile )
        {
            StyleSheet sheet;

            using (var stream = new FileStream( cssFile, FileMode.Open ))
            {
                sheet = LoadCss( stream );
            }

            return sheet;
        }

        public StyleSheet LoadCss( Stream stream )
        {
            StyleSheet sheet;

            using (var reader = new StreamReader( stream ))
            {
                sheet = new Parser().Parse( reader.ReadToEnd() );
            }

            return sheet;
        }
    }
}
