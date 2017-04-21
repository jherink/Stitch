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
            graph.ChartTitle = "My Daily Activities";
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
            graph.ChartTitle = "My Daily Activities";
            graph.AddSlice( "Work", 11 );
            graph.AddSlice( "Eat", 2 );
            graph.AddSlice( "Commute", 2 );
            graph.AddSlice( "Watch TV", 2 );
            graph.AddSlice( "Sleep", 7 );
            graph.PieSliceText = sliceText;
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( fileName, doc );
        }

        [Theory( DisplayName = "Simple2DPieTestPieHole" )]
        [InlineData( 0, "Simple2DPieTestPieHole0" )]
        [InlineData( 1, "Simple2DPieTestPieHole100" )]
        [InlineData( .4, "Simple2DPieTestPieHole50" )]
        [InlineData( .25, "Simple2DPieTestPieHole25" )]
        [InlineData( .75, "Simple2DPieTestPieHole75" )]
        [InlineData( .10, "Simple2DPieTestPieHole10" )]
        [InlineData( .9, "Simple2DPieTestPieHole90" )]
        public void Simple2DPieTestPieHole( double pieHole, string fileName )
        {
            var doc = new HydraDocument();
            var graph = new PieChart();
            graph.ChartTitle = "My Daily Activities";
            graph.AddSlice( "Work", 11 );
            graph.AddSlice( "Eat", 2 );
            graph.AddSlice( "Commute", 2 );
            graph.AddSlice( "Watch TV", 2 );
            graph.AddSlice( "Sleep", 7 );
            graph.PieHole = pieHole;
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( fileName, doc );
        }

        [Theory( DisplayName = "Test2DPieRotation" )]
        [InlineData( 100, "Test2PieRotation100" )]
        public void Test2DPieRotation( double rotation, string fileName )
        {
            var doc = new HydraDocument();
            var graph = new PieChart();
            graph.Width = 800;
            graph.StyleList.Add( "float", "left" );
            graph.ChartTitle = "Swiss Language Use (no rotation)";
            graph.LegendPosition = LegendPosition.None;
            graph.PieSliceText = PieSliceText.Label;
            graph.AddSlice( "German", 5.85 );
            graph.AddSlice( "French", 1.66 );
            graph.AddSlice( "Italian", .316 );
            graph.AddSlice( "Romansh", .0791 );
            graph.PieStartAngle = 0;
            doc.Add( graph );

            var rotatedGraph = graph.Clone() as PieChart;
            rotatedGraph.PieStartAngle = 100;
            rotatedGraph.ChartTitle = "Swiss Language Use (100 degree rotation)";
            doc.Add( rotatedGraph );

            IntegrationHelpers.SaveToTemp( fileName, doc );
        }
    }
}
