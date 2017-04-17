﻿using System;
using Xunit;
using HydraDoc.Elements;
using System.Collections.Generic;
using HydraDoc.Elements.Interface;
using System.Linq;

namespace HydraDoc.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class DocumentRenderTests
    {
        [Fact( DisplayName = "HelloWorldTest" )]
        public void HelloWorldTest()
        {
            var doc = new HydraDocument();
            doc.Head.Title = "Hello World";
            doc.Head.Styles.Add( ElementFactory.CreateStyleFromTheme( "black" ) );

            var p = ElementFactory.CreateParagraph( "Hello " );
            p.Content += new Big( "World" );
            p.Content += new Bold( "!!!" );
            doc.Add( p );
            IntegrationHelpers.SaveToTemp( "helloworld.html", doc );
        }

        [Fact( DisplayName = "HelloWorldInColor" )]
        public void HelloWorldInColor()
        {
            var doc = new HydraDocument();
            doc.Head.Title = "Hello World";
            doc.Head.Styles.Add( ElementFactory.CreateStyleFromTheme( "green" ) );

            var div = ElementFactory.CreateDiv();
            div.ClassList.Add( "w3-theme" );
            div.Children.Add( ElementFactory.CreateParagraph( "Hello World" ) );

            var p = ElementFactory.CreateParagraph( " You too!" );
            p.ClassList.Add( "w3-theme-d1" );
            div.Children.Add( p );

            doc.Add( div );
            IntegrationHelpers.SaveToTemp( "helloworldincolor.html", doc );
        }

        [Fact( DisplayName = "CreateTableTest" )]
        public void CreateTableTest()
        {
            var doc = new HydraDocument();
            doc.Head.Title = "Table Test";

            var div = ElementFactory.CreateDiv( true );
            var table = ElementFactory.CreateTable( IntegrationHelpers.GetSampleTableData() );
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
            var doc = new HydraDocument();
            doc.Head.Title = "Unordered List Test";

            var div = ElementFactory.CreateDiv( true );
            div.Children.Add( ElementFactory.CreateUnorderedList( list ) );

            div.Children.Add( ElementFactory.CreateHorizontalRule( false ) );
            div.Children.Add( ElementFactory.CreateOrderedList( list ) );

            div.Children.Add( ElementFactory.CreateHorizontalRule( false ) );
            div.Children.Add( ElementFactory.CreateOrderedList( list, OrderedListStyleType.UppercaseRomanNumeral ) );

            div.Children.Add( ElementFactory.CreateHorizontalRule( false ) );
            div.Children.Add( ElementFactory.CreateUnorderedList( list, UnorderedListStyleType.Circle ) );

            doc.Add( div );

            IntegrationHelpers.SaveToTemp( "unorderedListTest1.html", doc );
        }

        [Fact( DisplayName = "HeaderTests" )]
        public void HeaderTests()
        {
            var doc = new HydraDocument();
            doc.Head.Title = "Header Tests";

            var container = doc.AddBodyContainer();
            container.Children.Add( ElementFactory.CreateHeadingElement( HeadingLevel.H1, "Heading Level 1" ) );
            container.Children.Add( ElementFactory.CreateHeadingElement( HeadingLevel.H2, "Heading Level 2" ) );
            container.Children.Add( ElementFactory.CreateHeadingElement( HeadingLevel.H3, "Heading Level 3" ) );
            container.Children.Add( ElementFactory.CreateHeadingElement( HeadingLevel.H4, "Heading Level 4" ) );
            container.Children.Add( ElementFactory.CreateHeadingElement( HeadingLevel.H5, "Heading Level 5" ) );
            container.Children.Add( ElementFactory.CreateHeadingElement( HeadingLevel.H6, "Heading Level 6" ) );

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
            var doc = new HydraDocument();
            doc.AddStyleRule( "body { background-color:powderblue;" );
            doc.AddStyleRule( ".myCustomParagraph { color: red; }" );

            var container = doc.AddBodyContainer();

            var text = new DOMString();
            text.Append( "This is a " );
            text += new Bold( "red" );
            text += " paragraph";
            text.Append( "!!!!" );
            var p = ElementFactory.CreateParagraph( text );

            p.ClassList.Add( "myCustomParagraph" );

            container.Children.Add( p );

            IntegrationHelpers.SaveToTemp( "CustomCSSRuleTest.html", doc );
        }

        [Fact( DisplayName = "AddAnchorLinkTests" )]
        public void AddAnchorLinkTests()
        {
            var doc = new HydraDocument();
            doc.AddStyleRule( ".bigDiv { min-height: 1600px; !important }" );
            var container = doc.AddBodyContainer();

            // create a regular link.
            var link = new AnchorLinkElement()
            {
                Href = "http://www.w3schools.com/html/",
                Name = "Visit W3Schools!",
                ID = "GoBackLink"
            };

            container.Children.Add( ElementFactory.CreateDiv( link ) );


            // create div now so we have it's ID
            var linkDiv = ElementFactory.CreateDiv( new Paragraph { Content = "There is something here" } );

            // now create an link element that links to link div.
            link = new AnchorLinkElement
            {
                Href = $"#{linkDiv.ID}",
                Name = "Go to div link at the bottom of the page."
            };

            container.Children.Add( link ); // add the link before the space.

            // now create and add a large space in the page.
            var bigSpace = ElementFactory.CreateDiv();
            bigSpace.ClassList.Add( "bigDiv" );
            container.Children.Add( bigSpace ); // add a big space

            // now add the linked div at the bottom of the page.
            container.Children.Add( ElementFactory.CreateDiv( false, linkDiv ) ); // no add div linking to.

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
            var doc = new HydraDocument();
            var container = doc.AddBodyContainer();

            var table = ElementFactory.CreateTable();
            var row = ElementFactory.CreateRow();
            var cell = ElementFactory.CreateCell();

            var url = "http://www.w3schools.com/images/w3schools_green.jpg";

            cell.Content = new DOMString( ElementFactory.CreateImage( url ) );

            row.Children.Add( cell );
            var body = table.AddBody();
            body.Children.Add( row );

            var rowClone = (ITableRowElement)row.Clone();
            rowClone[0].Content = new DOMString( ElementFactory.CreateImage( url, "second row", 50, 50 ) );
            body.Children.Add( rowClone );

            rowClone = (ITableRowElement)row.Clone();

            // full path
            var fullPath = IntegrationHelpers.CreateLocalResource( "..\\..\\Resources\\Images\\tiger.jpg" );
            rowClone[0].Content = new DOMString( ElementFactory.CreateImage( fullPath ) );
            body.Children.Add( rowClone );

            // relative
            rowClone = (ITableRowElement)row.Clone();
            rowClone[0].Content = new DOMString( ElementFactory.CreateImage( System.IO.Path.GetFileName( fullPath ) ) );
            body.Children.Add( rowClone );
            
            container.Children.Add( table );

            IntegrationHelpers.SaveToTemp( "imageInTableTests.html", doc );
        }

        [Fact( DisplayName = "TableWidthACaptionTests" )]
        public void TableWidthACaptionTests()
        {
            var doc = new HydraDocument();
            var cont = doc.AddBodyContainer();

            var dt = new System.Data.DataTable();
            dt.Columns.Add( "Month", typeof( string ) );
            dt.Columns.Add( "Savings", typeof( string ) );

            dt.Rows.Add( "January", "$100" );
            dt.Rows.Add( "Febuary", "$50" );

            var table = ElementFactory.CreateTable( dt );
            table.TableCaption = new TableCaption( "Monthly Savings" );
            cont.Children.Add( table );

            IntegrationHelpers.SaveToTemp( "tablewithcaptiontest", doc );
        }

        [Fact( DisplayName = "CreateComplexSamplePage" )]
        public void CreateComplexSamplePage()
        {
            var doc = new HydraDocument();
            doc.AddStyleRule( ".limitedTable { max-width: 400px; }" );
            var container = doc.AddBodyContainer();

            container.Children.Add( new Div( new Bold( "DESIGN CRITERIA" ) ) );
            container.Children.Add( new HorizontalRule() );

            var table = ElementFactory.CreateTable();
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

            table = ElementFactory.CreateTable( "HEIGHT(FT)", "SHAFT WEIGHT (LBS)", "GROUND LINE DIAMETER (IN)", "TOP DIAMETER (IN)" );
            container.Children.Add( new Div( table ) );
            table.CreateRow( 30.0, 414, 9.00, 4.80 );

            container.Children.Add( new Div( new Bold( "SECTION CHARACTERISTICS" ) ) );
            container.Children.Add( new HorizontalRule() );
            table = ElementFactory.CreateTable( string.Empty, "SECTION - 1" );
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
            var doc = new HydraDocument();
            var rowDiv = ElementFactory.CreateDiv();
            rowDiv.ClassList.Add( "w3-row" ).Add( "w3-section" ); // like javascript!

            doc.Head.Styles.Add( new Style( new Uri( "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" ) ) );
            doc.AddStyleRule( ".contactBox { height:250px; }" );
            doc.AddStyleRule( ".contactLine { width: 30px; }" );
            doc.AddStyleRule( "body { max-width:1200px; }" );

            // Contact Info
            var contactInfo = ElementFactory.CreateDiv();
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
            var contactUs = ElementFactory.CreateDiv();
            contactUs.ClassList.Add( "w3-third w3-center w3-large w3-dark-grey w3-text-white contactBox" ); // split?
            contactUs.Children.Add( new Heading( HeadingLevel.H2, "Contact Us" ) );
            contactUs.Children.Add( new Paragraph( "If you have an idea." ) );
            contactUs.Children.Add( new Paragraph( "What are you waiting for?" ) );

            rowDiv.Children.Add( contactUs );

            // Like Us
            var likeUs = ElementFactory.CreateDiv();
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

            IntegrationHelpers.SaveToTemp( "W3CSSSampleTest", doc );
        }
    }
}