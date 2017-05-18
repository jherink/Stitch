using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    internal static class GraphicsHelper
    {
        public static double MeasureString(string text, string fontName, double fontSize)
        {
            var width = text.Length * fontSize;
            using (var g = Graphics.FromHwnd( IntPtr.Zero ))
            {
                var size = g.MeasureString( text, new Font( fontName, (float)fontSize ) );
            }
            return width;
        }
    }
}
