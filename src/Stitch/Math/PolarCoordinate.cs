using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.StitchMath
{
    internal class PolarCoordinate : Point
    {

        #region Properties 
        
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
        /// The distance between this point and the origin point 0,0
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

        #region Point Overrides

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

        #endregion

        #endregion

        public PolarCoordinate() : base() { }

        public PolarCoordinate( double radius, double theta )
        {
            _radius = radius;
            _theta = theta;
            DefineCartesianLocations();
        }
        
        public static PolarCoordinate CreateFromCartesian( double x, double y )
        {
            return new PolarCoordinate
            {
                X = x,
                Y = y
            };
        }

        #region Private Methods

        private void DefineCartesianLocations()
        {
            base.X = Radius * Math.Cos( ThetaR );
            base.Y = Radius * Math.Sin( ThetaR );
        }

        private void DefineSphericalLocations()
        {
            _radius = Math.Sqrt( Math.Pow( X, 2 ) + Math.Pow( Y, 2 ) );
            _theta = Trig.NormalizeAngle( Math.Atan2( Y, X ) * (180.0 / Math.PI) );
        }
        
        #endregion
    }
}
