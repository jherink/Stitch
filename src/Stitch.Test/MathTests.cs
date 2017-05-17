using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stitch.Chart;
using Xunit;

using Assert = Xunit.Assert;

namespace Stitch.Tests
{
    [TestClass]
    public class MathTests
    {

        [Theory( DisplayName = "Distance2DPoints" )]
        [InlineData( 10, 5, 23, 4, 13.0384 )]
        [InlineData( -18, 5, 23, -2, 41.5933 )]
        [InlineData( -18, 5, -3, 2, 15.2971 )]
        public void Distance2DPoints( double x1, double y1, double x2, double y2, double expectedDistance )
        {
            var p1 = new Point( x1, y1 );
            var p2 = new Point( x2, y2 );

            Assert.Equal( expectedDistance, p1.Distance( p2 ), 3 );
            Assert.Equal( expectedDistance, p2.Distance( p1 ), 3 );
        }

        [Theory( DisplayName = "Distance3DPoints" )]
        [InlineData( 3, 9, 4, 11, 6, 9, 9.89949 )]
        [InlineData( -5, 9, 4, 11, -2, 9, 20.0499 )]
        [InlineData( -5, 9, 7, 13, -2.5, -9, 26.688 )]
        public void Distance3DPoints( double x1, double y1, double z1,
                                      double x2, double y2, double z2,
                                      double expectedDistance )
        {
            var p1 = new Point3D( x1, y1, z1 );
            var p2 = new Point3D( x2, y2, z2 );
            Assert.Equal( expectedDistance, p1.Distance( p2 ), 3 );
            Assert.Equal( expectedDistance, p2.Distance( p1 ), 3 );
        }

        [Theory( DisplayName = "Distance2DTo3D" )]
        [InlineData( 3, 2, 5, -1, 0, 3.6055 )]
        [InlineData( -6, 2.3, 5, -1, 6, 12.957 )]
        [InlineData( 8, -2.2, 5, -1, -2, 3.8 )]
        [InlineData( 3, 1.25, 5, -13, 6, 15.59 )]
        public void Distance2DTo3D( double x1, double y1, double x2, double y2, double z2, double expectedDistance )
        {
            var p1 = new Point( x1, y1 );
            var p2 = new Point3D( x2, y2, z2 );

            Assert.Equal( expectedDistance, p1.Distance( p2 ), 3 );
            Assert.Equal( expectedDistance, p2.Distance( p1 ), 3 );
        }

        [Theory( DisplayName = "SphericalToCartesian" )]
        [InlineData( 5, 60, 30, 1.25, 2.165, 4.33 )]
        [InlineData( 15, -25, -10, -2.361, 1.101, 14.772 )]
        [InlineData( 15, 45, -10, -1.842, -1.842, 14.772 )]
        public void SphericalToCartesian( double radius, double theta, double phi, double expectedX, double expectedY, double expectedZ )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            Assert.Equal( p1.X, expectedX, 3 );
            Assert.Equal( p1.Y, expectedY, 3 );
            Assert.Equal( p1.Z, expectedZ, 3 );
        }

        [Theory( DisplayName = "SphericalRadiusGetSet" )]
        [InlineData( 8, 12, 88, 9.913, 2.107, 0.354, 10.141 )]
        [InlineData( 8, 12, 88, 15.641, 3.325, 0.558, 16 )]
        public void SphericalRadiusGetSet( double radius, double theta, double phi, double newX, double newY, double newZ, double newRadius )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            p1.Radius = newRadius;
            Assert.Equal( newX, p1.X, 3 );
            Assert.Equal( newY, p1.Y, 3 );
            Assert.Equal( newZ, p1.Z, 3 );
            Assert.Equal( newRadius, p1.Radius, 3 );
            Assert.Equal( theta, p1.Theta, 3 );
            Assert.Equal( phi, p1.Phi, 3 );
        }

        [Theory( DisplayName = "SphericalThetaGetSet" )]
        [InlineData( 8, 12, 88, 7.995, 0, 0.279, 0 )]
        [InlineData( 8, 12, 88, 3.998, 6.924, 0.279, 60 )]
        [InlineData( 8, 12, 88, 7.685, -2.204, 0.279, 360.0 - 16 )]
        public void SphericalThetaGetSet( double radius, double theta, double phi, double newX, double newY, double newZ, double newTheta )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            p1.Theta = newTheta;
            Assert.Equal( newX, p1.X, 3 );
            Assert.Equal( newY, p1.Y, 3 );
            Assert.Equal( newZ, p1.Z, 3 );
            Assert.Equal( radius, p1.Radius, 3 );
            Assert.Equal( newTheta, p1.Theta, 3 );
            Assert.Equal( phi, p1.Phi, 3 );
        }

        [Theory( DisplayName = "SphericalPhiGetSet" )]
        [InlineData( 8, 12, 88, 0, 0, 8, 0 )]
        [InlineData( 8, 12, 88, 2.157, 0.458, 7.69, 16 )]
        [InlineData( 8, 12, 88, -6.41, -1.362, 4.589, 360.0 - 55 )]
        public void SphericalPhiGetSet( double radius, double theta, double phi, double newX, double newY, double newZ, double newPhi )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            p1.Phi = newPhi;
            Assert.Equal( newX, p1.X, 3 );
            Assert.Equal( newY, p1.Y, 3 );
            Assert.Equal( newZ, p1.Z, 3 );
            Assert.Equal( radius, p1.Radius, 3 );
            Assert.Equal( theta, p1.Theta, 3 );
            Assert.Equal( newPhi, p1.Phi, 3 );
        }

        [Theory( DisplayName = "CreateSphericalFromCartesian" )]
        [InlineData( 8, 12, 88, 9.913, 2.107, 0.354, 10.141 )]
        [InlineData( 8, 12, 88, 15.641, 3.325, 0.558, 16 )]
        public void SphericalRadiusGetSet( double radius, double theta, double phi, double x, double y, double z )
        {
            var p1 = SphericalCoordinate.CreateFromCartesian( x, y, z );

            // comp against expected 
            Assert.Equal( radius, p1.Radius, 3 );
            Assert.Equal( theta, p1.Theta, 3 );
            Assert.Equal( phi, p1.Phi, 3 );
            Assert.Equal( x, p1.X, 3 );
            Assert.Equal( y, p1.Y, 3 );
            Assert.Equal( z, p1.Z, 3 );

            // comp against other constructor.
            var p2 = new SphericalCoordinate( radius, theta, phi );
            Assert.Equal( p2.Radius, p1.Radius, 3 );
            Assert.Equal( p2.Theta, p1.Theta, 3 );
            Assert.Equal( p2.Phi, p1.Phi, 3 );
            Assert.Equal( p2.X, p1.X, 3 );
            Assert.Equal( p2.Y, p1.Y, 3 );
            Assert.Equal( p2.Z, p1.Z, 3 );
        }

        [Theory( DisplayName = "SphericalXGetSet" )]
        [InlineData( 8, 12, 88, 7.82, 1.662, 0.279, 10, 10.141, 9.438, 88.422 )]
        [InlineData( 8, -6, -18, -2.459, 0.258, 7.608, -6, 9.693, 177.5339, 38.285 )]
        public void SphericalXGetSet( double radius, double theta, double phi, double x, double y, double z,
                                      double newX, double newRadius, double newTheta, double newPhi )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            p1.X = newX;
            Assert.Equal( newX, p1.X, 3 );
            Assert.Equal( y, p1.Y, 3 );
            Assert.Equal( z, p1.Z, 3 );
            Assert.Equal( newRadius, p1.Radius, 3 );
            Assert.Equal( newTheta, p1.Theta, 3 );
            Assert.Equal( newPhi, p1.Phi, 3 );
        }

        [Theory( DisplayName = "SphericalYGetSet" )]
        [InlineData( 8, 12, 88, 7.82, 1.662, 0.279, 10, 12.698, 51.973, 88.74 )]
        [InlineData( 8, -6, -18, -2.459, 0.258, 7.608, -6, 9.997, (360.0 - 112.282), 40.439 )]
        public void SphericalYGetSet( double radius, double theta, double phi, double x, double y, double z,
                              double newY, double newRadius, double newTheta, double newPhi )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            p1.Y = newY;
            Assert.Equal( x, p1.X, 3 );
            Assert.Equal( newY, p1.Y, 3 );
            Assert.Equal( z, p1.Z, 3 );
            Assert.Equal( newRadius, p1.Radius, 3 );
            Assert.Equal( newTheta, p1.Theta, 3 );
            Assert.Equal( newPhi, p1.Phi, 3 );
        }

        [Theory( DisplayName = "SphericalZGetSet" )]
        [InlineData( 8, 12, 88, 7.82, 1.662, 0.279, 6, 9.996, 12.0, 53.113 )]
        [InlineData( 8, -6, -18, -2.459, 0.258, 7.608, -10, 10.301, 174, 166.114 )]
        public void SphericalZGetSet( double radius, double theta, double phi, double x, double y, double z,
                              double newZ, double newRadius, double newTheta, double newPhi )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            p1.Z = newZ;
            Assert.Equal( x, p1.X, 3 );
            Assert.Equal( y, p1.Y, 3 );
            Assert.Equal( newZ, p1.Z, 3 );
            Assert.Equal( newRadius, p1.Radius, 3 );
            Assert.Equal( newTheta, p1.Theta, 3 );
            Assert.Equal( newPhi, p1.Phi, 3 );
        }

        [Theory( DisplayName = "Spherical2DDistance" )]
        [InlineData( 8, 12, 88, 5, 6, 5.182 )]
        [InlineData( 8, 12, 88, -3, -1, 11.147 )]
        public void Spherical2DDistance( double radius, double theta, double phi,
                                         double x, double y, double expectedDistance )
        {
            var p1 = new SphericalCoordinate( radius, theta, phi );
            var p2 = new Point( x, y );

            Assert.Equal( expectedDistance, p1.Distance( p2 ), 3 );
            Assert.Equal( expectedDistance, p2.Distance( p1 ), 3 );
        }

        [Theory( DisplayName = "PolarToCartesian" )]
        [InlineData( 1, 1, 0.9998, 0.0174 )]
        [InlineData( 15.1, 97.6, -1.997, 14.967 )]
        [InlineData( 16, 220, -12.257, -10.285 )]
        public void PolarToCartesian( double radius, double theta, double x, double y )
        {
            var p1 = new PolarCoordinate( radius, theta );
            Assert.Equal( radius, p1.Radius, 3 );
            Assert.Equal( theta, p1.Theta, 3 );
            Assert.Equal( x, p1.X, 3 );
            Assert.Equal( y, p1.Y, 3 );

            var p2 = PolarCoordinate.CreateFromCartesian( x, y );
            Assert.Equal( radius, p1.Radius, 3 );
            Assert.Equal( theta, p1.Theta, 3 );
            Assert.Equal( x, p1.X, 3 );
            Assert.Equal( y, p1.Y, 3 );
        }

    }
}
