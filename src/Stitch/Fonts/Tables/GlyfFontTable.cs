using System.Collections.Generic;

namespace Stitch.Fonts.Tables
{
    internal sealed class GlyfFontTable : FontTable
    {
        private readonly LocaFontTable Loca;
        public readonly Dictionary<uint, Glyph> Glyphs = new Dictionary<uint, Glyph>();

        public GlyfFontTable( byte[] data, uint offset, LocaFontTable loca ) : base( data, offset )
        {
            Loca = loca;
            Parse();
        }

        protected override void Parse()
        {
            if (Loca != null )
            {
                for ( uint i = 0; i < Loca.GlyphOffsets.Count - 1; i++ )
                {
                    var offset = Loca.GlyphOffsets[(int)i];
                    var nextOffset = Loca.GlyphOffsets[(int)i + 1];
                    Glyph glyph;
                    if ( offset != nextOffset )
                    {
                        glyph = new Glyph( i );
                    }
                    else
                    {
                        glyph = new Glyph( i );
                    }

                    Glyphs.Add( i, glyph );
                }
            }
        }
    }
}
