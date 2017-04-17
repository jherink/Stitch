using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydraDoc.Elements.Interface;
using System.Linq;
using HydraDoc.Elements;

namespace HydraDoc.Tests
{
    [TestClass]
    public class ElementFactoryTests
    {
        [TestMethod]
        public void CheckoutIdsTest()
        {
            var expected = new [] { "a", "b", "c", "d", "e", "f", "g", "h",
                                    "i", "j", "k", "l", "m", "n", "o", "p",
                                    "q", "r", "s", "t", "u", "v", "w", "x",
                                    "y", "z", "aa", "ab" };
            int e = 0;
            var diva = ElementFactory.CreateDiv();
            Assert.AreEqual( diva.ID, expected[e++] );
            var divb = ElementFactory.CreateDiv();
            Assert.AreEqual( divb.ID, expected[e++] );
            for (int i = 0; i < 26; i++)
            {
                diva = ElementFactory.CreateDiv();
                Assert.AreEqual( diva.ID, expected[e++] );
            }
        }

        [TestMethod]
        public void SetStyleTypeAttributeHelperTests()
        {
            Assert.AreEqual( "1", StyleTypeHelper.GetStyleType( OrderedListStyleType.Numbered ) );
        }

        [TestMethod]
        public void LabelTests()
        {
            var label = ElementFactory.CreateLabel();
            AssertElement( label );
            label = ElementFactory.CreateLabel( "Test" );
            AssertElement( label );

            var _for = new Div() { ID = "Test" };
            label = ElementFactory.CreateLabel( _for );
            AssertElement( label );
            Assert.AreEqual( "Test", label.For );

        }

        [TestMethod]
        public void DivTests()
        {
            var div = ElementFactory.CreateDiv();
            AssertElement( div );
            var label = ElementFactory.CreateLabel();
            div.Children.Add( label );
            Assert.IsTrue( div.Children.Any() );
        }

        [TestMethod]
        public void ParagraphTests()
        {
            var paragraph = ElementFactory.CreateParagraph();
            AssertElement( paragraph );
            paragraph = ElementFactory.CreateParagraph( "Test" );
            AssertElement( paragraph );
            Assert.IsTrue( !string.IsNullOrWhiteSpace( paragraph.Content ) );
            paragraph.Content += new Small( "tiny" );
            Assert.IsTrue( "Test<small>tiny</small>" == paragraph.Content );
        }

        [TestMethod]
        public void HRandBRTests()
        {
            var hr = ElementFactory.CreateHorizontalRule();
            Assert.IsNotNull( hr );
            var br = ElementFactory.CreateLineBreak();
            Assert.IsNotNull( br );
        }

        //[TestMethod]
        //public void LoadStyleRulesFromTheme()
        //{
        //    var theme = ElementFactory.CreateStyleFromTheme( "blue" );
        //    Assert.IsTrue( theme.Render().Length > 0 );
        //}


        public void AssertElement( IElement element )
        {
            Assert.IsNotNull( element );
            Assert.IsTrue( !string.IsNullOrWhiteSpace( element.ID ) );
        }
    }
}
