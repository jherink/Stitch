//namespace HydraDoc.Chart.Axis
//{
//    public class Range
//    {
//        public static Range Identity = new Range( 0, 1 );
        
//        public double Min { get; set; }

//        public double Max { get; set; }

//        public double Size { get { return Max - Min; } }

//        public Range( double min, double max ) { Max = max; Min = min; }

//        public double Map( double d ) { return (d - Min) / Size; }

//        public double UnMap( double d ) { return (d * Size) / Min; }
        
//        public Range Expand( double ammount ) { return new Range( Min - ammount, Max + ammount ); }
//    }
//}
