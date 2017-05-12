using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch
{
    public static class Helpers
    {
        public static List<string> GetDefaultColors()
        {
            return new List<string>() {
            "#3366cc", // "mariner"
            "#dc3912", // "tia maria"
            "#ff9900", // "orange peel"
            "#109618", // "la palma"
            "#990099", // "flirt"
            "#0099c6", // "pacific blue"
            "#dd4477", // "cabaret"
            "#66aa00", // "limeade"
            "#b82e2e", // "tall poppy"
            "#316395", // "azure"
            "#994499", // "plum"
            "#22aa99", // "jungle green"
            "#aaaa11", // "sahara"
            "#6633cc", // "purple heart"
            "#e67300", // "mango tango"
            "#8b0707", // "totem pole"
            "#651067", // "scarlet gum"
            "#329262", // "sea green"
            "#5574a6", // "wedgewood"
            "#3b3eac", // "governor bay"
            "#b77322", // "bourbon"
            "#16d620", // "malachite"
            };
        }

        private const int MAX_COLORS = 22;

        public static string GetColor( IList<string> colors, int index )
        {
            return colors[index % (colors.Count - 1)];
        }
        
        public static int TranslateOrientation( PageOrientation orientation, string pageSize = "Letter" )
        {
            // Maybe change page size in the future?  Added as option param for now
            // but doesn't do anything.
            switch (orientation)
            {
                case PageOrientation.Landscape:
                    return 1680;
                case PageOrientation.Portrait:
                    return 1024;
            }

            return 1024;
        }
    }
}
