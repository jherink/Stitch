using Stitch.Fonts;
using System;
using Xunit;

namespace Stitch.Tests
{
    public class FontTests
    {
        [Theory( DisplayName = "Os2FontTableTest" )]
        [InlineData( "Roboto-Black.ttf", "GOOG", 1082, 1206 )]
        [InlineData( "Verdana.ttf", "MS  ", 1117, 1041 )]
        public void Os2FontTableTest( string fontName, string achVendId, int sxHeight, int xAvgCharWidth )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( achVendId, result.os2.achVendID );
            Assert.Equal( sxHeight, result.os2.sxHeight );
            Assert.Equal( xAvgCharWidth, result.os2.xAvgCharWidth );
        }

        [Theory( DisplayName = "CmapFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 3, 12, 3004, 0, 249, 896 )]
        [InlineData( "Verdana.ttf", 2, 4, 1790, 0, 0, 900 )]
        public void CmapFontTableTest( string fontName, int numTables, ushort format, uint length, uint language, uint groupCount, int glyphMapSize )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( numTables, result.cmap.NumTables );
            Assert.Equal( format, result.cmap.Format );
            Assert.Equal( length, result.cmap.Length );
            Assert.Equal( language, result.cmap.Language );
            Assert.Equal( groupCount, result.cmap.GroupCount );
            Assert.Equal( glyphMapSize, result.cmap.GlyphIndexMap.Count );
        }

        [Theory( DisplayName = "CvtFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 45 )]
        [InlineData( "Verdana.ttf", 334 )]
        public void CvtFontTableTest( string fontName, int cvtSize )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( cvtSize, result.cvt.Length );
        }

        [Theory( DisplayName = "FpgmFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 444 )]
        [InlineData( "Verdana.ttf", 1244 )]
        public void FpgmFontTableTest( string fontName, int length )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( length, result.fpgm.Length );
        }

        [Theory( DisplayName = "HeadFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 2048, -1476, -555, 2482, 2163 )]
        [InlineData( "Verdana.ttf", 2048, -1146, -621, 3119, 2152 )]
        public void HeadFontTableTest( string fontName, int unitsPerEm, int xmin, int ymin, int xmax, int ymax )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( unitsPerEm, result.head.UnitsPerEm );
            Assert.Equal( xmin, result.head.XMin );
            Assert.Equal( ymin, result.head.YMin );
            Assert.Equal( xmax, result.head.XMax );
            Assert.Equal( ymax, result.head.YMax );
        }

        [Theory( DisplayName = "HheaFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 1900, -500, 1294 )]
        [InlineData( "Verdana.ttf", 2059, -430, 1397 )]
        public void HheaFontTableTest( string fontName, short ascender, short descender, ushort numberOfHMetrics )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( ascender, result.hhea.Ascender );
            Assert.Equal( descender, result.hhea.Descender );
            Assert.Equal( numberOfHMetrics, result.hhea.NumberOfHMetrics );
        }

        [Theory( DisplayName = "MaxpFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 1294, 143, 516 )]
        [InlineData( "Verdana.ttf", 1397, 121, 1260 )]
        public void MaxpFontTableTest( string fontName, uint numGlyphs, uint maxPoints, uint maxSizeInstructions )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( numGlyphs, result.maxp.NumGlyphs );
            Assert.Equal( maxPoints, result.maxp.MaxPoints );
            Assert.Equal( maxSizeInstructions, result.maxp.MaxSizeOfInstructions );
        }

        [Theory( DisplayName = "PrepFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 312 )]
        [InlineData( "Verdana.ttf", 1260 )]
        public void PrepFontTableTest( string fontName, int prepSize )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( prepSize, result.prep.Length );
        }

        [Theory( DisplayName = "PostFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 0 )]
        [InlineData( "Verdana.ttf", 0 )]
        public void PostFontTableTest( string fontName, int glyphNamesCount )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( glyphNamesCount, result.GlyphNames.Count );
        }

        [Theory( DisplayName = "NameFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", "Google", "Roboto Black", "Roboto-Black" )]
        [InlineData( "Verdana.ttf", "Matthew Carter", "Verdana", "Verdana" )]
        public void NameFontTableTest( string fontName, string designer, string fullName, string postScriptName )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( designer, result.name.Designer );
            Assert.Equal( fullName, result.name.FullName );
        }

        [Theory( DisplayName = "GlyfFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 1294 )]
        [InlineData( "Verdana.ttf", 1397 )]
        public void GlyfFontTableTest( string fontName, uint numGlyphs )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( numGlyphs, (uint)result.glyf.Glyphs.Count );
        }

        [Theory( DisplayName = "MeasureStringTest" )]
        [InlineData( "Verdana.ttf", "Watch TV", 107.742919921875, 23.642578125 )]
        [InlineData( "Roboto-Black.ttf", "Watch TV", 98.26171875, 23.763427734375 )]
        public void MeasureStringTest( string fontName, string test, double width, double height )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            var box = result.MeasureString( test, 15 );
            Assert.Equal( width, box.Item1, 3 );
            Assert.Equal( height, box.Item2, 3 );
        }
    }
}
