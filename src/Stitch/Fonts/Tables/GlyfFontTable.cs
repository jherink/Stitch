using System.Collections.Generic;

namespace Stitch.Fonts.Tables
{
    internal sealed class GlyfFontTable : FontTable
    {
        private readonly LocaFontTable Loca;
        public readonly GlyphSet Glyphs = new GlyphSet();

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
                        glyph = new Glyph( i, Data, Offset + offset);
                        //glyph = new Glyph( i );
                    }
                    else
                    {
                        glyph = new Glyph( i, Data, Offset );
                    }

                    Glyphs.Add( i, glyph );
                }
            }
        }
    }
}
