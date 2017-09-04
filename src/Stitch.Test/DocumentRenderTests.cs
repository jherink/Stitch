using System;
using Xunit;
using Stitch.Elements;
using System.Collections.Generic;
using Stitch.Elements.Interface;
using System.Linq;
using Stitch.Chart;
using System.Data;
using Stitch.Widgets;

namespace Stitch.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class DocumentRenderTests
    {
        [Fact( DisplayName = "LoadwkhtmlResource" )]
        public void LoadwkhtmlResource()
        {
            Assert.NotNull( new Export.wkhtmltopdfWrapper() );
        }

        [Fact( DisplayName = "HelloWorldTest" )]
        public void HelloWorldTest()
        {
            var doc = new StitchDocument();
            doc.Head.Title = "Hello World";
            var p = new Paragraph( "Hello" );
            p.Content += new Big( "World" );
            p.Content += new Bold( "!!!" );
            doc.Add( p );
            IntegrationHelpers.SaveToTemp( "helloworld.html", doc );
        }

        [Fact( DisplayName = "HelloWorldInColor" )]
        public void HelloWorldInColor()
        {
            var doc = new StitchDocument();
            doc.Head.Title = "Hello World";
            var div = new Div();
            div.ClassList.Add( "w3-theme" );
            div.Children.Add( new Paragraph( "Hello World" ) );

            var p = new Paragraph( " You too!" );
            p.ClassList.Add( "w3-theme-d1" );
            div.Children.Add( p );

            doc.Add( div );
            IntegrationHelpers.SaveToTemp( "helloworldincolor.html", doc );
        }

        [Fact( DisplayName = "CreateTableTest" )]
        public void CreateTableTest()
        {
            var doc = new StitchDocument();
            doc.Head.Title = "Table Test";

            var div = new Div( true );
            var table = new Table( IntegrationHelpers.GetSampleTableData() );
            table.ClassList.Add( "w3-table-all" );
            table.TableHead.Rows.First().ClassList.Add( "w3-green" );
            foreach (var row in table.TableBodies.First().Rows)
            {
                row.ClassList.Add( "w3-hover-green" );
            }

            div.Children.Add( table );

            doc.Add( div );

            IntegrationHelpers.SaveToTemp( "tabletest1.html", doc );
        }

        [Theory( DisplayName = "ListTests" )]
        [MemberData( nameof( ListData ) )]
        public void ListTests( params object[] list )
        {
            var doc = new StitchDocument();
            doc.Head.Title = "Unordered List Test";

            var div = new Div( true );
            //div.Children.Add( ElementFactory.CreateUnorderedList( list ) );
            div.Children.Add( new UnorderedList( list ) );

            //div.Children.Add( ElementFactory.CreateHorizontalRule( false ) );
            div.Children.Add( new HorizontalRule() );
            //div.Children.Add( ElementFactory.CreateOrderedList( list ) );
            div.Children.Add( new OrderedList( list ) );

            //div.Children.Add( ElementFactory.CreateHorizontalRule( false ) );
            div.Children.Add( new HorizontalRule() );
            //div.Children.Add( ElementFactory.CreateOrderedList( list, OrderedListStyleType.UppercaseRomanNumeral ) );
            div.Children.Add( new OrderedList( list, OrderedListStyleType.UppercaseRomanNumeral ) );

            //div.Children.Add( ElementFactory.CreateHorizontalRule( false ) );
            div.Children.Add( new HorizontalRule() );
            //div.Children.Add( ElementFactory.CreateUnorderedList( list, UnorderedListStyleType.Circle ) );
            div.Children.Add( new UnorderedList( list, UnorderedListStyleType.Circle ) );

            doc.Add( div );

            IntegrationHelpers.SaveToTemp( "unorderedListTest1.html", doc );
        }

        [Fact( DisplayName = "HeaderTests" )]
        public void HeaderTests()
        {
            var doc = new StitchDocument();
            doc.Head.Title = "Header Tests";

            var container = doc[0];
            // This was testing that Element factory and constructor yielded the
            // same. Now they are identical now that element factory was removed.
            container.Children.Add( new Heading( HeadingLevel.H1, "Heading Level 1" ) );
            container.Children.Add( new Heading( HeadingLevel.H2, "Heading Level 2" ) );
            container.Children.Add( new Heading( HeadingLevel.H3, "Heading Level 3" ) );
            container.Children.Add( new Heading( HeadingLevel.H4, "Heading Level 4" ) );
            container.Children.Add( new Heading( HeadingLevel.H5, "Heading Level 5" ) );
            container.Children.Add( new Heading( HeadingLevel.H6, "Heading Level 6" ) );

            container.Children.Add( new HorizontalRule() );
            container.Children.Add( new Heading( HeadingLevel.H6, "Heading Level 6" ) );
            container.Children.Add( new Heading( HeadingLevel.H5, "Heading Level 5" ) );
            container.Children.Add( new Heading( HeadingLevel.H4, "Heading Level 4" ) );
            container.Children.Add( new Heading( HeadingLevel.H3, "Heading Level 3" ) );
            container.Children.Add( new Heading( HeadingLevel.H2, "Heading Level 2" ) );
            container.Children.Add( new Heading( HeadingLevel.H1, "Heading Level 1" ) );

            IntegrationHelpers.SaveToTemp( "headingLevelTests.html", doc );
        }

        public static IEnumerable<object[]> ListData
        {
            get
            {


                return new[] {
                    new [] { new object[] { "test", 2, "%$^$", "Hello World" } }
                };
            }
        }

        [Fact( DisplayName = "AddCustomCSSRule" )]
        public void AddCustomCSSRule()
        {
            var doc = new StitchDocument();
            doc.AddStyleRule( "body { background-color:powderblue;" );
            doc.AddStyleRule( ".myCustomParagraph { color: red; }" );

            var container = doc[0];

            var text = new DOMString();
            text.Append( "This is a " );
            text += new Bold( "red" );
            text += " paragraph";
            text.Append( "!!!!" );
            var p = new Paragraph( text );

            p.ClassList.Add( "myCustomParagraph" );

            container.Children.Add( p );

            IntegrationHelpers.SaveToTemp( "CustomCSSRuleTest.html", doc );
        }

        [Fact( DisplayName = "AddAnchorLinkTests" )]
        public void AddAnchorLinkTests()
        {
            var doc = new StitchDocument();
            doc.AddStyleRule( ".bigDiv { min-height: 1600px; !important }" );
            var container = doc[0];

            // create a regular link.
            var link = new AnchorLinkElement()
            {
                Href = "http://www.w3schools.com/html/",
                Name = "Visit W3Schools!",
                ID = "GoBackLink"
            };

            container.Children.Add( new Div( link ) );


            // create div now so we have it's ID
            var linkDiv = new Div( new Paragraph { Content = "There is something here" } );

            // now create an link element that links to link div.
            link = new AnchorLinkElement
            {
                Href = $"#{linkDiv.ID}",
                Name = "Go to div link at the bottom of the page."
            };

            container.Children.Add( link ); // add the link before the space.

            // now create and add a large space in the page.
            var bigSpace = new Div();
            bigSpace.ClassList.Add( "bigDiv" );
            container.Children.Add( bigSpace ); // add a big space

            // now add the linked div at the bottom of the page.
            container.Children.Add( new Div( false, linkDiv ) ); // no add div linking to.

            // now create a link back to the top of the page.
            container.Children.Add( new AnchorLinkElement
            {
                Href = $"#GoBackLink",
                Name = "Go Back To Top"
            } ); // add a link to go back.

            IntegrationHelpers.SaveToTemp( "linkTests.html", doc );
        }

        [Fact( DisplayName = "ImageTests" )]
        public void ImageTests()
        {
            var doc = new StitchDocument();
            var container = doc[0];

            var table = new Table();
            var row = new TableRow();
            var cell = new TableCell();

            var url = "http://www.w3schools.com/images/w3schools_green.jpg";

            cell.Content = new DOMString( new ImageElement( url ) );

            row.Children.Add( cell );
            var body = table.AddBody();
            body.Children.Add( row );

            var rowClone = (ITableRowElement)row.Clone();
            rowClone[0].Content = new DOMString( new ImageElement( url, "second row", 50, 50 ) );
            body.Children.Add( rowClone );

            rowClone = (ITableRowElement)row.Clone();

            // full path
            var fullPath = IntegrationHelpers.CreateLocalResource( "..\\..\\Resources\\Images\\tiger.jpg" );
            rowClone[0].Content = new DOMString( new ImageElement( fullPath ) );
            body.Children.Add( rowClone );

            // relative
            rowClone = (ITableRowElement)row.Clone();
            rowClone[0].Content = new DOMString( new ImageElement( System.IO.Path.GetFileName( fullPath ) ) { ReferenceImage = true } );
            body.Children.Add( rowClone );

            container.Children.Add( table );

            IntegrationHelpers.SaveToTemp( "imageInTableTests.html", doc );
        }

        [Fact( DisplayName = "TableWidthACaptionTests" )]
        public void TableWidthACaptionTests()
        {
            var doc = new StitchDocument();
            var cont = doc[0];

            var dt = new System.Data.DataTable();
            dt.Columns.Add( "Month", typeof( string ) );
            dt.Columns.Add( "Savings", typeof( string ) );

            dt.Rows.Add( "January", "$100" );
            dt.Rows.Add( "Febuary", "$50" );

            var table = new Table( dt );
            table.TableCaption = new TableCaption( "Monthly Savings" );
            cont.Children.Add( table );

            IntegrationHelpers.SaveToTemp( "tablewithcaptiontest", doc );
        }

        [Fact( DisplayName = "CreateComplexSamplePage" )]
        public void CreateComplexSamplePage()
        {
            var doc = new StitchDocument();
            doc.AddStyleRule( ".limitedTable { max-width: 400px; }" );
            doc.AddStyleRule( "table { margin-top: 25px !important, margin-bottom: 25px !important }" );
            doc.AddStyleRule( "body, table, tr, td { font-size: 12 !important }" );
            var container = doc[0];

            container.Children.Add( new Div( new Bold( "DESIGN CRITERIA" ) ) );
            container.Children.Add( new HorizontalRule() );

            //var table = ElementFactory.CreateTable();
            var table = new Table();
            table.ClassList.Remove( "w3-table-all" );
            var row = table.CreateRow( "DESIGN CODE", "AASHTO-2015", "FATIGUE CATEGORY", "1" );
            row = table.CreateRow( "ULTIMATE WIND SPEED (MPH)", 115, "TRUCK GUST", "NO" );
            row = table.CreateRow( "MEAN RECURRENCE INTERVAL", 700, "GALLOPING", "NO" );
            row = table.CreateRow( "SERVICE WIND SPEED (MPH)", 115, "NATURAL WIND GUST", "NO" );
            row = table.CreateRow( "AASHTO ICE INCLUDED ?", "YES" );
            row = table.CreateRow( "ELEVATION OF FOUNDATION", 0.0 );
            row = table.CreateRow( "ABOVE SURROUNDING TERRAIN (FT)" );
            row[0].ColSpan = 4;

            container.Children.Add( new Div( table ) );
            container.Children.Add( new Div( new Bold( "DESIGN SUMMARY - POLE" ) ) );
            container.Children.Add( new HorizontalRule() );

            table = new Table( "HEIGHT(FT)", "SHAFT WEIGHT (LBS)", "GROUND LINE DIAMETER (IN)", "TOP DIAMETER (IN)" );
            table.ClassList.Remove( "w3-table-all" );
            container.Children.Add( new Div( table ) );
            table.CreateRow( 30.0, 414, 9.00, 4.80 );

            container.Children.Add( new Div( new Bold( "SECTION CHARACTERISTICS" ) ) );
            container.Children.Add( new HorizontalRule() );
            table = new Table( string.Empty, "SECTION - 1" );
            table.ClassList.Remove( "w3-table-all" );
            table.ClassList.Add( "limitedTable" );
            table.CreateRow( "SHAPE", "4W FLUTES" );
            table.CreateRow( "TOP DIAMETER(IN)", 4.80 );
            table.CreateRow( "BASE DIAMTER (IN)", 9.0 );
            table.CreateRow( "THICKNESS (IN)", 0.17930 );
            table.CreateRow( "LENGTH (FT)", 30 );
            table.CreateRow( "WEIGHT (LBS)", 414 );
            table.CreateRow( "TAPER (IN/FT)", .14 );
            table.CreateRow( "YIELD STRENGTH(KSI)", 55 );
            table.CreateRow( "MATERIAL", "S105-55" );
            container.Children.Add( new Div( table ) );

            IntegrationHelpers.SaveToTemp( "complexSamplePage", doc );
        }

        [Fact( DisplayName = "W3CSSSampleTest" )]
        public void W3SSSSampleTest()
        {
            var doc = new StitchDocument();
            //var rowDiv = ElementFactory.CreateDiv();
            var rowDiv = new Div();
            rowDiv.ClassList.Add( "w3-row" ).Add( "w3-section" ); // like javascript!

            doc.Head.Styles.Add( new Style( new Uri( "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" ) ) );
            doc.AddStyleRule( ".contactBox { height:250px; }" );
            doc.AddStyleRule( ".contactLine { width: 30px; }" );
            doc.AddStyleRule( "body { max-width:1200px; }" );

            // Contact Info
            //var contactInfo = ElementFactory.CreateDiv();
            var contactInfo = new Div();
            contactInfo.ClassList.Add( "w3-third", "w3-container", "w3-black", "w3-large", "contactBox" );
            contactInfo.Children.Add( new Heading( HeadingLevel.H2, "Contact Info" ) );
            var p = new Paragraph();
            var i = new Italic();
            i.ClassList.Add( "fa", "fa-map-marker", "contactLine" );
            p.Content.AppendElement( i );
            p.Content.AppendElement( new DOMString( " Chicago, US" ) );
            contactInfo.Children.Add( p );

            p = new Paragraph();
            i = new Italic();
            i.ClassList.Add( "fa", "fa-phone", "contactLine" );
            p.Content.AppendElement( i );
            p.Content.AppendElement( new DOMString( " Phone: +00 151515" ) );
            contactInfo.Children.Add( p );

            p = new Paragraph();
            i = new Italic();
            i.ClassList.Add( "fa", "fa-envelope", "contactLine" );
            p.Content.AppendElement( i );
            p.Content.AppendElement( new DOMString( " Email: mail@mail.com" ) );
            contactInfo.Children.Add( p );

            rowDiv.Children.Add( contactInfo );

            // Contact Us
            //var contactUs = ElementFactory.CreateDiv();
            var contactUs = new Div();
            contactUs.ClassList.Add( "w3-third w3-center w3-large w3-dark-grey w3-text-white contactBox" ); // split?
            contactUs.Children.Add( new Heading( HeadingLevel.H2, "Contact Us" ) );
            contactUs.Children.Add( new Paragraph( "If you have an idea." ) );
            contactUs.Children.Add( new Paragraph( "What are you waiting for?" ) );

            rowDiv.Children.Add( contactUs );

            // Like Us
            //var likeUs = ElementFactory.CreateDiv();
            var likeUs = new Div();
            likeUs.ClassList.Add( "w3-third w3-center w3-large w3-grey w3-text-white contactBox" );
            likeUs.Children.Add( new Heading( HeadingLevel.H2, "Like Us" ) );
            i = new Italic();
            i.ClassList.Add( "w3-xlarge fa fa-facebook-official" );
            likeUs.Children.Add( i );
            likeUs.Children.Add( new LineBreak() );

            i = i.Clone() as Italic;
            i.ClassList.Remove( "fa-facebook-official" ).Add( "fa-pinterest-p" );
            likeUs.Children.Add( i );
            likeUs.Children.Add( new LineBreak() );

            i = i.Clone() as Italic;
            i.ClassList.Remove( "fa-pinterest-p" ).Add( "fa-twitter" );
            likeUs.Children.Add( i );
            likeUs.Children.Add( new LineBreak() );

            i = i.Clone() as Italic;
            i.ClassList.Remove( "fa-twitter" ).Add( "fa-flickr" );
            likeUs.Children.Add( i );
            likeUs.Children.Add( new LineBreak() );

            i = i.Clone() as Italic;
            i.ClassList.Remove( "fa-flickr" ).Add( "fa-linkedin" );
            likeUs.Children.Add( i );
            likeUs.Children.Add( new LineBreak() );

            rowDiv.Children.Add( likeUs );

            doc.Add( rowDiv );
            doc.Body.ClassList.Add( "w3-content" );

            //IntegrationHelpers.SaveToTemp( "W3CSSSampleTest", doc );
            IntegrationHelpers.ExportPdfToTemp( "W3CSSSampleTest", doc );
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void ExportTest01()
        {
            var doc = new StitchDocument();
            var container = doc[0];
            container.Children.Add( new Heading( HeadingLevel.H3, "Total Beverage Amounts" ) );
            container.Children.Add( new HorizontalRule() );

            var salesData = IntegrationHelpers.GetSalesByCategory( "Beverages" );
            var chart = new PieChart( 400, 800 );
            chart.LegendPosition = LegendPosition.Right;
            foreach (DataRow row in salesData.Rows)
            {
                chart.AddSlice( row["Product Name"] as string, double.Parse( row["Total Purchase"].ToString() ) );
            }
            container.Children.Add( chart );
            container.Children.Add( new Table( salesData ) );

            IntegrationHelpers.ExportPdfToTemp( "ExportTest01", doc );
        }

        [Fact( DisplayName = "PageTest" )]
        public void PageTest()
        {
            var doc = new StitchDocument();
            Assert.Equal( 1, doc.PageCount );
            var page = doc.CreatePage();
            Assert.NotNull( page );
            Assert.Equal( 2, doc.PageCount );
            page = new Page();
            page.PageSize = PaperSize.A0;
            doc.InsertPage( page, 2 );
            for (int i = 0; i < doc.PageCount; i++)
            {
                Assert.Equal( i + 1, doc[i].PageNumber );
            }

            page = page.Clone() as Page;
            page.PageOrientation = PageOrientation.Landscape;
            doc.AddPage( page );
            Assert.Equal( 4, page.PageNumber );

            page = new Page();
            doc.AddPage( page );
            page.PageOrientation = PageOrientation.Landscape;
            Assert.Equal( 5, doc.PageCount );

            IntegrationHelpers.SaveToTemp( "Page Test", doc );
        }

        [Fact( DisplayName = "PageTest2" )]
        public void PageTest2()
        {
            var ids = new[] { "a", "b", "c", "d", "e" };
            var ids2 = new[] { "a", "d", "b", "c", "e" };
            var doc = new StitchDocument();

            var checkPages = new Action<int, string[]>( ( pages, _ids ) =>
             {
                 Assert.Equal( pages, doc.PageCount );
                 for (int i = 0; i < doc.PageCount; i++)
                 {
                     Assert.Equal( i + 1, doc[i].PageNumber );
                     Assert.Equal( _ids[i], doc[i].ID );
                 }
             } );
            checkPages( 1, ids );
            doc.CreatePage();
            checkPages( 2, ids );
            doc.CreatePage();
            checkPages( 3, ids );
            var pg = new Page() { ID = doc.IDFactory.GetId() };
            doc.InsertPage( pg, 2 );
            checkPages( 4, ids2 );
            doc.CreatePage();
            checkPages( 5, ids2 );
        }

        [Fact( DisplayName = "TOCWidgetTest1" )]
        public void TOCWidgetTest1()
        {
            var doc = new StitchDocument();
            var toc = doc.AddTableOfContents();

            doc[1].Add( new Paragraph( "page 2" ) );
            doc[1].StyleList.Add( "height", "600px" );
            toc.AddTOCLink( doc[1] );

            var pg = doc.CreatePage();
            pg.StyleList.Add( "height", "600px" );
            pg.Add( new Paragraph( "page 3" ) );
            toc.AddTOCLink( pg );

            IntegrationHelpers.ExportPdfToTemp( "TOCWidgetTest1", doc, true );
        }

        [Fact( DisplayName = "TOC9Test" )]
        public void TOC9Test()
        {
            // Sample from: http://www.makeuseof.com/tag/10-best-table-contents-templates-microsoft-word/
            var doc = new StitchDocument();
            var toc = doc.AddTableOfContents();
            doc.AddStyleRule( ".page { min-height: 600px }" );
            toc.SetStyleType( OrderedListStyleType.UppercaseLetter );

            var benifits = doc.CreatePage();
            var getTheTemplate = doc.CreatePage();

            var templatePage = doc.CreatePage();
            var spacing = doc.CreatePage();
            var styles = doc.CreatePage();

            benifits.Add( new Paragraph( "Benefits &amp; Word Versions" ) );
            getTheTemplate.Add( new Paragraph( "Get the template" ) );
            templatePage.Add( new Paragraph( "Sample text &amp; Arrangement of your ETDR &amp; Basic formatting requirements &amp; fonts" ) );
            spacing.Add( new Paragraph( "line spacing &amp; margins &amp; footnotes/endpoints &amp; page numbers" ) );
            styles.Add( new Paragraph( "styles" ) );

            toc.AddTOCCategory( "Getting Started", benifits );
            toc.AddTOCCategory( "Using the Template", templatePage );

            toc.AddTOCLinkToCategory( "Getting Started", "Benefits", benifits );
            toc.AddTOCLinkToCategory( "Getting Started", "Word Versions", benifits );
            toc.AddTOCLinkToCategory( "Getting Started", "Get the Template", getTheTemplate );
            toc.AddTOCLinkToCategory( "Using the Template", "Sample text", templatePage );
            toc.AddTOCLinkToCategory( "Using the Template", "Arrangement of your ETDR", templatePage );
            toc.AddTOCLinkToCategory( "Using the Template", "Basic formatting requirements", templatePage );
            toc.AddTOCLinkToCategory( "Using the Template", "Styles", styles );


            IntegrationHelpers.ExportPdfToTemp( "TOC9Test", doc, true );
        }

        [Fact ( DisplayName = "TOC3Test")]
        public void TOC3Test()
        {
            // Sample from: http://www.makeuseof.com/tag/10-best-table-contents-templates-microsoft-word/
            var doc = new StitchDocument();
            var toc = doc.TableOfContents;
            toc.SetStyleType( OrderedListStyleType.None );

            var _abstract = doc.CreatePage();
            var acknowledgements = doc.CreatePage();
            var listOfTables = doc.CreatePage();
            var tableOfFigures = doc.CreatePage();
            var chapter1 = doc.CreatePage();

            toc.AddTOCLink( "ABSTRACT", _abstract );
            toc.AddTOCLink( "ACKNOWLEDGMENTS", acknowledgements );
            toc.AddTOCLink( "LIST OF TABLES", listOfTables );
            toc.AddTOCLink( "LIST OF FITURES", tableOfFigures );
            toc.AddTOCLink( "CHAPTER I: Introduction", chapter1 );

            doc.SetPageHeight( 600 );

            IntegrationHelpers.ExportPdfToTemp( "TOC3Test", doc, true );

        }
    }
}
