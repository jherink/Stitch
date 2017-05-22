using Stitch.Chart.Axis;
using Stitch.Chart.Axis.Algorithms;
using Stitch.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    public class Line<T1, T2> where T1 : IComparable<T1>
                              where T2 : IComparable<T2>
    {
        public string LineName { get; set; }
        public List<Tuple<T1, T2>> Values { get; set; } = new List<Tuple<T1, T2>>();
        public string Color { get; set; }

        public void AddPoint( T1 x, T2 y )
        {
            Values.Add( new Tuple<T1, T2>( x, y ) );
        }

        public void Sort()
        {
            Values.OrderBy( t => t.Item1 );
        }
    }
    public class LineChart<T1, T2> : AxisChart<T1, T2> where T1 : IComparable<T1>
                                                       where T2 : IComparable<T2>
    {
        private readonly List<Line<T1, T2>> Lines = new List<Line<T1, T2>>();

        #region Constructors

        public LineChart() : this( 900, 500 )
        {

        }

        public LineChart( int width, int height ) : base( width, height )
        {
        }

        #endregion

        public Line<T1, T2> GetLine( string lineName )
        {
            return Lines.FirstOrDefault( t => t.LineName.Equals( lineName, StringComparison.InvariantCultureIgnoreCase ) );
        }

        public Line<T1, T2> GetLine( int index )
        {
            return Lines[index];
        }

        public void AddPoint( string lineName, T1 xValue, T2 yValue )
        {
            var line = GetLine( lineName );

            if (line == default( Line<T1, T2> ))
            {
                line = new Line<T1, T2>() { LineName = lineName };
                AddLine( line );
            }

            line.AddPoint( xValue, yValue ); // add point to line.
        }

        public void AddLine( Line<T1, T2> line )
        {
            Lines.Add( line );
        }

        private void Sort()
        {
            foreach (var line in Lines) line.Sort();
        }

        protected override void RenderAxisChartImpl()
        {
            // labeled axis is on the bottom.

            var horizontalAxisLocations = new double[] { };
            var verticalAxisLocations = new double[] { };
            var horizontalIntervals = AxisHelper.SuggestIntervals( GetChartableAreaWidth() );
            var verticalIntervals = AxisHelper.SuggestIntervals( Height );
            //T1 horizontalMin, horizontalMax;
            //T2 verticalMin, verticalMax;
            Sort();
            var horizontalSet = GetHorizontalSet().ToList();
            var verticalSet = GetVerticalSet().ToList();
            if (LabeledAxis.IncludeDefault) horizontalSet.Insert( 0, default( T1 ) );
            if (MeasuredAxis.IncludeDefault) verticalSet.Insert( 0, default( T2 ) );
            LabeledAxis.SetTicks( LabeledAxisTickAlgorithm.SuggestTicks( horizontalSet, horizontalIntervals ) );
            MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( verticalSet, verticalIntervals ) );

            var horizontalClone = LabeledAxis.Clone() as IAxis<T1>;

            // Because of how SVG renders we must reverse the measured values first.
            var measuredClone = MeasuredAxis.Clone() as IAxis<T2>;
            measuredClone.SetTicks( measuredClone.Ticks.Reverse() );

            AxisHelper.RenderAxis( measuredClone, horizontalClone, GetChartableAreaWidth(), Height - GetLegendBottomOffset() - GetLegendTopOffset(),
                                       GetLegendLeftOffset(), GetTitleHeight() + GetLegendTopOffset(),
                                       VerticalAxisGroup, HorizontalAxisGroup,
                                       out verticalAxisLocations, out horizontalAxisLocations );

            var baseLineY = verticalAxisLocations.Max();
            var baseLineX = horizontalAxisLocations.Min();
            var horizSpace = verticalAxisLocations.Min();
            var chartableWidth = horizontalAxisLocations.Max() - horizontalAxisLocations.Min();
            var chartableHeight = verticalAxisLocations.Max() - verticalAxisLocations.Min();
            var i = 1;
            foreach (var line in Lines)
            {
                var svgLine = new SVGPath()
                {
                    StrokeWidth = 2,
                    FillOpacity = 1,
                    Stroke = line.Color,
                    Fill = "none"
                };
                //svgLine.ClassList.Add( GetChartStrokeTheme( i++ ) );
                svgLine.ClassList.Add( GetChartTheme( i++ ) );
                int lineI = 0;

                foreach (var point in line.Values)
                {
                    var x = LabeledAxisTickAlgorithm.Subtract( horizontalSet, point.Item1, LabeledAxisTickAlgorithm.Min( horizontalSet ) );
                    var y = MeasuredAxisTickAlgorithm.Subtract( verticalSet, point.Item2, MeasuredAxisTickAlgorithm.Min( verticalSet ) );
                    var pctX = LabeledAxisTickAlgorithm.Percentage( horizontalSet, point.Item1 );
                    var pctY = MeasuredAxisTickAlgorithm.Percentage( verticalSet, point.Item2 );
                    var px = baseLineX + pctX * chartableWidth;
                    var py = (chartableHeight - pctY * chartableHeight) + horizSpace;

                    if (lineI++ == 0)
                    { // move to start point if first line point.
                        if (!LabeledAxis.IncludeDefault && MeasuredAxis.IncludeDefault)
                        {
                            svgLine.MoveTo( baseLineX, py );
                        }
                        else
                        {
                            svgLine.MoveTo( baseLineX, baseLineY );
                        }
                    }

                    svgLine.LineTo( px, py );
                }
                ChartGroup.Add( svgLine );
            }
        }

        private IEnumerable<T1> GetHorizontalSet()
        {
            var set = new List<T1>();
            foreach (var line in Lines)
            {
                set.AddRange( line.Values.Select( t => t.Item1 ) );
            }
            return set.Distinct();
        }

        private IEnumerable<T2> GetVerticalSet()
        {
            var set = new List<T2>();
            foreach (var line in Lines)
            {
                set.AddRange( line.Values.Select( t => t.Item2 ) );
            }
            return set.Distinct();
        }

        public override double GetLegendLeftOffset()
        {
            return LegendPosition == LegendPosition.Left ? GraphicsHelper.MeasureStringWidth( Lines.Select( t => t.LineName ), ChartTextStyle ) : 0;
        }

        public override double GetLegendRightOffset()
        {
            return LegendPosition == LegendPosition.Right ? GraphicsHelper.MeasureStringWidth( Lines.Select( t => t.LineName ), ChartTextStyle ) : 0;
        }
               
        protected override IEnumerable<Tuple<string, string>> GetLegendItems()
        {
            return Lines.Select( t => new Tuple<string, string>( t.LineName, t.Color ) );
        }
    }
}

