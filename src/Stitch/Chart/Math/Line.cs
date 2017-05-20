using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    internal class Line
    {
        public double Slope { get; set; }

        public double YIntercept { get; private set; }

        public Line() { }

        public Line( Point point1, Point point2 )
        {
            SetSlope( point1, point2 );
        }

        public void SetSlope( Point point1, Point point2 )
        {
            if (point2.X == point1.X) Slope = double.NaN;
            else {
                Slope = (point2.Y - point1.Y) / (point2.X - point1.X);
                YIntercept = point1.Y - (Slope * point1.X);
            }
        }

        public double CalculateY( double x )
        {
            return Slope * x + YIntercept;
        }

        public double CalculateX( double y )
        {
            return (y - YIntercept) / Slope;
        }

        public Point FindIntersectionPoint( Line line2 )
        {
            // First check if lines are parallel using vectors
            var v1 = new Vector( new Point( 0, YIntercept ), new Point( 1, CalculateY( 1 ) ) );
            var v2 = new Vector( new Point( 0, line2.YIntercept ), new Point( 1, line2.CalculateY( 1 ) ) );
            if (v1.Parallel( v2 )) return new Point( double.NaN, double.NaN );

            var x = (line2.YIntercept - YIntercept) / (Slope - line2.Slope);

            return new Point( x, CalculateY( x ) );
        }
    }
}
