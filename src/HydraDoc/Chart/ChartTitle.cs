using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public class ChartTitle : SVGGroup
    {
        public readonly SVGText Text;

        public AdvancedTextStyle Style { get; set; }

        public ChartTitle()
        {
            Text = new SVGText();
            Style = new AdvancedTextStyle();
        }
    }
}
