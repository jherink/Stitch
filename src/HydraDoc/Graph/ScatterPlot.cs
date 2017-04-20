using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Graph
{
    public class ScatterPlot : SVG
    {
        private string FillColor = "black";
        private double PlotSize = 4;

        private double AxisLimit
        {
            get
            {
                return RoundAxisValue( ScatterPoints.Max( t => t.Y ) );
            }
        }

        private class ScatterPoint
        {
            public SVGCircle Circle { get; set; } = new SVGCircle();

            public string Color
            {
                get
                { return Circle.Fill; }
                set { Circle.Fill = value; }
            }

            public double Radius { get { return Circle.R; } set { Circle.R = value; } }

            public int X
            {
                get
                {
                    if (Circle.Attributes.ContainsKey( "x-val" ))
                    {
                        return int.Parse( Circle.Attributes["x-val"] );
                    }
                    return 0;
                }
                set
                {
                    if (Circle.Attributes.ContainsKey( "x-val" ))
                    {
                        Circle.Attributes.Add( "x-val", value.ToString() );
                    }
                    else
                    {
                        Circle.Attributes["x-val"] = value.ToString();
                    }
                }
            }

            public int Y
            {
                get
                {
                    if (Circle.Attributes.ContainsKey( "y-val" ))
                    {
                        return int.Parse( Circle.Attributes["y-val"] );
                    }
                    return 0;
                }
                set
                {
                    if (Circle.Attributes.ContainsKey( "y-val" ))
                    {
                        Circle.Attributes.Add( "y-val", value.ToString() );
                    }
                    else
                    {
                        Circle.Attributes["y-val"] = value.ToString();
                    }
                }
            }

        };

        private readonly SVGGroup Data = new SVGGroup();
        private readonly SVGGroup YAxis = new SVGGroup();
        private readonly SVGGroup XAxis = new SVGGroup();
        private readonly SVGGroup XLabels = new SVGGroup();
        private readonly SVGGroup YLabels = new SVGGroup();

        private readonly List<ScatterPoint> ScatterPoints = new List<ScatterPoint>();

        public string Color
        {
            get
            {
                return ScatterPoints.Any() ? ScatterPoints[0].Color : FillColor;
            }
            set
            {
                FillColor = value;
                foreach (var pt in ScatterPoints) pt.Color = value;
            }
        }

        public double PointRadius
        {
            get
            {
                return ScatterPoints.Any() ? ScatterPoints[0].Radius : PlotSize;
            }
            set
            {
                PlotSize = value;
                foreach (var pt in ScatterPoints) pt.Radius = value;
            }
        }

        public int AxisIntervals { get; set; }

        public int GraphHeight { get { return (int)Height; } set { Height = value; } }
        public int GraphWidth { get { return (int)Width; } set { Width = value; } }
        public int Start { get; set; }

        public ScatterPlot() : base( 600, 800 )
        {
            AxisIntervals = (GraphHeight / 200) + 1;
            ClassList.Add( "graph" );
            Children.Add( XAxis );
            Children.Add( YAxis );
            Children.Add( XLabels );
            Children.Add( YLabels );
            Children.Add( Data );

            YLabels.ClassList.Add( "labels", "y-labels" );
            XLabels.ClassList.Add( "labels", "x-labels" );
            XAxis.ClassList.Add( "grid", "x-grid" );
            YAxis.ClassList.Add( "grid", "y-grid" );
        }

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
            Data.Add( pt.Circle );
            CalculateScatterPoints();
        }

        private void CalculateScatterPoints()
        {
            // max Y value of all lines.  Use this to figure a scale.
            var maxY = AxisLimit;//ScatterPoints.Max( t => t.Y ) * 1.1;
            var minY = ScatterPoints.Min( t => t.Y ) * .9;
            var maxX = ScatterPoints.Max( t => t.X );
            var minX = ScatterPoints.Min( t => t.X );
            var fill = Color;
            var r = PointRadius;
            double x = 0, y = 0;

            foreach (var pt in ScatterPoints)
            {
                if (maxY == minY)
                {
                    y = ((pt.Y - minY) * (GraphHeight - Start)) + Start;
                }
                else
                {
                    y = (((pt.Y - minY) * (GraphHeight - Start)) / (maxY - minY)) + Start;
                }

                if (maxX == minX)
                {
                    x = ((pt.X - minX) * (GraphWidth)) + Start;
                }
                else
                {
                    x = (((pt.X - minX) * (GraphWidth)) / (maxX - minX)) + Start;
                }

                pt.Circle.Cx = (int)x;
                pt.Circle.Cy = GraphHeight - (int)y;
            }
        }

        public override string Render()
        {
            CalculateScatterPoints();
            PopulateYAxis();
            return base.Render();
        }

        public void PopulateYAxis()
        {
            YLabels.Children.Clear();           

            var startX = (int)(.1 * GraphWidth);

            YAxis.Add( new SVGLine( startX, 5, startX, GraphHeight ) );

            var max = AxisLimit;
            var interval = max / AxisIntervals;

            for (int i = 0; i < AxisIntervals; i++)
            {
                var text = new SVGText();
                text.X = startX;
                text.Y = (AxisIntervals - i) * (GraphHeight / AxisIntervals);
                text.Text.Append( (interval * i).ToString() );
                YLabels.Add( text );
            }
        }

        private void PopulateXAxis()
        {
            YLabels.Children.Clear();

            var startX = (int)(.1 * GraphWidth);

            YAxis.Add( new SVGLine( startX, 5, startX, GraphHeight ) );

            var max = AxisLimit;
            var interval = max / AxisIntervals;

            for (int i = 0; i < AxisIntervals; i++)
            {
                var text = new SVGText();
                text.X = startX;
                text.Y = (AxisIntervals - i) * (GraphHeight / AxisIntervals);
                text.Text.Append( (interval * i).ToString() );
                YLabels.Add( text );
            }
        }

        private double RoundAxisValue( double axisValue )
        {
            var aAxisValue = Math.Abs( axisValue );
            if (aAxisValue <= 1)
            {
                return axisValue;
            }

            if (aAxisValue <= 10)
            {
                return (int)axisValue;
            }

            var power = Math.Ceiling( Math.Log10( axisValue / 10 ) );
            var p2 = axisValue;
            while (p2 > 10) p2 /= 10;
            var number = (int)((p2 + 1)) * Math.Pow( 10, power );
            if (number < aAxisValue)
            {
                number *= 2;
            }

            if (axisValue < 0) number *= -1;

            return number;
        }
    }
}
