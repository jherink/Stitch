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
    
    public class BarChart : SVG, IChart
    {
        private Dictionary<string, double> Values = new Dictionary<string, double>();

        #region AxisOrientation 

        private Orientation _axisOrientation;

        public Orientation AxisOrientation
        {
            get { return _axisOrientation; }
            set
            {
                _axisOrientation = value;
                LabeledAxis.Orientation = value;
                switch (value) {
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

        #region IChart Implementation

        public AdvancedTextStyle TitleTextStyle { get; set; } = new AdvancedTextStyle();

        public string ChartTitle { get; set; }

        public List<string> Colors { get; private set; }

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
        }

        #endregion

        public void AddData( string label, double value )
        {
            Values.Add( label, value );
        }

        public override string Render()
        {
            LabeledAxis.AxisLength = LabeledAxis.Orientation == Orientation.Vertical ? base.Height : base.Width; 
            MeasuredAxis.AxisLength = MeasuredAxis.Orientation == Orientation.Vertical ? base.Height : base.Width;
            var ticks = MeasuredAxis.GenerateAxisData( Values.Values.Min(), Values.Values.Max() );
            MeasuredAxis.GenerateAxisData( ticks );
            LabeledAxis.GenerateAxisData( Values.Keys );
            return base.Render();
        }
    }
}
