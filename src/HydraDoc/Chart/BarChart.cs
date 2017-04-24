using HydraDoc.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Chart
{
    public enum BarChartOrientation
    {
        Horizontal,
        Vertical
    };

    public class BarChart : SVG, IChart
    {
        public BarChartOrientation Orientation { get; set; } = BarChartOrientation.Vertical;

        public LegendPosition LegendPosition { get; set; } = LegendPosition.Right;

        #region IChart Implementation

        public AdvancedTextStyle TitleTextStyle { get; set; } = new AdvancedTextStyle();

        public string ChartTitle { get; set; }

        public List<string> Colors { get; private set; }

        double IChart.Width
        {
            get
            {
                return Width;
            }

            set
            {
                Width = value;
            }
        }

        double IChart.Height
        {
            get
            {
                return Height;
            }

            set
            {
                Height = value;
            }
        }


        #endregion
    }
}
