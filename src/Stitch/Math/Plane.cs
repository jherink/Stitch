namespace Stitch.StitchMath
{
    /// <summary>
    /// Class representing a plane.
    /// </summary>
    internal class Plane
    {
        /// <summary>
        /// Point in the plane
        /// </summary>
        public readonly Point3D P;

        /// <summary>
        /// A normal vector to the plane.
        /// </summary>
        public readonly Vector NormalVector;

        public double D
        {
            get
            {
                return P.X * NormalVector.X + P.Y * NormalVector.Y + P.Z * NormalVector.Z;
            }
        }

        public Plane( Point p, Vector normalVector )
        {
            P = new Point3D( p.X, p.Y, ((p is Point3D) ? (p as Point3D).Z : 0) );
            NormalVector = normalVector;
        }

        public Plane( Point p, Point q, Point r ) : this( p, default( Vector ) )
        {
            var pq = new Vector( p, q );
            var pr = new Vector( p, r );
            NormalVector = pq.Cross( pr );
        }

        /// <summary>
        /// Check to see if a point exists in a plane.
        /// </summary>
        /// <param name="p">The point to check.</param>
        /// <returns>True if the point lies in the plane.</returns>
        public bool Contains( Point p )
        {
            return Compare.AreEqual( Distance( p ), 0 );
        }

        /// <summary>
        /// Calculate the distance between a point and a plane.
        /// </summary>
        /// <param name="p">The point to check.</param>
        /// <returns>The distance between the point and the plane.</returns>
        public double Distance( Point p )
        {
            var z = p is Point3D ? (p as Point3D).Z : 0;
            var a = NormalVector.X;
            var b = NormalVector.Y;
            var c = NormalVector.Z;

            var x0 = P.X;
            var y0 = P.Y;
            var z0 = P.Z;

            return a * (p.X - x0) + b * (p.Y - y0) + c * (z - z0);
        }

        public bool Parallel( Plane p )
        { // two planes are parallel if their normal vectors are parallel.
            return NormalVector.Parallel( p.NormalVector );
        }

        public override string ToString()
        {
            return string.Format( "{0}x + {1}y + {2}z = {3}", 
                                  NormalVector.X, NormalVector.Y, NormalVector.Z, D);
        }
    }
}
