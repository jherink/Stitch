using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart.Axis.Algorithms
{
    public class SimpleNumericTickAlgorithm : ITickAlgorithm<double>
    {
        private static readonly int[] bases = { 1, 2, 5 };

        private static double GetNiceInterval( double min, double max, int n )
        {

            var rawInterval = (max - min) / n;
            var rawExponent = Math.Log10( rawInterval );

            var exponents = new[] { Math.Floor( rawExponent ), Math.Ceiling( rawExponent ) };
            var nicestInterval = double.PositiveInfinity;

            foreach (var b in bases)
            {
                foreach (var exponent in exponents)
                {
                    var currentInterval = b * Math.Pow( 10, exponent );

                    var currentDeviation = Math.Abs( rawInterval - currentInterval );
                    var nicestDeviation = Math.Abs( rawInterval - nicestInterval );

                    if (currentDeviation < nicestDeviation)
                    {
                        nicestInterval = currentInterval;
                    }
                }
            }
            return nicestInterval;
        }

        private static double GetFirstTickValue( double min, double interval )
        {
            return Math.Floor( min / interval ) * interval;
        }

        private static IEnumerable<double> GenerateTicks( double min, double max, int n, bool tight = false )
        {
            if (min > max)
            {
                var t = min;
                min = max;
                max = t;
            }

            var interval = GetNiceInterval( min, max, n );
            var value = GetFirstTickValue( min, interval );

            var ticks = new List<double>() { value };
            while (value < max)
            {
                value += interval;
                ticks.Add( value );
            }

            var multiplier = Math.Pow( 10, Math.Ceiling( Math.Log10( interval ) ) + 1 );
            for (int i = 0; i < ticks.Count; i++)
            {
                ticks[i] = Math.Round( ticks[i] * multiplier ) / multiplier;
            }

            if (tight)
            {
                ticks[0] = min;
                ticks[ticks.Count] = max;
            }

            return ticks;
        }

        public IEnumerable<double> SuggestTicks( IEnumerable<double> set, int intervals )
        {
            intervals = Math.Max( intervals, 3 );
            return GenerateTicks( set.Min(), set.Max(), intervals - 2 );
        }

        public double Subtract( IEnumerable<double> set, double a, double b )
        {
            return a - b;
        }

        public double Percentage( IEnumerable<double> range, double value )
        {
            return (value - range.Min()) / (range.Max() - range.Min());
        }

        public double Min( IEnumerable<double> set )
        {
            return set.Min();
        }

        public double Max( IEnumerable<double> set )
        {
            return set.Max();
        }

        public int Compare( double x, double y )
        {
            return x.CompareTo( y );
        }
    }
}
