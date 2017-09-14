using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.StitchMath
{
    internal static class Trig
    {
        #region Constants

        public const double Deg120 = 2 * Math.PI / 3;
        public const double Deg180 = Math.PI;
        public const double Deg270 = 3 * Math.PI / 2;
        public const double Deg30 = Math.PI / 6;
        public const double Deg360 = 2 * Math.PI;
        public const double Deg45 = Math.PI / 4;
        public const double Deg60 = Math.PI / 3;
        public const double Deg90 = Math.PI / 2;

        #endregion

        /// <summary>
        /// Convert radians to degrees.
        /// </summary>
        /// <param name="radians">The value in radians to convert.</param>
        /// <returns>The value in degrees.</returns>
        public static double RadToDeg( double radians )
        {
            return radians * (180.0 / Math.PI);
        }

        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        /// <param name="degrees">The value in degrees to convert.</param>
        /// <returns>The value in radians.</returns>
        public static double DegToRad( double degrees )
        {
            return degrees * (Math.PI / 180.0);
        }

        /// <summary>
        /// Normalize an angle, in degrees, to within the range 0 - 360
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns></returns>
        public static double NormalizeAngle( double angle )
        {
            const double FULL_CIRCLE = 360.0;
            int numCircles = (int)Math.Abs( angle / FULL_CIRCLE );
            double normalizedAngle = angle;

            if (angle < 0)
            {
                normalizedAngle = angle + (numCircles + 1) * FULL_CIRCLE;
            }
            else if (angle > 360.0)
            {
                normalizedAngle = angle - numCircles * FULL_CIRCLE;
            }

            return normalizedAngle;
        }

        /// <summary>
        /// Calculate the intersection point between lines AB and CD.
        /// </summary>
        /// <param name="A">Point A of line AB.</param>
        /// <param name="B">Point B of line AB.</param>
        /// <param name="C">Point C of line CD.</param>
        /// <param name="D">Point  of line CD.</param>
        /// <returns></returns>
        public static Point CaclulateIntersection( Point A, Point B, Point C, Point D )
        {
            var ABSlope = (B.Y - A.Y) / (B.X - A.X);
            var CDSlope = (D.Y - C.Y) / (D.X - C.X);

            var AB_CD_XInterceptSolved = 0.0;
            var AB_CD_YInterceptSolved = 0.0;
            if (double.IsInfinity( ABSlope ) && double.IsInfinity( CDSlope ))
            { // both lines are vertical.  They will never intersect.
                AB_CD_XInterceptSolved = double.PositiveInfinity;
                AB_CD_YInterceptSolved = double.PositiveInfinity;
            }
            else if (double.IsInfinity( ABSlope ))
            { // vertical
                AB_CD_XInterceptSolved = A.X; // A & B have the same x
                AB_CD_YInterceptSolved = CDSlope * (AB_CD_XInterceptSolved - A.X) + A.Y;
            }
            else if (double.IsInfinity( CDSlope ))
            { // vertical
                AB_CD_XInterceptSolved = C.X; // C & D have the same X
                AB_CD_YInterceptSolved = ABSlope * (AB_CD_XInterceptSolved - C.X) + C.Y;
            }
            else
            { // normal
                AB_CD_XInterceptSolved = ((-CDSlope * D.X) + D.Y + (ABSlope * B.X) - B.Y) / (ABSlope - CDSlope);
                AB_CD_YInterceptSolved = ABSlope * (AB_CD_XInterceptSolved - A.X) + A.Y;
            }

            return new Point( AB_CD_XInterceptSolved, AB_CD_YInterceptSolved );
        }

    }
}
