using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stitch.Chart
{
    internal static class GraphicsHelper
    {
        // These match the W3 defaults.
        internal static string DefaultFont = "Verdana";
        internal static double DefaultFontSize = 15;
        
        public static double MeasureStringWidth( string text, double fontSize = -1, string fontName = "" )
        {
            return MeasureString( text, fontSize, fontName ).Width;
        }

        public static double MeasureStringHeight( string text, double fontSize = -1, string fontName = "" )
        {
            return MeasureString( text, fontSize, fontName ).Height;
        }

        public static double MeasureStringWidth( IEnumerable<string> text, double fontSize = -1, string fontName = "" )
        {
            return MeasureString( text, fontSize, fontName ).Width;
        }

        public static double MeasureStringHeight( IEnumerable<string> text, double fontSize = -1, string fontName = "" )
        {
            return MeasureString( text, fontSize, fontName ).Height;
        }

        public static double MeasureStringWidth( string text, ITextStyle textStyle )
        {
            return MeasureString( text, textStyle.FontSize, textStyle.FontName ).Width;
        }

        public static double MeasureStringHeight( string text, ITextStyle textStyle )
        {
            return MeasureString( text, textStyle.FontSize, textStyle.FontName ).Height;
        }

        public static double MeasureStringWidth( IEnumerable<string> text, ITextStyle textStyle )
        {
            return MeasureString( text, textStyle.FontSize, textStyle.FontName ).Width;
        }

        public static double MeasureStringHeight( IEnumerable<string> text, ITextStyle textStyle )
        {
            return MeasureString( text, textStyle.FontSize, textStyle.FontName ).Height;
        }

        public static SizeF MeasureString( IEnumerable<string> text, double fontSize = -1, string fontName = "" )
        {
            if (text.Any())
            {
                var limitText = text.OrderBy( t => t.Length ).FirstOrDefault();
                return MeasureString( limitText, fontSize, fontName );
            }
            return new SizeF(0,0);
        }

        public static SizeF MeasureString( string text, double fontSize = -1, string fontName = "" )
        {
            if (fontSize <= 0) fontSize = DefaultFontSize;
            if (string.IsNullOrWhiteSpace( fontName )) fontName = DefaultFont;
            if (text == null) text = string.Empty;

            var size = new SizeF( Convert.ToSingle( text.Length * fontSize ), Convert.ToSingle( fontSize ) );
            using (var g = Graphics.FromHwnd( IntPtr.Zero ))
            {
                size = g.MeasureString( text, new Font( fontName, (float)fontSize ) );
            }

            return size;
        }
    }
}
