using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Graph
{
    public class ScatterPlot : SVG
    {
        private const string DEFAULT_FILL_COLOR = "black";
        private const int DEFAULT_PLOT_SIZE = 4;

        private class ScatterPoint
        {
            public SVGCircle Circle { get; set; } = new SVGCircle();

            public string Color
            {
                get
                { return Circle.Fill; }
                set { Circle.Fill = value; }
            }

            public int Radius { get { return Circle.R; } set { Circle.R = value; } }

            public int X { get; set; }
            public int Y { get; set; }

        };

        private readonly List<ScatterPoint> ScatterPoints = new List<ScatterPoint>();

        public string Color
        {
            get
            {
                return ScatterPoints.Any() ? ScatterPoints[0].Color : DEFAULT_FILL_COLOR;
            }
            set
            {
                foreach (var pt in ScatterPoints) pt.Color = value;
            }
        }

        public int PointRadius
        {
            get
            {
                return ScatterPoints.Any() ? ScatterPoints[0].Radius : DEFAULT_PLOT_SIZE;
            }
            set { foreach (var pt in ScatterPoints) pt.Radius = value; }
        }

        public int GraphHeight { get { return (int)Height; } set { Height = value; } }
        public int GraphWidth { get { return (int)Width; } set { Width = value; } }


        public ScatterPlot() : base( 600, 800 ) { }

        public void AddData( double xAxisValue, double yAxisValue )
        {
            var pt = new ScatterPoint()
            {
                Color = Color,
                Radius = PointRadius,
                X = (int)xAxisValue,
                Y = (int)yAxisValue
            };
            ScatterPoints.Add( pt );
            Children.Add( pt.Circle );
            CalculateScatterPoints();
        }

        private void CalculateScatterPoints()
        {
            // max Y value of all lines.  Use this to figure a scale.
            var maxY = ScatterPoints.Max( t => t.Y );
            var minY = ScatterPoints.Min( t => t.Y );
            var maxX = ScatterPoints.Max( t => t.X );
            var minX = ScatterPoints.Min( t => t.X );
            var fill = Color;
            var r = PointRadius;
            double x = 0, y = 0;

            foreach (var pt in ScatterPoints)
            {
                //if (minX != maxX)
                //{
                //    x = GraphWidth - (((pt.X - minX) * (GraphHeight - minX)) / (maxX - minX));
                //}
                //if (minY != maxY)
                //{
                //    y = GraphHeight - (((pt.Y - minY) * (GraphHeight - minY)) / (maxY - minY));
                //}

                pt.Circle.Cx = (int)x;
                pt.Circle.Cy = (int)y;
            }
        }

        public override string Render()
        {
            CalculateScatterPoints();
            return base.Render();
        }
    }
}
