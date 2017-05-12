//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Stitch.Chart.Formatters
//{
//    public abstract class NumericFormat : Format
//    {
//        /// <summary>
//        /// If true, 10^power portion will be placed on the axis title.
//        /// </summary>
//        protected bool _factored;

//        /// <summary>
//        /// If true, labels will be extended to the same number of decimal places
//        /// </summary>
//        protected bool _decimalExtend;

//        public NumericFormat( bool factored, bool decimalExtend, double weight ) : base( weight )
//        {
//            _factored = factored;
//            _decimalExtend = decimalExtend;
//        }

//        public override double Score( IEnumerable<object> values )
//        {
//            throw new NotImplementedException();
//        }

//        public override Tuple<IEnumerable<string>, string> FormatLabels( IEnumerable<object> values )
//        {
//            throw new NotImplementedException();
//        }

//        public abstract double Score( decimal d );
//        public abstract Tuple<IEnumerable<string>, string> FormatLabels( IEnumerable<decimal> d );

//        protected int POT( decimal val )
//        {
//            return (int)Math.Floor( Math.Log10( (double)Math.Abs( val ) ) );
//        }

//        protected decimal Pow10( int i )
//        {
//            decimal a = 1m;
//            for (int j = 0; j < i; j++) a *= 10m;
//            return a;
//        }

//        protected int DecimalPlaces( decimal i )
//        {
//            var t = i.ToString( "G29" );
//            var s = t.IndexOf( '.' );
//            return s < 0 ? 0 : t.Length - (s + 1);
//        }

//    }
//}
