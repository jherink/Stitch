using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public class AdvancedTextStyle : TextStyle
    {
        public bool Bold { get; set; }
        public bool Italic { get; set; }
    }

    public class TextStyle
    {
        public string Color { get; set; }
        public string FontName { get; set; }
    }
}
