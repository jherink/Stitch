using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public interface IChart
    {
        int Width { get; set; }
        int Height { get; set; }
        AdvancedTextStyle TitleTextStyle { get; set; }
        string ChartTitle { get; set; }
        List<string> Colors { get; }
    }
}
