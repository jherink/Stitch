using HydraDoc.Chart.Axis;
using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart.Axis
{
    public static class SVGAxisHelpers
    {
        public static int MaxHorizontalAxisLength = 4;

        private static string FormatValueToAxisSpecification<T>( T value, string format )
        {
            return !string.IsNullOrWhiteSpace( format ) ? string.Format( "{0:" + format + "}", value ) : value.ToString();
        }

        public static double[] RenderAxisVertically<T>( IAxis<T> axis, ISVGGroup group, double x,
                                                        double startY,
                                                        double horizontalSpacing,
                                                        double chartWidth ) where T : IComparable<T>
        {
            var locations = new double[axis.Ticks.Count];
            var maxTickLength = axis.Ticks.Max( t => FormatValueToAxisSpecification( t, axis.Format ).Length );
            var i = 0;

            foreach (var tick in axis.Ticks)
            {
                var text = new SVGText()
                {
                    X = x,
                    Y = startY - (axis.AxisTextStyle.FontSize / 4),

                };
                text.StyleList.Add( "font-size", $"{axis.AxisTextStyle.FontSize}px" );

                var formattedLabel = FormatValueToAxisSpecification( tick, axis.Format );
                text.Text.Append( formattedLabel );
                group.Add( text );

                if (axis.GridLines)
                { // Add grid lines
                    var labeledRect = new SVGRectangle
                    {
                        //X = x + maxTickLength * axis.AxisTextStyle.FontSize / 2, 
                        X = x + Math.Max( maxTickLength, 4 ) * axis.AxisTextStyle.FontSize / 1.5,
                        Y = startY - (axis.AxisTextStyle.FontSize / 2),
                        Fill = axis.GridLineColor,
                        Width = chartWidth,
                        Height = 1
                    };
                    group.Add( labeledRect );
                }
                locations[i++] = startY - (axis.AxisTextStyle.FontSize / 2);
                startY += horizontalSpacing;
            }

            return locations.ToArray();
        }

        public static double[] RenderAxisHorizontally<T>( IAxis<T> axis, ISVGGroup group, double startX,
                                                          double gridTopY, double gridBottomY,
                                                          double verticalSpacing, double chartHeight ) where T : IComparable<T>
        {
            var locations = new double[axis.Ticks.Count];
            var i = 0;
            var rotation = string.Empty;
            var maxTickLength = axis.Ticks.Max( t => FormatValueToAxisSpecification( t, axis.Format ).Length );
            var textY = gridBottomY + axis.AxisTextStyle.FontSize;

            foreach (var label in axis.Ticks)
            {
                var formattedLabel = FormatValueToAxisSpecification( label, axis.Format );
                var text = new SVGText()
                {
                    X = startX - (formattedLabel.Length - 1) * axis.AxisTextStyle.FontSize / 2
                };
                if (maxTickLength > MaxHorizontalAxisLength)
                {
                    text.X = startX - axis.AxisTextStyle.FontSize / 2;
                    textY = gridBottomY;
                    text.Transform = $"rotate(90 {startX - axis.AxisTextStyle.FontSize},{textY})";
                }
                text.StyleList.Add( "font-size", $"{axis.AxisTextStyle.FontSize}px" );
                text.Y = textY;

                text.Text.Append( formattedLabel );
                group.Add( text );

                if (axis.GridLines)
                { // add grid lines
                    var labeledRect = new SVGRectangle()
                    {
                        X = startX,
                        Y = gridTopY,
                        Fill = axis.GridLineColor,
                        Height = gridBottomY - gridTopY,
                        Width = 1
                    };
                    group.Add( labeledRect );
                }
                locations[i++] = startX;
                startX += verticalSpacing;
            }

            return locations;
        }

        public static void RenderAxis<T1, T2>( IAxis<T1> vertical,
                                               IAxis<T2> horizontal,
                                               double chartWidth, double chartHeight, double startX, double startY,
                                               SVGGroup verticalGroup, SVGGroup horizontalGroup,
                                               out double[] verticalAxisLocations,
                                               out double[] horizontalAxisLocations ) where T1 : IComparable<T1>
                                                                                      where T2 : IComparable<T2>
        {
            var verticalAxis = vertical.Clone() as IAxis<T1>;
            var horizontalAxis = horizontal.Clone() as IAxis<T2>;

            var titleHeight = startY;
            var rotation = string.Empty;
            var verticalSpace = chartWidth / vertical.Ticks.Count;
            var horizontalSpace = 0.0;
            var labeledAxisX = 0.0;
            var verticalOffset = !string.IsNullOrWhiteSpace( verticalAxis.AxisTitle ) ? verticalAxis.AxisTitleTextStyle.FontSize : 0;

            // We need to generate this first so we know how far down the measured axis can go.
            var space = 1.5 * verticalAxis.AxisTextStyle.FontSize;
            var horizTicks = horizontalAxis.ReverseDirection ? horizontalAxis.Ticks.Reverse() : horizontalAxis.Ticks;
            horizontalAxis.SetTicks( horizTicks );

            var verticalTicks = verticalAxis.ReverseDirection ? verticalAxis.Ticks.Reverse() : verticalAxis.Ticks;
            verticalAxis.SetTicks( verticalTicks );

            var maxTickLength = verticalAxis.Ticks.Max( t => FormatValueToAxisSpecification( t, verticalAxis.Format ).Length );

            // Render vertical axis
            labeledAxisX = 1.125 * verticalAxis.AxisTextStyle.FontSize;
            horizontalSpace = (chartHeight - titleHeight - space - verticalOffset) / verticalAxis.Ticks.Count;

            if (!string.IsNullOrWhiteSpace( verticalAxis.AxisTitle ))
            {
                labeledAxisX = 1.125 * verticalAxis.AxisTitleTextStyle.FontSize;
                var labeledAxisTitle = new SVGText
                {
                    X = labeledAxisX,
                    Y = chartHeight / 2
                };
                labeledAxisTitle.Append( verticalAxis.AxisTitle );
                labeledAxisTitle.StyleList.Add( "text-anchor", "middle" );
                labeledAxisTitle.Transform = $"rotate( -90 {labeledAxisTitle.X},{labeledAxisTitle.Y})";
                verticalAxis.AxisTextStyle.ApplyStyle( labeledAxisTitle );
                verticalGroup.Add( labeledAxisTitle );
                labeledAxisX *= 2;
            }
            var preLabelX = labeledAxisX;
            // Render Vertical Axis. We need to render this first so we know where the final grid line is at.
            verticalAxisLocations = RenderAxisVertically( verticalAxis, verticalGroup,
                                                          labeledAxisX,
                                                          titleHeight + horizontalSpace / 2,
                                                          horizontalSpace,
                                                          chartWidth );

            // Render horizontal axis
            verticalSpace = (chartWidth - (preLabelX - horizontalAxis.AxisTitleTextStyle.FontSize)) / horizontal.Ticks.Count;
            // This increase needs to match the grid lines X calculation when rendering vertical axis.
            //labeledAxisX += Math.Max(maxTickLength, 3) * verticalAxis.AxisTextStyle.FontSize / 1.5; 
            labeledAxisX += Math.Max( maxTickLength, 4 ) * verticalAxis.AxisTextStyle.FontSize / 1.5;

            if (!string.IsNullOrWhiteSpace( horizontalAxis.AxisTitle ))
            {
                var measuredAxisTitle = new SVGText
                {
                    X = chartWidth / 2,
                    Y = chartHeight - horizontalAxis.AxisTitleTextStyle.FontSize / 4
                };
                measuredAxisTitle.Append( horizontalAxis.AxisTitle );
                measuredAxisTitle.StyleList.Add( "text-anchor", "middle" );
                horizontalAxis.AxisTextStyle.ApplyStyle( measuredAxisTitle );
                horizontalGroup.Add( measuredAxisTitle );
            }

            // Render Labeled Axis.  
            horizontalAxisLocations = RenderAxisHorizontally( horizontalAxis, horizontalGroup,
                                                              labeledAxisX, verticalAxisLocations.Min(),
                                                              verticalAxisLocations.Max(),
                                                              verticalSpace, chartHeight - verticalOffset );
        }

        public static int SuggestIntervals( double size )
        {
            var guess = (int)(size / 100.0);
            if (guess <= 5)
            {
                return guess;
            }
            else if (guess <= 7)
            {
                return 6;
            }
            else if (guess <= 9)
            {
                return 7;
            }
            else if (guess <= 12)
            {
                return 8;
            }
            else if (guess <= 15)
            {
                return 9;
            }
            else {
                return 10;
            }

        }

        #region Approximating Axis Intervals

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

        #endregion

        public static IEnumerable<double> SuggestTicks( double min, double max, int intervals = 5 )
        {
            intervals = Math.Max( intervals, 3 );
            return GenerateTicks( min, max, intervals - 2 );
        }
    }
}
