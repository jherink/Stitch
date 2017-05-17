using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
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

        //public static int SideOfLine( Point a, Point b, Point c )
        //{
        //    var val = (b.X - a.X) * (c.Y - c.Y) - (b.Y - a.Y) * (c.X - a.X);
        //    return val < 0 ? -1 : 1;
        //}

        public static int test( Point lineStart, Point lineEnd, Point examPoint )
        {
            var normal = new Vector( lineStart, lineEnd ).UnitVector;


            return 0;
        }
    }
}
