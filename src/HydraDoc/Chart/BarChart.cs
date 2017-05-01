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

        internal SVGRectangle Rect { get; set; } = new SVGRectangle();
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
                LabeledAxis.Orientation = value;
                switch (value)
                {
                    case Orientation.Horizontal:
                        MeasuredAxis.Orientation = Orientation.Vertical;
                        break;
                    case Orientation.Vertical:
                        MeasuredAxis.Orientation = Orientation.Horizontal;
                        break;
                }
            }
        }

        #endregion

        public LegendPosition LegendPosition { get; set; } = LegendPosition.Right;

        public Axis<double> MeasuredAxis { get; private set; } = new Axis<double>();

        public Axis<string> LabeledAxis { get; private set; } = new Axis<string>();

        public int BarSpacing { get; set; } = 15;

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
            AxisOrientation = Orientation.Vertical;
            ChartTextStyle.FontSize = MeasuredAxis.AxisTextStyle.FontSize = LabeledAxis.AxisTextStyle.FontSize = 15;
        }

        public BarChart( int width, int height ) : base( height, width )
        {
            StyleList.Add( "overflow: hidden;" );
            Children.Add( TitleGroup );

            SvgTitle = new SVGText();
            TitleTextStyle = new TextStyle( SvgTitle );
            ChartTextStyle = new TextStyle( this );

            LabeledAxis.GraphWidth = MeasuredAxis.GraphWidth = Width;
            LabeledAxis.GraphHeight = MeasuredAxis.GraphHeight = Height;

            TitleGroup.Add( SvgTitle );
            //Children.Add( MeasuredAxis );
            //Children.Add( LabeledAxis );
            Children.Add( VerticalAxisGroup );
            Children.Add( HorizontalAxisGroup );
            Children.Add( BarGroup );
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
            BarGroup.Add( bar.Rect );
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

        private void RenderTitle()
        {
            if (!string.IsNullOrWhiteSpace( ChartTitle ))
            {
                SvgTitle.X = ((Width - ChartTitle.Length * (ChartTextStyle.FontSize / 2)) / 4);
                SvgTitle.Y = (.05 * Height);
                SvgTitle.Fill = TitleTextStyle.Color;
            }
        }

        private void GenerateBars( double maxTick )
        {
            //var _height = .8 * Height;
            //_height -= AxisOrientation == Orientation.Vertical ? MeasuredAxis.Ticks.Max( t => t.ToString().Length ) * MeasuredAxis.AxisTextStyle.FontSize : 0;
            //var barHeight = _height / Bars.Count;

            //var y = .1 * Height;
            //var x = .05 * Width;
            //foreach (var bar in Bars)
            //{
            //    bar.Rect.X = x;
            //    bar.Rect.Y = y;
            //    bar.Rect.Fill = string.IsNullOrWhiteSpace( bar.Color ) ? Helpers.GetColor( Colors, 0 ) : bar.Color;

            //    switch (AxisOrientation)
            //    {
            //        case Orientation.Vertical:
            //            bar.Rect.Height = barHeight;
            //            bar.Rect.Width = .85 * Width * bar.Value / maxTick;

            //            y += barHeight + BarSpacing;
            //            break;
            //        case Orientation.Horizontal:
            //            bar.Rect.Width = barHeight;
            //            bar.Rect.Height = .9 * Height * bar.Value / maxTick;

            //            x += barHeight + BarSpacing;
            //            break;
            //    }
            //}
        }

        //private void RenderChart()
        //{
        //    LabeledAxis.GraphWidth = MeasuredAxis.GraphWidth = base.Width;
        //    LabeledAxis.GraphHeight = MeasuredAxis.GraphHeight = base.Height;
        //    switch (LabeledAxis.Orientation)
        //    {
        //        case Orientation.Horizontal:
        //            LabeledAxis.AxisLength = Width;                    
        //            break;
        //        case Orientation.Vertical:
        //            LabeledAxis.AxisLength = Height;
        //            break;
        //    }
        //    switch (MeasuredAxis.Orientation)
        //    {
        //        case Orientation.Horizontal:
        //            MeasuredAxis.AxisLength = Width;
        //            break;
        //        case Orientation.Vertical:
        //            MeasuredAxis.AxisLength = Height;
        //            break;
        //    }

        //    var ticks = MeasuredAxis.GenerateAxisData( Bars.Min( t => t.Value ), Bars.Max( t => t.Value ) );
        //    ticks = MeasuredAxis.Orientation == Orientation.Vertical ? ticks.Reverse() : ticks;
        //    var measuredTexts = MeasuredAxis.GenerateAxisData( ticks );
        //    var labeledTexts = LabeledAxis.GenerateAxisData( Bars.Select( t => t.Label ) );

        //    //TODO: Render Bars

        //    //TODO: Render Grid Lines

        //    //TODO: Render Axis Lines

        //    //TODO: Render Axis Text
        //}

        public void RenderChart()
        {
            BarGroup.Children.Clear();
            HorizontalAxisGroup.Children.Clear();
            VerticalAxisGroup.Children.Clear();

            LabeledAxis.SetTicks( Bars.Select( t => t.Label ) );
            var titleHeight = 0.0;
            if (!string.IsNullOrWhiteSpace( ChartTitle ))
            {
                titleHeight = TitleTextStyle.FontSize * 4;
                SvgTitle.X = Width / 2;
                SvgTitle.Y = 1.5 * TitleTextStyle.FontSize;
                SvgTitle.StyleList.Add( "text-anchor", "middle" );
            }

            var rotation = string.Empty;
            var verticalSpace = Width / Bars.Count;
            var horizontalSpace = 0.0;
            var measuredAxisX = 0.0;
            var measuredAxisY = 0.0;
            var labeledAxisX = 0.0;
            var labeledAxisY = 0.0;
            var maxTickLength = 0;

            switch (AxisOrientation)
            {
                case Orientation.Horizontal:
                    // Bars going --->>
                    // Measured Axis is on bottom. 




                    break;
                case Orientation.Vertical:
                    // Bars going   |
                    //              |
                    //              |
                    //              V
                    //              V
                    // Labeled Axis is on bottom.
                    // We need to generate this first so we know how far down the measured axis can go.
                    var space = 1.5 * ChartTextStyle.FontSize;
                    var measuredTicks = MeasuredAxis.SuggestTicks( Bars.Min( t => t.Value ), Bars.Max( t => t.Value ) );
                    // Reverse ticks if necessary.  Note: because of the way SVG is rendered we 
                    // render them backwards when reverse isn't specified so they render correctly.
                    measuredTicks = MeasuredAxis.ReverseDirection ? measuredTicks : measuredTicks.Reverse();
                    MeasuredAxis.SetTicks( measuredTicks );
                    labeledAxisX = verticalSpace / 2;
                    maxTickLength = LabeledAxis.Ticks.Max( t => t.Length );
                    labeledAxisY = Height - (maxTickLength * (ChartTextStyle.FontSize + 1) / 2);

                    // Render Labeld Axsis
                    foreach (var label in LabeledAxis.Ticks)
                    {
                        var bar = Bars.First( t => t.Label == label );
                        rotation = maxTickLength > 4 ? $"rotate(90 {labeledAxisX},{labeledAxisY})" : string.Empty;
                        var text = new SVGText()
                        {
                            X = labeledAxisX,
                            Y = labeledAxisY,
                            Transform = rotation
                        };

                        //var barHeight = (((Height - (Height - labeledAxisY)) * bar.Value) / MeasuredAxis.MaxValue) - 2 * (ChartTextStyle.FontSize + 1);
                        var barHeight = bar.Value * (Height - titleHeight - (Height - labeledAxisY)) / MeasuredAxis.MaxValue;
                        var barY = labeledAxisY - barHeight - space;
                        var svgBar = new SVGRectangle()
                        {
                            X = labeledAxisX - (verticalSpace / 4.5), // check this
                            Y = barY,
                            Height = barHeight,
                            Width = verticalSpace / 2,
                            Fill = string.IsNullOrWhiteSpace( bar.Color ) ? Helpers.GetColor( Colors, 0 ) : bar.Color
                        };

                        text.Text.Append( label );
                        HorizontalAxisGroup.Add( text );
                        BarGroup.Add( svgBar );

                        if (LabeledAxis.GridLines)
                        { // add grid lines
                            var labeledRect = new SVGRectangle()
                            {
                                X = labeledAxisX,
                                Y = titleHeight - ChartTextStyle.FontSize,
                                Fill = LabeledAxis.GridLineColor,
                                Height = barY,
                                Width = 1
                            };
                            HorizontalAxisGroup.Add( labeledRect );
                        }                                               

                        labeledAxisX += verticalSpace;
                    }

                    measuredAxisX = ChartTextStyle.FontSize;
                    measuredAxisY = titleHeight;
                    horizontalSpace = (Height - titleHeight - space) / MeasuredAxis.Ticks.Count;

                    // Render Vertical Axis
                    foreach (var tick in MeasuredAxis.Ticks)
                    {
                        var text = new SVGText()
                        {
                            X = measuredAxisX,
                            Y = measuredAxisY
                        };
                        text.Text.Append( tick.ToString( MeasuredAxis.Format ) );
                        VerticalAxisGroup.Add( text );

                        if (MeasuredAxis.GridLines)
                        { // Add grid lines
                            var labeledRect = new SVGRectangle
                            {
                                X = measuredAxisX + space,
                                Y = measuredAxisY - (ChartTextStyle.FontSize / 2),
                                Fill = MeasuredAxis.GridLineColor,
                                Width = Width,
                                Height = 1
                            };
                            VerticalAxisGroup.Add( labeledRect );
                        }

                        measuredAxisY += horizontalSpace;
                    }

                    var plotableArea = measuredAxisY - titleHeight;
                    
                    break;
                    
            }
        }

        public override string Render()
        {
            RenderTitle();
            RenderChart();
            return base.Render();
        }
    }
}
