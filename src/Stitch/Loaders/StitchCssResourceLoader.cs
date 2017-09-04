using Stitch.CSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Loaders
{
    internal class StitchCssResourceLoader : CssResourceLoader
    {
        public StyleSheet LoadTheme( string themeName )
        {
            return LoadCssResource( $"Stitch.{themeName}" );
        }
    }
}
