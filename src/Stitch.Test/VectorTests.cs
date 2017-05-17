using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stitch.Chart;

namespace Stitch.Tests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void VectorAlgebraTest()
        {
            var a = new Vector( 4, 0, 3 );
            var b = new Vector( -2, 1, 5 );
            Assert.AreEqual( a.Magnitude, 5 );
            AssertVectorEquals( a + b, new Vector( 2, 1, 8 ) );
            AssertVectorEquals( a - b, new Vector( 6, -1, -2 ) );
            AssertVectorEquals( 3 * b, new Vector( -6, 3, 15 ) );
            AssertVectorEquals( 2 * a + 5 * b, new Vector( -2, 5, 31 ) );
        }

        [TestMethod]
        public void TerminalPointTest()
        {
            var p1 = new Point3D( 2, -3, 4 );
            var p2 = new Point3D( -2, 1, 1 );
            var v = new Vector( p1, p2 );
            AssertVectorEquals( v, new Vector( -4, 4, -3 ) );
        }

        [TestMethod]
        public void UnitVectorTest()
        {
            var v = new Vector( 2, -1, -2 );
            AssertVectorEquals( v.UnitVector, new Vector( 2.0 / 3, -1.0 / 3, -2.0 / 3 ) );
        }

        [TestMethod]
        public void OrthogonalTest()
        {
            var v1 = new Vector( 2, 2, -1 );
            var v2 = new Vector( 5, -4, 2 );
            Assert.IsTrue( v1.Orthogonal( v2 ) );
        }

        [TestMethod]
        public void AngleBetweenTest()
        {
            var a = new Vector( 2, 2, -1 );
            var b = new Vector( 5, -3, 2 );
            Assert.AreEqual( Trig.RadToDeg( a.AngleBetween( b ) ), 83.7914, .0001 );
        }

        [TestMethod]
        public void VectorScalarProjectionTest()
        {
            var b = new Vector( 1, 1, 2 );
            var a = new Vector( -2, 3, 1 );

            // scalar projection b onto a
            Assert.AreEqual( b.ScalarProjection( a ), 3 / Math.Sqrt( 14 ), .0001 );
            // vector projection b onto a
            AssertVectorEquals( b.VectorProjection( a ), new Vector( -3 / 7.0, 9 / 14.0, 3 / 14.0 ) );
        }

        [TestMethod]
        public void CrossTest()
        {
            var v1 = new Vector( 1, 3, 4 );
            var v2 = new Vector( 2, 7, -5 );
            AssertVectorEquals( v1.Cross( v2 ), new Vector( -43, 13, 1 ) );
        }

        [TestMethod]
        public void VectorCrossOrthogonal()
        {
            var v1 = new Vector( 1, 3, 4 );
            var v2 = new Vector( 2, 7, -5 );
            var cross = v1.Cross( v2 );
            Assert.IsTrue( cross.Orthogonal( v1 ) );
            Assert.IsTrue( cross.Orthogonal( v2 ) );
        }

        private void AssertVectorEquals( Vector v1, Vector v2 )
        {
            Assert.AreEqual( v1.X, v2.X, .0001 );
            Assert.AreEqual( v1.Y, v2.Y, .0001 );
            Assert.AreEqual( v1.Z, v2.Z, .0001 );
        }
    }
}
