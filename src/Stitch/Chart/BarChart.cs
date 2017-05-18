using Stitch.Chart.Axis;
using Stitch.Chart.Axis.Algorithms;
using Stitch.Elements;
using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    };

    public class Bar
    {
        public string Label { get; set; }

        public string Color { get; set; }

        public double Value { get; set; }
    }

    public class BarChart : AxisChart<string, double>
    {
        private readonly List<Bar> Bars = new List<Bar>();

        public Orientation AxisOrientation { get; set; }

        private readonly Dictionary<string, List<Bar>> BarGroups = new Dictionary<string, List<Bar>>();

        #region Constructor

        public BarChart() : this( 900, 500 )
        {
        }

        public BarChart( int width, int height ) : base( width, height )
        {
            AxisOrientation = Orientation.Vertical;
            LabeledAxis.GridLines = false;
            LabeledAxisTickAlgorithm = null;
        }

        #endregion

        #region Public Methods

        public void AddBar( string label, double value, string color = "" )
        {
            AddBar( new Bar() { Label = label, Value = value, Color = color } );
        }

        public void AddBar( Bar bar )
        {
            Bars.Add( bar );
            RenderChart();
        }

        #endregion

        protected override void RenderAxisChartImpl()
        {
            LabeledAxis.SetTicks( Bars.Select( t => t.Label ) );

            switch (AxisOrientation)
            {
                case Orientation.Horizontal:
                    RenderChartHorizontally();
                    break;
                case Orientation.Vertical:
                    RenderChartVertically();
                    break;

            }
        }

        private void RenderChartHorizontally()
        {
            // Bars going --->>
            // Measured Axis is on bottom. 

            // Render bars
            var horizontalAxisLocations = new double[] { };
            var verticalAxisLocations = new double[] { };
            var measuredIntervals = SVGAxisHelpers.SuggestIntervals( GetChartableAreaWidth() );
            var min = MeasuredAxis.IncludeDefault ? default( double ) : Bars.Min( t => t.Value );
            //MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( min, Bars.Max( t => t.Value ), measuredIntervals ) );
            var set = new List<double>() { 0 };
            set.AddRange( Bars.Select( t => t.Value ) );
            MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( set, measuredIntervals ) );
            var labeledClone = LabeledAxis.Clone() as IAxis<string>;
            var ticks = labeledClone.Ticks.ToList();
            ticks.Insert( 0, string.Empty );
            ticks.Add( string.Empty );
            labeledClone.SetTicks( ticks );

            SVGAxisHelpers.RenderAxis( labeledClone, MeasuredAxis, GetChartableAreaWidth(), Height, GetLegendLeftOffset(), GetTitleHeight(),
                                       VerticalAxisGroup, HorizontalAxisGroup, out verticalAxisLocations,
                                       out horizontalAxisLocations );

            var horizontalSpace = (horizontalAxisLocations.Max() - horizontalAxisLocations.Min()) / horizontalAxisLocations.Length;
            var verticalSpace = (verticalAxisLocations.Max() - verticalAxisLocations.Min()) / verticalAxisLocations.Length;
            var baseLineX = horizontalAxisLocations.Min();
            for (int i = 1; i < labeledClone.Ticks.Count - 1; i++)
            {
                var label = labeledClone.Ticks[i];
                var bar = Bars.First( t => t.Label == label );
                var physicalChartSize = horizontalAxisLocations.Max() - baseLineX;
                var barWidth = bar.Value * physicalChartSize / MeasuredAxis.MaxValue;
                var barY = verticalAxisLocations[i] - (verticalSpace / 2);
                var svgBar = new SVGRectangle
                {
                    X = baseLineX,
                    Y = barY,
                    Width = barWidth,
                    Height = verticalSpace,
                    Fill = bar.Color
                };
                svgBar.ClassList.Add( "stitch-theme" );

                ChartGroup.Add( svgBar );
            }
        }

        private void RenderChartVertically()
        {
            // Bars going   |
            //              |
            //              |
            //              V
            //              V
            // Labeled Axis is on bottom.

            var horizontalAxisLocations = new double[] { };
            var verticalAxisLocations = new double[] { };
            var intervals = SVGAxisHelpers.SuggestIntervals( Height );
            var min = MeasuredAxis.IncludeDefault ? default( double ) : Bars.Min( t => t.Value );

            var set = new List<double>() { 0 };
            set.AddRange( Bars.Select( t => t.Value ) );
            MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( set, intervals ) );

            // Because of how SVG renders we must reverse the measured values first.
            var measuredClone = MeasuredAxis.Clone() as IAxis<double>;
            measuredClone.SetTicks( measuredClone.Ticks.Reverse() );
            var horizontalClone = LabeledAxis.Clone() as IAxis<string>;
            var ticks = horizontalClone.Ticks.ToList();
            ticks.Insert( 0, string.Empty );
            horizontalClone.SetTicks( ticks );

            SVGAxisHelpers.RenderAxis( measuredClone, horizontalClone, GetChartableAreaWidth(), Height, GetLegendLeftOffset(), GetTitleHeight(),
                                       VerticalAxisGroup, HorizontalAxisGroup,
                                       out verticalAxisLocations, out horizontalAxisLocations );

            var verticalSpace = (verticalAxisLocations.Max() - verticalAxisLocations.Min()) / verticalAxisLocations.Length;

            // Render bars.
            var baseLineY = verticalAxisLocations.Max();
            for (int i = 1; i < horizontalClone.Ticks.Count; i++)
            {
                var label = horizontalClone.Ticks[i];
                var bar = Bars.First( t => t.Label == label );
                var physicalChartSize = baseLineY - verticalAxisLocations.Min();
                var barHeight = bar.Value * physicalChartSize / MeasuredAxis.MaxValue;
                var barX = horizontalAxisLocations[i] - (verticalSpace); // center bar on grid line
                var svgBar = new SVGRectangle
                {
                    X = barX,
                    Y = baseLineY - barHeight,
                    Height = barHeight,
                    Width = 2 * verticalSpace,
                    Fill = bar.Color
                };
                svgBar.ClassList.Add( "stitch-theme" );

                ChartGroup.Add( svgBar );
            }
        }

        public override double GetLegendLeftOffset()
        {
            return LegendPosition == LegendPosition.Left && LabeledAxis.Visible ? Bars.Max( t => t.Label.Length ) * ChartTextStyle.FontSize : 0;
        }

        public override double GetLegendRightOffset()
        {
            return LegendPosition == LegendPosition.Right && LabeledAxis.Visible ? Bars.Max( t => t.Label.Length ) * ChartTextStyle.FontSize : 0;            
        }

        public override void RenderLegend()
        {
            Legend.Children.Clear();
            if (LegendPosition != LegendPosition.None)
            {
                var i = 1;

                double _cx = ChartTextStyle.FontSize / 2, _cy = 0;

                switch (LegendPosition)
                {
                    case LegendPosition.Top:
                        _cy = GetTitleHeight();
                        break;
                    case LegendPosition.Bottom:
                        _cy = Height - GetLegendBottomOffset();
                        break;
                    case LegendPosition.Left:
                        _cy = 1.5 * GetTitleHeight() ;
                        break;
                    case LegendPosition.Right:
                        _cx = GetChartableAreaWidth() + 2 * ChartTextStyle.FontSize;
                        _cy = 1.5 * GetTitleHeight();
                        break;
                }

                var j = 1;
                foreach (var bar in Bars)
                {
                    //var item = CreateLegendLine( bar.Label, bar.Color, j++ ); 
                    var item = CreateLegendLine( bar.Label, bar.Color, j ); // until multiple bars there is only one theme.
                    var circle = item.Children.ToList()[0] as SVGCircle;
                    var text = item.Children.ToList()[1] as SVGText;

                    switch (LegendPosition)
                    {
                        case LegendPosition.Bottom:
                        case LegendPosition.Top:
                            circle.Cx = _cx;
                            text.X = _cx + 2 * circle.R;
                            circle.Cy = _cy;
                            text.Y = _cy + ChartTextStyle.FontSize / 3.0;
                            _cx += (1.75 + text.Text.Text.Length) * ChartTextStyle.FontSize;
                            break;
                        case LegendPosition.Right:
                        case LegendPosition.Left:
                            circle.Cx = _cx;
                            text.X = _cx + 2 * circle.R;
                            circle.Cy = _cy;
                            text.Y = _cy + ChartTextStyle.FontSize / 3.0;
                            _cy += 1.75 * ChartTextStyle.FontSize;
                            break;

                    }

                    Legend.Children.Add( item );
                }
            }
        }
    }
}
