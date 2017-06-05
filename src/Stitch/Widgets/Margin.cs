using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch
{
    public enum MarginUnit
    {
        Pixels,
        Inches,
        Centimeters,
        Millimeters,
        Points,
        Picas
    };

    public class Margin : ICloneable
    {
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public MarginUnit MarginUnit { get; set; }

        public Margin() { }

        public Margin( double top, double right, double bottom, double left )
        {
            Top = top; Right = right; Left = left; Bottom = bottom;
        }

        public void Apply( IElement element )
        {
            var unit = GetUnit();
            var marginString = string.Format( "margin: {0}{4} {1}{4} {2}{4} {3}{4};", Top, Right, Bottom, Left, unit );
            element.StyleList.Add( marginString );
        }

        private string GetUnit()
        {
            switch (MarginUnit) {
                case MarginUnit.Pixels:
                    return "px";
                case MarginUnit.Inches:
                    return "in";
                case MarginUnit.Centimeters:
                    return "cm";
                case MarginUnit.Millimeters:
                    return "mm";
                case MarginUnit.Points:
                    return "pt";
                case MarginUnit.Picas:
                    return "pc";
            }
            return "px";
        }

        public object Clone()
        {
            return new Margin( Top, Right, Bottom, Left ) { MarginUnit = this.MarginUnit };
        }
    }
}
