using HydraDoc.Chart.Axis;
using HydraDoc.Chart.Axis.Algorithms;
using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public abstract class AxisChart<T1, T2> : SVG, IChart where T1 : IComparable<T1>
                                                          where T2 : IComparable<T2>
    {
        #region Axis

        public IAxis<T1> LabeledAxis { get; protected set; } = new Axis<T1>();

        public ITickAlgorithm<T1> LabeledAxisTickAlgorithm { get; set; }

        public IAxis<T2> MeasuredAxis { get; protected set; } = new Axis<T2>();

        public ITickAlgorithm<T2> MeasuredAxisTickAlgorithm { get; set; }

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

        #region SVGMembers 

        protected readonly SVGGroup TitleGroup = new SVGGroup();
        protected readonly SVGText SvgTitle = new SVGText();
        protected readonly SVGGroup Legend = new SVGGroup();
        protected readonly SVGGroup HorizontalAxisGroup = new SVGGroup();
        protected readonly SVGGroup VerticalAxisGroup = new SVGGroup();
        protected readonly SVGGroup ChartGroup = new SVGGroup();

        #endregion

        protected AxisChart() : this( 900, 500 )
        {
        }

        protected AxisChart( int width, int height ) : base( height, width )
        {
            StyleList.Add( "overflow: visible;" );
            Children.Add( TitleGroup );

            SvgTitle = new SVGText();
            TitleTextStyle = new TextStyle( SvgTitle );
            ChartTextStyle = new TextStyle( this );
            
            TitleGroup.Add( SvgTitle );
            Children.Add( VerticalAxisGroup );
            Children.Add( HorizontalAxisGroup );
            Children.Add( ChartGroup );

            ChartTextStyle.FontSize = 15;

            var txtDummy = new SVGText();
            ChartTextStyle.ApplyStyle( txtDummy );
            MeasuredAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            MeasuredAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );

            LabeledAxisTickAlgorithm = ChooseDefaultTickAlgorithm<T1>( typeof( T1 ) );
            MeasuredAxisTickAlgorithm = ChooseDefaultTickAlgorithm<T2>( typeof( T2 ) );
        }

        protected double GetTitleHeight()
        {
            return !string.IsNullOrWhiteSpace( ChartTitle ) ? TitleTextStyle.FontSize * 4 : 0;
        }

        protected void RenderChart()
        {
            ChartGroup.Children.Clear();
            HorizontalAxisGroup.Children.Clear();
            VerticalAxisGroup.Children.Clear();

            if (!string.IsNullOrWhiteSpace( ChartTitle ))
            {
                SvgTitle.X = Width / 2;
                SvgTitle.Y = 1.5 * TitleTextStyle.FontSize;
                SvgTitle.StyleList.Add( "text-anchor", "middle" );
            }

            RenderChartImpl();
        }

        protected abstract void RenderChartImpl();

        private ITickAlgorithm<T> ChooseDefaultTickAlgorithm<T>( Type type ) where T : IComparable<T>
        {
            if (type == typeof( double ) ||
                type == typeof( int ) ||
                type == typeof( decimal ) ||
                type == typeof( float ) ||
                type == typeof( short ))
            {
                return new SimpleNumericTickAlgorithm() as ITickAlgorithm<T>;
            }
            else if (type == typeof( string ))
            {
                return new StringTickAlgorithm() as ITickAlgorithm<T>;
            }

            return null;
        }

        public override string Render()
        {
            RenderChart();
            return base.Render();
        }
    }
}
