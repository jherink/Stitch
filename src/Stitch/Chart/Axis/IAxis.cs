using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;

namespace Stitch.Chart.Axis
{
    public interface IAxis<T> : ICloneable where T : IComparable<T>
    {
        bool IncludeDefault { get; set; }
        string BaselineColor { get; set; }
        bool ReverseDirection { get; set; }
        ITextStyle AxisTextStyle { get; set; }
        string Format { get; set; }
        bool GridLines { get; set; }
        string GridLineColor { get; set; }
        IReadOnlyList<T> Ticks { get; }
        string AxisTitle { get; set; }
        ITextStyle AxisTitleTextStyle { get; set; }
        T MaxValue { get; }
        T MinValue { get; }        
        void SetTicks( IEnumerable<T> ticks );
        bool Visible { get; set; }
    }
}
