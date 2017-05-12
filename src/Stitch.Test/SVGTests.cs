using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stitch.Elements;
using System.Linq;

namespace Stitch.Tests
{
    [TestClass]
    public class SVGTests
    {
        [TestMethod]
        public void SVGRectangleTest()
        {
            var svg = new SVG()
            {
                Width = 400,
                Height = 110
            };
            var rect = new SVGRectangle()
            {
                Width = 300,
                Height = 100,
                Fill = "rgb(0,0,255)",
                StrokeWidth = 3,
                Stroke = "rgb(0,0,0)"
            };
            var doc = new StitchDocument();
            svg.Add( rect );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGRectangleTest", doc );
        }

        [TestMethod]
        public void SVGRectangle2Test()
        {
            var svg = new SVG( 180, 400 );
            var rect = new SVGRectangle()
            {
                Width = 150,
                Height = 150,
                Fill = "blue",
                StrokeWidth = 5,
                Stroke = "pink",
                StrokeOpacity = 0.9,
                FillOpacity = 0.1,
                X = 50,
                Y = 20
            };
            var doc = new StitchDocument();
            svg.Add( rect );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGRectangle2Test", doc );
        }

        [TestMethod]
        public void SVGRectangle3Test()
        {
            var svg = new SVG( 180, 400 );
            var rect = new SVGRectangle()
            {
                Width = 150,
                Height = 150,
                Fill = "blue",
                StrokeWidth = 5,
                Stroke = "pink",
                Opacity = .5,
                X = 50,
                Y = 20,
                Rx = 20,
                Ry = 20
            };
            var doc = new StitchDocument();
            svg.Add( rect );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGRectangle3Test", doc );
        }

        [TestMethod]
        public void SVGCircleTest()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 100, 100 );
            var circle = new SVGCircle
            {
                R = 40,
                Cx = 50,
                Cy = 50,
                Fill = "red",
                StrokeWidth = 3,
                Stroke = "black"
            };
            svg.Add( circle );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGCircleTest", doc );
        }

        [TestMethod]
        public void SVGEllipse1Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 140, 500 );
            var ellipse = new SVGEllipse
            {
                Cx = 200,
                Cy = 80,
                Rx = 100,
                Ry = 50,
                Fill = "yellow",
                StrokeWidth = 2,
                Stroke = "purple"
            };
            svg.Add( ellipse );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGEllipse1Test", doc );
        }

        [TestMethod]
        public void SVGEllipse2Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 150, 500 );
            var ellipse = new SVGEllipse() { Cx = 240, Cy = 100, Rx = 200, Ry = 30, Fill = "purple" };
            svg.Add( ellipse );
            ellipse = new SVGEllipse() { Cx = 220, Cy = 70, Rx = 190, Ry = 20, Fill = "lime" };
            svg.Add( ellipse );
            ellipse = new SVGEllipse() { Cx = 210, Cy = 45, Rx = 170, Ry = 15, Fill = "yellow" };
            svg.Add( ellipse );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGEllipse2Test", doc );
        }

        [TestMethod]
        public void SVGEllipse3Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 100, 500 );
            var ellipse = new SVGEllipse() { Cx = 240, Cy = 50, Rx = 220, Ry = 30, Fill = "yellow" };
            svg.Add( ellipse );
            ellipse = new SVGEllipse() { Cx = 220, Cy = 50, Rx = 190, Ry = 20, Fill = "white" };
            svg.Add( ellipse );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGEllipse3Test", doc );
        }

        [TestMethod]
        public void SVGLineTest()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 210, 500 );
            var line = new SVGLine( 0, 0, 200, 200 ) { Stroke = "rgb(255,0,0)", StrokeWidth = 2 };
            svg.Add( line );
            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGLineTest", doc );
        }

        [TestMethod]
        public void SVGPolygon1Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 210, 500 );
            var poly = new SVGPolygon() { Fill = "lime", Stroke = "purple", StrokeWidth = 1 };
            poly.Add( 200, 10 );
            poly.Add( 250, 190 );
            poly.Add( 160, 210 );
            svg.Add( poly );
            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGPolygon1Test", doc );
        }

        [TestMethod]
        public void SVGPolygon2Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 250, 500 );
            var poly = new SVGPolygon() { Fill = "lime", Stroke = "purple", StrokeWidth = 1 };
            poly.Add( 200, 10 );
            poly.Add( 300, 210 );
            poly.Add( 170, 250 );
            poly.Add( 123, 234 );
            svg.Add( poly );
            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGPolygon2Test", doc );
        }

        [TestMethod]
        public void SVGPolygon3Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 250, 500 );
            var poly = new SVGPolygon() { Fill = "lime", Stroke = "purple", StrokeWidth = 5, FillRule = "nonzero" };
            poly.Add( 100, 10 );
            poly.Add( 40, 198 );
            poly.Add( 190, 78 );
            poly.Add( 10, 78 );
            poly.Add( 160, 198 );
            svg.Add( poly );
            doc.Add( svg );

            svg = svg.Clone() as SVG;
            (svg.GetNodes( poly.Tag ).ToArray()[0] as SVGPolygon).FillRule = "evenodd";
            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGPolygon3Test", doc );
        }

        [TestMethod]
        public void SVGPolyLineTest()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 200, 500 );
            var line = new SVGPolyline() { Fill = "none", Stroke = "black", StrokeWidth = 3 };
            line.Add( 20, 20 );
            line.Add( 40, 25 );
            line.Add( 60, 40 );
            line.Add( 80, 120 );
            line.Add( 120, 140 );
            line.Add( 200, 180 );
            svg.Add( line );
            doc.Add( svg );

            svg = new SVG( 200, 500 );
            line = new SVGPolyline() { Fill = "white", Stroke = "red", StrokeWidth = 4 };
            line.Add( 0, 40 );
            line.Add( 40, 40 );
            line.Add( 40, 80 );
            line.Add( 80, 80 );
            line.Add( 80, 120 );
            line.Add( 120, 120 );
            line.Add( 120, 160 );
            svg.Add( line );
            doc.Add( svg );

            IntegrationHelpers.SaveToTemp( "SVGPolyLineTest", doc );
        }

        [TestMethod]
        public void SVGPath1Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 210, 400 );
            var path = new SVGPath();
            path.MoveTo( 150, 0 );
            path.LineTo( 75, 200 );
            path.LineTo( 255, 200 );
            path.ClosePath();
            svg.Add( path );
            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGPath1Test", doc );
        }

        [TestMethod]
        public void SVGPath2Test()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 400, 450 );
            var path = new SVGPath() { ID = "lineAB", Stroke = "red", StrokeWidth = 3, Fill = "none" };
            path.MoveTo( 100, 350 );
            path.LineTo( 150, -300, false );
            svg.Add( path );

            path = path.Clone() as SVGPath;
            path.Clear();
            path.ID = "lineBC";
            path.MoveTo( 250, 50 );
            path.LineTo( 150, 300, false );
            svg.Add( path );

            path = path.Clone() as SVGPath;
            path.Clear();
            path.Stroke = "green";
            path.MoveTo( 175, 200 );
            path.LineTo( 150, 0, false );
            svg.Add( path );

            path = new SVGPath() { Stroke = "blue", StrokeWidth = 5, Fill = "none" };
            path.MoveTo( 100, 350 );
            path.QuadraticBezierCurve( 150, -300, 300, 0, false );
            svg.Add( path );

            var group = new SVGGroup() { Stroke = "black", StrokeWidth = 3, Fill = "black" };
            group.Children.Add( new SVGCircle( 100, 350, 3 ) { ID = "pointA" } );
            group.Children.Add( new SVGCircle( 250, 50, 3 ) { ID = "pointB" } );
            group.Children.Add( new SVGCircle( 400, 350, 3 ) { ID = "pointC" } );
            svg.Add( group );

            group = new SVGGroup() { Stroke = "none", Fill = "black" };
            group.StyleList.Add( "font-size", "30" );
            group.StyleList.Add( "font-family", "sans-serif" );
            group.StyleList.Add( "text-anchor", "middle" );
            group.Children.Add( new SVGText( 100, 350, "A" ) { Dx = -30 } );
            group.Children.Add( new SVGText( 250, 50, "B" ) { Dy = -10 } );
            group.Children.Add( new SVGText( 400, 350, "C" ) { Dx = 30 } );

            svg.Add( group );

            doc.Add( svg );
            IntegrationHelpers.SaveToTemp( "SVGPath2Test", doc );
        }

        [TestMethod]
        public void SVGTextTest1()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 30, 200 );
            var text = new SVGText() { X = 0, Y = 15, Fill = "red" };
            text.Text = "I love SVG";
            svg.Add( text );
            doc.Add( svg );

            svg = new SVG( 60, 200 );
            text = new SVGText( "I love SVG" ) { X = 0, Y = 15, Fill = "red", Transform = "rotate(30 20,40)" };
            svg.Add( text );
            doc.Add( svg );

            IntegrationHelpers.SaveToTemp( "SVGTextTest1", doc );
        }

        [TestMethod]
        public void SVGTextTest2()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 30, 200 );
            var text = new SVGText() { X = 0, Y = 15, Fill = "red" };
            text.Text = "I love SVG";
            var a = new AnchorLinkElement();
            a.Href = "https://www.w3schools.com/graphics/";
            a.Children.Add( text );
            svg.Add( a );
            doc.Add( svg );

            IntegrationHelpers.SaveToTemp( "SVGTextTest2", doc );
        }

        [TestMethod]
        public void SVGTextTest3()
        {
            var doc = new StitchDocument();
            var svg = new SVG( 90, 200 );
            var text = new SVGText() { X = 10, Y = 20, Fill = "red" };
            text.Append( "Several lines:" );
            text.AppendLine( 10, 45, "First Line" );
            text.AppendLine( 10, 70, "Second Line" );            
            svg.Add( text );
            doc.Add( svg );

            IntegrationHelpers.SaveToTemp( "SVGTextTest3", doc );
        }

        [TestMethod]
        public void SVGStrokeTests()
        {
            var doc = new StitchDocument();

            // testing stroke
            var svg = new SVG( 80, 300 );
            var g = new SVGGroup();
            var path = new SVGPath() { Stroke = "red" };
            path.MoveTo( 5, 20 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { Stroke = "black" };
            path.MoveTo( 5, 40 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { Stroke = "blue" };
            path.MoveTo( 5, 60 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            svg.Add( g );
            doc.Add( svg );

            // testing stroke width.
            svg = new SVG( 80, 300 );
            g = new SVGGroup() { Stroke = "black", Fill = "none" };
            path = new SVGPath() { StrokeWidth = 2 };
            path.MoveTo( 5, 20 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { StrokeWidth = 4 };
            path.MoveTo( 5, 40 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { StrokeWidth = 6 };
            path.MoveTo( 5, 60 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            svg.Add( g );
            doc.Add( svg );

            // testing stroke line-cap
            svg = new SVG( 80, 300 );
            g = new SVGGroup() { Stroke = "black", Fill = "none", StrokeWidth = 6 };
            path = new SVGPath() { StrokeLineCap = "butt" };
            path.MoveTo( 5, 20 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { StrokeLineCap = "round" };
            path.MoveTo( 5, 40 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { StrokeLineCap = "square" };
            path.MoveTo( 5, 60 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            svg.Add( g );
            doc.Add( svg );

            // test stroke-dasharray
            svg = new SVG( 80, 300 );
            g = new SVGGroup() { Stroke = "black", Fill = "none", StrokeWidth = 4 };
            path = new SVGPath() { StrokeDashArray = new int[] { 5, 5 } };
            path.MoveTo( 5, 20 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { StrokeDashArray = new int[] { 10, 10 } };
            path.MoveTo( 5, 40 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            path = new SVGPath() { StrokeDashArray = new int[] { 20, 10, 5, 5, 5, 10 } };
            path.MoveTo( 5, 60 );
            path.LineTo( 215, 0, false );
            g.Add( path );

            svg.Add( g );
            doc.Add( svg );

            IntegrationHelpers.SaveToTemp( "SVGStrokeTest", doc );
        }

    }
}
