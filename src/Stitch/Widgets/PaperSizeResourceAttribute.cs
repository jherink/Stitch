using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch
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

    //internal class PaperSizeResourceAttribute : Attribute
    //{
    //    // we will use inches in all calcs since 1 inch = 96 px;
    //    private const int PIXEL_CONVERSION_FACTOR = 96;

    //    /// <summary>
    //    /// The width of the paper size in inches
    //    /// </summary>
    //    public readonly double Width;
    //    /// <summary>
    //    /// The height of the paper size in inches
    //    /// </summary>
    //    public readonly double Height;
    //    public readonly string CSSClass = string.Empty;

    //    public double PixelWidth { get { return Width * PIXEL_CONVERSION_FACTOR; } }
    //    public double PixelHeight { get { return Height * PIXEL_CONVERSION_FACTOR; } }

    //    public string GetCSSRule()
    //    {
    //        return $".{CSSClass} {{ height: {Height}in; width: {Width}in; }}";
    //    }

    //    public PaperSizeResourceAttribute( string cssClass, double height, double width )
    //    {
    //        CSSClass = cssClass;
    //        Height = height;
    //        Width = width;
    //    }


    //}
}
