using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
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
            //Text.Fill = "white";
            Path.StrokeWidth = 1;
            Text.StyleList.Add( "text-anchor", "middle" );
            Children.Add( Path );
            Children.Add( Text );
        }
    }

    public class PieChart : SVG, IChart
    {
        #region IChart Implementation

        public ITextStyle TitleTextStyle { get; set; }

        public ITextStyle ChartTextStyle { get; set; }

        public string ChartTitle { get { return SvgTitle.Text; } set { SvgTitle.Text = value; } }

        public List<string> Colors { get; private set; } = Helpers.GetDefaultColors();

        #endregion

        #region Properties

        //#region SliceVisibilityThreshold

        //private double _sliceVisibilityThreshold { get; set; } = 0;

        //public double SliceVisibilityThreshold
        //{
        //    get { return _sliceVisibilityThreshold; }
        //    set
        //    {
        //        _sliceVisibilityThreshold = Quantative0To1Check( value, _sliceVisibilityThreshold );
        //    }
        //}

        //#endregion

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

        //public bool Is3D { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; } = 14;

        //public string PieResidueSliceLabel { get; set; } = "Other";

        //public string PieResidueSliceColor { get; set; } = "#ccc";

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
            Children.Add( TitleGroup );
            Children.Add( Legend );

            SvgTitle = new SVGText();
            TitleTextStyle = new TextStyle( SvgTitle );
            ChartTextStyle = new TextStyle( this );
            ChartTextStyle.FontSize = 15;

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
                    var item = slice.CreateLegendLine( string.IsNullOrWhiteSpace( slice.Color ) ? Helpers.GetColor( Colors, i++ ) : slice.Color );
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
            /** NOTE: "left-most" & "right-most" refer to if you were looking at the slice
             * facing the tip.
             */

            var radius = .4 * Math.Min( Width, Height );
            var startAngle = PieStartAngle - 90.0;
            var endAngle = PieStartAngle - 90.0;
            var cx = 1.5 * radius;
            var cy = (1.05 * Height) / 2;
            if (LegendPosition == LegendPosition.Left)
            { // If the legend is on the left then we need to move the graph right so it will fit.
                cx += Slices.Max( t => t.Text.Text.Text.Length ) * FontSize + .15 * Width;
            }
            var margin = .05 * Height;
            var total = Slices.Sum( t => t.Value );
            var i = 0;
            foreach (var slice in Slices)
            {
                startAngle = endAngle; // start angle becomes the old end angle.
                endAngle += Math.Ceiling( 360 * slice.Value / total ); // calculate new end angle for slice.
                var angleDifference = endAngle - startAngle;

                var _cx = cx;
                var _cy = cy;
                if (slice.Offset > 0)
                {   // if offsetting slice then we need to calculate the angle between
                    // the start and end angle and calculate a new origin for the slice's tip.
                    var midAngle = startAngle + ((endAngle - startAngle) / 2);
                    _cx += slice.Offset * radius * Math.Cos( Math.PI * midAngle / 180 );
                    _cy += slice.Offset * radius * Math.Sin( Math.PI * midAngle / 180 );
                }

                // Calculate the left-most point of the slice.
                var x1 = _cx + radius * Math.Cos( Math.PI * startAngle / 180 );
                var y1 = _cy + radius * Math.Sin( Math.PI * startAngle / 180 );

                // Calculate the right-most point of the slice.
                var x2 = _cx + radius * Math.Cos( Math.PI * endAngle / 180 );
                var y2 = _cy + radius * Math.Sin( Math.PI * endAngle / 180 );

                double x5 = 0, y5 = 0;
                if (angleDifference >= 180)
                {   // If the angle difference is greater than or equal to 50% of the graph then 
                    // it will render incorrectly.  Add two points at the midpoint of the circle so it
                    // will render correctly.
                    x5 = _cx + radius * Math.Cos( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                    y5 = _cy + radius * Math.Sin( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                }

                var path = slice.Path;
                path.Clear(); // Clear old path renderings.
                if (PieHole > 0)
                {
                    var r2 = (1 - PieHole) * radius; // r2 is the new radius with the pie hole factored in.

                    // Calculate a point for the left-most inner pie hole point.
                    var x3 = x1 - r2 * Math.Cos( Math.PI * startAngle / 180 );
                    var y3 = y1 - r2 * Math.Sin( Math.PI * startAngle / 180 );

                    // Calculate a point for the right-most inner pie hole point.
                    var x4 = x2 - r2 * Math.Cos( Math.PI * endAngle / 180 );
                    var y4 = y2 - r2 * Math.Sin( Math.PI * endAngle / 180 );

                    // move to the left-most inner pie hole point instead of the radius.
                    path.MoveTo( x3, y3 );

                    if (angleDifference >= 180)
                    {   // See note above about case where slice is 50% or more of graph.
                        var x6 = x5 - r2 * Math.Cos( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                        var y6 = y5 - r2 * Math.Sin( Math.PI * (startAngle + (angleDifference / 2)) / 180 );
                        path.EllipticalArc( radius - r2, radius - r2, false, false, 1, x6, y6 );
                    }

                    // Close the inner pie hole arc by moving to the right-most point.
                    path.EllipticalArc( radius - r2, radius - r2, false, false, 1, x4, y4 );
                    path.LineTo( x2, y2 ); // move to right-most slice point.

                    if (angleDifference >= 180)
                    { // See note above about case where slice is 50% or more of graph.
                        path.EllipticalArc( radius, radius, false, false, 0, x5, y5 );
                    }

                    // Move to left-most slice point.
                    path.EllipticalArc( radius, radius, false, false, 0, x1, y1 );
                }
                else
                { // No pie hole.
                    path.MoveTo( _cx, _cy ); // move to graph origin.
                    path.LineTo( x1, y1 ); // move to left-most slice point.

                    if (angleDifference >= 180)
                    { // See note above about case where slice is 50% or more of graph.
                        path.EllipticalArc( radius, radius, false, false, 1, x5, y5 );
                    }

                    // Arc to right-most slice point.
                    path.EllipticalArc( radius, radius, false, false, 1, x2, y2 );
                }
                path.ClosePath(); // Complete the slice.
                path.Fill = string.IsNullOrWhiteSpace( slice.Color ) ? Helpers.GetColor( Colors, i++ ) : slice.Color;

                var text = slice.Text;
                //text.Fill = "#fff";
                ChartTextStyle.ApplyStyle( text );
                text.Text.Clear();

                // TODO: Calculate more precise point to draw the text and an orientation
                // of the text so that it fits in the slice better.
                var _tAngle = (endAngle + startAngle) / 2;
                var pct = CalculateSlicePercentage( slice );
                text.X = _cx + radius * Math.Cos( Math.PI * _tAngle / 180 );
                text.Y = _cy + radius * Math.Sin( Math.PI * _tAngle / 180 );
                switch (PieSliceText)
                {
                    case PieSliceText.Label:
                        text.Text.Append( slice.Label );
                        break;
                    case PieSliceText.Percentage:
                        text.Text.Append( $"{pct.ToString( "0.0" )}%" );
                        break;
                    case PieSliceText.Value:
                        text.Text.Append( $"{slice.Value.ToString( "0.0" )}" );
                        break;
                }

                var newCoords = CalculateSliceTextCoordinates( text.GetContent(), ChartTextStyle.FontSize, pct, _tAngle, _cx, _cy, x1, y1, x2, y2, text.X, text.Y );
                text.X = newCoords.Item1;
                text.Y= newCoords.Item2;
                if (!newCoords.Item3) text.StyleList.Add( "display", "none" );
                else text.StyleList.Remove( "display" );
                //text.Fill = newCoords.Item3;
            }
        }

        private void CalculateSliceTextCoordinates( string content, double cx, double cy, double x1, double y1, double x2, double y2 )
        {
            var deltaCX1 = Math.Abs( cx - x1 );
            var deltaCY1 = Math.Abs( cy - y1 );
            var deltaCX2 = Math.Abs( cx - x2 );
            var deltaCY2  
        }

        private Tuple<double, double, bool> CalculateSliceTextCoordinates( string content, double fontSize, double slicePct, double angle, double cx, double cy, double x1, double y1, double x2, double y2, double x, double y ) {
            var width = content.Length * fontSize;
            var height = 2 * fontSize;
            var show = true;
            var xSpan = Math.Abs( cx - x1 );
            var ySpan = Math.Abs( cy - y1 );
            var xSpanW = Math.Abs( x2 - x1 );
            var ySpanH = Math.Abs( y2 - y1 );
            
            var deltaX = (width / 2) * Math.Cos( angle * (Math.PI / 180) );
            var deltaY = height * Math.Sin( angle * (Math.PI / 180) );

            var newX = x - deltaX;
            var newY = y - deltaY;
            //if (slicePct < 10 && content.Length > 5)
            //{
            //    newX = x + deltaX;
            //    newY = y + deltaY;
            //}
            return new Tuple<double, double, bool>( newX, newY, show );
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
