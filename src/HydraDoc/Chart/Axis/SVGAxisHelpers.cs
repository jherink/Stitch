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
                                                        double startY, double horizontalSpacing, 
                                                        double chartWidth, double fontSize ) where T : IComparable<T>
        {
            var space = 1.5 * fontSize;
            var locations = new double[axis.Ticks.Count];
            var i = 0;
            foreach (var tick in axis.Ticks)
            {
                var text = new SVGText()
                {
                    X = x,
                    Y = startY
                };
                
                text.Text.Append( FormatValueToAxisSpecification( tick, axis.Format ) );
                group.Add( text );

                if (axis.GridLines)
                { // Add grid lines
                    var labeledRect = new SVGRectangle
                    {
                        X = x + space,
                        Y = startY - (fontSize / 2),
                        Fill = axis.GridLineColor,
                        Width = chartWidth,
                        Height = 1
                    };
                    group.Add( labeledRect );
                }
                locations[i++] = startY - (fontSize / 2);
                startY += horizontalSpacing;
            }
            
            return locations.ToArray();
        }

        public static double[] RenderAxisHorizontally<T>( IAxis<T> axis, ISVGGroup group, double startX, 
                                                          double gridTopY, double gridBottomY,
                                                          double verticalSpacing, double chartHeight,
                                                          double fontSize ) where T : IComparable<T>
        {
            var locations = new double[axis.Ticks.Count];
            var i = 0;
            var rotation = string.Empty;
            var maxTickLength = axis.Ticks.Max( t => FormatValueToAxisSpecification( t, axis.Format ).Length );
            var textY = chartHeight - (maxTickLength * (fontSize + 1) / 2);

            foreach (var label in axis.Ticks)
            {
                var text = new SVGText()
                {
                    X = startX,
                    Y = textY
                };
                if (maxTickLength > MaxHorizontalAxisLength)
                {
                    text.Transform = $"rotate(90 {startX},{textY})";
                }
                else
                {
                    text.StyleList.Add( "text-anchor", "middle" );
                }                          

                text.Text.Append( FormatValueToAxisSpecification(label, axis.Format) );
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
    }
}
