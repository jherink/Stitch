using Stitch.Fonts;
using System;
using Xunit;

namespace Stitch.Tests
{
    public class FontTests
    {
        [Theory( DisplayName = "Os2FontTableTest" )]
        [InlineData("Roboto-Black.ttf", "GOOG", 1082, 1206 )]
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
        public void CvtFontTableTest( string fontName, int cvtSize )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( cvtSize, result.cvt.Length );
        }

        [Theory( DisplayName = "FpgmFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 444 )]
        public void FpgmFontTableTest( string fontName, int cvtSize )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( cvtSize, result.fpgm.Length );
        }

        [Theory( DisplayName = "HeadFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 2048, -1476, -555, 2482, 2163 )]
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
        public void PrepFontTableTest( string fontName, int prepSize)
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( prepSize, result.prep.Length );
        }

        [Theory( DisplayName = "PostFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", 0 )]
        public void PostFontTableTest( string fontName, int glyphNamesCount )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( glyphNamesCount, result.GlyphNames.Count );
        }

        [Theory( DisplayName = "NameFontTableTest" )]
        [InlineData( "Roboto-Black.ttf", "Google", "Roboto Black", "Roboto-Black" )]
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
        public void GlyfFontTableTest( string fontName, uint numGlyphs )
        {
            var font = IntegrationHelpers.GetFontPath( fontName );
            var parser = new OpenTypeParser();
            var result = parser.Parse( System.IO.File.ReadAllBytes( font ) );
            Assert.Equal( numGlyphs - 1, (uint)result.glyf.Glyphs.Count );
        }
    }
}
