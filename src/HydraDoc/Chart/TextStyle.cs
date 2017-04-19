using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public class AdvancedTextStyle : TextStyle
    {
        bool Bold { get; set; }
        bool Italic { get; set; }
    }

    public class TextStyle
    {
        string Color { get; set; }
        string FontName { get; set; }
        string FontSize { get; set; }
    }
}
