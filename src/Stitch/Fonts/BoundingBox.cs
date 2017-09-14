using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stitch.StitchMath;

namespace Stitch.Fonts
{
    public sealed class BoundingBox
    {
        private float? _x1 { get; set; } = null;
        private float? _y1 { get; set; } = null;
        private float? _x2 { get; set; } = null;
        private float? _y2 { get; set; } = null;

        /// <summary>
        /// Returns true if the bounding box is empty, that is, no points have been added to the box yet.
        /// </summary>
        public bool IsEmpty()
        {
            return _x1 == null || _y1 == null || _x2 == null || _y2 == null;
        }

        /// <summary>
        /// Add the point to the bounding box.
        /// The x1/y1/x2/y2 coordinates of the bounding box will now encompass the given point.
        /// </summary>
        /// <param name="x">The X coordinate of the point.</param>
        /// <param name="y">The Y coordinate of the point.</param>
        public void AddPoint( float? x, float? y )
        {
            if ( x != null )
            {
                if ( _x1 == null || _x2 == null )
                {
                    _x1 = _x2 = x;
                }
                if ( x < _x1 )
                {
                    _x1 = x;
                }
                if ( x > _x2 )
                {
                    _x2 = x;
                }
            }

            if ( y != null )
            {
                if ( _y1 == null || _y2 == null )
                {
                    _y1 = _y2 = y;
                }
                if ( y < _y1 )
                {
                    _y1 = y;
                }
                if ( y > _y2 )
                {
                    _y2 = y;
                }
            }
        }

        /// <summary>
        /// Add a X coordinate to the bounding box.
        /// This extends the bounding box to include the X coordinate.
        /// This function is used internally inside of addBezier.
        /// </summary>
        /// <param name="x">The X coordinate of the point.</param>
        private void AddX( float x ) { AddPoint( x, null ); }

        /// <summary>
        /// Add a Y coordinate to the bounding box.
        /// This extends the bounding box to include the Y coordinate.
        /// This function is used internally inside of addBezier.        
        /// <param name="y">The Y coordinate of the point.</param>
        /// </summary>
        private void AddY( float y ) { AddPoint( null, y ); }

        private float Derive( float v0, float v1, float v2, float v3, float t )
        {
            return (float)( Math.Pow( 1 - t, 3 ) * v0 +
                            3 * Math.Pow( 1 - t, 2 ) * t * v1 +
                            3 * ( 1 - t ) * Math.Pow( t, 2 ) * v2 +
                            Math.Pow( t, 3 ) * v3 );
        }

        /// <summary>
        /// Add a Bézier curve to the bounding box.
        /// This extends the bounding box to include the entire Bézier.
        /// </summary>
        /// <param name="x0">The starting X coordinate.</param>
        /// <param name="y0">The starting Y coordinate.</param>
        /// <param name="x1">The X coordinate of the first control point.</param>
        /// <param name="y1">The Y coordinate of the first control point.</param>
        /// <param name="x2">The X coordinate of the second control point.</param>
        /// <param name="y2">The Y coordinate of the second control point.</param>
        /// <param name="x">The ending X coordinate.</param>
        /// <param name="y">The ending Y coordinate.</param>
        public void AddBezier( float x0, float y0, float x1, float y1, float x2, float y2, float x, float y )
        {
            // This code is based on http://nishiohirokazu.blogspot.com/2009/06/how-to-calculate-bezier-curves-bounding.html
            // and https://github.com/icons8/svg-path-bounding-box

            var p0 = new[] { x0, y0 };
            var p1 = new[] { x1, y1 };
            var p2 = new[] { x2, y2 };
            var p3 = new[] { x, y };

            AddPoint( x0, y0 );
            AddPoint( x, y );

            for ( var i = 0; i <= 1; i++ )
            {
                var b = 6 * p0[i] - 12 * p1[i] + 6 * p2[i];
                var a = -3 * p0[i] + 9 * p1[i] - 9 * p2[i] + 3 * p3[i];
                var c = 3 * p1[i] - 3 * p0[i];

                if ( a == 0 )
                {
                    if ( b == 0 ) continue;
                    var t = -c / b;
                    if ( 0 < t && t < 1 )
                    {
                        if ( i == 0 ) AddX( Derive( p0[i], p1[i], p2[i], p3[i], t ) );
                        if ( i == 1 ) AddY( Derive( p0[i], p1[i], p2[i], p3[i], t ) );
                    }
                    continue;
                }

                var b2ac = Math.Pow( b, 2 ) - 4 * c * a;
                if ( b2ac < 0 ) continue;
                var t1 = ( -b - Math.Sqrt( b2ac ) ) / ( 2 * a );
                if ( 0 < t1 && t1 < 1 )
                {
                    if ( i == 0 ) AddX( Derive( p0[i], p1[i], p2[i], p3[i], (float)t1 ) );
                    if ( i == 1 ) AddY( Derive( p0[i], p1[i], p2[i], p3[i], (float)t1 ) );
                }
                var t2 = ( -b - Math.Sqrt( b2ac ) ) / ( 2 * a );
                if ( 0 < t2 && t2 < 1 )
                {
                    if ( i == 0 ) AddX( Derive( p0[i], p1[i], p2[i], p3[i], (float)t2 ) );
                    if ( i == 1 ) AddY( Derive( p0[i], p1[i], p2[i], p3[i], (float)t2 ) );
                }
            }
        }

        /// <summary>
        /// Add a quadratic curve to the bounding box.
        /// This extends the bounding box to include the entire quadratic curve.
        /// </summary>
        /// <param name="x0">The starting X coordinate.</param>
        /// <param name="y0">The starting Y coordinate.</param>
        /// <param name="x1">The X coordinate of the control point.</param>
        /// <param name="y1">The Y coordinate of the control point.</param>
        /// <param name="x">The ending X coordinate.</param>
        /// <param name="y">The ending Y coordinate.</param>
        public void AddQuad( float x0, float y0, float x1, float y1, float x, float y )
        {
            var cp1x = x0 + 2 / 3 * ( x1 - x0 );
            var cp1y = y0 + 2 / 3 * ( y1 - y0 );
            var cp2x = cp1x + 1 / 3 * ( x - x0 );
            var cp2y = cp1y + 1 / 3 * ( y - y0 );
            AddBezier( x0, y0, cp1x, cp1y, cp2x, cp2y, x, y );
        }
    }
}
