using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    public enum LegendPosition
    {
        /// <summary>
        /// Displays the legend below the chart.
        /// </summary>
        Bottom,
        /// <summary>
        /// Displays the legend left of the chart.
        /// </summary>
        Left,
        /// <summary>
        /// Displays no legend.
        /// </summary>
        None,
        /// <summary>
        /// Displays the legend right of the chart.
        /// </summary>
        Right,
        /// <summary>
        /// Displays the legend right of the chart.
        /// </summary>
        Top
    }

    public interface IChart
    {
        double Width { get; set; }
        double Height { get; set; }
        LegendPosition LegendPosition { get; set; }
        ITextStyle TitleTextStyle { get; set; }
        ITextStyle ChartTextStyle { get; set; }
        string ChartTitle { get; set; }
    }
}
