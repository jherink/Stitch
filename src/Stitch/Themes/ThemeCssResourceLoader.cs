using Stitch.CSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Themes
{
    internal class ThemeCssResourceLoader
    {
        public StyleSheet LoadTheme( string themeName )
        {
            return LoadCssResource( $"Stitch.Themes.{themeName}" );
        }

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
