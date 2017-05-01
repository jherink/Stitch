using HydraDoc.Chart.Axis;
using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public class Axis<T> : SVGGroup, IAxis<T> where T : IComparable<T>
    {
        readonly int[] bases = { 1, 2, 5 };

        public bool IncludeDefault { get; set; } = true;

        #region IAxis<T> Implementation

        public double AxisLength { get; set; } = 100;

        public ITextStyle AxisTextStyle { get; set; }

        public string BaselineColor { get; set; } = "#000";

        public bool ReverseDirection { get; set; } = false;

        public string Format { get; set; } = string.Empty;

        public bool GridLines { get; set; } = true;

        public string GridLineColor { get; set; } = "#ccc";

        #region Ticks

        private List<T> _ticks = null;

        public IReadOnlyList<T> Ticks { get { return _ticks; } }

        #endregion

        public string AxisTitle { get; set; }

        public ITextStyle TitleTextStyle { get; set; }

        public Orientation Orientation { get; set; }

        public T MaxValue
        {
            get
            {
                if (Ticks != default( IReadOnlyList<T> ))
                {
                    return Ticks.Max();
                }
                return default( T );
            }
        }

        public T MinValue
        {
            get
            {
                if (Ticks != default( IReadOnlyList<T> ))
                {
                    return Ticks.Min();
                }
                return default( T );
            }
        }

        public double GraphWidth { get; set; }

        public double GraphHeight { get; set; }

        #endregion

        #region SVG Groups

        private readonly SVGGroup TitleGroup = new SVGGroup();
        private readonly SVGGroup TickGroup = new SVGGroup();
        private readonly SVGGroup GridLineGroup = new SVGGroup();

        #endregion

        #region Constructors

        public Axis() : this( 400 ) { }

        public Axis( int axisLength )
        {
            AxisLength = axisLength;
            AxisTextStyle = new TextStyle( this );
            TitleTextStyle = new TextStyle( TitleGroup );
            Children.Add( TitleGroup );
            Children.Add( TickGroup );
            Children.Add( GridLineGroup );
        }

        #endregion

        #region Approximating Axis Intervals

        private double GetNiceInterval( double min, double max, int n )
        {

            var rawInterval = (max - min) / n;
            var rawExponent = Math.Log10( rawInterval );

            var exponents = new[] { Math.Floor( rawExponent ), Math.Ceiling( rawExponent ) };
            var nicestInterval = double.PositiveInfinity;

            foreach (var b in bases)
            {
                foreach (var exponent in exponents)
                {
                    var currentInterval = b * Math.Pow( 10, exponent );

                    var currentDeviation = Math.Abs( rawInterval - currentInterval );
                    var nicestDeviation = Math.Abs( rawInterval - nicestInterval );

                    if (currentDeviation < nicestDeviation)
                    {
                        nicestInterval = currentInterval;
                    }
                }
            }
            return nicestInterval;
        }

        private double GetFirstTickValue( double min, double interval )
        {
            return Math.Floor( min / interval ) * interval;
        }

        private IEnumerable<double> GenerateTicks( double min, double max, int n, bool tight = false )
        {
            if (min > max)
            {
                var t = min;
                min = max;
                max = t;
            }

            if (IncludeDefault && min > 0) min = 0;

            var interval = GetNiceInterval( min, max, n );
            var value = GetFirstTickValue( min, interval );

            var ticks = new List<double>() { value };
            while (value < max)
            {
                value += interval;
                ticks.Add( value );
            }

            var multiplier = Math.Pow( 10, Math.Ceiling( Math.Log10( interval ) ) + 1 );
            for (int i = 0; i < ticks.Count; i++)
            {
                ticks[i] = Math.Round( ticks[i] * multiplier ) / multiplier;
            }

            if (tight)
            {
                ticks[0] = min;
                ticks[ticks.Count] = max;
            }

            return ticks;
        }

        #endregion

        private SVGRectangle CreateTickLine( double x, double y, double size )
        {
            var rect = new SVGRectangle()
            {
                Fill = "#cccccc",
                X = x,
                Y = y,
                Height = 1,
                Width = 1
            };
            switch (Orientation)
            {
                case Orientation.Vertical:
                    rect.Height = size;
                    break;
                case Orientation.Horizontal:
                    rect.Width = size;
                    break;
            }
            return rect;
        }

        public IEnumerable<double> SuggestTicks( double min, double max, int intervals = 5 )
        {
            intervals = Math.Max( intervals, 3 );
            return GenerateTicks( min, max, intervals - 2 );
        }

        public void SetTicks( IEnumerable<T> ticks )
        {
            _ticks = ticks.ToList();
        }

        public IEnumerable<ISVGText> GenerateAxisData( IEnumerable<T> ticks )
        {
            TickGroup.Children.Clear();
            _ticks = (ReverseDirection ? ticks.Reverse() : ticks).ToList();
            var texts = new List<ISVGText>();
            double space = AxisLength / _ticks.Count;
            var fs = AxisTextStyle.FontSize;
            var maxTickLength = ticks.Max( t => t.ToString().Length );
            double x = Orientation == Orientation.Horizontal ? 0 : fs;
            double y = Orientation == Orientation.Horizontal ?
                       GraphHeight - (maxTickLength * (fs + 1) / 2) :
                       0;
            var rotation = string.Empty;


            foreach (var tick in _ticks)
            {
                var tickString = tick.ToString();
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        x += space - (tickString.Length * fs / 2.0);
                        rotation = maxTickLength > 4 ? $"rotate(90 {x},{y})" : string.Empty; // rotate 90 degrees about (x,y)
                        break;
                    case Orientation.Vertical:
                        y += space - fs / 2;
                        break;
                }
                var t = new SVGText()
                {
                    X = x,
                    Y = y,
                    Transform = rotation
                };
                t.Text.Append( tickString );
                texts.Add( t );
                TickGroup.Children.Add( t );
            }

            var minX = texts.Min( t => t.X );
            var minY = texts.Min( t => t.Y );
            var maxX = texts.Max( t => t.X );
            var maxY = texts.Max( t => t.Y );
            var axisRect = new SVGRectangle() { Fill = GridLineColor };
            axisRect.X = minX;
            axisRect.Y = minY;
            axisRect.Width = Orientation == Orientation.Vertical ? 1 : maxX - minX;
            axisRect.Height = Orientation == Orientation.Vertical ? maxY - minY : 1;
            TickGroup.Children.Add( axisRect );

            // TODO: Render Axis
            //GridLineGroup.Children.Clear();
            //if (GridLines)
            //{
            //    foreach (var t in texts)
            //    {
            //        SVGRectangle line = new SVGRectangle() { Fill = GridLineColor };
            //        switch (Orientation)
            //        {
            //            case Orientation.Horizontal:
            //                line.X = t.X;
            //                line.Y = 0;
            //                line.Width = 1;
            //                line.Height = GraphHeight;
            //                break;
            //            case Orientation.Vertical:
            //                line.X = t.X + (1.5 * maxTickLength) * fs;
            //                line.Y = t.Y;
            //                line.Width = GraphWidth;
            //                line.Height = 1;
            //                break;
            //        }
            //        GridLineGroup.Children.Add( line );
            //    }
            //}

            return texts;
        }

        public override string Render()
        {
            return base.Render();
        }
    }
}
