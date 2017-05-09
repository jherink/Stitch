using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc
{
    internal static class Helpers
    {
        public static List<string> GetDefaultColors()
        {
            return new List<string>() {
            "#3366cc",
            "#dc3912",
            "#ff9900",
            "#109618",
            "#990099",
            "#0099c6",
            "#dd4477",
            "#66aa00",
            "#b82e2e",
            "#316395",
            "#994499",
            "#22aa99",
            "#aaaa11",
            "#6633cc",
            "#e67300",
            "#8b0707",
            "#651067",
            "#329262",
            "#5574a6",
            "#3b3eac",
            "#b77322",
            "#16d620",
            "#16d620"
            //"rgb(51,102,204)",
            //"rgb(220, 57, 18)",
            //"rgb(255,153,0)",
            //"rgb(16,150,24)",
            //"rgb(153,0,153)",
            //"rgb(204,204,204)",
            //"rgb(22,214,32)",
            //"rgb(183,115,34)",
            //"rgb(59,62,172)",
            //"rgb(85, 116, 166)",
            //"rgb(50, 146, 98)",
            //"rgb(139, 7, 7)",
            //"rgb(230, 115, 0)",
            //"rgb(102, 51, 204)",
            //"rgb(170, 170, 17)",
            //"rgb(34, 170, 153)",
            //"rgb(153, 68, 153)",
            //"rgb(49, 99, 149)",
            //"rgb(184, 46, 46)",
            //"rgb(102, 170, 0)",
            //"rgb(221, 68, 119)",
            //"rgb(0, 153, 198)",
        };
        }

        public static string GetColor( IList<string> colors, int index )
        {
            return colors[index % (colors.Count - 1)];
        }
    }
}
