using Stitch.Fonts.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    internal abstract class FontParser
    {
        // Based on opentype.js https://github.com/nodebox/opentype.js

        public Font Parse( byte[] buffer )
        {
            var font = new Font();
            var signature = ByteParser.GetTag( buffer, 0 );
            var ttComp = "0100";
            var tableEntries = default( TableEntry[] );
            var numTables = 0;

            if ( signature == ttComp || signature == "typ1" || signature == "typ1" )
            {
                font.OutlinesFormat = OutlinesFormat.TrueType;
                numTables = ByteParser.GetUShort( buffer, 4 );
                tableEntries = ParseFontTables( buffer, numTables );
            }
            else if ( signature == "OTTO" )
            {
                font.OutlinesFormat = OutlinesFormat.cff;
                numTables = ByteParser.GetUShort( buffer, 4 );
                tableEntries = ParseFontTables( buffer, numTables );
            }
            else if ( signature == "wOFF" )
            {
                var flavor = ByteParser.GetTag( buffer, 4 );
                if (flavor == ttComp)
                {
                    font.OutlinesFormat = OutlinesFormat.TrueType;
                }
                else if (flavor == "OTTO")
                {
                    font.OutlinesFormat = OutlinesFormat.cff;
                }
                else
                {
                    throw new NotImplementedException( $"Unsupported OpenType flavor {signature}" );
                }

                numTables = ByteParser.GetUShort( buffer, 12 );
                tableEntries = ParseFontTables( buffer, numTables );                
            }

            Tuple<byte[], uint> table;
            LtagFontTable ltag = default(LtagFontTable);

            var cffTableEntry = default( TableEntry );
            var fvarTableEntry = default( TableEntry );
            var glyfTableEntry = default( TableEntry );
            var gposTableEntry = default( TableEntry );
            var gsubTableEntry = default( TableEntry );
            var htmxTableEntry = default( TableEntry );
            var kernTableEntry = default( TableEntry );
            var locaTableEntry = default( TableEntry );
            var nameTableEntry = default( TableEntry );
            var metaTableEntry = default( TableEntry );

            // there are lots of tables to look at see here:
            // https://www.microsoft.com/typography/otspec/otff.htm#otttables
            foreach ( var tableEntry in tableEntries)
            {
                switch (tableEntry.Tag)
                {
                    case "cmap":
                        table = UncompressTable( buffer, tableEntry );
                        font.cmap = new CMapFontTable( table.Item1, table.Item2 );
                        break;
                    case "cvt ":
                        table = UncompressTable( buffer, tableEntry );
                        font.cvt = ByteParser.GetShortList( table.Item1, table.Item2, (uint)(tableEntry.Length / sizeof( short ) ));
                        break;
                    case "fvar":
                        fvarTableEntry = tableEntry;
                        break;
                    case "fpgm":
                        table = UncompressTable( buffer, tableEntry );
                        font.fpgm = ByteParser.GetByteList( table.Item1, table.Item2, tableEntry.Length );
                        break;
                    case "head":
                        table = UncompressTable( buffer, tableEntry );
                        font.head = new HeadFontTable( table.Item1, table.Item2 );
                        break;
                    case "hhea":
                        table = UncompressTable( buffer, tableEntry );
                        font.hhea = new HheaFontTable( table.Item1, table.Item2 );
                        break;
                    case "hmtx":
                        htmxTableEntry = tableEntry;
                        break;
                    case "ltag":
                        table = UncompressTable( buffer, tableEntry );
                        ltag = new LtagFontTable( table.Item1, table.Item2 );
                        break;
                    case "maxp":
                        table = UncompressTable( buffer, tableEntry );
                        font.maxp = new MaxPFontTable( table.Item1, table.Item2 );
                        break;
                    case "name":
                        nameTableEntry = tableEntry;
                        break;
                    case "OS/2":
                        table = UncompressTable( buffer, tableEntry );
                        font.os2 = new OS2FontTable( table.Item1, table.Item2 );
                        break;
                    case "post":
                        table = UncompressTable( buffer, tableEntry );
                        font.post = new PostFontTable( table.Item1, table.Item2 );
                        font.GlyphNames = new GlyphNames( font.post );
                        break;
                    case "prep":
                        table = UncompressTable( buffer, tableEntry );
                        font.prep = ByteParser.GetByteList( table.Item1, table.Item2, tableEntry.Length );
                        break;
                    case "glyf":
                        glyfTableEntry = tableEntry;
                        break;
                    case "loca":
                        locaTableEntry = tableEntry;
                        break;
                    case "CFF ":
                        cffTableEntry = tableEntry;
                        break;
                    case "kern":
                        kernTableEntry = tableEntry;
                        break;
                    case "GPOS":
                        gposTableEntry = tableEntry;
                        break;
                    case "GSUB":
                        gsubTableEntry = tableEntry;
                        break;
                    case "meta":
                        metaTableEntry = tableEntry;
                        break;
                }
            }

            table = UncompressTable( buffer, nameTableEntry );
            font.name = new NameFontTable( table.Item1, table.Item2, ltag );

            if (glyfTableEntry != default(TableEntry) && locaTableEntry != default( TableEntry ) )
            {
                var shortVersion = font.head.IndexToLocFormat == 0;
                table = UncompressTable( buffer, locaTableEntry );
                font.loca = new LocaFontTable( table.Item1, table.Item2, font.NumGlyphs, shortVersion );
                table = UncompressTable( buffer, glyfTableEntry );
                font.glyf = new GlyfFontTable( table.Item1, table.Item2, font.loca );
                font.GlyphSet = font.glyf.Glyphs;
            }
            else if (cffTableEntry != null )
            {
                table = UncompressTable( buffer, cffTableEntry );
                // not done.
            }
            else
            {
                throw new Exception( "Font doesn't contain TrueType or CFF outlines." );
            }

            table = UncompressTable( buffer, htmxTableEntry );
            var htmx = new HtmxFontTable( table.Item1, table.Item2, font.NumberOfHMetrics, font.NumGlyphs, font.GlyphSet );
            
            font.MapGlyphNames();

            if ( kernTableEntry != null )
            {
                table = UncompressTable( buffer, kernTableEntry );
                font.kern = new KernFontTable( table.Item1, table.Item2 );
            }

            return font;
        }

        private Tuple<byte[], uint> UncompressTable( byte[] buffer, TableEntry tableEntry )
        {
            if (tableEntry.Compression )
            { // WOFF

                return new Tuple<byte[], uint>( null, 0 );
            }
            else
            {
                return new Tuple<byte[], uint>( buffer, tableEntry.Offset );
            }
        }

        protected abstract TableEntry[] ParseFontTables( byte[] buffer, int numTables );          
    }
}
