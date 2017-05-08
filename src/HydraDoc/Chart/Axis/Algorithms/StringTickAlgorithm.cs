using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart.Axis.Algorithms
{
    public class StringTickAlgorithm : ITickAlgorithm<string>
    {
        public string Max( IEnumerable<string> set )
        {
            var val = set.Last();
            if (val == null) val = string.Empty;
            return val;
        }

        public string Min( IEnumerable<string> set )
        {
            var val = set.First();
            if (val == null) val = string.Empty;
            return val;
        }

        public double Percentage( IEnumerable<string> range, string value )
        {
            var i = 0.0;
            foreach (var val in range)
            {
                if ((val == null && value == null) || (val != null && val.Equals( value ))) break;
                i += 1;
            }
            return i / range.Count();
        }

        public double Subtract( IEnumerable<string> set, string a, string b )
        {
            double posA = 0, posB = 0;
            var i = 0.0;
            foreach (var val in set)
            {
                if ((val == null && a == null) || (val != null && val.Equals( a ))) posA = i;
                if ((val == null && b == null) || (val != null && val.Equals( b ))) posB = i;
                i++;
            }
            return posA - posB;
        }

        public IEnumerable<string> SuggestTicks( IEnumerable<string> set, int intervals )
        {
            var ticks = new List<string>();
            var setArr = set.ToArray();
            var increment = 1;
            var size = setArr.Length;
            while ((size -= 10) > 1) increment++;

            for (int i = 0; i < setArr.Length; i++)
            {
                if (i % increment == 0) ticks.Add( setArr[i] == null ? string.Empty : setArr[i] );
            }

            return ticks;
        }
    }
}
