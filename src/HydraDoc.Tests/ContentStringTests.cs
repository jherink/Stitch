using System;
using HydraDoc.Elements;
using System.Collections.Generic;
using Xunit;

namespace HydraDoc.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ContentStringTests
    {
        [Fact( DisplayName = "ImplicitStringOperatorsTest" )]
        public void ImplicitStringOperatorsTest()
        {
            DOMString domstr = "test";
            string regstring = domstr;
            Assert.True( domstr == regstring );
        }

        [Fact( DisplayName = "FormattedTextCombine" )]
        public void FormattedTextCombine()
        {
            var sp = new Big( "big!!!" );
            DOMString dom = "This was ";
            Assert.Equal( "This was <big>big!!!</big>", dom + sp );
        }

        public static IEnumerable<object[]> SpecialTextTestData
        {
            get
            {
                return new[] {
                    new object[] { new Abbreviation("WHO", "World Health Organization"),
                                  "<abbr title=\"World Health Organization\">WHO</abbr>" },
                    new object[] { new Address($"Written By John Doe{Environment.NewLine}Visit us at:"),
                                   $"<address>Written By John Doe{Environment.NewLine}Visit us at:</address>"},
                    new object[] { new Big("test"), "<big>test</big>" },
                    new object[] { new BlockQuote("A quote.", "http://www.worldwildlife.org/who/index.html"),
                                   "<blockquote cite=\"http://www.worldwildlife.org/who/index.html\">A quote.</blockquote>" },
                    new object[] { new Bold("test"), "<b>test</b>" },
                    new object[] { new DeletedText("Blue"), "<del>Blue</del>" },
                    new object[] { new Emphasized("Emphasized text"), "<em>Emphasized text</em>" },
                    new object[] { new InsertedText("insert here"), "<ins>insert here</ins>" },
                    new object[] { new Italic( "The lightning" ), "<i>The lightning</i>" },
                    new object[] { new Marked("milk"), "<mark>milk</mark>" },
                    new object[] { new PlainText("Plain Jane Baby"), "Plain Jane Baby" },
                    new object[] { new Quotation("This is a quote."), "<q>This is a quote.</q>" },
                    new object[] { new Small("super-duper baby text"), "<small>super-duper baby text</small>" },
                    new object[] { new Strong("Here is some strong text."), "<strong>Here is some strong text.</strong>" },
                    new object[] { new Subscript("test"), "<sub>test</sub>" },
                    new object[] { new Superscript("test"), "<sup>test</sup>" },
                    new object[] { new Underline("test"), "<u>test</u>" },
                };
            }
        }

        [Theory( DisplayName = "SpecialTextTest" ), MemberData( nameof( SpecialTextTestData ) )]
        public void TestMethod1( FormattedElement text, string answer )
        {
            Assert.Equal( text.Render(), answer );
        }
    }
}
