using System;
using HydraDoc.Chart;
using Xunit;

namespace HydraDoc.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ChartTests
    {
        [Theory(DisplayName ="Simple2DPieTestLegendPositions")]
        [InlineData(LegendPosition.Right, "Simple2DPieTestRight")]
        [InlineData(LegendPosition.Left, "Simple2DPieTestLeft" )]
        [InlineData(LegendPosition.Top, "Simple2DPieTestTop" )]
        [InlineData(LegendPosition.Bottom, "Simple2DPieTestBottom" )]
        [InlineData(LegendPosition.None, "Simple2DPieTestNone" )]
        public void Simple2DPieTestLegendPositions( LegendPosition position, string fileName)
        {
            var doc = new HydraDocument();
            var graph = new PieChart( );            
            graph.ChartTitle = "My Daily Activites";
            graph.AddSlice( "Work", 11 );
            graph.AddSlice( "Eat", 2 );
            graph.AddSlice( "Commute", 2 );
            graph.AddSlice( "Watch TV", 2 );
            graph.AddSlice( "Sleep", 7 );
            graph.LegendPosition = position;
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( fileName, doc );
        }

        [Theory( DisplayName = "Simple2DPieTestLabels" )]
        [InlineData( PieSliceText.Label, "Simple2DPieTestLabelsLabel" )]
        [InlineData( PieSliceText.Percentage,  "Simple2DPieTestLabelsPercentage" )]
        [InlineData( PieSliceText.Value,  "Simple2DPieTestLabelsValue" )]
        [InlineData( PieSliceText.None,  "Simple2DPieTestLabelsNone" )]
        public void Simple2DPieTestLabels( PieSliceText sliceText, string fileName )
        {
            var doc = new HydraDocument();
            var graph = new PieChart();
            graph.ChartTitle = "My Daily Activites";
            graph.AddSlice( "Work", 11 );
            graph.AddSlice( "Eat", 2 );
            graph.AddSlice( "Commute", 2 );
            graph.AddSlice( "Watch TV", 2 );
            graph.AddSlice( "Sleep", 7 );
            graph.PieSliceText = sliceText;
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( fileName, doc );
        }
    }
}
