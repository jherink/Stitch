using Stitch.Fonts.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    public enum OutlinesFormat
    {
        TrueType,
        cff
    };

    public interface IFontTable
    {
        void Parse( byte[] data, int offset );
    }

    public class Font
    {
        public OutlinesFormat OutlinesFormat { get; set; }

        public GlyphNames GlyphNames { get; internal set; }

        public GlyphSet GlyphSet { get; internal set; } = new GlyphSet();

        #region Font Tables

        internal HeadFontTable head;
        internal HheaFontTable hhea;
        internal CMapFontTable cmap;
        internal OS2FontTable os2;
        internal MaxPFontTable maxp;
        internal PostFontTable post;
        internal LocaFontTable loca;
        internal GlyfFontTable glyf;
        internal KernFontTable kern;
        internal NameFontTable name;
        internal short[] cvt;
        internal byte[] fpgm;
        internal byte[] prep;


        #endregion

        #region Ghosts

        public short Ascender
        {
            get
            {
                return ( hhea != null ? hhea.Ascender : default( short ) );
            }
        }

        public short Descender
        {
            get
            {
                return ( hhea != null ? hhea.Descender : default( short ) );
            }
        }

        public ushort NumberOfHMetrics
        {
            get
            {
                return ( hhea != null ? hhea.NumberOfHMetrics : default( ushort ) );
            }
        }

        public ushort UnitsPerEm
        {
            get
            {
                return ( head != null ? head.UnitsPerEm : default(ushort) );
            }
        }

        public ushort NumGlyphs
        {
            get
            {
                return ( maxp != null ? maxp.NumGlyphs : default(ushort) );
            }
        }

        public IReadOnlyDictionary<string, short> KerningPairs
        {
            get
            {
                return ( kern != null ? kern.KernTable : default( IReadOnlyDictionary<string, short> ) );
            }
        }

        #endregion

        #region Methods

        public double GetFontScale(double fontSize )
        {
            return 1.5 / UnitsPerEm * fontSize;
        }

        internal void MapGlyphNames()
        {
            var glyph = default( Glyph );
            var glyphIndexMap = cmap.GlyphIndexMap;
            var charCodes = glyphIndexMap.Keys.ToArray();
            
            for (int i = 0; i < charCodes.Length; i++ )
            {
                var c = charCodes[i];
                var glyphIndex = glyphIndexMap[c];
                glyph = GlyphSet[ glyphIndex ];
                glyph.AddUnicode( c );
            }

            for (uint i = 0; i < glyf.Glyphs.Count; i++ )
            {
                glyph = GlyphSet[ i ];
                
            }
        }

        /// <summary>
        /// Calculate the approximate size of the given string at the specified font size.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="fontSize">The font size.</param>
        /// <returns>A tuple (width, height) of the string size.</returns>
        public Tuple<float, float> MeasureString( string str, double fontSize )
        {
            var width = 0f;
            var height = 0f;
            var scale = (float)GetFontScale( fontSize );
            for (int i = 0; i < str.Length; i++ )
            {
                var glyph = GlyphSet.LookupGlyph( str[i] );
                //var _width = Math.Abs( glyph.XMax - glyph.XMin );
                //var _height = Math.Abs( glyph.YMax - glyph.YMin );
                var _width = glyph.AdvanceWidth;
                var _height = head.YMax;
                width += scale * _width;
                height = Math.Max(height, scale * _height );
            }
            return new Tuple<float, float>( width, height );
        }

        #endregion

    }
}
