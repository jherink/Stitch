//using HydraDoc.Chart.Axis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HydraDoc.Chart.Formatters
//{
//    public class UnitFormat : NumericFormat
//    {
//        public readonly decimal Unit;
//        public readonly string Name;
//        public readonly Range Range;

//        public UnitFormat( decimal unit, string name, Range range, bool factored, bool decimalExtend, double weight ) : base( factored, decimalExtend, weight )
//        {
//            Unit = unit;
//            Name = name;
//            Range = range;
//        }

//        public override double Score( decimal d )
//        {
//            var pot_d = POT( d );
//            return (pot_d >= Range.Min && pot_d <= Range.Max) ? 1 : 0;
//        }

//        public override Tuple<IEnumerable<string>, string> FormatLabels( IEnumerable<decimal> d )
//        {
//            var r = from x in d select x / Unit;
//            var decimals = (from x in r select DecimalPlaces( x )).Max();
//            return new Tuple<IEnumerable<string>, string>( from x in r select x.ToString( _decimalExtend ? "N" + decimals : "G29" ) + (_factored ? "" : Name), (_factored ? Name : "") );
//        }
//    }
//}
