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
                var limitText = text.OrderByDescending( t => t.Length ).FirstOrDefault();
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

            // jherink 9/18/2017: Removed GDI+ call to measure string for .NET standard upgrade.
            //using (var g = Graphics.FromHwnd( IntPtr.Zero ))
            //{
            //    size = g.MeasureString( text, new Font( fontName, (float)fontSize ) );
            //}


            lock ( FontCache )
            {
                var font = LoadFont( fontName );
                if ( font != default( Fonts.Font ) )
                {
                    var _size = font.MeasureString( text, fontSize );
                    size.Width = _size.Item1;
                    size.Height = _size.Item2;
                }
            }

            return size;
        }

        private static Fonts.Font LoadFont(string name)
        {
            name = name.ToLowerInvariant();

            // try to get cached font first.
            if ( FontCache.ContainsKey( name ) )
            {
                return FontCache[name];
            }

            // not cached.  Try to find on OS.
            var fontsRoot = Environment.GetFolderPath( Environment.SpecialFolder.Fonts );
            var match = $"{name.ToString()}.*" ;
            var fontFile = System.IO.Directory.GetFiles( fontsRoot, match, System.IO.SearchOption.TopDirectoryOnly ).FirstOrDefault();
            var font = default( Fonts.Font );
            if ( !string.IsNullOrWhiteSpace( fontFile ) )
            {
                try
                {
                    var parser = new Fonts.OpenTypeParser();
                    font = parser.Parse( System.IO.File.ReadAllBytes( fontFile ) );
                }
                catch { /* Nope... */ }
            }

            FontCache.Add( name, font ); // cahce font.
            return font;
        }

        private static Dictionary<string, Fonts.Font> FontCache = new Dictionary<string, Fonts.Font>();
    }
}
