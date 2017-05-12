using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stitch.Elements.Interface;
using Stitch.Elements;

namespace Stitch.Tests
{
    [TestClass]
    public class ThemeTests
    {
        [TestMethod]
        public void GenerateColorSwatch()
        {
            var doc = new StitchDocument();
            var cont = doc.AddBodyContainer();
            var div = new Div();
            div.StyleList.Add( "width", "90%" );
            div.StyleList.Add( "height", "100px" );
            var colors = Helpers.GetDefaultColors();

            foreach (var color in colors)
            {
                var c = div.Clone() as IDivElement;
                c.StyleList.Add( "background", color );
                c.Add( new Paragraph( color ) );
                var p2 = new Paragraph( color );
                p2.StyleList.Add( "color", "#fff" );
                c.Add( p2 );
                cont.Add( c );
            }

            IntegrationHelpers.SaveToTemp( "ColorSwatch", doc );
        }
    }
}
