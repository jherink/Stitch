//using HydraDoc.Chart.Formatters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HydraDoc.Chart.Axis
//{
//    /* Implements the axis labeling routine described in 
// *  Talbot, Lin, and Hanrahan. An Extension of Wilkinson’s Algorithm for Positioning Tick Labels on Axes, Infovis 2010.
// */

//    public class Axis_old : ICloneable
//    {
//        public static double AXIS_DENSITY = 1.0 / 150;
//        public static double AXIS_FONT_SIZE = 12.0;

//        /// <summary>
//        /// Tick placement, label text
//        /// </summary>
//        public List<Tuple<decimal, string>> Labels = new List<Tuple<decimal, string>>();

//        private List<Format> Formats = new List<Format>();

//        private List<double> Values = new List<double>();

//        public Range AxisRange { get; set; } = new Range( 0, 100 );

//        public double AxisLength { get; set; }

//        public Axis_old( double axisLength = 0 )
//        {
//            AxisLength = axisLength;
//            Formats.Add( new UnitFormat( 1m, "", new Range( -4, 6 ), false, false, 1 ) );
//            AddUnitFormat( 1000m, "K", new Range( 3, 6 ), 0.75, 0.4 );
//            AddUnitFormat( 1000000m, "M", new Range( 6, 9 ), 0.75, 0.4 );
//            AddUnitFormat( 1000000000m, "B", new Range( 9, 12 ), 0.75, 0.4 );
//            AddUnitFormat( 100m, "hundred", new Range( 2, 3 ), 0.35 );
//            AddUnitFormat( 1000m, "thousand", new Range( 3, 6 ), 0.5 );
//            AddUnitFormat( 1000000m, "million", new Range( 6, 9 ), 0.5 );
//            AddUnitFormat( 1000000000m, "billion", new Range( 9, 12 ), 0.5 );
//            AddUnitFormat( 0.01m, "hundredth", new Range( -2, -3 ), 0.3 );
//            AddUnitFormat( 0.001m, "thousandth", new Range( -3, -6 ), 0.5 );
//            AddUnitFormat( 0.000001m, "millionth", new Range( -6, -9 ), 0.5 );
//            AddUnitFormat( 0.000000001m, "billionth", new Range( -9, -12 ), 0.5 );
//            Formats.Add( new ScientificFormat( true, false, 0.3 ) );
//            Formats.Add( new ScientificFormat( true, true, 0.3 ) );
//            Formats.Add( new ScientificFormat( false, false, 0.25 ) );
//            Formats.Add( new ScientificFormat( false, true, 0.25 ) );
//        }

//        private void AddUnitFormat( decimal unit, string name, Range logRange, double weight, double factoredWeight )
//        {
//            // Add all possible combinations.
//            Formats.Add( new UnitFormat( unit, name, logRange, false, false, weight ) );
//            Formats.Add( new UnitFormat( unit, name, logRange, false, true, weight ) );
//            Formats.Add( new UnitFormat( unit, name, logRange, true, false, factoredWeight ) );
//            Formats.Add( new UnitFormat( unit, name, logRange, true, true, factoredWeight ) );
//        }

//        void AddUnitFormat( decimal unit, string name, Range logRange, double factoredWeight )
//        {
//            Formats.Add( new UnitFormat( unit, name, logRange, true, false, factoredWeight ) );
//            Formats.Add( new UnitFormat( unit, name, logRange, true, true, factoredWeight ) );
//        }

//        private double Coverage( double dMin, double dMax, double lMin, double lMax )
//        {
//            return 1 - 0.5 * (double)(((dMax - lMax) * (dMax - lMax) + (dMin - lMin) * (dMin - lMin)) / ((0.1 * (dMax - dMin)) * (0.1 * (dMax - dMin))));
//        }

//        protected double MaxCoverage( double dMin, double dMax, double span )
//        {
//            var range = dMax - dMin;

//            if (span > range)
//            {
//                var half = (span - range) / 2;
//                return 1 - 0.5 * (double)((half * half + half * half) / ((0.1 * (dMax - dMin)) * (0.1 * (dMax - dMin))));
//            }
//            else
//            {
//                return 1;
//            }
//        }

//        private double Density( double r, double rt )
//        {
//            return (2 - Math.Max( r / rt, rt / r ));
//        }

//        private double MaxDensity( double r, double rt )
//        {
//            if (r >= rt)
//                return 2 - r / rt;
//            else
//                return 1;
//        }

//        private double Simplicity( double q, int j, double lMin, double lMax, double lStep )
//        {
//            var eps = 1e-10;
//            var i = Values.IndexOf( q ) + 1;
//            var v = FlooredMod( lMin, lStep ) < eps && lMin >= 0 && lMax >= 0 ? 1 : 0;

//            return 1 - (Values.Count <= 1 ? j + v : (i - 1) / (Values.Count - 1) - j + v);
//        }

//        private double MaxSimplicity( double q, int j )
//        {
//            double i = Values.IndexOf( q ) + 1;
//            return 1 - (Values.Count <= 1 ? j + 1 : (i - 1) / (Values.Count - 1) - j + 1);
//        }

//        //private double Legibility( AxisData data, Format format )
//        //{

//        //}

//        //private double MaxLegibility( AxisData data, Format format )
//        //{

//        //}

//        private double FlooredMod( double a, double n )
//        {
//            return a - n * Math.Floor( a / n );
//        }

//        private double Score( double simplicity, double coverage, double density, double legibility )
//        {
//            return 0.2 * simplicity +
//                   0.25 * coverage +
//                   0.5 * density +
//                   0.05 * legibility;
//        }

//        private decimal Pow10( int i )
//        {
//            decimal a = 1m;
//            for (int j = 0; j < i; j++) a *= 10m;
//            return a;
//        }

//        public void AddData( string label, double value )
//        {
//            Values.Add( value );
//            AxisRange.Max = Values.Max();
//            AxisRange.Min = Values.Min();
//            //AutoGenerateAxis( .8 );
//        }

//        public AxisData AutoGenerateAxis( double density )
//        {
//            var dMax = AxisRange.Max;
//            var dMin = AxisRange.Min;

//            if (dMax == dMin) return null;

//            double bestScore = -2;

//            AxisData best = null;

//            int j = 1;
//            while (j < int.MaxValue)
//            {
//                foreach (var q in Values)
//                {
//                    double maxSimplicity = MaxSimplicity( 1, j );
//                    if (Score( maxSimplicity, 1, 1, 1 ) < bestScore)
//                    {
//                        j = int.MaxValue - 1; break;
//                    }

//                    int k = 2;
//                    while (k < int.MaxValue)
//                    {
//                        var maxDensity = MaxDensity( k / AxisLength, density );

//                        if (Score( maxSimplicity, 1, maxDensity, 1 ) < bestScore) break;

//                        var delta = (dMax - dMin) / (k + 1) / (j * q);
//                        var z = (int)Math.Ceiling( Math.Log10( delta ) );

//                        while (z < int.MaxValue)
//                        {
//                            var step = j * q * (double)Pow10( z );
//                            var maxCoverage = MaxCoverage( dMin, dMax, step * (k - 1) );

//                            if (Score( maxSimplicity, maxCoverage, maxDensity, 1 ) < bestScore) break;

//                            for (int start = (int)(Math.Floor( dMax / step - (k - 1) ) * j);
//                                 start <= (int)(Math.Ceiling( dMin / step )) * j; start++)
//                            {
//                                var lMin = start * step / j;
//                                var lMax = lMin + step * (k - 1);

//                                var s = Simplicity( q, j, lMin, lMax, step );
//                                var d = Density( k / AxisLength, density );
//                                var c = Coverage( dMin, dMax, lMin, lMax );

//                                if (Score( s, c, d, 1 ) < bestScore) continue;

//                                var option = new AxisData();

//                                var stepSequence = Enumerable.Range( 0, k ).Select( x => lMin + x * step ).ToList();
//                                var newLabels = stepSequence.Select( t => new Tuple<double, string>( t, t.ToString() ) ).ToList();

//                                option.Granularity = d;
//                                option.Coverage = c;
//                                option.Simplicity = s;
//                                option.Score = s + c + d;

//                                var score = Score( option.Simplicity, option.Coverage, option.Granularity, 1 );

//                                if (score > bestScore)
//                                {
//                                    bestScore = score;
//                                    best = option;
//                                }


//                            } // end for
//                            z++;
//                        } // end while z
//                        k++;
//                    } // end while k
//                }
//                j++;
//            } // end while j

//            return best;
//        }

//        //private AxisData PickFormat( IEnumerable<AxisData> list, double bestScore = double.NegativeInfinity )
//        //{
//        //    foreach (var data in list)
//        //    {
//        //        foreach (var format in Formats)
//        //        {
//        //            var maxLegibility = 
//        //            data.Score = Score( data.Simplicity, data.Coverage, data.Granularity, 1 );
//        //        }
//        //    }
//        //}

//        public object Clone()
//        {
//            var clone = MemberwiseClone() as Axis_old;
//            clone.Labels = new List<Tuple<decimal, string>>( Labels );

//            return clone;
//        }
//    }
//}
