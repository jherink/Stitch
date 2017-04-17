using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Elements
{
    public class SetStyleTypeAttribute : Attribute
    {
        public readonly string TypeCode;

        public SetStyleTypeAttribute( string typeCode )
        {
            TypeCode = typeCode;
        }
    }

    public static class StyleTypeHelper
    {
        public static string GetStyleType<T>( T value )
        {
            var t = typeof( T );
            var member = t.GetMember( value.ToString() );
            var attributes = member[0].GetCustomAttributes( typeof( SetStyleTypeAttribute ), false );
            return ((SetStyleTypeAttribute)attributes[0])?.TypeCode;
        }
    }
}
