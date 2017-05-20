using Stitch.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    //internal class Point
    //{
    //    public double X { get; set; }
    //    public double Y { get; set; }

    //    public Point() { }

    //    public Point( double x, double y ) { X = x; Y = y; }
    //}

    //internal static class Trig
    //{
    //    private static double DegToRad = Math.PI / 180.0;
    //    private static double RadToDeg = 180.0 / Math.PI;
    //    private static double Rad90 = Math.PI / 2.0;
    //    private static double Rad180 = Math.PI;
    //    private static double Rad270 = 3.0 * Math.PI / 2.0;

    //    /// <summary>
    //    /// Calculate the intersection point between lines AB and CD.
    //    /// </summary>
    //    /// <param name="A">Point A of line AB.</param>
    //    /// <param name="B">Point B of line AB.</param>
    //    /// <param name="C">Point C of line CD.</param>
    //    /// <param name="D">Point  of line CD.</param>
    //    /// <returns></returns>
    //    public static SVGPoint CaclulateIntersection( SVGPoint A, SVGPoint B, SVGPoint C, SVGPoint D )
    //    {
    //        var ABSlope = (B.Y - A.Y) / (B.X - A.X);
    //        var CDSlope = (D.Y - C.Y) / (D.X - C.X);

    //        var AB_CD_XInterceptSolved = 0.0;
    //        var AB_CD_YInterceptSolved = 0.0;
    //        if (double.IsInfinity( ABSlope ) && double.IsInfinity( CDSlope ))
    //        { // both lines are vertical.  They will never intersect.
    //            AB_CD_XInterceptSolved = double.PositiveInfinity;
    //            AB_CD_YInterceptSolved = double.PositiveInfinity;
    //        }
    //        else if (double.IsInfinity( ABSlope ))
    //        { // vertical
    //            AB_CD_XInterceptSolved = A.X; // A & B have the same x
    //            AB_CD_YInterceptSolved = CDSlope * (AB_CD_XInterceptSolved - A.X) + A.Y;
    //        }
    //        else if (double.IsInfinity( CDSlope ))
    //        { // vertical
    //            AB_CD_XInterceptSolved = C.X; // C & D have the same X
    //            AB_CD_YInterceptSolved = ABSlope * (AB_CD_XInterceptSolved - C.X) + C.Y;
    //        }
    //        else
    //        { // normal
    //            AB_CD_XInterceptSolved = ((-CDSlope * D.X) + D.Y + (ABSlope * B.X) - B.Y) / (ABSlope - CDSlope);
    //            AB_CD_YInterceptSolved = ABSlope * (AB_CD_XInterceptSolved - A.X) + A.Y;
    //        }

    //        return new SVGPoint( AB_CD_XInterceptSolved, AB_CD_YInterceptSolved );
    //    }

    //    public static double Distance( SVGPoint A, SVGPoint B )
    //    {
    //        return Math.Sqrt( Math.Pow( B.Y - A.Y, 2 ) + Math.Pow( B.X - A.X, 2 ) );
    //    }
                
    //}
}
