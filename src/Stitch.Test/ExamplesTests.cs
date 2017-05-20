using Stitch.Chart;
using Stitch.Elements;
using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ExamplesTests
    {
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void NorthwindReportTestOutput()
        {
            var report = new NorthwindReport();
            IntegrationHelpers.ExportPdfToTemp( "NorthwindReportTestOutput", report.Report );
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void RenderThemesTest()
        {
            var table = IntegrationHelpers.GetSampleTableData();
            var tmp = System.IO.Path.Combine( IntegrationHelpers.EnsuredTempDirectory(), "Themes" );
            var pieData = IntegrationHelpers.GetCategorySalesSummary();
            if (!System.IO.Directory.Exists( tmp )) System.IO.Directory.CreateDirectory( tmp );

            foreach (Theme theme in Enum.GetValues( typeof( Theme ) ))
            {
                var doc = new StitchDocument();
                doc.SetTheme( theme );
                var b = new Paragraph();
                b.ClassList.Add( "stitch-text-theme" );
                b.StyleList.Add( "display", "none" );
                doc.Add( b );

                var div = new Div();
                var p = new Bold( theme.ToString() );
                p.ClassList.Add( "stitch-text-theme" );
                div.Add( new Heading( HeadingLevel.H2, $"Default Theme: {p.Render()}" ) );
                div.Add( new HorizontalRule() );
                doc.Add( div );

                doc.Add( new Table( table ) );

                // Bar chart
                var chart = new BarChart();
                chart.ChartTitle = "Bar Chart";
                chart.TitleTextStyle.Bold = true;
                chart.Width = 600;
                chart.MeasuredAxis.Format = "C0";
                chart.AddBar( "Q1", 18450 );
                chart.AddBar( "Q2", 34340.72 );
                chart.AddBar( "Q3", 43145.52 );
                chart.AddBar( "Q4", 18415 );
                doc.Add( chart );

                var pieChart = new PieChart();
                //chart.LegendPosition = LegendPosition.Left;
                pieChart.ChartTitle = "Pie Chart";
                foreach (DataRow row in pieData.Rows)
                {
                    pieChart.AddSlice( row[0].ToString(), double.Parse( row[1].ToString().Remove( 0, 1 ) ) );
                }
                doc.Add( pieChart );

                // Pie Chart
                //var pieChart = new PieChart();
                //pieChart.ChartTitle = "Pie Chart";
                //pieChart.TitleTextStyle.Bold = true;
                //pieChart.AddSlice( "Work", 11 );
                //pieChart.AddSlice( "Eat", 2 );
                //pieChart.AddSlice( "Commute", 2 );
                //pieChart.AddSlice( "Watch TV", 2 );
                //pieChart.AddSlice( "Sleep", 7 );
                //doc.Add( pieChart );

                //// Donut Chart
                //pieChart = pieChart.Clone() as PieChart;
                //pieChart.ChartTitle = "Donut Chart";
                //pieChart.PieHole = .4; // just add this
                //doc.Add( pieChart );

                IntegrationHelpers.ExportPdfToTemp( $"Themes\\{theme}", doc, true );
            }
        }

    }

    public class NorthwindReport
    {
        public readonly StitchDocument Report = new StitchDocument();

        public NorthwindReport()
        {
            Report.AddStyleRule( "h3 { font-weight: bold }" );
            Report.AddStyleRule( "h4 { font-weight: bold }" );
            Report.AddStyleRule( ".employee-photo { max-height: 150px; max-width: 150px; }" );
            //Report.AddStyleRule( ".northwind-logo { background: url('data:image/png;base64,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'); }" );
            //Report.AddStyleRule( ".northwind-logo { height:100px; width: 100px; }" );

            Report.Add( CreateYearlySalesReportPage() );

            Report.Add( CreateTenMostExpensiveProductsPage() );

            Report.Add( CreateYearSummarySales() );

            Report.Add( CreateCategorySalesSummaryPage() );

            Report.Add( CreateEmployeeSummaryPage() );
        }

        private IElement CreateYearlySalesReportPage()
        {
            var div = new Div();


            Report.InsertPageBreak( div );
            return div;
        }

        private IElement CreateTenMostExpensiveProductsPage()
        {
            var div = new Div();
            div.Add( CreatePageHeader( "Top Ten Most Expensive Products" ) );

            var data = IntegrationHelpers.GetTenMostExpensiveProducts();
            var table = new Table( data );
            div.Add( table );
            div.Add( new Paragraph( $"There are {IntegrationHelpers.TotalProductCount()} total products" ) );
            var discontinuedDisclaimer = new Paragraph( "*** These do not include discontinued products" );
            div.Add( discontinuedDisclaimer );

            Report.InsertPageBreak( div );
            return div;
        }

        private IElement CreateYearSummarySales()
        {
            var div = new Div();
            div.Add( CreatePageHeader( "Northwind Sales" ) );

            var data = IntegrationHelpers.GetYearSummarySales();
            var chart = new BarChart( 900, 400 );
            chart.ChartTitle = "Northwind Total Annual Sales";
            chart.TitleTextStyle.Bold = true;
            chart.MeasuredAxis.AxisTitle = "Total Sales";
            chart.LabeledAxis.AxisTitle = "Year";
            chart.MeasuredAxis.Format = "C0";
            chart.AxisOrientation = Orientation.Vertical;
            foreach (DataRow row in data.Rows)
            {
                chart.AddBar( row["year"].ToString(), double.Parse( row["total"].ToString().Remove( 0, 1 ) ) );
            }
            chart.StyleList.Add( "margin-bottom", "30px" );
            div.Add( chart );
            div.Add( new HorizontalRule() );
            div.Add( new Heading( HeadingLevel.H4, "Annual Sales" ) );
            var statementAboutSales = new Paragraph( "Donec sed odio dui. Cras justo odio, dapibus ac facilisis in, egestas eget quam. Vestibulum id ligula porta felis euismod semper. Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus." );
            div.Add( statementAboutSales );

            var table = new Table( data );
            div.Add( table );

            Report.InsertPageBreak( div );
            return div;
        }

        private IElement CreateCategorySalesSummaryPage()
        {
            var div = new Div();
            div.Add( CreatePageHeader( "Category Sales Summary" ) );

            var data = IntegrationHelpers.GetCategorySalesSummary();
            var chart = new PieChart( 500, 750 );
            //chart.LegendPosition = LegendPosition.Left;
            chart.ChartTitle = "Total Sales By Category";
            chart.PieSliceText = PieSliceText.Percentage;
            chart.TitleTextStyle.Bold = true;
            foreach (DataRow row in data.Rows)
            {
                chart.AddSlice( row[0].ToString(), double.Parse( row[1].ToString().Remove( 0, 1 ) ) );
            }
            div.Add( chart );
            div.Add( new HorizontalRule() );
            div.Add( new Heading( HeadingLevel.H4, "Category Total Sales" ) );
            var statementAboutSales = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum.";
            div.Add( new Paragraph( statementAboutSales ) );

            div.Add( new Table( data ) );

            Report.InsertPageBreak( div );
            return div;
        }

        private IElement CreateEmployeeSummaryPage()
        {
            var div = new Div();
            div.Add( CreatePageHeader( "Active Employees" ) );
            var data = IntegrationHelpers.GetEmployeeData();
            var ul = new UnorderedList();

            data.Columns[0].ColumnName = "Employee Id";
            data.Columns[1].ColumnName = "First Name";
            data.Columns[2].ColumnName = "Last Name";
            data.Columns[4].ColumnName = "Employee Photo";

            var table = new Table( data );

            foreach (var body in table.TableBodies)
            {
                var i = 0;
                foreach (var row in body.Children)
                {
                    var iRow = row as ITableRowElement;
                    var cell = iRow.Children.ElementAt( 4 ) as ITableCellElement;
                    var image = new ImageElement();
                    var photo = data.Rows[i++]["Employee Photo"] as byte[];
                    var base64Photo = Convert.ToBase64String( photo, 78, photo.Length - 78 );
                    image.Src = new Uri( $"data:image/png;base64, {base64Photo}" );
                    image.StyleList.Add( "max-height", "100px" );
                    image.StyleList.Add( "max-width", "100px" );

                    cell.Content = new DOMString( image );
                }
            }

            div.Add( table );
            //foreach (DataRow row in data.Rows)
            //{
            //    var li = new ListItemElement();
            //    li.ClassList.Add( "w3-row" );
            //    var fNColumn = new Div();
            //    fNColumn.StyleList.Add( "float", "left" );
            //    fNColumn.StyleList.Add( "padding-right", "10px" );

            //    var lNColumn = fNColumn.Clone() as IDivElement;

            //    var photo = row["photo"] as byte[];               
            //    var base64Photo = Convert.ToBase64String( photo, 78, photo.Length - 78 );
            //    var image = new ImageElement();
            //    image.Src = new Uri( $"data:image/png;base64, {base64Photo}" );
            //    image.ClassList.Add( "employee-photo" );
            //    li.Children.Add( image );

            //    var span = new Span();
            //    span.Children.Add( new PlainText( row["FirstName"] as string ) );
            //    fNColumn.Children.Add( span );
            //    span = new Span();
            //    span.Children.Add( new PlainText( row["LastName"] as string ) );
            //    lNColumn.Children.Add( span );
            //    li.Children.Add( fNColumn );
            //    li.Children.Add( lNColumn );
            //    //var li = new ListItemElement();
            //    ul.Children.Add( li );
            //}
            //div.Add( ul );

            return Report.InsertPageBreak( div );
        }

        private IElement CreatePageHeader( string heading )
        {
            var header = new Div();

            var row = new Div();
            row.ClassList.Add( "w3-row" );
            var leftColumn = new Div();
            leftColumn.StyleList.Add( "float", "left" );
            var logo = new ImageElement();
            //logo.ClassList.Add( "northwind-logo" );
            logo.Src = new Uri( "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAJ0AAACJCAYAAAA2YTpbAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADdYAAA3WAZBveZwAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTM0A1t6AAASLUlEQVR4Xu3dCZgcZZ3H8QpnQgQUkdt4sCuLwGZR8MAD8AE1HMt6sK6ycgoKWQ4VBEWgZzIhQRJUkBVQbhcXXNmFB1G8QHDFdSMCD8eCSIQkM5lcRuRIQsjs99+pd3in5tc91T1d3VXV7/Pk82T6/75V9db7/rv67a6u6mhoaCgI2koGgyBLMhgEWZLBIMiSDAZBlmQwCLIkg0GQJRkMgizJYBBkSQaDIEsyGARZksEgyJIMBkGWZDAIsiSDQZAlGQyCLMlgEGRJBoP1BnqjHTELF6lyZ+K7Tngl7sXh2FDVCV4mg92OJNsT12MNhnCNqufESTcU+yM+iy1U3SAk3TASawIOxZ1xovkaSTrnz5iL16llupkMdhMSajOciMfiBFOaSTpnLW7E29Wy3UgGuwGJtD1mYnmcWPWMJ+l8/42PoKvnfTJYZiTQVFyL1XFCpdGqpHOexKnYXK2v7GSwbEgam68djJ/FSdSoViedsxIX4rVqvWUlg2VBskzCp/FonDzNyirpnBfxXeyt1l82Mlh0JMl2mIGlcdKMV9ZJ57sHH8IGaltlIINFRXLsgavRyHwtjXYmnfMETsZktc0ik8EiISFsvjYNP4kTJAudSDrnT7gAO6ltF5EMFgGJMBHH4+E4MbLUyaRzbN73b3irakORyGCekQDboAdL4oRohzwkne8XOAyFnPfJYB4x8Lvh21gVJ0I75S3pnMcxHZupduWVDOYJA/5+/MhLgE7Ia9I5KzALO6j25Y0MdhqDvCmOxUPxoHda3pPOWYPrsKdqZ17IYKcwuK/BuRiMBzsvipJ0vp/jEExQbe4kGWw3BnVXXIEX4kHOmyImnfMYPoPczPtksF0YzANwO9bFg5tXRU46Zxn6sL3ah3aSwSwxgJvgaDwYD2gRlCHpnNW4BlPVvrSDDGaBgdsaX8ZAPJBFUqak8/0UB6Gt8z4ZbCUGbBdchufjASyisiad8yhOwCS1f60mg63AQL0PtyHv87U0yp50zlL0Ylu1n60ig81icDbEkbg/Hqyy6Jakc1bhKrxZ7e94yeB4MED7wd6RqsErqm5LuuWwI942an/HSwZbgYHaHXYtgrt2tMi6Jenm4xRk+h0+GWwlBmwnzMEz8QAWUdmT7j58HG25Sk0G09hnn30OQeprORm4LXEm+uOBLJKyJt0dOEDtk8J4b4yjMEWVpyWDabDh2RjCL2AJmOqzHgbQPhy2k/mPxANaBGVKOvsy6HeQ+sNhxnYLnI6FsDH/O1UvLRlMgw27pHMexjHYRNVPYiDdbRzuiQc2z8qQdH/BV5H6KMVY7oCv4M/wxzo3SecswpnYUi2nMKjvwM14KR7kvCly0i3G2XiVarvC2O2Gq7Eaaoxzl3TOM5iD1BeUMLhvwuXI27dNiph09u0SO8swUbVZYaz2xW1YBzWmTm6TzlmDa7GHWo/CIG+LPqyIB73TipR0v0Lq62YZlw3wUfwGavyU3Ced74fYX61PYbAn41T8MR78Tsl70q3DLXi3ap/COEzCSXgCaqzqKVTSOfPwMaT6XIhB3wifwO/iJGi3C1S7HAZ7Muw6BZUQWbKvKV2JXVW7FPp8a5yHpVBjk0Yhk855Ev+C1N9qJQEORJYXVjv2RYVbkerowcBb4tmdmJ6CSpBWshvvzEbqL2TSx2/EpXgeaiwaUeikc5ahF69R21JIBrtF6w1YC5U0zbJbUlyJ1EcPH4mwEY7A/VAJMx4L8HmkvsUYfbo3bsJaqL5vRimSznkB38RfqW0qJMfr8XU8C5VEaa3EbLTs69wkx/thX5RUCdSIB3EkNlbbSaL/JuAg3AnVz+NVqqRzXsJ/oJHTbFvBvpnc6JVkC3E6MrsxNcnyFtitwOxWsCqparEruqapdSr0lztN9RBUv7ZKKZPOdzcaOc1m9zixe9I9DpVkjl1TexRSHT1agQR6Ay7Bc1BJZiwxb8Jeah0KfWOnqc6AO02VtdInnfMIjkXa02wb4CP4Nfxkuwt2V86OXQ9KQr0a52IJXLI9j0vxRrWMQl/sCHWaKmtdk3ROPxo9zfZeXIy3qfJOIcEm4SSch61VHYV9t9NU18A+eFd9lLWuSzrHTrPNRWnu2zYW9nU//ABjnabKWtcmnWPP9uuQ+jRbkbBfG+JwNHKaKmtdn3Q+O832PtXeomE/3GmqP0DtayeFpBMaOs2WJ7TZTlNVMJ7TVFkLSVeHnWY7Gbm/aSBt3BmtOk2VtZB0KSyHnWbL5JK68aBN7jSVfSCu2p5HIekaYKfZLsNfq31qF7Zvp6kOxl1Q7cy7kHRNsKPK9/EOtW9ZYXub4GhkfZoqayHpxslOsx2KzM5QsG53msquH1FtKJqQdC3S0Gm2NFiXnaa6EO0+TZW1kHQtZqfZzsIr1X6nwbK7o5OnqbIWki4j7jRb6p/FpG5eTlNlLSRdxuxodYrqAx917E4HavkyCknXBpepPvBRZ2VimTILSdcGIelGCknXBiHpRgpJ1wYh6UYKSdcGIelGCknXBiHpRgpJ1wYh6UbqWNKdk2hImYWke5ndKaCpux84MpgWGz8Q9hXxsn8CH5Ju/Rmar+ENav8bIYONoiFvxrdg31dTDS66bk66p2H3G059yedYZLBZNGwb2G2oBqF2oKi6Mens6rN/wkZqf8dDBseLhk7Ep2A3v1Y7VDTdknT25dabkfrmis2QwVah8fa17A/ix1A7WRRlT7pncQl2VvvWajKYBXZoD1yFVVA7nmdlTTq74Y7doqPp7w42QwazxA5uC7syK8/XdSaVLel+iyPQtjtW+WSwHdhhu4L9BDwK1TF5Uoaks/naLdhXtb+dZLCd6AR318ifQnVWHhQ56Z6DXcTd0csufTLYKXTMVNi1BbV+qaVTiph0duXZF7GVam8nyWCn0VHbYybsBtiqQ9utSEn3O3wSHZmvpSGDeUHHbYYT8RhUB7dL3pPOTkPazysV4o5VMpg3dKbN++yC6KzuFj6WvCad3WzHbpOxi2pTXslgntHBe+J6tPOa0rwl3QC+jFertuSdDBYBHW5Xz8/CCqiBaaW8JN0DsHuhtOwuBJ0gg0XCAEzGdPweaqBaoZNJZ/O125H6Z8/zTgaLiEGxn5A8DFlc9NyJpLOviV2BcX1hMo9ksOgYqL1wA16EGtBGtTPp7Gth5yL176QVjQyWBQO3E+zHPf4ENcBptSPp7J51x2FTtf4ykcGyYSBfgVPQ7J3Ks0y6O/ABtc6yksGyYnBt3vdh/BIqAWppddLZ17uuxO5qXWUng92AAX8b/h1p5n2tSrol6EHubrjdTjLYTUiAKZiDenfLHG/S2V0+j8dEtWy3kcFuREJsjtMwH8mkaTbpfoJp6NgvLuaRDHYzEsT9Fte9cMnTSNLZ17Kuxt+qekFIurpInHfie/iGKvdR5wn0YTtVHrxMBkeLJvT3Ru/RZZ5KtEF/X/TugRnRflW90b7zK1FbL/rIAok05mdnqs7SC6LNrQ+G+wNLK9EOyXpmcV8kf9NixexoS3/5gb58/WZtM2RQWdwbLcSYv6m/vBJtQUffhsWDlSj1rzSXlfXBQE/0P/SH/ar2BYTk/I6y+xZWIvnbtfT7sbb84p6or78Spf4x4rySQYWdfhor6IAxf1e1vyf6JHWvVmU+6uzGs/ejHEU/Q4ceVi9JF18YTabD/8a3aEa0y8D5Uc3TRaz/SDvy9s+Idk0u6yzqjfa0NuAgW2ZwRrSzX66O1MSn+HXqtcHYOmnLOrZxXp1yS8ovqPJq0vVE/+se//7iaFN/+2ZxJRr+GMbaQ2yKe1zL4MxoW38dbl8X9EU7+nGfjZGNRXJdjZBBhcR4nE5ZhAHrJFXHoZM+Qb0rVZkhwT7OAPyWOt+kM4+yl25LPB5/F79aPCMa/Q3YSrQR2z2A8rUscw8qLDMTPya2DHPtpchfhthvbKBpz1kOsQFbPhGby3q+ZctUE8AdmWZE0xmIUR9z2KCw3M3VOrRjSSV6RbJOEvV/Sf37VBnbOYOyF/j/AVneG32PsjOHYzdFG/Jk2os225hYG47x21A9unKQ4Mj5puFlBEse2vWleB2HP1yJql+ZIrm2ZnvXVeO90SW2j8M4WuNW/r6X7c8a6wmnyKBiO2jzDjb4JOZbx6t6hh2pkXTRhHhnHh7sjaaOLq8OuiXWAo5OZ6tyyh60nfVj1hbi/Wz3Dj8+2BMd6D82tjzmjoozX3J/s55TqLPCL0/iiXModYZs3qbKk1j/iVafZHndqLL1A/g5K+eINfIsBU824ivtqD4iDtZ5DmXzk/GnZkWvsnXBpjh130XH06EFyXh1bs46aiWuHW3pg4uoMygPEnXIoGJJt6gvem38UtCPR+wZoerWSjqbkxB/VnW8j5e8d1Fvna0nWUY77k8mXTXeE53PMkNjHYWpI5POx2BOp84yVeaQKAfb9tIc5Uz16NEbvcgAnerHbR5H/Nc2iPy/kvXO9svZ1/cQf8SPOazri5Q9Oars8mhj4qvwf7ApUc25ePxm5+lknIPCO4nXTDqHOnbkW1bvIJQkg4pLOvvbno1saDnm2TMlWVclXXw0WsOAft6P12LL42l7pvvxWklnc0LqD9XrYEOdjiSdob69wbrTj5E4J7O9M+xvju7fpvwp/hx+s2FPJuMe+8ZIumdsnmf9xd9/ob37j6qH8Sad7T/1+ln/HFWuyKDiJ52xt+62Y7hrwUXRJL+uSjo69kzbiSUzolQX/dKhf2/12ZlpfrxW0lHvYmvPWC931OlY0lXnssxJF54fDV/bwOO73BsotmsfM9lL8Hu98vuwt3vsGyvp7G97c0Bb7+XxC9bmZN3xJp2hns3FF6kyRQaVZNIZdmL/6s7wDLYddfEaSXcVsdXJI1ct8cv4ENv9nB9PJp29eeAI8WHqrqA9x/h1FeqlTbrlNmC1UP6PaCjpeIndjGWexdH22I5EtHn4Xal9zknZApLpCnvMurezx/wpP2ZJk3QmPhr9HGtI/I/5dVuSdD1Rr9VNHnxqkUFFJZ1hJ2xC/SIDf4N1WjUmko7lv05spR+rJ54D2WT43BHxOOmqRwUGrFqHNyfWSX69WqifNulWsW+X1UL57bbtRpLOkCjfof3/ZX/zsnk8ffUlv5x12hxphb2T5P+j2ceaZ0PSJp3hiTKRmL28v0T7j3PxFh3pvmB1k58e1CKDSq2kM3GS2c5Uz1HKI511IA2zz4b8eC32EmP12e4/j4h7R7q4w+7GD20i7terhbode3k1LDeN5Z6Pj3o/Sg6qvdu09fIk/gf+v9HezfvlvkaSzovfiHX04WkWa0XS2ZFZraMWGVTqJZ2h3D5nsznYbJV08ZsPKx81r1AYePvsarW9xPhxP+lM9SWKt+24hYdjfpuDeh1NuvgjkCX0kZ1lmKfqEH+Q8v/k/z/Um440nHTGXsLXT3WqryItOtLNw62qTJFBZaykM+xM9c0CA2IT1xFJZ+wlmPhD1inJMp9NtKm3jI6/PFmWTDrjEoD/K35coV5nkw4s+w2WXYoRU4fh8rgf2f/rVbnTVNJVVc+l23THTq3ZVKHppIvn02s4cr9FlSsyqKRJOmMJUd0Z5i7JsngC/hRzma/Zp+rJcmMvO3S2fdo/z/4eVS6SzrDMV6vb5SibLPNRp+NJ5w2o/BEQ62fK15EQIyb9Sc0n3Xqs3z43tSPeYLIsTdIxDm+lvxdaO1R5LTKYZC9xNGC1TXxVeRID8q/U/4Eqszkdg3oNde6xMxw2wbW4HeZ51nyQ5R6zcvX5n707otw+H/xZsox/E1jnHMpW0RGfUnO8+NP3lbjb6ifLnfjZv6bek4zy0zHUyDPcx7KP1nvppHz4o5RaqPN9PJecghDbDWvTzJ/jo+rqZJw+rH7JgD7df6ASvd5hjHa1oxt9dBHLPlBvzlmLDPrs7AEZfRobr7CRM+wEuao3UvXwfYIuW88Gi8Z/lh27FNeykzezzEzbKVXf3hmxo8dV2wFLUMKjEsdOF1H2adZ5kv8yvqASbWUxtzzPzkMIj1qeJ9YH2M/z4jqnLp0ZbZ+sQ1vfTp1zqnV6o7PGOguisHz1Cwa1sO6ac197BaCdR7h9sX50baBsijde01X7k6h3onvyG45yU1nn2W79I5CkbPtD1XGq8Wo1FhkMgizJYBBkSQaDIEsyGARZksEgyJIMBkGWZDAIsiSDQZAlGQyCLMlgEGRJBoMgSzIYBFmSwSDIkgwGQZZkMAiyMxT9P5Jv0/677uz8AAAAAElFTkSuQmCC" );
            leftColumn.Add( logo );

            var rightColumn = new Div();
            rightColumn.StyleList.Add( "float", "right" );
            rightColumn.Add( new Paragraph( DateTime.Now.ToString( "MMM d, yyyy" ) ) );

            row.Add( leftColumn, rightColumn );

            header.Add( row );

            row = row.Clone() as Div;
            row.Children.Clear();
            row.Add( new Heading( HeadingLevel.H3, heading ) );
            header.Add( row, new HorizontalRule() );

            return header;
        }
    }


}
