using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HydraDoc.Elements.Interface;

namespace HydraDoc.Graph
{
    /// <summary>
    /// Create a Pie Chart.  You are responsible for making sure it adds to 100.
    /// </summary>
    public class PieChart : SVG
    {
        private class PieSlice
        {
            public string Name { get; set; }
            public double Value { get; set; }
            public string Color { get; set; }
            public string PercentageFormat { get; set; }
            public SVGPath AssociatedSVG { get; set; }
        }

        private List<PieSlice> Slices = new List<PieSlice>();

        public int GraphHeight { get { return (int)Height; } set { Height = Width = value; } }

        public int GraphWidth { get { return (int)Width; } set { Width = Height = value; } }

        public double Total { get { return Slices.Sum( t => t.Value ); } }
        
        public PieChart() : this( 600, 600 )
        {
        }

        public PieChart( int height, int width ) : base( height, width )
        {
        }

        public void AddData( string color, string name, string format, double value )
        {
            Slices.Add( new PieSlice
            {
                Color = color,
                Name = name,
                PercentageFormat = format,
                Value = value,
                AssociatedSVG = new SVGPath()
            } );

            RecalculateSliceSizes();

            Children.Add( Slices.Last().AssociatedSVG );
        }

        public void AddData( string color, string name, double value )
        {
            AddData( color, name, string.Empty, value );
        }

        private double CalculateSlicePercentage( PieSlice slice )
        {
            return (slice.Value * Total) / 100;
        }

        private void RecalculateSliceSizes()
        {
            var startAngle = 0.0;
            var endAngle = 0.0;
            var _cx = GraphWidth / 2;
            var _cy = GraphHeight / 2;
            foreach (var slice in Slices)
            {
                startAngle = endAngle;
                endAngle += Math.Ceiling( 360 * slice.Value / Total );
                var x1 = _cx + 180 * Math.Cos( Math.PI * startAngle / 180 );
                var y1 = _cy + 180 * Math.Sin( Math.PI * startAngle / 180 );

                var x2 = _cx + 180 * Math.Cos( Math.PI * endAngle / 180 );
                var y2 = _cy + 180 * Math.Sin( Math.PI * endAngle / 180 );

                var path = slice.AssociatedSVG;
                path.Clear();
                path.MoveTo( _cx, _cy );
                path.LineTo( (int)x1, (int)y1 );
                path.EllipticalArc( 180, 180, false, false, 1, (int)x2, (int)y2 );
                path.ClosePath();
                path.Fill = slice.Color;
            }
        }

        public IUnorderedListElement CreateLegend()
        {
            var ul = new UnorderedListElement() { StyleType = UnorderedListStyleType.None };
            ul.StyleList.Add( "display", "table" );
            foreach (var slice in Slices)
            {
                var li = new ListItemElement();
                li.StyleList.Add( "display", "table-row" );

                var strong = new Strong( $"{CalculateSlicePercentage( slice ).ToString( slice.PercentageFormat )}%" );
                strong.ClassList.Add( "pie-graph-percent" );
                strong.StyleList.Add( "display", "table-cell" );
                strong.StyleList.Add( "border-bottom", "12px solid white" );
                strong.StyleList.Add( "background", slice.Color );
                li.Children.Add( strong );

                var span = new Span( new DOMString( slice.Name ) );
                span.StyleList.Add( "padding-left", "10px" );
                li.Children.Add( span );

                ul.Children.Add( li );
            }
            return ul;
        }
    }
}
