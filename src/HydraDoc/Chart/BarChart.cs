using HydraDoc.Elements;
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

        public int FontSize { get; set; } = 14;

        #endregion

        #region IChart Implementation

        public AdvancedTextStyle TitleTextStyle { get; set; } = new AdvancedTextStyle();

        public string ChartTitle { get; set; }

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
        }

        public BarChart( int width, int height ) : base( height, width )
        {
            StyleList.Add( "overflow: hidden;" );
            //StyleList.Add( "transform", "rotate(-90deg)" );
            //Children.Add( TitleGroup );
            //Children.Add( Legend );

            //SvgTitle = new SVGText();

            //TitleGroup.Add( SvgTitle );            
            Children.Add( MeasuredAxis );
            Children.Add( LabeledAxis );
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

        #endregion

        private void RenderTitle()
        {
            if (!string.IsNullOrWhiteSpace( ChartTitle ))
            {
                SvgTitle.StyleList.Add( "font-family", TitleTextStyle.FontName );
                SvgTitle.StyleList.Add( "font-weight", TitleTextStyle.Bold ? "bold" : string.Empty );
                SvgTitle.StyleList.Add( "font-style", TitleTextStyle.Italic ? "italic" : string.Empty );
                SvgTitle.X = ((Width - ChartTitle.Length * (FontSize / 2)) / 4);
                SvgTitle.Y = (.05 * Height);
                SvgTitle.Fill = TitleTextStyle.Color;
            }
        }

        private void GenerateBars()
        {
            var barHeight = (Height * .8) / Bars.Count;
            var i = 0;
            var y = .1 * Height;
            foreach (var bar in Bars)
            {
                bar.Rect.X = .15 * Width;
                bar.Rect.Y = y;
                bar.Rect.Height = barHeight;
                bar.Rect.Fill = string.IsNullOrWhiteSpace( bar.Color ) ? Helpers.GetColor( Colors, 0 ) : bar.Color;
                bar.Rect.Width = .15 * Width;

                y += barHeight + 15;
            }
        }

        public override string Render()
        {
            RenderTitle();
            LabeledAxis.AxisLength = LabeledAxis.Orientation == Orientation.Vertical ? Height : Width;
            MeasuredAxis.AxisLength = MeasuredAxis.Orientation == Orientation.Vertical ? Height : Width;
            var ticks = MeasuredAxis.GenerateAxisData( Bars.Min( t => t.Value ), Bars.Max( t => t.Value ) );
            MeasuredAxis.GenerateAxisData( ticks );
            LabeledAxis.GenerateAxisData( Bars.Select( t => t.Label ) );
            GenerateBars();
            return base.Render();
        }
    }
}
