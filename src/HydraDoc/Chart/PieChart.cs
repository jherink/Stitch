using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HydraDoc.Chart
{
    public enum PieSliceText
    {
        Percentage,
        Value,
        Label,
        None
    };

    public class PieSlice : SVGGroup
    {
        public string Label { get; set; }
        /// <summary>
        /// The color to use for this slice. Specify a valid HTML color string.
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// How far to separate the slice from the rest of the pie, from 0.0 (not at all) to 1.0 (the pie's radius)
        /// </summary>
        public double Offset { get; set; }
        /// <summary>
        /// The value of the slice.
        /// </summary>
        public double Value { get; set; }

        internal SVGText Text { get; set; } = new SVGText();

        internal SVGPath Path { get; set; } = new SVGPath();

        internal SVGGroup CreateLegendLine( string colorOverride = "" )
        {
            var g = new SVGGroup();

            var text = new SVGText() { Text = Label };

            var circle = new SVGCircle()
            {
                R = 7,
                Fill = !string.IsNullOrWhiteSpace( colorOverride ) ? colorOverride : Color,
                Stroke = "none"
            };

            g.Children.Add( circle );
            g.Children.Add( text );

            return g;
        }

        public PieSlice()
        {
            Path.Stroke = "white";
            Path.StrokeWidth = 1;
            Children.Add( Path );
            Children.Add( Text );
        }
    }

    public class PieChart : SVG, IChart
    {
        #region IChart Implementation

        public AdvancedTextStyle TitleTextStyle { get; set; } = new AdvancedTextStyle();

        int IChart.Width
        {
            get
            {
                return (int)base.Width;
            }

            set
            {
                base.Width = value;
            }
        }

        int IChart.Height
        {
            get
            {
                return (int)base.Height;
            }

            set
            {
                base.Height = value;
            }
        }

        public string ChartTitle { get { return SvgTitle.Text; } set { SvgTitle.Text = value; } }

        public List<string> Colors { get; private set; } = new List<string>()
        {
            "rgb(51,102,204)",
            "rgb(220, 57, 18)",
            "rgb(255,153,0)",
            "rgb(16,150,24)",
            "rgb(153,0,153)",
            "rgb(204,204,204)",
            "rgb(22,214,32)",
            "rgb(183,115,34)",
            "rgb(59,62,172)",
            "rgb(85, 116, 166)",
            "rgb(50, 146, 98)",
            "rgb(139, 7, 7)",
            "rgb(230, 115, 0)",
            "rgb(102, 51, 204)",
            "rgb(170, 170, 17)",
            "rgb(34, 170, 153)",
            "rgb(153, 68, 153)",
            "rgb(49, 99, 149)",
            "rgb(184, 46, 46)",
            "rgb(102, 170, 0)",
            "rgb(221, 68, 119)",
            "rgb(0, 153, 198)",
        };

        #endregion

        #region Properties

        #region SliceVisibilityThreshold

        private double _sliceVisibilityThreshold { get; set; } = 0;

        public double SliceVisibilityThreshold
        {
            get { return _sliceVisibilityThreshold; }
            set
            {
                _sliceVisibilityThreshold = Quantative0To1Check( value, _sliceVisibilityThreshold );
            }
        }

        #endregion

        #region PieHole

        private double _pieHole = 0;

        /// <summary>
        /// If between 0 and 1, displays a donut chart. The hole with have a radius equal to number times the radius of the chart.
        /// </summary>
        public double PieHole
        {
            get { return _pieHole; }
            set
            {
                _pieHole = Quantative0To1Check( value, _pieHole );
            }
        }

        #endregion

        #region LegendPosition

        private LegendPosition _legendPosition = LegendPosition.Right;

        public LegendPosition LegendPosition
        {

            get { return _legendPosition; }
            set { _legendPosition = value; }
        }

        #endregion

        public string BackgroundColor { get; set; }

        public IChartArea ChartArea { get; set; }

        public bool Is3D { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; } = 14;

        public string PieResidueSliceLabel { get; set; } = "Other";

        public string PieResidueSliceColor { get; set; } = "#ccc";

        public int PieStartAngle { get; set; }

        public TextStyle PieSliceTextStyle { get; set; }

        #region PieSliceText

        private PieSliceText _pieSliceText = PieSliceText.Percentage;

        public PieSliceText PieSliceText
        {
            get { return _pieSliceText; }
            set { _pieSliceText = value; }
        }

        #endregion

        private readonly List<PieSlice> Slices = new List<PieSlice>();

        #endregion

        #region Public Methods

        public void AddSlice( string label, double value, string color = "", double offset = 0 )
        {
            AddSlice( new PieSlice { Label = label, Color = color, Offset = offset, Value = value } );
        }

        public void AddSlice( PieSlice slice )
        {
            Slices.Add( slice );
            Children.Add( slice );
            CalculateSliceSizes();
        }

        public void SetOffset( string label, double offset )
        {
            foreach (var slice in Slices.Where( t => t.Label.Equals( label ) ))
            {
                slice.Offset = offset;
            }
        }

        #endregion

        #region Constructor

        public PieChart() : this( 900, 500 )
        {
        }

        public PieChart( int width, int height ) : base( height, width )
        {
            StyleList.Add( "overflow: hidden;" );
            //StyleList.Add( "transform", "rotate(-90deg)" );
            Children.Add( TitleGroup );
            Children.Add( Legend );

            SvgTitle = new SVGText();

            TitleGroup.Add( SvgTitle );
        }

        #endregion

        #region SVG Members 

        private readonly SVGGroup TitleGroup = new SVGGroup();
        private readonly SVGText SvgTitle = new SVGText();
        private readonly SVGGroup Legend = new SVGGroup();

        #endregion

        private double CalculateSlicePercentage( PieSlice slice )
        {
            return (slice.Value / Slices.Sum( t => t.Value )) * 100;
        }

        private string GetColor( int index )
        {
            return Colors[index % (Colors.Count - 1)];
        }

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

        private void RenderLegend()
        {
            Legend.Children.Clear();
            if (LegendPosition != LegendPosition.None)
            {
                var i = 0;

                double _cx = 0, _cy = 0;

                switch (LegendPosition)
                {
                    case LegendPosition.Top:
                        _cx = Width / 8;
                        _cy = (.05 * Height) + 1.75 * FontSize;
                        break;
                    case LegendPosition.Bottom:
                        _cx = Width / 8;
                        _cy = (.95 * Height);
                        break;
                    case LegendPosition.Left:
                        _cx = .05 * Width;
                        _cy = .15 * Height;
                        break;
                    case LegendPosition.Right:
                        _cx = (Width / 1.75);
                        _cy = .15 * Height;
                        break;
                }


                foreach (var slice in Slices)
                {
                    var item = slice.CreateLegendLine( string.IsNullOrWhiteSpace( slice.Color ) ? GetColor( i++ ) : slice.Color );
                    var circle = item.Children[0] as SVGCircle;
                    var text = item.Children[1] as SVGText;

                    switch (LegendPosition)
                    {
                        case LegendPosition.Bottom:
                        case LegendPosition.Top:
                            circle.Cx = _cx;
                            text.X = _cx + 2 * circle.R;
                            circle.Cy = _cy;
                            text.Y = _cy + FontSize / 3.0;
                            _cx += (1.75 + text.Text.Text.Length) * FontSize;
                            break;
                        case LegendPosition.Right:
                        case LegendPosition.Left:
                            circle.Cx = _cx;
                            text.X = _cx + 2 * circle.R;
                            circle.Cy = _cy;
                            text.Y = _cy + FontSize / 3.0;
                            _cy += 1.75 * FontSize;
                            break;

                    }

                    Legend.Children.Add( item );
                }

                switch (LegendPosition)
                {
                    case LegendPosition.Right:

                        break;
                }
            }
        }

        private void CalculateSliceSizes()
        {
            var radius = .4 * Math.Min( Width, Height );
            var startAngle = PieStartAngle - 90.0;
            var endAngle = PieStartAngle - 90.0;
            var _cx = Width / 4;
            var _cy = (1.05 * Height) / 2;
            if (LegendPosition == LegendPosition.Left)
            {
                _cx += Slices.Max( t => t.Text.Text.Text.Length ) * FontSize + .15 * Width;
            }
            var margin = .05 * Height;
            var total = Slices.Sum( t => t.Value );
            var i = 0;
            foreach (var slice in Slices)
            {
                startAngle = endAngle;
                endAngle += Math.Ceiling( 360 * slice.Value / total );
                var angleDifference = endAngle - startAngle;

                var x1 = _cx + radius * Math.Cos( Math.PI * startAngle / 180 );
                var y1 = _cy + radius * Math.Sin( Math.PI * startAngle / 180 );

                var x2 = _cx + radius * Math.Cos( Math.PI * endAngle / 180 );
                var y2 = _cy + radius * Math.Sin( Math.PI * endAngle / 180 );

                double x5 = 0, y5 = 0;
                if (angleDifference >= 180)
                {
                    x5 = _cx + radius * Math.Cos( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                    y5 = _cy + radius * Math.Sin( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                }

                var path = slice.Path;
                path.Clear();
                if (PieHole > 0)
                {
                    var r2 = (1 - PieHole) * radius;

                    var x3 = x1 - r2 * Math.Cos( Math.PI * startAngle / 180 );
                    var y3 = y1 - r2 * Math.Sin( Math.PI * startAngle / 180 );

                    var x4 = x2 - r2 * Math.Cos( Math.PI * endAngle / 180 );
                    var y4 = y2 - r2 * Math.Sin( Math.PI * endAngle / 180 );

                    path.MoveTo( x3, y3 );

                    if (angleDifference >= 180)
                    {
                        var x6 = x5 - r2 * Math.Cos( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                        var y6 = y5 - r2 * Math.Sin( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                        path.EllipticalArc( radius - r2, radius - r2, false, false, 1, x6, y6 );
                    }

                    path.EllipticalArc( radius - r2, radius - r2, false, false, 1, x4, y4 );
                    path.LineTo( x2, y2 );

                    if (angleDifference >= 180)
                    {
                        path.EllipticalArc( radius, radius, false, false, 0, x5, y5 );
                    }

                    path.EllipticalArc( radius, radius, false, false, 0, x1, y1 );
                }
                else
                {
                    path.MoveTo( _cx, _cy );
                    path.LineTo( x1, y1 );

                    if (angleDifference >= 180)
                    {
                        path.EllipticalArc( radius, radius, false, false, 1, x5, y5 );
                    }

                    path.EllipticalArc( radius, radius, false, false, 1, x2, y2 );
                }
                path.ClosePath();
                path.Fill = string.IsNullOrWhiteSpace( slice.Color ) ? GetColor( i++ ) : slice.Color;

                var text = slice.Text;
                text.Text.Clear();

                var _tAngle = (endAngle + startAngle) / 2;
                text.X = _cx + radius * Math.Cos( Math.PI * _tAngle / 180 );
                text.Y = _cy + radius * Math.Sin( Math.PI * _tAngle / 180 );
                switch (PieSliceText)
                {
                    case PieSliceText.Label:
                        text.Text.Append( slice.Label );
                        break;
                    case PieSliceText.Percentage:
                        text.Text.Append( $"{CalculateSlicePercentage( slice ).ToString( "0.0" )}%" );
                        break;
                    case PieSliceText.Value:
                        text.Text.Append( $"{slice.Value.ToString( "0.0" )}" );
                        break;
                }
            }
        }

        private Tuple<double, double> CalculateTextPostion( string text, double x, double y, double angle )
        {
            var rAngle = angle * Math.PI / 180;
            var _x = ((text.Length * 2) * FontSize) * Math.Cos( rAngle );
            var _y = ((text.Length * 2) * FontSize) * Math.Sin( rAngle );
            if (rAngle >= 0 && rAngle <= Math.PI)
            { // quadrant 1

            }
            else if (rAngle > Math.PI && rAngle <= 2 * Math.PI)
            { // quadrant 2

            }
            else if (rAngle > 2 * Math.PI && rAngle <= 3 * Math.PI / 2)
            { // quadrant 3

            }
            else
            { // quadrant 4
                //_x *= -1;
                //_y *= -1;
            }
            return new Tuple<double, double>( x + _x, y + _y );
        }

        private static double Quantative0To1Check( double newValue, double oldValue )
        {
            return (newValue >= 0 && newValue <= 1) ? newValue : oldValue;
        }

        public override string Render()
        {
            CalculateSliceSizes();
            RenderTitle();
            RenderLegend();
            return base.Render();
        }
    }
}
