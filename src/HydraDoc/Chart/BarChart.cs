using HydraDoc.Chart.Axis;
using HydraDoc.Chart.Axis.Algorithms;
using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
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

        // TODO?
        //public LegendPosition LegendPosition { get; set; } = LegendPosition.Right;
        
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
        }

        #endregion
        
        protected override void RenderChartImpl()
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
            var measuredIntervals = SVGAxisHelpers.SuggestIntervals( Width );
            var min = MeasuredAxis.IncludeDefault ? default(double) : Bars.Min( t => t.Value );
            //MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( min, Bars.Max( t => t.Value ), measuredIntervals ) );
            var set = new List<double>() { 0 };
            set.AddRange( Bars.Select( t => t.Value ) );
            MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( set, measuredIntervals ) );
            var labeledClone = LabeledAxis.Clone() as IAxis<string>;
            var ticks = labeledClone.Ticks.ToList();
            ticks.Insert( 0, string.Empty );
            ticks.Add( string.Empty );
            labeledClone.SetTicks( ticks );

            SVGAxisHelpers.RenderAxis( labeledClone, MeasuredAxis, Width, Height, 0, GetTitleHeight(),                                     
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
                    Fill = string.IsNullOrWhiteSpace( bar.Color ) ? Helpers.GetColor( Colors, 0 ) : bar.Color
                };

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
            //MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( min, Bars.Max( t => t.Value ), intervals ) );
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

            SVGAxisHelpers.RenderAxis( measuredClone, horizontalClone, Width, Height, 0, GetTitleHeight(),
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
                    Width = 2* verticalSpace,
                    Fill = string.IsNullOrWhiteSpace( bar.Color ) ? Helpers.GetColor( Colors, 0 ) : bar.Color
                };

                ChartGroup.Add( svgBar );
            }
        }
    }
}
