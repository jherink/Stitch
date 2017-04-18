using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Graph
{
    public class LineGraph : SVG
    {

        private class LineGraphLine
        {
            public readonly string Name;
            public List<Tuple<double, string>> Values { get; set; } = new List<Tuple<double, string>>();
            public string Color { get { return Polyline.Stroke; } set { Polyline.Stroke = value; } }
            public int StrokeWidth { get { return Polyline.StrokeWidth; } set { Polyline.StrokeWidth = value; } }
            public SVGPolyline Polyline { get; set; } = new SVGPolyline()
            {
                Stroke = "black",
                StrokeWidth = 3,
                Fill = "none"
            };

            public LineGraphLine( string name ) { Name = name; }
            public double MaxValue { get { return Values.Max( t => t.Item1 ); } }
            public double MinValue { get { return Values.Min( t => t.Item1 ); } }
        }

        public int GraphHeight { get { return (int)Height; } set { Height = value; } }
        public int GraphWidth { get { return (int)Width; } set { Width = value; } }

        public int Start { get; set; } = 0;

        private readonly Dictionary<string, LineGraphLine> Lines = new Dictionary<string, LineGraphLine>();

        public LineGraph() : base( 200, 600 )
        {
        }

        public void AddData( string line, double yAxisValue, string xAxisValue = "" )
        {
            if (!Lines.ContainsKey( line ))
            {
                Lines.Add( line, new LineGraphLine( line ) );
                Children.Add( Lines[line].Polyline );
            }

            Lines[line].Values.Add( new Tuple<double, string>( yAxisValue, xAxisValue ) );
            CalculateLinePoints();
        }

        public void SetLineColor( string line, string color )
        {
            if (!Lines.ContainsKey( line ))
            {
                Lines.Add( line, new LineGraphLine( line ) );
                Children.Add( Lines[line].Polyline );
            }

            Lines[line].Color = color;
        }

        public void SetLineThickness( string line, int thickness )
        {
            if (!Lines.ContainsKey( line ))
            {
                Lines.Add( line, new LineGraphLine( line ) );
                Children.Add( Lines[line].Polyline );
            }

            Lines[line].StrokeWidth = thickness;
        }

        private void CalculateLinePoints()
        {
            // max Y value of all lines.  Use this to figure a scale.
            var maxMax = Lines.Max( t => t.Value.MaxValue );
            var minMax = Lines.Min( t => t.Value.MinValue );
            foreach (var line in Lines)
            {
                var gLine = line.Value;
                gLine.Polyline.Points.Clear();
                var xInterval = (GraphWidth * .95) / gLine.Values.Count;

                double x = 0, y = 0;

                foreach (var pt in gLine.Values)
                {
                    if (maxMax == minMax)
                    {
                        y = GraphHeight - ((pt.Item1 - minMax) * (GraphHeight - Start)) + Start;
                    }
                    else {
                        y = GraphHeight - (((pt.Item1 - minMax) * (GraphHeight - Start)) / (maxMax - minMax)) + Start;
                    }

                    gLine.Polyline.Add( new SVGPoint( (int)x, (int)y ) );

                    x += xInterval;
                }
            }
        }

        public override string Render()
        {
            CalculateLinePoints();
            return base.Render();
        }

    }
}
