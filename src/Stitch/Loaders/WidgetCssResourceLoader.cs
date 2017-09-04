using Stitch.CSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Loaders
{
    internal class WidgetCssResourceLoader : CssResourceLoader
    {
        public StyleSheet LoadWidget( string themeName )
        {
            return LoadCssResource( $"Stitch.Widgets.{themeName}" );
        }
    }
}
