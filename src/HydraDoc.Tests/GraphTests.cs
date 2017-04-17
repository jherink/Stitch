using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydraDoc.Graph;

namespace HydraDoc.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void HorizontalBarChartTest()
        {
            var doc = new HydraDocument();
            var graph = new HorizontalBarChart();
            graph.GraphTitle = "Browser market share June 2015";
            graph.StyleList.Add( "max-width", "700px" );
            graph.AddData( "IE 11: 11.33%", 11.33 );
            graph.AddData( "Chrome: 49.77%", 49.77 );
            graph.AddData( "Firefox: 16.09%", 16.09 );
            graph.AddData( "Safari: 5.41%", 5.41 );
            graph.AddData( "Opera: 1.62%", 1.62 );
            graph.AddData( "Android 4.4: 2%", 2 );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "BarGraphTest", doc );
        }

        [TestMethod]
        public void BarGraphTest()
        {
            var doc = new HydraDocument();
            var graph = new BarGraph();
            graph.GraphTitle = "Quarterly Results";
            graph.GraphWidth = 600;
            graph.FontSize = 10;
            graph.YAxisFormat = "c0";
            graph.PlotYAxis = false;
            graph.PlotBarValues = true;
            graph.BarColors.Add( "#39cccc" );
            graph.BarColors.Add( "#7fdbff" );
            graph.AddData( "Q1", "c", 18450, 16500 );
            graph.AddData( "Q2", "c", 34340.72, 32340.72 );
            graph.AddData( "Q3", "c", 43145.52, 32225.52 );
            graph.AddData( "Q4", "c", 18415, 32425 );
            doc.Add( graph );
            doc.Add( graph.CreateTicks() );

            IntegrationHelpers.SaveToTemp( "BarGraphTest", doc );
        }

        [TestMethod]
        public void PieGraphTest()
        {
            var doc = new HydraDocument();
            var graph = new PieChart(400,400);
            graph.AddData( " #F15854", "Jump right in, startupville here I come.", 4 );
            graph.AddData( "#4D4D4D", "Email back to discuss, flattered and positive.", 4 );
            graph.AddData( "#B276B2", "Respond and say \"Thanks but no thanks.\"", 6 );
            graph.AddData( "#5DA5DA", "	Email back to discuss, all business.", 9 );
            graph.AddData( "#DECF3F", "Email back to discuss, but skeptically.", 31 );
            graph.AddData( "#FAA43A", "Delete the email.", 46 );
            graph.StyleList.Add( "transform", "rotate(-90deg)" );
            graph.StyleList.Add( "float", "left" );
            doc.Add( graph );
            var legend = graph.CreateLegend();
            legend.StyleList.Add( "float", "left" );
            legend.StyleList.Add( "padding-top", "20px" );
            doc.Add( legend );

            IntegrationHelpers.SaveToTemp( "PieGraphTest", doc );
        }

        [TestMethod]
        public void PieGraph2Test()
        {
            var doc = new HydraDocument();
            var graph = new PieChart();
            graph.GraphWidth = 400;
            graph.AddData( "#468966", "Asia", 113 );
            graph.AddData( "#FFF0A5", "NA", 100 );
            graph.AddData( "#FFB03B", "Europe", 50 );
            graph.AddData( "#B64926", "Africa", 28 );
            graph.AddData( "#8E2800", "Australia", 27 );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "PieGraph2Test", doc );
        }

        [TestMethod]
        public void LineGraphTest()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.Start = 0;
            graph.AddData( "Temperatures In NY City", 43, "1" );
            graph.AddData( "Temperatures In NY City", 53, "2" );
            graph.AddData( "Temperatures In NY City", 50, "3" );
            graph.AddData( "Temperatures In NY City", 57, "4" );
            graph.AddData( "Temperatures In NY City", 59, "5" );
            graph.AddData( "Temperatures In NY City", 69, "6" );
            graph.SetLineColor( "Temperatures In NY City", "#0074d9" );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraphTest", doc );
        }

        [TestMethod]
        public void LineGraph2Test()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.AddData( "line", 120, "0" );
            graph.AddData( "line", 60, "1" );
            graph.AddData( "line", 80, "2" );
            graph.AddData( "line", 20, "3" );
            graph.AddData( "line", 80, "4" );
            graph.AddData( "line", 80, "5" );
            graph.AddData( "line", 60, "6" );
            graph.AddData( "line", 100, "7" );
            graph.AddData( "line", 90, "8" );
            graph.AddData( "line", 80, "9" );
            graph.AddData( "line", 110, "10" );
            graph.AddData( "line", 10, "11" );
            graph.AddData( "line", 70, "12" );
            graph.AddData( "line", 100, "13" );
            graph.AddData( "line", 100, "14" );
            graph.AddData( "line", 40, "15" );
            graph.AddData( "line", 0, "16" );
            graph.AddData( "line", 100, "17" );
            graph.AddData( "line", 100, "18" );
            graph.AddData( "line", 120, "19" );
            graph.AddData( "line", 60, "20" );
            graph.AddData( "line", 70, "21" );
            graph.AddData( "line", 80, "22" );
            graph.SetLineColor( "line", "#0074d9" );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraph2Test", doc );
        }


        [TestMethod]
        public void LineGraph3Test()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.AddData( "line", 24000, "2001" );
            graph.AddData( "line", 22500, "2002" );
            graph.AddData( "line", 19700, "2003" );
            graph.AddData( "line", 17500, "2004" );
            graph.AddData( "line", 14500, "2005" );
            graph.AddData( "line", 10000, "2006" );
            graph.AddData( "line", 5800, "2007" );           
            graph.SetLineColor( "line", "green" );
           
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraph3Test", doc );
        }

        [TestMethod]
        public void LineGraph4Test()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.Start = 0;
            graph.AddData( "Temperatures In NY City", 10, "1" );
            graph.AddData( "Temperatures In NY City", 22, "2" );
            graph.AddData( "Temperatures In NY City", 33, "3" );
            graph.AddData( "Temperatures In NY City", 14, "4" );
            graph.AddData( "Temperatures In NY City", 42, "5" );
            graph.AddData( "Temperatures In NY City", 29, "6" );

            graph.AddData( "Temperatures In Omaha", 43, "1" );
            graph.AddData( "Temperatures In Omaha", 53, "2" );
            graph.AddData( "Temperatures In Omaha", 50, "3" );
            graph.AddData( "Temperatures In Omaha", 57, "4" );
            graph.AddData( "Temperatures In Omaha", 59, "5" );
            graph.AddData( "Temperatures In Omaha", 69, "6" );

            graph.SetLineColor( "Temperatures In NY City", "red" );
            graph.SetLineColor( "Temperatures In Omaha", "blue" );
            graph.SetLineThickness( "Temperatures In Omaha", 5 );
            graph.SetLineThickness( "Temperatures In NY City", 5 );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraph4Test", doc );
        }

        [TestMethod]
        public void ScatterPlotTest()
        {
            var doc = new HydraDocument();
            var graph = new ScatterPlot();

            graph.AddData( 2008, 7 );
            graph.AddData( 2009, 9 );
            graph.AddData( 2010, 7.5 );
            graph.AddData( 2011, 6 );
            graph.AddData( 2012, 12 );

            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "ScatterPlotTest", doc );
        }
    }
}
