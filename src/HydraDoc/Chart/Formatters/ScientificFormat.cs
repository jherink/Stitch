//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HydraDoc.Chart.Formatters
//{
//    public class ScientificFormat : NumericFormat
//    {
//        public ScientificFormat( bool factored, bool decimalExtend, double weight )
//            : base( factored, decimalExtend, weight )
//        {
//        }

//        //scientific format for general numbers
//        public override double Score( decimal d )
//        {
//            return 1;
//        }

//        public override Tuple<IEnumerable<string>, string> FormatLabels( IEnumerable<decimal> d )
//        {
//            int avgpot = (int)Math.Round( (from x in d.Where( x => x != 0 ) select POT( x )).Average() );
//            decimal s = Pow10( avgpot );
//            IEnumerable<decimal> r = from x in d select x / s;
//            int decimals = (from x in r select DecimalPlaces( x )).Max();
//            string label = "x10\\^" + avgpot + "\\^";
//            return new Tuple<IEnumerable<string>, string>( from x in r select x.ToString( _decimalExtend ? "N" + decimals : "0.#" ) + (_factored ? "" : label), (_factored ? label : "") );
//        }
//    }
//}
