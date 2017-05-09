using System;
using HydraDoc.Chart;
using Xunit;
using System.Data;
using System.Collections.Generic;

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
                     "Weight",
                     "Precious Metals",
                     Orientation.Horizontal,
                     "HorizontalBarChartTest" )]
        [InlineData( "Density of Precious Metals, in g/cm^3",
                     new[] { "Copper", "Silver", "Gold", "Platinum" },
                     new[] { 8.94, 10.49, 19.30, 21.45 },
                     new[] { "#b87333", "silver", "gold", "#e5e4e2" },
                     "Weight",
                     "Precious Metals",
                     Orientation.Vertical,
                    "VerticalBarChartTest" )]
        public void BarChartTestSimple( string label, string[] labels, double[] axisData, string[] colors, string measuredAxisTitle, string labeledAxisTitle, Orientation graphOrientation, string fileName )
        {
            Assert.Equal( labels.Length, axisData.Length );
            Assert.Equal( labels.Length, colors.Length );
            var chart = new BarChart();
            chart.AxisOrientation = graphOrientation;
            chart.ChartTitle = label;
            chart.MeasuredAxis.AxisTitle = measuredAxisTitle;
            chart.LabeledAxis.AxisTitle = labeledAxisTitle;
            chart.MeasuredAxis.AxisTitleTextStyle.FontSize = chart.LabeledAxis.AxisTitleTextStyle.FontSize = 22;
            chart.MeasuredAxis.AxisTitleTextStyle.Bold = chart.LabeledAxis.AxisTitleTextStyle.Bold = true;
            chart.TitleTextStyle.Bold = true;
            for (int i = 0; i < labels.Length; i++)
            {
                chart.AddBar( labels[i], axisData[i], colors[i] );
            }
            var doc = new HydraDocument();
            doc.Add( chart );
            IntegrationHelpers.SaveToTemp( fileName, doc );
        }

        [Theory( DisplayName = "AxisRenderSizeTests" )]
        [InlineData( "abcdefghijklmnopqrstuvwxz",
                     new[] { "abc", "abcdefghijk", "abcdefghijklmnop", "abcdefghijklmnopqrstuvwxz", "12345" },
                     new[] { 77.0, 114.0, 215.0, 45.0, 175.0 },
                     null,
                     new[] { 300.0, 500.0, 700.0, 900.0, 1100.0, 1300.0 },
                     new[] { 200.0, 300.0, 400.0, 500.0, 600.0, 700.0, 800.0, 900.0, 1000.0 },
                     new[] { 8, 10, 12, 14, 16, 18, 20, 22, 24 },
                     "abcdefghijklmnopqrstuvwxz",
                     "abcdefghijklmnopqrstuvwxz",
                     "AxisRenderSizeTests" )]
        [InlineData( "Density of Precious Metals, in g/cm^3",
                     new[] { "Copper", "Silver", "Gold", "Platinum" },
                     new[] { 8.94, 10.49, 19.30, 21.45 },
                     new[] { "#b87333", "silver", "gold", "#e5e4e2" },
                     new[] { 800.0 },
                     new[] { 400.0 },
                     new[] { 12 },
                     "Weight",
                     "Precious Metals",
                     "PracticalAxisRenderSizeTests" )]
        public void AxisRenderSizeTests( string title, string[] labels, double[] axisData, string[] colors,
                                         double[] graphWidths, double[] graphHeights, int[] fontSizes,
                                         string measuredAxisTitle, string labeledAxisTitle, string fileName )
        {
            // The graphs rendered from this unit test will look very strange at small sizes.
            // This tests just as a whole bars and chosen axis values look okay.

            Assert.Equal( labels.Length, axisData.Length );
            if (colors != null) Assert.Equal( labels.Length, colors.Length );
            var doc = new HydraDocument();

            foreach (var width in graphWidths)
            {
                foreach (var height in graphHeights)
                {
                    foreach (var font in fontSizes)
                    {
                        var chart = new BarChart();
                        chart.Width = width;
                        chart.Height = height;
                        chart.MeasuredAxis.AxisTitleTextStyle.FontSize = chart.LabeledAxis.AxisTitleTextStyle.FontSize = chart.ChartTextStyle.FontSize = font;
                        chart.MeasuredAxis.AxisTitleTextStyle.Bold = chart.LabeledAxis.AxisTitleTextStyle.Bold = true;
                        chart.AxisOrientation = Orientation.Horizontal;
                        chart.ChartTitle = title;
                        chart.MeasuredAxis.AxisTitle = measuredAxisTitle;
                        chart.MeasuredAxis.AxisTitle = measuredAxisTitle;
                        chart.LabeledAxis.AxisTitle = labeledAxisTitle;
                        chart.TitleTextStyle.Bold = true;
                        for (int i = 0; i < labels.Length; i++)
                        {
                            chart.AddBar( labels[i], axisData[i], colors != null ? colors[i] : string.Empty );
                        }
                        doc.Add( chart );
                        doc.Add( new Elements.LineBreak() );

                        var verticalClone = chart.Clone() as BarChart;
                        verticalClone.AxisOrientation = Orientation.Vertical;
                        doc.Add( verticalClone );
                        doc.Add( new Elements.LineBreak() );
                    }
                }
            }
            IntegrationHelpers.SaveToTemp( fileName, doc );
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void NorthwindSalesByYearTest()
        {
            var doc = new HydraDocument();
            var data = IntegrationHelpers.GetYearSummarySales();
            var chart = new BarChart();
            chart.ChartTitle = "Northwind Total Annual Sales";
            chart.TitleTextStyle.Bold = true;
            chart.MeasuredAxis.AxisTitle = "Year";
            chart.LabeledAxis.AxisTitle = "Total Sales";
            chart.MeasuredAxis.Format = "C0";
            chart.AxisOrientation = Orientation.Vertical;
            foreach (DataRow row in data.Rows)
            {
                chart.AddBar( row["year"].ToString(), double.Parse( row["total"].ToString() ) );
            }
            var container = doc.AddBodyContainer();
            container.Children.Add( chart );

            IntegrationHelpers.SaveToTemp( "NorthwindSalesByYearTest", doc );
        }

        [Fact( DisplayName = "HorizontalBarChartTest" )]
        public void HorizontalBarChartTest()
        {
            var doc = new HydraDocument();
            var chart = new BarChart( 700, 400 );
            chart.AxisOrientation = Orientation.Horizontal;
            chart.MeasuredAxis.Visible = false;
            chart.MeasuredAxis.GridLines = false;
            chart.ChartTitle = "Browser market share June 2015";
            chart.AddBar( "IE 11: 11.33%", 11.33 );
            chart.AddBar( "Chrome: 49.77%", 49.77 );
            chart.AddBar( "Firefox: 16.09%", 16.09 );
            chart.AddBar( "Safari: 5.41%", 5.41 );
            chart.AddBar( "Opera: 1.62%", 1.62 );
            chart.AddBar( "Android 4.4: 2%", 2 );
            doc.Add( chart );

            IntegrationHelpers.SaveToTemp( "HorizontalBarChartTest", doc );
        }

        [Fact( DisplayName = "VerticalBarChartTest" )]
        public void VerticalBarChartTest()
        {
            var doc = new HydraDocument();
            var chart = new BarChart();
            var cont = doc.AddBodyContainer();
            chart.ChartTitle = "Quarterly Results";
            chart.TitleTextStyle.Bold = true;
            chart.Width = 600;
            chart.MeasuredAxis.Format = "C0";
            chart.StyleList.Add( "padding-left", "25%" );
            chart.AddBar( "Q1", 18450 );
            chart.AddBar( "Q2", 34340.72 );
            chart.AddBar( "Q3", 43145.52 );
            chart.AddBar( "Q4", 18415 );
            cont.Add( chart );

            IntegrationHelpers.SaveToTemp( "VerticalBarChartTest", doc );
        }

        [Theory( DisplayName = "LineChartWithNumbersTest" )]
        [InlineData( "LineChartWithNumbersTest1" )]
        public void LineChartTest( string saveName )
        {
            var doc = new HydraDocument();
            var chart = new LineChart<double, double>();

            var dogPoints = new[] { 0, 10, 23, 17, 18, 9, 11, 27,
                                    33, 40, 32, 35, 30, 40, 42, 47,
                                    44, 48, 52, 54, 42, 55, 56, 57,
                                    60, 50, 52, 51, 49, 53, 55, 60,
                                    61, 59, 62, 65, 62, 58, 55, 61, 64,
                                    65, 63, 66, 67, 69, 69, 70, 72, 68, 66,
                                    65, 67, 70, 71, 72, 73, 75, 70, 68,
                                    64, 60, 65, 67, 68, 69, 70, 72, 75, 80 };

            int i = 0;
            foreach (var p in dogPoints) chart.AddPoint( "Dogs", i++, p );

            var catPoints = new[] { 0, 5, 15, 9, 10, 5, 3, 19, 25,
                                    32, 24, 27, 22, 32, 34, 39, 36,
                                    40, 44, 46, 34, 47, 48, 49, 52,
                                    42, 44, 43, 41, 45, 47, 52, 53,
                                    51, 54, 57, 54, 50, 47, 53, 56,
                                    57, 55, 58, 59, 61, 61, 62, 64,
                                    60, 58, 57, 59, 62, 64, 60, 58,
                                    57, 59, 62, 63, 64, 65, 67, 62,
                                    60, 56, 52, 57, 59, 60, 61, 62,
                                    64, 67, 72 };

            var j = 0;
            foreach (var c in catPoints) chart.AddPoint( "Cats", j++, c );

            doc.Add( chart );
            IntegrationHelpers.SaveToTemp( saveName, doc );
        }

        [Fact( DisplayName = "TemperaturesLineGraphTest" )]
        public void TemperaturesLineGraphTest()
        {
            var doc = new HydraDocument();
            var chart = new LineChart<double, double>();
            chart.ChartTitle = "Temperatures in NY City";

            chart.AddPoint( "Temperatures In NY City", 1, 43 );
            chart.AddPoint( "Temperatures In NY City", 2, 53 );
            chart.AddPoint( "Temperatures In NY City", 3, 50 );
            chart.AddPoint( "Temperatures In NY City", 4, 57 );
            chart.AddPoint( "Temperatures In NY City", 5, 59 );
            chart.AddPoint( "Temperatures In NY City", 6, 69 );

            var chart2 = new LineChart<string, double>();
            chart.ChartTitle = "Temperatures in NY City";
            chart2.AddPoint( "Temperatures In NY City", "Monday", 43 );
            chart2.AddPoint( "Temperatures In NY City", "Tuesday", 53 );
            chart2.AddPoint( "Temperatures In NY City", "Wednesday", 50 );
            chart2.AddPoint( "Temperatures In NY City", "Thursday", 57 );
            chart2.AddPoint( "Temperatures In NY City", "Friday", 59 );
            chart2.AddPoint( "Temperatures In NY City", "Saturday", 69 );
            chart2.AddPoint( "Temperatures In NY City", "Sunday", 51 );
            chart2.LabeledAxis.IncludeDefault = false;

            chart.GetLine( 0 ).Color = "green";
            chart2.GetLine( 0 ).Color = "red";

            doc.Add( chart, chart2, chart.Clone() as LineChart<double, double> );
            IntegrationHelpers.SaveToTemp( "TemperaturesLineGraphTest", doc );
        }
    }
}
