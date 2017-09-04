using Stitch.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Attributes
{
    internal static class StitchCssResourceHelper
    {
        public static string GetClass( UnorderedListStyleType styleType )
        {
            return GetAttribute( styleType.GetType(), styleType.ToString() ).Class;
        }

        public static string GetClass( OrderedListStyleType styleType )
        {
            return GetAttribute( styleType.GetType(), styleType.ToString() ).Class;
        }

        private static StitchCssResourceAttribute GetAttribute( Type type, string member )
        {
            var memInfo = type.GetMember( member );
            var attributes = memInfo[0].GetCustomAttributes( typeof( StitchCssResourceAttribute ), false );
            return ((StitchCssResourceAttribute)attributes[0]);
        }
    }

    internal class StitchCssResourceAttribute : Attribute
    {
        public string Class { get; set; }
        public StitchCssResourceAttribute( string _class )
        {
            Class = _class;
        }
    }
}
