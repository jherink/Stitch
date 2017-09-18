using System;
using Stitch.Elements.Interface;
using Stitch.Elements;
using System.Collections.Generic;
using Xunit;

namespace Stitch.Tests
{
    public class ThemeTests
    {
        private static List<string> DefaultColors = new List<string>() {
            "#3366cc", // "mariner"
            "#dc3912", // "tia maria"
            "#ff9900", // "orange peel"
            "#109618", // "la palma"
            "#5574a6", // "wedgewood"
            "#8b0707", // "totem pole"
            "#6633cc", // "purple heart"
            "#e67300", // "mango tango"
            "#dd4477", // "cabaret"
            "#66aa00", // "limeade"
            "#316395", // "azure"
            "#994499", // "plum"
            "#22aa99", // "jungle green"
            "#aaaa11", // "sahara"
            "#b77322", // "bourbon"
            "#329262", // "sea green"
            "#16d620", // "malachite"
            "#0099c6", // "pacific blue"
            "#990099", // "flirt"
            "#651067", // "scarlet gum"
            "#3b3eac", // "governor bay"            
            "#b82e2e", // "tall poppy"
            };

        [Fact]
        public void GenerateColorSwatch()
        {
            var doc = new StitchDocument();
            var div = new Div();
            div.StyleList.Add( "width", "100%" );
            div.StyleList.Add( "height", "100px" );
            var colors = DefaultColors;
            var themes = Enum.GetNames( typeof( Theme ) );
            var i = 0;

            foreach (var color in colors)
            {
                if (themes[i] == "Plum") doc.CreatePage();
                var c = div.Clone() as IDivElement;
                c.StyleList.Add( "background", color );
                c.Add( new Paragraph( color ) );
                var p2 = new Paragraph( color );
                var p3 = new Paragraph( themes[i++] );
                p2.StyleList.Add( "color", "#fff" );
                c.Add( p2, p3 );
                doc.Add( c );
            }

            IntegrationHelpers.ExportPdfToTemp( "Default Themes Color Swatches", doc );
        }
    }
}
