using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Graph
{
    public class BarGraph : Table, IGraph
    {
        private const double GRAPH_HEIGHT_CAP = .95;
        private const double GRAPH_AXIS_CAP = 1.05;
        private const string GRAPH_VAL_KEY = "graph-value";
        private const double FONT_SIZE_FACTOR = 1;
        private const double FONT_CAPTION_FACTOR = 5;
        public virtual string GraphTitle
        {
            get
            {
                return TableCaption.Caption;
            }
            set
            {
                TableCaption.Caption = value;
            }
        }

        public int FontSize
        {
            get
            {
                return int.Parse( StyleList["font-size"].Replace( "px", string.Empty ) );
            }
            set
            {
                StyleList["font-size"] = $"{value}px";
            }
        }

        public bool PlotXAxis { get; set; } = true;

        public bool PlotYAxis { get; set; } = true;

        public string YAxisFormat { get; set; } = string.Empty;

        public int AxisIntervals { get; set; } = 5;

        private int _barSize { get; set; } = 60;

        public int BarSize
        {
            get
            {
                return _barSize;
            }
            set
            {
                _barSize = value;
                ReCalculateRowSizes();
            }
        }

        public bool PlotBarValues { get; set; } = false;

        public int BarPad { get; set; } = 13;

        public int RowWidth { get { return GraphWidth / Rows.Count(); } }

        /// <summary>
        /// The highest value that can be plotted on the measurable axis.
        /// </summary>
        public double AxisLimit { get; set; } = 100;

        /// <summary>
        /// The width in pixels of the graph.
        /// </summary>
        public int GraphWidth
        {
            get
            {
                return int.Parse( StyleList["width"].Replace( "px", string.Empty ) );
            }
            set
            {
                StyleList["width"] = $"{value}px";
                ReCalculateRowSizes();
            }
        }

        /// <summary>
        /// The width in pixels of the graph.
        /// </summary>
        public int GraphHeight
        {
            get
            {
                return int.Parse( StyleList["height"].Replace( "px", string.Empty ) );
            }
            set
            {
                StyleList["height"] = $"{value}px";
                ReCalculateRowSizes();
            }
        }

        public readonly List<string> BarColors = new List<string>();

        public BarGraph()
        {
            DefineCSS();
        }

        public void AddData( string key, params double[] values )
        {
            AddData( key, string.Empty, values );
        }

        public void AddData( string key, string format, params double[] values )
        {
            var row = CreateRow();
            row.ClassList.Add( "bar-graph-tr" );

            var th = new HeaderCell() { Content = key };
            th.ClassList.Add( "bar-graph-th" );
            th.StyleList.Add( "width", $"{RowWidth}px" );
            row.Children.Add( th );
            row.StyleList.Add( "height", $"{GRAPH_HEIGHT_CAP * GraphHeight}px" );
            row.StyleList.Add( "left", $"{RowWidth}px" );

            var i = 0;
            foreach (var value in values)
            {
                var td = new TableCell();
                AxisLimit = Math.Max( AxisLimit, GRAPH_AXIS_CAP * value );
                td.ClassList.Add( "bar-graph-td", "bar-graph-bar" );
                td.StyleList.Add( "width", $"{BarSize}px" );
                td.StyleList.Add( "height", $"{CalculateBarHeight( value )}px" );
                td.StyleList.Add( "left", $"{BarPad + BarSize * i}px" );
                if (BarColors.Any())
                {
                    td.StyleList.Add( "background-color", BarColors[i % BarColors.Count] );
                }
                var pContent = new Paragraph( value.ToString( format ) );
                pContent.Attributes.Add( GRAPH_VAL_KEY, value.ToString() );

                if (!PlotBarValues)
                {
                    pContent.StyleList.Add( "visibility", "hidden" );
                }

                td.Content.AppendElement( pContent );
                row.Children.Add( td );
                i++;
            }
        }

        protected virtual int CalculateBarHeight( double value )
        {
            return (int)(value * GraphHeight / AxisLimit);
        }

        protected virtual int CalculateGraphLeftMargin()
        {
            return (int)(RoundAxisValue( AxisLimit ).ToString().Length * FontSize * FONT_SIZE_FACTOR);
        }

        protected virtual int CalculateGraphTopMargin()
        {
            return (int)(FontSize * FONT_CAPTION_FACTOR);
        }

        protected virtual void DefineCSS()
        {
            StyleList.Add( "width", $"{700}px" );
            StyleList.Add( "height", $"{300}px" );
            StyleList.Add( "font-size", $"{11}px" );
            StyleList.Add( "margin-left", "0" );
            StyleList.Add( "margin-top", "0" );
            ClassList.Add( "bar-graph" );
            TableCaption.ClassList.Add( "bar-graph-caption" );
            TableCaption.StyleList.Add( "width", $"{GraphWidth}px" );
        }

        private void ReCalculateRowSizes()
        {
            foreach (var tb in TableBodies)
            {
                var i = 0;
                foreach (var row in tb.Rows)
                {
                    row.StyleList["height"] = $"{GRAPH_HEIGHT_CAP * GraphHeight}px";
                    row.StyleList["left"] = $"{RowWidth * i++}px";
                    var j = 0;
                    var k = 1;
                    foreach (var td in row.Cells)
                    {
                        var cellContent = (td.Content.Elements.Last() as IParagraphElement);
                        if (cellContent != null)
                        {
                            //var value = double.Parse( cellContent.Content );
                            var value = double.Parse( cellContent.Attributes[GRAPH_VAL_KEY] );
                            td.StyleList["width"] = $"{BarSize}px";
                            td.StyleList["height"] = $"{CalculateBarHeight( value )}px";
                            td.StyleList.Add( "left", $"{BarPad + ((BarPad / 2) * k++) + BarSize * j}px" );
                            j++;
                        }
                        else
                        { // th 
                            td.StyleList["width"] = $"{RowWidth}px";
                        }
                    }
                }
            }
            StyleList["margin-top"] = $"{CalculateGraphTopMargin()}px";
            if (PlotYAxis)
            {
                StyleList["margin-left"] = $"{CalculateGraphLeftMargin()}px";
            }
        }

        public IDivElement CreateTicks()
        {
            var div = new Div();
            div.ClassList.Add( "tick-graph" );
            div.StyleList.Add( "width", $"{GraphWidth}px" );
            div.StyleList.Add( "font-size", $"{FontSize}px" );
            div.StyleList.Add( "top", $"{(-GraphHeight)}px" );
            div.StyleList.Add( "margin-bottom", $"{(-GraphHeight)}px" );
            div.StyleList.Add( "margin-left", $"{CalculateGraphLeftMargin()}px" );
            var height = GraphHeight / AxisIntervals;
            var template = new Div();
            template.StyleList.Add( "height", $"{height}px" );
            template.ClassList.Add( "tick" );
            for (int i = 0; i < AxisIntervals; i++)
            {
                var clone = template.Clone() as Div;
                var tickLimit = (AxisIntervals - i) * (AxisLimit / AxisIntervals);

                clone.Children.Add( new Paragraph( $"{RoundAxisValue( tickLimit ).ToString( YAxisFormat )}" ) );
                div.Children.Add( clone );
            }
            return div;
        }

        private double RoundAxisValue( double axisValue )
        {
            var aAxisValue = Math.Abs( axisValue );
            if (aAxisValue <= 1)
            {
                return axisValue;
            }

            if (aAxisValue <= 10)
            {
                return (int)axisValue;
            }

            var power = Math.Ceiling( Math.Log10( axisValue / 10 ) );
            var p2 = axisValue;
            while (p2 > 10) p2 /= 10;
            var number = (int)((p2 + 1)) * Math.Pow( 10, power );
            if (number < aAxisValue)
            {
                number *= 2;
            }

            if (axisValue < 0) number *= -1;

            return number;
        }

        public override string Render()
        {
            ReCalculateRowSizes();
            return base.Render();
        }
    }
}
