using System;

namespace Stitch.StitchMath
{
    internal struct Vector : ICloneable
    {
        public double X;

        public double Y;

        public double Z;

        public double Magnitude
        {
            get
            {
                return Math.Sqrt( Math.Pow( X, 2 ) + Math.Pow( Y, 2 ) + Math.Pow( Z, 2 ) );
            }
        }

        public Vector UnitVector
        {
            get
            {
                var m = Magnitude;
                return new Vector( X / m, Y / m, Z / m );
            }
        }

        #region Operators

        public static Vector operator +( Vector a, Vector b )
        {
            return new Vector( a.X + b.X, a.Y + b.Y, a.Z + b.Z );
        }

        public static Vector operator -( Vector a, Vector b )
        {
            return new Vector( a.X - b.X, a.Y - b.Y, a.Z - b.Z );
        }

        public static Vector operator *( Vector a, double c )
        {
            return new Vector( c * a.X, c * a.Y, c * a.Z );
        }

        public static Vector operator *( double c, Vector a )
        {
            return new Vector( c * a.X, c * a.Y, c * a.Z );
        }


        #endregion

        public double Dot( Vector v )
        {
            return (X * v.X) + (Y * v.Y) + (Z * v.Z);
        }

        public double AngleBetween( Vector v )
        {
            var dot = Dot( v );
            var m = Magnitude * v.Magnitude;
            return Math.Acos( dot / m );
        }

        public bool Orthogonal( Vector v )
        {
            return Dot( v ) == 0;
        }

        public bool Parallel( Vector v )
        {
            var cross = Cross( v );
            return cross.X == 0 && cross.Y == 0 && cross.Z == 0;
        }

        /// <summary>
        /// Compute the vector projection of this vector onto v.
        /// </summary>
        /// <param name="v">The vector to project onto.</param>
        /// <returns>The projected vector.</returns>
        public Vector VectorProjection( Vector v )
        {
            return (ScalarProjection( v ) / v.Magnitude) * v;
        }

        /// <summary>
        /// Compute a scalar projection of this vector onto v;
        /// </summary>
        /// <param name="v">The vector to project onto.</param>
        /// <returns>The projected scalar.</returns>
        public double ScalarProjection( Vector v )
        {
            return v.Dot( this ) / v.Magnitude;
        }

        public Vector Cross( Vector v )
        {
            return new Vector( Y * v.Z - Z * v.Y,
                               Z * v.X - X * v.Z,
                               X * v.Y - Y * v.X );
        }

        public object Clone()
        {
            return MemberwiseClone();
        }


        #region Constructors

        public Vector( double x, double y ) { X = x; Y = y; Z = 0; }

        public Vector( double x, double y, double z ) { X = x; Y = y; Z = z; }

        public Vector( Point a, Point b )
        {
            var z = 0.0;
            if (b is Point3D && a is Point3D)
            {
                z = ((Point3D)b).Z - ((Point3D)a).Z;
            }
            else if (b is Point3D)
            {
                z = ((Point3D)b).Z;
            }
            else if (a is Point3D)
            {
                z = -((Point3D)a).Z;
            }
            X = b.X - a.X;
            Y = b.Y - a.Y;
            Z = z;
        }

        #endregion
    }
}
