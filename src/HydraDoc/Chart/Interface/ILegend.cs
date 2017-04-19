using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public enum LegendPosition
    {
        /// <summary>
        /// Displays the legend below the chart.
        /// </summary>
        Bottom,
        /// <summary>
        /// Draws lines connecting slices to their data values.
        /// </summary>
        Labeled,
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

    public enum LegendAlignment
    {
        /// <summary>
        ///  Aligned to the start of the area allocated for the legend.
        /// </summary>
        Start,
        /// <summary>
        /// Centered in the area allocated for the legend.
        /// </summary>
        Center,
        /// <summary>
        /// Aligned to the end of the area allocated for the legend.
        /// </summary>
        End
    }

    public interface ILegend
    {
        int MaxLines { get; set; }
        LegendPosition Positiong { get; set; }
        LegendAlignment Alignment { get; set; }
        AdvancedTextStyle TextStyle { get; set; }
    }
}
