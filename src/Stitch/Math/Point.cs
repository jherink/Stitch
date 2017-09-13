using System;

namespace Stitch.StitchMath
{
    /// <summary>
    /// Class for representing 2D points.
    /// </summary>
    internal class Point : ICloneable, IPoint
    {
        #region Properties

        /// <summary>
        /// This point's X coordinate.
        /// </summary>
        public virtual double X { get; set; }

        /// <summary>
        /// This point's Y coordinate.
        /// </summary>
        public virtual double Y { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Point() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Point( double x, double y ) : this()
        {
            X = x; Y = y;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculate the distance between this point and another arbitrary point.
        /// </summary>
        /// <param name="x">The point's x coordinate.</param>
        /// <param name="y">The point's y coordinate.</param>
        /// <returns></returns>
        public virtual double Distance( double x, double y )
        {
            return Math.Sqrt( Math.Pow( this.X - x, 2 ) +
                              Math.Pow( this.Y - y, 2 ) );
        }
        public override bool Equals( object obj )
        {
            var p = obj as Point;
            if (p != null)
            { // both of type point
                return Compare.AreEqual( p.X, X ) &&
                       Compare.AreEqual( p.Y, Y );
            }
            return false;
        }
        public override string ToString()
        {
            return string.Format( "{0:0.000},{1:0.000}", X, Y );
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region ICloneable Implementation
        
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region IPoint Implementation
        
        public double Distance( IPoint point )
        {
            var p = point as Point3D;
            if (p != null)
            { // comp this 2D point against their 3D point.
                return p.Distance( X, Y, 0 );
            } 
            return Distance( (point as Point).X, (point as Point).Y );
        }

        #endregion
    }
}
