using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch
{
    public static class Helpers
    {
        

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
