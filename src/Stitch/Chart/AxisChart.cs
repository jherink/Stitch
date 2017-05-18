using Stitch.Chart.Axis;
using Stitch.Chart.Axis.Algorithms;
using Stitch.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stitch.Elements.Interface;

namespace Stitch.Chart
{
    public abstract class AxisChart<T1, T2> : ChartBase where T1 : IComparable<T1>
                                                        where T2 : IComparable<T2>
    {
        #region Axis

        public IAxis<T1> LabeledAxis { get; protected set; } = new Axis<T1>();

        public ITickAlgorithm<T1> LabeledAxisTickAlgorithm { get; set; }

        public IAxis<T2> MeasuredAxis { get; protected set; } = new Axis<T2>();

        public ITickAlgorithm<T2> MeasuredAxisTickAlgorithm { get; set; }

        #endregion

        #region SVGMembers 

        protected readonly SVGGroup HorizontalAxisGroup = new SVGGroup();
        protected readonly SVGGroup VerticalAxisGroup = new SVGGroup();

        #endregion

        protected AxisChart() : this( 900, 500 )
        {
        }

        protected AxisChart( int width, int height ) : base( height, width )
        {
            Children.Add( VerticalAxisGroup );
            Children.Add( HorizontalAxisGroup );

            var txtDummy = new SVGText();
            ChartTextStyle.ApplyStyle( txtDummy );
            MeasuredAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            MeasuredAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );
            LabeledAxis.AxisTitleTextStyle = new TextStyle( txtDummy.Clone() as SVGText );

            LabeledAxisTickAlgorithm = ChooseDefaultTickAlgorithm<T1>( typeof( T1 ) );
            MeasuredAxisTickAlgorithm = ChooseDefaultTickAlgorithm<T2>( typeof( T2 ) );
        }

        protected abstract void RenderAxisChartImpl();

        public override void RenderChart()
        {
            ChartGroup.Children.Clear();
            HorizontalAxisGroup.Children.Clear();
            VerticalAxisGroup.Children.Clear();

            RenderAxisChartImpl();
            RenderLegend();
        }

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

        public override IEnumerable<IElement> GetAllNodes()
        {
            RenderChart();
            return base.GetAllNodes();
        }

        public override IEnumerable<IElement> GetNodes( string tagFilter )
        {
            RenderChart();
            return base.GetNodes( tagFilter );
        }

        protected double GetChartableAreaWidth()
        {
            return .9 * Width - Math.Max( GetLegendLeftOffset(), GetLegendRightOffset() );
        }                
    }
}
