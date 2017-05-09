using HydraDoc.Chart.Axis.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HydraDoc.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class TickAlgorithmTests
    {
        [Theory( DisplayName = "SimpleNumericTickAlgorithmTest" )]
        [InlineData(0, 25, 6, new[] { 0.0, 5, 10, 15, 20, 25 })]
        [InlineData(0, 30, 4, new[] { 0.0, 10, 20, 30 })]
        [InlineData(0, 30, 4, new[] { 0.0, 10, 20, 30 })]
        [InlineData(0, 43333, 5, new[] { 0.0, 10000, 20000, 30000, 40000, 50000 })]
        public void SimpleNumericTickAlgorithmTest( double min, double max, int intervals, double[] expected )
        {
            var algorithm = new SimpleNumericTickAlgorithm();
            AssertAlgorithm( algorithm, new[] { min, max }, intervals, expected );
        }

        private void AssertAlgorithm<T>( ITickAlgorithm<T> algorithm, IEnumerable<T> set, int intervals, T[] expected ) where T : IComparable<T> {
            var ticks = algorithm.SuggestTicks( set, intervals ).ToArray();
            Assert.Equal( expected.Length, ticks.Length );
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal( ticks[i], expected[i] );
            }
        }
    }
}
