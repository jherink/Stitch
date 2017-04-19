using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public interface IChartArea
    {
        string BackgroundColor { get; set; }
        string Left { get; set; }
        string Top { get; set; }
        string Right { get; set; }
        string Width { get; set; }
        string Height { get; set; }
    }
}
