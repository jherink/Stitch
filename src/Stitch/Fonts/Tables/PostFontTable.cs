using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// The "post" table
    /// This table contains additional information needed to use TrueType or OpenType™ fonts on PostScript printers. This includes data for the FontInfo dictionary entry and the PostScript names of all the glyphs. For more information about PostScript names, see the Adobe document Unicode and Glyph Names.
    ///Versions 1.0, 2.0, and 2.5 refer to TrueType fonts and OpenType fonts with TrueType data.OpenType fonts with TrueType data may also use Version 3.0. OpenType fonts with CFF data use Version 3.0 only.
    /// https://www.microsoft.com/typography/otspec/post.htm
    /// </summary>
    internal sealed class PostFontTable : FontTable
    {
        public float Version { get; private set; }
        public float ItalicAngle { get; private set; }
        public short UnderlinePosition { get; private set; }
        public short UnderlineThickness { get; private set; }
        public uint IsFixedPitch { get; private set; }
        public uint MinMemType42 { get; private set; }
        public uint MaxMemType42 { get; private set; }
        public uint MinMemType1 { get; private set; }
        public uint MaxMemType1 { get; private set; }
        public ushort NumGlyphs { get; private set; }
        public ushort[] GlyphNameIndex { get; private set; }
        public List<string> Names { get; private set; }
        public byte[] GlyphOffset { get; private set; }

        public PostFontTable( byte[] data, uint offset ) : base( data, offset )
        {
        }

        protected override void Parse()
        {
            Version = GetFixed();
            ItalicAngle = GetFixed();
            UnderlinePosition = GetShort();
            UnderlineThickness = GetShort();
            IsFixedPitch = GetUInt();
            MinMemType42 = GetUInt();
            MaxMemType42 = GetUInt();
            MinMemType1 = GetUInt();
            MaxMemType1 = GetUInt();            

            if ( Math.Abs( Version - 1.0 ) < .001 )
            { // Version 1
              /*
               * This version is used in order to supply PostScript glyph 
               * names when the font file contains exactly the 258 glyphs 
               * in the standard Macintosh TrueType font file 
               * (see 'post' Format 1 in Apple's specification for a list of 
               * the 258 Macintosh glyph names), 
               * and the font does not otherwise supply glyph names.
               * As a result, the glyph names are taken from the system with no 
               * storage required by the font.
               * 
               */
                Names.AddRange( Encodings.StandardNames );
            }
            else if ( Math.Abs( Version - 2.0 ) < .001 )
            { // Version 2
                NumGlyphs = GetUShort();
                GlyphNameIndex = new ushort[NumGlyphs];
                for (int i = 0; i < NumGlyphs; i++ )
                {
                    GlyphNameIndex[i] = GetUShort();
                }
                for (int i = 0; i < NumGlyphs; i++ )
                {
                    if (GlyphNameIndex[i] >= Encodings.StandardNames.Length )
                    {
                        var nameLength = (uint)GetByte();
                        Names.Add( GetString( nameLength ) );
                    }
                }
            }
            else if ( Math.Abs( Version - 2.5 ) < .001 )
            { // Version 2.5
                NumGlyphs = GetUShort();
                GlyphOffset = new byte[NumGlyphs];
                for (int i = 0; i < NumGlyphs; i++ )
                {
                    GlyphOffset[i] = GetByte();
                }
            }
        }
    }
}
