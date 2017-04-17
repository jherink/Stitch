using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.CSS
{
    internal class CssThemeResourceLoader
    {
        public StyleSheet LoadTheme( string themeColor )
        {
            return LoadCssResource( $"w3-theme-{themeColor}" );
        }

        internal StyleSheet LoadCssResource( string resource )
        {
            StyleSheet sheet;
            resource = $"HydraDoc.CSS.Themes.{resource}";
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
