using Stitch.CSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Loaders
{
    internal class ThemeCssResourceLoader : CssResourceLoader
    {
        public StyleSheet LoadTheme( string themeName )
        {
            return LoadCssResource( $"Stitch.Themes.{themeName}" );
        }
    }
}
