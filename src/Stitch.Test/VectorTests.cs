using System;
using Stitch.Chart;
using Stitch.StitchMath;
using Xunit;

namespace Stitch.Tests
{
    public class VectorTests
    {
        [Fact]
        public void VectorAlgebraTest()
        {
            var a = new Vector( 4, 0, 3 );
            var b = new Vector( -2, 1, 5 );
            Assert.Equal( a.Magnitude, 5 );
            AssertVectorEquals( a + b, new Vector( 2, 1, 8 ) );
            AssertVectorEquals( a - b, new Vector( 6, -1, -2 ) );
            AssertVectorEquals( 3 * b, new Vector( -6, 3, 15 ) );
            AssertVectorEquals( 2 * a + 5 * b, new Vector( -2, 5, 31 ) );
        }

        [Fact]
        public void TerminalPointTest()
        {
            var p1 = new Point3D( 2, -3, 4 );
            var p2 = new Point3D( -2, 1, 1 );
            var v = new Vector( p1, p2 );
            AssertVectorEquals( v, new Vector( -4, 4, -3 ) );
        }

        [Fact]
        public void UnitVectorTest()
        {
            var v = new Vector( 2, -1, -2 );
            AssertVectorEquals( v.UnitVector, new Vector( 2.0 / 3, -1.0 / 3, -2.0 / 3 ) );
        }

        [Fact]
        public void OrthogonalTest()
        {
            var v1 = new Vector( 2, 2, -1 );
            var v2 = new Vector( 5, -4, 2 );
            Assert.True( v1.Orthogonal( v2 ) );
        }

        [Fact]
        public void AngleBetweenTest()
        {
            var a = new Vector( 2, 2, -1 );
            var b = new Vector( 5, -3, 2 );
            Assert.Equal( Trig.RadToDeg( a.AngleBetween( b ) ), 83.7914553738142, 4 );
        }

        [Fact]
        public void VectorScalarProjectionTest()
        {
            var b = new Vector( 1, 1, 2 );
            var a = new Vector( -2, 3, 1 );

            // scalar projection b onto a
            Assert.Equal( b.ScalarProjection( a ), 3 / Math.Sqrt( 14 ), 4 );
            // vector projection b onto a
            AssertVectorEquals( b.VectorProjection( a ), new Vector( -3 / 7.0, 9 / 14.0, 3 / 14.0 ) );
        }

        [Fact]
        public void CrossTest()
        {
            var v1 = new Vector( 1, 3, 4 );
            var v2 = new Vector( 2, 7, -5 );
            AssertVectorEquals( v1.Cross( v2 ), new Vector( -43, 13, 1 ) );
        }

        [Fact]
        public void VectorCrossOrthogonal()
        {
            var v1 = new Vector( 1, 3, 4 );
            var v2 = new Vector( 2, 7, -5 );
            var cross = v1.Cross( v2 );
            Assert.True( cross.Orthogonal( v1 ) );
            Assert.True( cross.Orthogonal( v2 ) );
        }

        private void AssertVectorEquals( Vector v1, Vector v2 )
        {
            Assert.Equal( v1.X, v2.X, 4 );
            Assert.Equal( v1.Y, v2.Y, 4 );
            Assert.Equal( v1.Z, v2.Z, 4 );
        }
    }
}
