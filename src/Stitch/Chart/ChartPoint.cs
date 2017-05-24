using System;

namespace Stitch.Chart
{
    public class ChartPoint<T1, T2> : ICloneable where T1 : IComparable<T1>
                                                 where T2 : IComparable<T2>
    {
        public T1 LabeledValue { get; set; }
        public T2 MeasuredValue { get; set; }
        public string Color { get; set; }
        public ChartPoint() { }
        public ChartPoint( T1 labeledValue, T2 measuredValue )
        {
            LabeledValue = labeledValue;
            MeasuredValue = measuredValue;
        }

        public object Clone()
        {
            return new ChartPoint<T1, T2>( LabeledValue, MeasuredValue );
        }
    }
}
