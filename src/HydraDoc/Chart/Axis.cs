using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public class Axis<T> : SVGGroup where T : IComparable
    {
        readonly int[] bases = { 1, 2, 5 };

        public double AxisLength { get; set; } = 100;

        public bool IncludeZero { get; set; } = true;
        
        public Orientation Orientation { get; set; }

        #region Constructors

        public Axis() : this(400) { }

        public Axis( int axisLength )
        {
            AxisLength = axisLength;
        }

        #endregion

        private double GetNiceInterval( double min, double max, int n )
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

        private double GetFirstTickValue( double min, double interval )
        {
            return Math.Floor( min / interval ) * interval;
        }

        private IEnumerable<double> GenerateTicks( double min, double max, int n, bool tight = false )
        {
            if (min > max)
            {
                var t = min;
                min = max;
                max = t;
            }

            if (IncludeZero && min > 0) min = 0;

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

        public IEnumerable<double> GenerateAxisData(double min, double max, int intervals = 5)
        {
            intervals = Math.Max( intervals, 3 );
            return GenerateTicks( min, max, intervals - 2 ).ToArray();     // reverse for SVG rendering               
        }

        public void GenerateAxisData( IEnumerable<T> ticks )
        {
            Children.Clear();
            if (Orientation == Orientation.Vertical) ticks = ticks.Reverse();
            var _ticks = ticks.ToArray();
            double space = AxisLength / _ticks.Length;
            double x = 0, y = 30;
            
            foreach (var tick in _ticks)
            {
                var tickString = tick.ToString();
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        x += space - (tickString.Length * 15 / 2.0);
                        break;
                    case Orientation.Vertical:
                        y += space - 15 / 2; // TODO: FontSize
                        break;
                }
                var g = new SVGGroup();
                var t = new SVGText()
                {
                    X = x,
                    Y = y
                };
                t.Text.Append( tickString );
                g.Children.Add( t );
                Children.Add( g );
            }
        }

        public override string Render()
        {
            return base.Render();
        }
    }
}
