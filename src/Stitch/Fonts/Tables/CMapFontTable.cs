using Stitch.StitchMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// This table defines mapping of character codes to a default glyph index. Different subtables may be defined that each content mappings for different character encoding schemes. The table header indicates the character encodings for which subtables are present.
    ///Regardless of the encoding scheme, character codes that do not correspond to any glyph in the font should be mapped to glyph index 0. The glyph at this location must be a special glyph representing a missing character, commonly known as .notdef.
    ///Each subtable is in one of seven possible formats and begins with a format field indicating the format used.The first four formats — formats 0, 2, 4 and 6 — were originally defined prior to Unicode 2.0. These formats allow for 8-bit single-byte, 8-bit multi-byte, and 16-bit encodings. With the introduction of supplementary planes in Unicode 2.0, the Unicode addressable code space extends beyond 16 bits.To accommodate this, three additional formats were added — formats 8, 10 and 12 — that allow for 32-bit encoding schemes.
    ///Other enhancements in Unicode led to the addition of other subtable formats.Subtable format 13 allows for an efficient mapping of many characters to a single glyph; this is useful for “last-resort” fonts that provide fallback rendering for all possible Unicode characters with a distinct fallback glyph for different Unicode ranges.Subtable format 14 provides a unified mechanism for supporting Unicode variation sequences.
    /// https://www.microsoft.com/typography/otspec/cmap.htm
    /// </summary>
    internal sealed class CMapFontTable : FontTable
    {
        public ushort Version { get; private set; }
        public ushort NumTables { get; private set; }
        public ushort Format { get; private set; }
        public uint Length { get; private set; }
        public uint Language { get; private set; }
        public uint GroupCount { get; private set; }
        public ushort SegCount { get; private set; }
        public IReadOnlyDictionary<uint, uint> GlyphIndexMap { get; private set; }

        public CMapFontTable( byte[] data, uint offset ) : base( data, offset )
        {
        }

        protected override void Parse()
        {
            uint offset = 0;
            uint start = Offset;
            Version = ByteParser.GetUShort( Data, Offset );
            if ( Version != 0 ) throw new Exception( "CMap Version should be 0." );
            NumTables = ByteParser.GetUShort( Data, Offset + sizeof( ushort ) );

            uint idx = 2 * sizeof( ushort );
            uint width = 4 * sizeof( ushort );
            for ( ushort i = (ushort)( NumTables - 1 ); i >= 0; i-- )
            {
                var platformId = ByteParser.GetUShort( Data, Offset + idx + ( i * width ) );
                var encodingId = ByteParser.GetUShort( Data, Offset + idx + ( i * width ) + sizeof( ushort ) );
                if ( platformId == 3 && ( encodingId == 0 || encodingId == 1 || encodingId == 10 ) )
                {
                    offset = ByteParser.GetUInt( Data, Offset + idx + ( i * width ) + 2 * sizeof( ushort ) );
                    break;
                }
                GetUInt(); // read so we stay on track.
            }

            if ( Compare.AreEqual( offset, -1 ) )
            {
                throw new Exception( "No valid cmap sub-tables found." );
            }
            Offset = start + offset; // reset to appropriate location

            Format = GetUShort();

            if ( Compare.AreEqual( Format, 12 ) )
            {
                ParseCmapFormat12();
            }
            else if ( Compare.AreEqual( Format, 4 ) )
            {
                ParseCmapFormat4( start, offset );
            }
            else
            {
                throw new Exception( $"Only format 4 and 12 cmap tables are supported (found format {Format})." );
            }
        }

        private void ParseCmapFormat12()
        {
            Skip( sizeof( ushort ) ); // skip reserved.

            Length = GetUInt();
            Language = GetUInt();

            GroupCount = GetUInt();
            var glyphIndexMap = new Dictionary<uint, uint>();
            for ( uint i = 0; i < GroupCount; i++ )
            {
                var startCharCode = GetUInt();
                var endCharCode = GetUInt();
                var startGlyphId = GetUInt();
                for ( var c = startCharCode; c <= endCharCode; c++ )
                {
                    glyphIndexMap.Add( c, startGlyphId++ );
                }
            }
            GlyphIndexMap = glyphIndexMap;
        }

        private void ParseCmapFormat4( uint start, uint offset )
        {
            Length = GetUShort();
            Language = GetUShort();

            // segCount is stored x 2.
            SegCount = (ushort)( GetUShort() >> 1 );

            // skip searchRange, engtySelector, rangeShift
            Skip( sizeof( ushort ), 3 );

            var glyphIndexMap = new Dictionary<uint, uint>();
            var endCountIndex = start + offset + 14;
            var startCountIndex = (uint)( start + offset + 16 + SegCount * 2 );
            var idDeltaIndex = (uint)( start + offset + 16 + SegCount * 4 );
            var idRangeOffsetIndex = (uint)( start + offset + 16 + SegCount * 6 );
            var glyphIndexOffset = (uint)( start + offset + 16 + SegCount * 8 );
            for ( var i = 0; i < SegCount - 1; i++ )
            {
                var endCount = ByteParser.GetUShort( Data, endCountIndex );
                endCountIndex += sizeof( ushort );
                var startCount = ByteParser.GetUShort( Data, startCountIndex );
                startCountIndex += sizeof( ushort );
                var idDelta = ByteParser.GetShort( Data, idDeltaIndex );
                idDelta += sizeof( short );
                var idRangeOffset = ByteParser.GetUShort( Data, idRangeOffsetIndex );
                idRangeOffsetIndex += sizeof( ushort );

                ushort glyphIndex;
                for ( var c = startCount; c <= endCount; c++ )
                {
                    if ( idRangeOffset != 0 )
                    {
                        // The idRangeOffset is relative to the current index in the 
                        // idRangeOffset array. Take the current offset in the 
                        // idRangeOffset array.
                        glyphIndexOffset = (uint)( idRangeOffset - sizeof( ushort ) );

                        // Add the value of the idRangeOffset, 
                        // which will move us into the glyphIndex array.
                        glyphIndexOffset += idRangeOffset;

                        // Then add the character index of the current segment, 
                        // multiplied by 2 for USHORTs.
                        glyphIndexOffset += (uint)( ( c - startCount ) * sizeof( ushort ) );

                        glyphIndex = ByteParser.GetUShort( Data, glyphIndexOffset );
                        if ( glyphIndex != 0 )
                        {
                            glyphIndex = (ushort)( ( glyphIndex + idDelta ) & 0xFFFF );
                        }
                    }
                    else
                    {
                        glyphIndex = (ushort)( ( c + idDelta ) & 0xFFFF );
                    }

                    glyphIndexMap.Add( c, glyphIndex );
                }
            }

            GlyphIndexMap = glyphIndexMap;
        }
    }
}
