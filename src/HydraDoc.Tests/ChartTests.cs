using System;
using HydraDoc.Chart;
using Xunit;

namespace HydraDoc.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ChartTests
    {
        [Theory( DisplayName = "Simple2DPieTestLegendPositions" )]
        [InlineData( LegendPosition.Right, "Simple2DPieTestRight" )]
        [InlineData( LegendPosition.Left, "Simple2DPieTestLeft" )]
        [InlineData( LegendPosition.Top, "Simple2DPieTestTop" )]
        [InlineData( LegendPosition.Bottom, "Simple2DPieTestBottom" )]
        [InlineData( LegendPosition.None, "Simple2DPieTestNone" )]
        public void Simple2DPieTestLegendPositions( LegendPosition position, string fileName )
        {
            var doc = new HydraDocument();
            var graph = new PieChart();
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
        [InlineData( PieSliceText.Percentage, "Simple2DPieTestLabelsPercentage" )]
        [InlineData( PieSliceText.Value, "Simple2DPieTestLabelsValue" )]
        [InlineData( PieSliceText.None, "Simple2DPieTestLabelsNone" )]
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

        [Fact( DisplayName = "ExplodeSliceTest" )]
        public void ExplodeSliceTest()
        {
            var doc = new HydraDocument();
            var graph = new PieChart();
            graph.LegendPosition = LegendPosition.None;
            graph.PieSliceText = PieSliceText.Label;
            graph.ChartTitle = "Indian Language Use";
            graph.AddSlice( "Assamese", 13 );   // #3366cc
            graph.AddSlice( "Bengali", 83 );    // #dc3912
            graph.AddSlice( "Bodo", 1.4 );      // #ff9900
            graph.AddSlice( "Dogri", 2.3 );     // #109618
            graph.AddSlice( "Gujarati", 46, offset: .2 ); // #990099
            graph.AddSlice( "Hindi", 300 );     // #0099c6
            graph.AddSlice( "Kannada", 38 );    // #dd4477
            graph.AddSlice( "Kashmiri", 5.5 );  // #66aa00
            graph.AddSlice( "Konkani", 5 );     // #b82e2e
            graph.AddSlice( "Maithili", 20 );   // #316395
            graph.AddSlice( "Malayalam", 33 );  // #22aa99
            graph.AddSlice( "Manipuri", 1.5 );  // #22aa99
            graph.AddSlice( "Marathi", 72, offset: .3 );    // #994499
            graph.AddSlice( "Nepali", 2.9 );                // #6633cc
            graph.AddSlice( "Oriya", 33, offset: .4 );      // #e67300
            graph.AddSlice( "Punjabi", 29, offset: .5 );    // #8b0707
            graph.AddSlice( "Sanskrit", 0.01 ); // #dd4477
            graph.AddSlice( "Santhali", 6.5 );  // #329262
            graph.AddSlice( "Sindhi", 2.5 );    // #5574a6
            graph.AddSlice( "Tamil", 61 );      // #3b3eac
            graph.AddSlice( "Telugu", 74 );     // #b77322
            graph.AddSlice( "Urdu", 52 );       // #16d620

            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "ExplodeSliceTest", doc );
        }

        [Theory( DisplayName = "BarChartTest" )]
        [InlineData( "Density of Precious Metals, in g/cm^3", 
                     new[] { "Copper", "Silver", "Gold", "Platinum" },
                     new[] { 8.94, 10.49, 19.30, 21.45 },  
                     new[] { "#b87333", "silver", "gold", "#e5e4e2" },
                     Orientation.Horizontal, 
                     "HorizontalBarChartTest" )]
        [InlineData( "Density of Precious Metals, in g/cm^3",
                     new[] { "Copper", "Silver", "Gold", "Platinum" },
                     new[] { 8.94, 10.49, 19.30, 21.45 },
                     new[] { "#b87333", "silver", "gold", "#e5e4e2" },
                     Orientation.Vertical, 
                    "VerticalBarChartTest" )]
        public void BarChartTest( string label, string[] labels, double[] axisData, string[] colors, Orientation graphOrientation, string fileName )
        {
            Assert.Equal( labels.Length, axisData.Length );
            Assert.Equal( labels.Length, colors.Length );
            var chart = new BarChart();
            chart.AxisOrientation = graphOrientation;
            chart.ChartTitle = label;
            chart.TitleTextStyle.Bold = true;
            for (int i = 0; i < labels.Length; i++)
            {
                chart.AddBar( labels[i], axisData[i], colors[i] );
            }
            var doc = new HydraDocument();
            doc.Add( chart );
            IntegrationHelpers.SaveToTemp( fileName, doc );
        }
    }
}
