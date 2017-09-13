using System;

namespace Stitch.StitchMath
{
    /// <summary>
    /// Class for representing 3D points.
    /// </summary>
    internal class Point3D : Point, IPoint
    {
        #region Properties

        /// <summary>
        /// The z coordinate of the point.
        /// </summary>
        public virtual double Z { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public Point3D() { }

        /// <summary>
        /// Constructor for a point in the XY-Plane.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Point3D( double x, double y ) : base( x, y ) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        public Point3D( double x, double y, double z ) : this( x, y )
        {
            Z = z;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compute the distance between this point and a 2D Cartesian coordinate.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The distance between the points.</returns>
        public override double Distance( double x, double y )
        {
            return Distance( x, y, 0 );
        }

        /// <summary>
        /// Calculates the distance between this point and another 3D point.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <returns>The distance between the points.</returns>
        public double Distance( double x, double y, double z )
        {
            // d = sqrt(x2 - x1)^2 + (y2 - y1)^2
            return Math.Sqrt( Math.Pow( this.X - x, 2 ) +
                              Math.Pow( this.Y - y, 2 ) +
                              Math.Pow( this.Z - z, 2 ) );
        }

        public override bool Equals( object obj )
        {
            var p = obj as Point3D;
            if (p != null)
            {
                return Compare.AreEqual( p.X, X ) &&
                       Compare.AreEqual( p.Y, Y ) &&
                       Compare.AreEqual( p.Z, Z );
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format( "{0:0.000},{1:0.000},{2:0.000}", X, Y, Z );
        }

        #region IPoint Implementation

        public new double Distance( IPoint point )
        {
            if (point.GetType() == typeof( Point ))
            {   // if this is true then we (of type Point 3D, in 3D space)
                // are calculating a distance with a 2D point in 2D space.
                // Call Distance with x,y,z parameters using 0 as the z coordinate
                // to calculate the correct distance.
                return Distance( (point as Point).X, (point as Point).Y, 0 );
            }
            // otherwise everything else should inherit from a 3D point.
            var p = point as Point3D;
            return Distance( p.X, p.Y, p.Z );

        }

        #endregion

        #endregion
    }
}
