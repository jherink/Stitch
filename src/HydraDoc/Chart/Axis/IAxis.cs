using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;

namespace HydraDoc.Chart.Axis
{
    public interface IAxis<T> where T : IComparable<T>
    {
        //double AxisLength { get; set; }
        string BaselineColor { get; set; }
        bool ReverseDirection { get; set; }
        ITextStyle AxisTextStyle { get; set; }
        string Format { get; set; }
        bool GridLines { get; set; }
        string GridLineColor { get; set; }
        IReadOnlyList<T> Ticks { get; }
        string AxisTitle { get; set; }
        ITextStyle TitleTextStyle { get; set; }
        //Orientation Orientation { get; set; }
        T MaxValue { get; }
        T MinValue { get; }
        IEnumerable<double> SuggestTicks( double min, double max, int intervals );
        void SetTicks( IEnumerable<T> ticks );
        //double GraphWidth { get; set; }
        //double GraphHeight { get; set; }
        //IEnumerable<ISVGText> GenerateAxisData( IEnumerable<T> ticks );
    }
}
