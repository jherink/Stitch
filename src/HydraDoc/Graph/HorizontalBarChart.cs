using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HydraDoc.Elements;
using HydraDoc.Elements.Interface;

namespace HydraDoc.Graph
{
    public class HorizontalBarChart : DescriptionListElement
    {
        private IDescriptionTermElement _titleElement;

        public string GraphTitle
        {
            get
            {
                return _titleElement.Content;
            }
            set
            {
                _titleElement.Content = value;
            }
        }

        public bool PlotXAxis { get; set; } = true;

        public bool PlotYAxis { get; set; } = true;

        /// <summary>
        /// The highest value that can be plotted on the measurable axis.
        /// </summary>
        public double AxisLimit { get; set; } = 100;

        /// <summary>
        /// The width in pixels of the graph.
        /// </summary>
        public int GraphWidth { get; set; } = 700;
        
        //public StyleList BarStyleTemplate = new StyleList();

        public HorizontalBarChart()
        {
            _titleElement = new DescriptionTermElement();

            InitializeCSS();
        }

        private void InitializeCSS()
        {
            // style graph element.
            //StyleList.Add( "display", "flex" );
            //StyleList.Add( "flex-direction", "column" );
            //StyleList.Add( "width", "100%" );
            //StyleList.Add( "position", "relative" );
            //StyleList.Add( "padding", "20px" );
            ClassList.Add( "horizontal-bar-graph" );

            // style graph title.
            //_titleElement.StyleList.Add( "align-self", "flex-start" );
            //_titleElement.StyleList.Add( "width", "100%" );
            //_titleElement.StyleList.Add( "font-weight", "700" );
            //_titleElement.StyleList.Add( "display", "block" );
            //_titleElement.StyleList.Add( "text-align", "center" );
            //_titleElement.StyleList.Add( "font-size", "1.2em" );
            //_titleElement.StyleList.Add( "margin-bottom", "20px" );
            _titleElement.ClassList.Add( "graph-title" );

            // style the bars
            //BarStyleTemplate.Add( "font-size", ".8em" );
            //BarStyleTemplate.Add( "line-height", "1" );
            //BarStyleTemplate.Add( "text-transform", "uppercase" );
            //BarStyleTemplate.Add( "width", "100%" );
            //BarStyleTemplate.Add( "height", "40px" );
            //BarStyleTemplate.Add( "margin-left", "130px" );
            //BarStyleTemplate.Add( "background", "repeating-linear-gradient(to right,#ddd,#ddd 1px,#fff 1px,#fff 5%)" );

            Children.Add( _titleElement );
        }

        private double CalculateStyleWidth( double value )
        {
            return ((value * AxisLimit) / 100.0) * GraphWidth / 100.0;
        }


        public DescriptionDescribeElement AddData( string yAxisData, double xAxisData )
        {
            var dd = new DescriptionDescribeElement();
            dd.ClassList.Add( "percentage");
            dd.StyleList.Add( "width", CalculateStyleWidth( xAxisData ).ToString() + "px" );
            var span = new Span( new PlainText( yAxisData ) );
            span.ClassList.Add( "graph-text" );
            dd.Content.AppendElement( span );           
            Children.Add( dd );
            return dd;
        }
    }
}
