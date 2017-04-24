using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydraDoc.Elements;
using HydraDoc.Chart;
using System.Data;

namespace HydraDoc.Tests
{
    [TestClass]
    public class ExportTest
    {
        [TestMethod]
        public void ExportTest01()
        {
            var doc = new HydraDocument();
            var container = doc.AddBodyContainer();
            container.Children.Add( new Heading( HeadingLevel.H3, "Total Beverage Amounts" ) );
            container.Children.Add( new HorizontalRule() );

            var salesData = IntegrationHelpers.GetSalesByCategory( "Beverages" );
            var chart = new PieChart(850, 400);
            chart.LegendPosition = LegendPosition.Right;
            foreach (DataRow row in salesData.Rows)
            {
                chart.AddSlice( row["Product Name"] as string, double.Parse(row["Total Purchase"].ToString()) );
            }
            container.Children.Add( chart );
            container.Children.Add( ElementFactory.CreateTable( salesData ) );

            IntegrationHelpers.ExportPdfToTemp( "ExportTest01", doc );
        }
    }
}
