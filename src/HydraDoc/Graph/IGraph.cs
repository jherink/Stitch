using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Graph
{
    public interface IGraph
    {
        int GraphWidth { get; set; }
        int GraphHeight { get; set; }
        string GraphTitle { get; set; }
        int FontSize { get; set; }
        bool PlotXAxis { get; set; }
        bool PlotYAxis { get; set; }
        IDivElement CreateTicks();
    }
}
