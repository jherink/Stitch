using Stitch.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Stitch.Chart.Axis
{
    public class Axis<T> : IAxis<T> where T : IComparable<T>
    {

        #region IAxis<T> Implementation

        public bool IncludeDefault { get; set; } = true;

        public ITextStyle AxisTextStyle { get; set; }

        public string BaselineColor { get; set; } = "#000";

        public bool ReverseDirection { get; set; } = false;

        public string Format { get; set; } = string.Empty;

        public bool GridLines { get; set; } = true;

        public string GridLineColor { get; set; } = "#ccc";

        #region Ticks

        private List<T> _ticks = new List<T>();

        public IReadOnlyList<T> Ticks { get { return _ticks; } }

        #endregion

        public string AxisTitle { get; set; }

        public ITextStyle AxisTitleTextStyle { get; set; }

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

        public bool Visible { get; set; } = true;

        #endregion

        #region Constructors

        public Axis()
        {
            AxisTextStyle = new TextStyle( new SVGText() );
            AxisTitleTextStyle = new TextStyle( new SVGText() );
        }

        #endregion
                
        public void SetTicks( IEnumerable<T> ticks )
        {
            _ticks = ticks.ToList();
        }

        public object Clone()
        {
            var clone = MemberwiseClone() as IAxis<T>;
            clone.AxisTextStyle = this.AxisTextStyle.Clone() as ITextStyle;
            clone.AxisTitleTextStyle = this.AxisTextStyle.Clone() as ITextStyle;
            var ticks = new List<T>();
            if (this.Ticks != null) foreach (var t in this.Ticks) ticks.Add( t );
            clone.SetTicks( ticks );
            return clone;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Ticks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Ticks.GetEnumerator();
        }
    }
}
