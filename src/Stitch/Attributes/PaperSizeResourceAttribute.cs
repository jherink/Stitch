using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Attributes
{
    internal static class PaperSizeHelper
    {
        public static string GetClass( PaperSize size, PageOrientation orienation )
        {
            //return GetAttribute( size ).GetCSSRule();
            var cls = GetAttribute( size ).CSSClass;
            return orienation == PageOrientation.Portrait ? cls : $"{cls}-landscape";
        }

        private static PaperSizeResourceAttribute GetAttribute( PaperSize size )
        {
            var memInfo = typeof( PaperSize ).GetMember( size.ToString() );
            var attributes = memInfo[0].GetCustomAttributes( typeof( PaperSizeResourceAttribute ), false );
            return ((PaperSizeResourceAttribute)attributes[0]);
        }
    }

    internal class PaperSizeResourceAttribute : Attribute
    {

        public readonly string CSSClass = string.Empty;
        
        public PaperSizeResourceAttribute( string cssClass )
        {
            CSSClass = cssClass;
        }


    }
    
}
