using HydraDoc.Chart.Axis;
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

    public class BarChart : SVG, IChart
    {

        #region Properties

        private readonly List<Bar> Bars = new List<Bar>();

        #region AxisOrientation 

        private Orientation _axisOrientation;

        public Orientation AxisOrientation
        {
            get { return _axisOrientation; }
            set
            {
                _axisOrientation = value;
            }
        }

        #endregion

        //public LegendPosition LegendPosition { get; set; } = LegendPosition.Right;

        public IAxis<double> MeasuredAxis { get; private set; } = new Axis<double>();

        public IAxis<string> LabeledAxis { get; private set; } = new Axis<string>();
        
        #endregion

        #region IChart Implementation

        public ITextStyle TitleTextStyle { get; set; }

        public string ChartTitle { get { return SvgTitle.Text; } set { SvgTitle.Text = value; } }

        public ITextStyle ChartTextStyle { get; set; }

        public List<string> Colors { get; private set; } = Helpers.GetDefaultColors();

        double IChart.Width
        {
            get
            {
                return Width;
            }

            set
            {
                Width = value;
            }
        }

        double IChart.Height
        {
            get
            {
                return Height;
            }

            set
            {
                Height = value;
            }
        }
        
        #endregion

        #region Constructor

        public BarChart() : this( 900, 500 )
        {            
        }

        public BarChart( int width, int height ) : base( height, width )
        {
            StyleList.Add( "overflow: visible;" );
            Children.Add( TitleGroup );

            SvgTitle = new SVGText();
            TitleTextStyle = new TextStyle( SvgTitle );
            ChartTextStyle = new TextStyle( this );

            LabeledAxis.GridLines = false;

            TitleGroup.Add( SvgTitle );
            Children.Add( VerticalAxisGroup );
            Children.Add( HorizontalAxisGroup );
            Children.Add( BarGroup );

            AxisOrientation = Orientation.Vertical;
            ChartTextStyle.FontSize = 15;
            var txtDummy = new SVGText();
            ChartTextStyle.ApplyStyle( txtDummy );
            MeasuredAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            MeasuredAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
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

        #region SVG Members

        private readonly SVGGroup TitleGroup = new SVGGroup();
        private readonly SVGText SvgTitle = new SVGText();
        private readonly SVGGroup Legend = new SVGGroup();
        private readonly SVGGroup BarGroup = new SVGGroup();
        private readonly SVGGroup HorizontalAxisGroup = new SVGGroup();
        private readonly SVGGroup VerticalAxisGroup = new SVGGroup();

        #endregion

        private void RenderChart()
        {
            BarGroup.Children.Clear();
            HorizontalAxisGroup.Children.Clear();
            VerticalAxisGroup.Children.Clear();

            LabeledAxis.SetTicks( Bars.Select( t => t.Label ) );
            if (!string.IsNullOrWhiteSpace( ChartTitle ))
            {
                SvgTitle.X = Width / 2;
                SvgTitle.Y = 1.5 * TitleTextStyle.FontSize;
                SvgTitle.StyleList.Add( "text-anchor", "middle" );
            }

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
            MeasuredAxis.SetTicks( SVGAxisHelpers.SuggestTicks( min, Bars.Max( t => t.Value ), measuredIntervals ) );
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

                BarGroup.Add( svgBar );
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
            MeasuredAxis.SetTicks( SVGAxisHelpers.SuggestTicks( min, Bars.Max( t => t.Value ), intervals ) );
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

                BarGroup.Add( svgBar );
            }
        }

        private double GetTitleHeight()
        {
            return !string.IsNullOrWhiteSpace( ChartTitle ) ? TitleTextStyle.FontSize * 4 : 0;
        }

        public override string Render()
        {
            RenderChart();
            return base.Render();
        }
    }
}
