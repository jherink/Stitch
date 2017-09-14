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
            return 1.0 / UnitsPerEm * fontSize;
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
                glyph = GetGlyph( glyphIndex );
                glyph.AddUnicode( c );
            }

            for (uint i = 0; i < glyf.Glyphs.Count; i++ )
            {
                glyph = GetGlyph( i );
                
            }
        }

        public Glyph GetGlyph(uint glyphIndex)
        {
            if (!GlyphSet.ContainsKey(glyphIndex))
            {

            }
            return GlyphSet[glyphIndex];
        }

        public BoundingBox MeasureString( string str, double fontSize )
        {
            var width = 0f;
            var height = 0f;
            var scale = (float)GetFontScale( fontSize );
            for (int i = 0; i < str.Length; i++ )
            {
                var glyph = GetGlyph( str[i] );
                width += scale * glyph.AdvanceWidth;
                height = Math.Max(height, scale * glyph.YMax);
            }
            var box = new BoundingBox();
            box.AddPoint( width, height );
            return box;
        }

        #endregion

    }
}
