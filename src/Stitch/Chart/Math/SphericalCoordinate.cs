using System;

namespace Stitch.Chart
{
    /// <summary>
    /// Class representing spherical point coordinates.
    /// Information can be found here: http://mathworld.wolfram.com/SphericalCoordinates.html
    /// </summary>
    internal class SphericalCoordinate : Point3D
    {
        #region Properties

        #region Phi

        private double _phi;

        /// <summary>
        /// The normalized polar angle along the z-axis ranging from 0 to 360 degrees.
        /// </summary>
        public double Phi
        {
            get { return _phi; }
            set
            {
                _phi = Trig.NormalizeAngle( value );
                DefineCartesianLocations();
            }
        }

        /// <summary>
        /// The normalized phi angle in radians
        /// </summary>
        public double PhiR { get { return Trig.DegToRad( Phi ); } }

        #endregion

        #region Theta

        private double _theta;

        /// <summary>
        /// The normalized azimuthal angle in the x-y plane ranging from 0 to 360 degrees.
        /// </summary>
        public double Theta
        {
            get { return _theta; }
            set
            {
                _theta = Trig.NormalizeAngle( value );
                DefineCartesianLocations();
            }
        }

        /// <summary>
        /// The normalized azimuthal angle in radians.
        /// </summary>
        public double ThetaR { get { return Trig.DegToRad( Theta ); } }

        #endregion

        #region Radius

        private double _radius;

        /// <summary>
        /// The distance between this point and the origin point 0,0,0
        /// </summary>
        public double Radius
        {
            get { return _radius; }
            set
            {
                if (value < 0) return;
                _radius = value;
                DefineCartesianLocations();
            }
        }

        #endregion

        #region Point3D Overrides

        public override double X
        {
            get
            {
                return base.X;
            }

            set
            {
                // Changing the x dimension will change the theta value.
                base.X = value;
                DefineSphericalLocations();
            }
        }

        public override double Y
        {
            get
            {
                return base.Y;
            }

            set
            {
                // Changing the y dimension will change the theta value.
                base.Y = value;
                DefineSphericalLocations();
            }
        }

        public override double Z
        {
            get
            {
                return base.Z;
            }

            set
            {
                // Changing the z dimension will change the phi value.
                base.Z = value;
                DefineSphericalLocations();
            }
        }

        #endregion

        #endregion

        #region Constructors 

        public SphericalCoordinate() : base() { }

        public SphericalCoordinate( double radius, double theta, double phi )
        {
            _radius = radius;
            _theta = theta;
            _phi = phi;
            DefineCartesianLocations();
        }
        
        public static SphericalCoordinate CreateFromCartesian( double x, double y, double z )
        {
            return new SphericalCoordinate
            {
                X = x,
                Y = y,
                Z = z
            };
        }

        #endregion

        #region Private Methods

        private void DefineCartesianLocations()
        {
            base.X = Radius * Math.Cos( ThetaR ) * Math.Sin( PhiR );
            base.Y = Radius * Math.Sin( ThetaR ) * Math.Sin( PhiR );
            base.Z = Radius * Math.Cos( PhiR );
        }

        private void DefineSphericalLocations()
        {
            _radius = Math.Sqrt( Math.Pow( X, 2 ) + Math.Pow( Y, 2 ) + Math.Pow( Z, 2 ) );
            _theta = Trig.NormalizeAngle( Math.Atan2( Y, X ) * (180.0 / Math.PI) );
            _phi = Trig.NormalizeAngle( Math.Atan2( Math.Sqrt( Math.Pow( X, 2 ) + Math.Pow( Y, 2 ) ), Z ) * (180 / Math.PI) );
        }

        #endregion
    }
}
