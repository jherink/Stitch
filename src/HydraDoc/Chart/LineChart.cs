using HydraDoc.Chart.Axis;
using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public class Line
    {
        public string Label { get; set; }
        public string Color { get; set; }
        public List<double> Values { get; set; }        
    }
    public class LineChart : SVG, IChart
    {
        private readonly List<Line> Lines = new List<Line>();

        #region Properties

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

        #region SVG Members

        private readonly SVGGroup TitleGroup = new SVGGroup();
        private readonly SVGText SvgTitle = new SVGText();
        private readonly SVGGroup Legend = new SVGGroup();
        private readonly SVGGroup LineGroup = new SVGGroup();
        private readonly SVGGroup HorizontalAxisGroup = new SVGGroup();
        private readonly SVGGroup VerticalAxisGroup = new SVGGroup();

        #endregion

        #region Constructors

        public LineChart() : this( 900, 500 )
        {

        }

        public LineChart( int width, int height ) : base( height, width )
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
            Children.Add( LineGroup );
            
            ChartTextStyle.FontSize = 15;
            var txtDummy = new SVGText();
            ChartTextStyle.ApplyStyle( txtDummy );
            MeasuredAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            MeasuredAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
        }

        #endregion
        

        public void AddLine( Line line )
        {
            Lines.Add( line );
        }

        private double GetTitleHeight()
        {
            return !string.IsNullOrWhiteSpace( ChartTitle ) ? TitleTextStyle.FontSize * 4 : 0;
        }

        private void RenderChart()
        {
            LineGroup.Children.Clear();
            HorizontalAxisGroup.Children.Clear();
            VerticalAxisGroup.Children.Clear();

        }

        public override string Render()
        {
            RenderChart();
            return base.Render();
        }
    }
}
