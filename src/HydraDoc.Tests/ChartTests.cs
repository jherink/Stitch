using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydraDoc.Chart;

namespace HydraDoc.Tests
{
    [TestClass]
    public class ChartTests
    {
        [TestMethod]
        public void Simple2DPieTest()
        {
            var doc = new HydraDocument();
            var graph = new PieChart( 400, 400 );
            graph.AddSlice( "Work", 11 );
            graph.AddSlice( "Eat", 2 );
            graph.AddSlice( "Commute", 2 );
            graph.AddSlice( "Watch TV", 2 );
            graph.AddSlice( "Sleep", 7 );
            //graph.StyleList.Add( "transform", "rotate(-90deg)" );
            //graph.StyleList.Add( "float", "left" );
            doc.Add( graph );
            //var legend = graph.CreateLegend();
            //legend.StyleList.Add( "float", "left" );
            //legend.StyleList.Add( "padding-top", "20px" );
            //doc.Add( legend );

            IntegrationHelpers.SaveToTemp( "PieChartTest", doc );
        }
    }
}
