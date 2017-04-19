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

        public PieSlice()
        {
            Children.Add( Path );
            Children.Add( Text );
        }
    }

    public class PieChart : SVG, IChart
    {
        #region IChart Implementation

        public AdvancedTextStyle TitleTextStyle { get; set; }

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

        public string ChartTitle { get { return _chartTitle.Text.Text; } set { _chartTitle.Text.Text = value; } }

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
        public double PieHole { get { return _pieHole; } set { _pieHole = Quantative0To1Check( value, _pieHole ); } }

        #endregion

        public string BackgroundColor { get; set; }

        public IChartArea ChartArea { get; set; }

        public bool Is3D { get; set; }

        public ILegend Legend { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; }

        public string PieResidueSliceLabel { get; set; } = "Other";

        public string PieResidueSliceColor { get; set; } = "#ccc";

        public int PieStartAngle { get; set; }

        public TextStyle PieSliceTextStyle { get; set; }

        public PieSliceText PieSliceText { get; set; } = PieSliceText.Percentage;

        private readonly List<PieSlice> Slices = new List<PieSlice>();

        #endregion

        #region Public Methods

        public void AddSlice( string label, double value, string color = "", double offset = 0 )
        {
            AddSlice( new PieSlice { Label = label, Color = color, Offset = offset, Value = value });
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

        public PieChart() : this(600,600)
        {           
        }

        public PieChart( int width, int height ) : base( height, width ) {
            StyleList.Add( "overflow: hidden;" );
            StyleList.Add( "transform", "rotate(-90deg)" );
            Children.Add( _chartTitle );
        }

        #endregion

        #region SVG Members 

        private readonly ChartTitle _chartTitle = new ChartTitle();

        #endregion

        private string GetColor(int index) {
            return Colors[index % (Colors.Count - 1)];
        }

        private void CalculateSliceSizes()
        {
            var startAngle = 0.0;
            var endAngle = 0.0;
            var _cx = Width / 2;
            var _cy = Height / 2;
            var total = Slices.Sum( t => t.Value );
            var i = 0;
            foreach (var slice in Slices)
            {
                startAngle = endAngle;
                endAngle += Math.Ceiling( 360 * slice.Value / total );
                var x1 = _cx + 180 * Math.Cos( Math.PI * startAngle / 180 );
                var y1 = _cy + 180 * Math.Sin( Math.PI * startAngle / 180 );

                var x2 = _cx + 180 * Math.Cos( Math.PI * endAngle / 180 );
                var y2 = _cy + 180 * Math.Sin( Math.PI * endAngle / 180 );

                var path = slice.Path;
                path.Clear();
                path.MoveTo( _cx, _cy );
                path.LineTo( x1, y1 );
                path.EllipticalArc( 180, 180, false, false, 1, x2, y2 );
                path.ClosePath();
                path.Fill = string.IsNullOrWhiteSpace( slice.Color ) ? GetColor( i++ ) : slice.Color;
            }
        }

        private static double Quantative0To1Check( double newValue, double oldValue )
        {
            return (newValue >= 0 && newValue <= 1) ? newValue : oldValue;
        }
    }
}
