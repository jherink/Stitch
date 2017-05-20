using Stitch.Chart.Axis;
using Stitch.Elements;
using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    public abstract class ChartBase : SVG, IChart
    {
        #region IChart Implementation

        public LegendPosition LegendPosition { get; set; } = LegendPosition.None;

        public ITextStyle TitleTextStyle { get; set; }

        public string ChartTitle { get { return SvgTitle.Text; } set { SvgTitle.Text = value; } }

        public ITextStyle ChartTextStyle { get; set; }

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

        public void RenderLegend()
        {
            Legend.Children.Clear();
            if (LegendPosition != LegendPosition.None)
            {
                var i = 1;

                double _cx = ChartTextStyle.FontSize / 2, _cy = 0;
                var minH = GraphicsHelper.MeasureStringHeight( "W", ChartTextStyle );

                switch (LegendPosition)
                {
                    case LegendPosition.Top:
                        _cy = GetTitleHeight();
                        break;
                    case LegendPosition.Bottom:
                        _cy = Height - minH;
                        break;
                    case LegendPosition.Left:
                        _cy = GetTitleHeight();
                        break;
                    case LegendPosition.Right:
                        _cx = Width - GetLegendRightOffset();
                        _cy = Math.Max(GetTitleHeight(), minH );
                        break;
                }

                var j = 1;
                foreach (var pair in GetLegendItems())
                {
                    if (string.IsNullOrWhiteSpace( pair.Item1 )) continue;
                    var item = CreateLegendLine( pair.Item1, pair.Item2, j++ );
                    var circle = item.Children.ToList()[0] as SVGCircle;
                    var text = item.Children.ToList()[1] as SVGText;

                    switch (LegendPosition)
                    {
                        case LegendPosition.Bottom:
                        case LegendPosition.Top:
                            circle.Cx = _cx;
                            text.X = _cx + 2 * circle.R;
                            circle.Cy = _cy;
                            text.Y = _cy + ChartTextStyle.FontSize / 3.0;
                            var qString = text.Text.Text.Length < AxisHelper.MaxHorizontalAxisLength ? new string( 'a', AxisHelper.MaxHorizontalAxisLength ) : text.Text.Text;
                            _cx += circle.R + GraphicsHelper.MeasureStringWidth( qString, ChartTextStyle );
                            break;
                        case LegendPosition.Right:
                        case LegendPosition.Left:
                            circle.Cx = _cx;
                            text.X = _cx + 2 * circle.R;
                            circle.Cy = _cy;
                            text.Y = _cy + ChartTextStyle.FontSize / 3.0;
                            _cy += GraphicsHelper.MeasureStringHeight( text.Text.Text, ChartTextStyle );
                            break;

                    }

                    Legend.Children.Add( item );
                }
            }
        }

        public abstract void RenderChart();
        public virtual double GetLegendBottomOffset()
        {
            return LegendPosition == LegendPosition.Bottom ? ChartTextStyle.FontSize : 0;
        }
        public virtual double GetLegendTopOffset()
        {
            return LegendPosition == LegendPosition.Top ? 2 * GraphicsHelper.MeasureStringHeight( ChartTitle, TitleTextStyle ) : 0;
        }
        public abstract double GetLegendLeftOffset();
        public abstract double GetLegendRightOffset();

        #endregion

        #region SVGMembers 

        protected readonly SVGGroup TitleGroup = new SVGGroup();
        protected readonly SVGText SvgTitle = new SVGText();
        protected readonly SVGGroup Legend = new SVGGroup();
        protected readonly SVGGroup ChartGroup = new SVGGroup();

        #endregion

        protected ChartBase( int height, int width ) : base( height, width )
        {
            StyleList.Add( "overflow: visible;" );
            //StyleList.Add( "width", "100%" );
            Children.Add( TitleGroup );

            SvgTitle = new SVGText();
            TitleTextStyle = new TextStyle( SvgTitle ) { Bold = true };
            ChartTextStyle = new TextStyle( this );

            TitleGroup.Add( SvgTitle );
            Children.Add( Legend );
            Children.Add( ChartGroup );

            ChartTextStyle.FontSize = 15;
            ChartTextStyle.ApplyStyle( new SVGText() );
            StyleList.Add( "margin-top", "30px" );
            StyleList.Add( "margin-bottom", "30px" );
        }

        protected double GetTitleHeight()
        {
            //return !string.IsNullOrWhiteSpace( ChartTitle ) ? TitleTextStyle.FontSize * 4 : 0;
            return !string.IsNullOrWhiteSpace( ChartTitle ) ? 2 * GraphicsHelper.MeasureStringHeight( ChartTitle, TitleTextStyle ) : 0;
        }

        protected string GetChartTheme( int id )
        {
            return $"stitch-chart-theme-{(id % 23)}";
        }

        protected string GetChartStrokeTheme( int id )
        {
            return $"stitch-chart-stroke-theme-{(id % 23)}";
        }

        protected string GetChartTextTheme( int id )
        {
            return $"stitch-chart-text-theme-{(id % 23)}";
        }

        protected void RenderChartTitle()
        {
            if (!string.IsNullOrWhiteSpace( ChartTitle ))
            {
                SvgTitle.X = Width / 2;
                SvgTitle.Y = 1.5 * TitleTextStyle.FontSize;
                SvgTitle.StyleList.Add( "text-anchor", "middle" );
            }
        }

        protected SVGGroup CreateLegendLine( string label, string color, int id )
        {
            var g = new SVGGroup();

            var text = new SVGText() { Text = label };

            var circle = new SVGCircle()
            {
                R = ChartTextStyle.FontSize / 2,
                Fill = color,
                Stroke = "none"
            };

            circle.ClassList.Add( GetChartTheme( id ) );

            g.Children.Add( circle );
            g.Children.Add( text );

            return g;
        }

        protected abstract IEnumerable<Tuple<string, string>> GetLegendItems();

        protected abstract double GetChartableAreaWidth();

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

        public override string Render()
        {
            RenderChartTitle();
            RenderLegend();
            RenderChart();
            return base.Render();
        }
    }
}
