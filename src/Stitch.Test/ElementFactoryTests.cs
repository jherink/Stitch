using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stitch.Elements.Interface;
using System.Linq;
using Stitch.Elements;

namespace Stitch.Tests
{
    [TestClass]
    public class ElementFactoryTests
    {
        [TestMethod]
        public void CheckoutIdsTest()
        {
            var expected = new[] { "a", "b", "c", "d", "e", "f", "g", "h",
                                    "i", "j", "k", "l", "m", "n", "o", "p",
                                    "q", "r", "s", "t", "u", "v", "w", "x",
                                    "y", "z", "aa", "ab" };
            int e = 0;
            var diva = IDFactory.GetElementId();
            Assert.AreEqual( diva, expected[e++] );
            var divb = IDFactory.GetElementId();
            Assert.AreEqual( divb, expected[e++] );
            for (int i = 0; i < 26; i++)
            {
                diva = IDFactory.GetElementId();
                Assert.AreEqual( diva, expected[e++] );
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
            var label = new Label();
            AssertElement( label );
            label = new Label( "Test" );
            AssertElement( label );

            var _for = new Div() { ID = "Test" };
            label = new Label( _for );
            AssertElement( label );
            Assert.AreEqual( "Test", label.For );

        }

        [TestMethod]
        public void DivTests()
        {
            var div = new Div();
            AssertElement( div );
            var label = new Label();
            div.Children.Add( label );
            Assert.IsTrue( div.Children.Any() );
        }

        [TestMethod]
        public void ParagraphTests()
        {
            var paragraph = new Paragraph();
            AssertElement( paragraph );
            paragraph =new Paragraph( "Test" );
            AssertElement( paragraph );
            Assert.IsTrue( !string.IsNullOrWhiteSpace( paragraph.Content ) );
            paragraph.Content += new Small( "tiny" );
            Assert.IsTrue( "Test<small>tiny</small>" == paragraph.Content );
        }

        [TestMethod]
        public void HRandBRTests()
        {
            var hr = new HorizontalRule();
            Assert.IsNotNull( hr );
            var br = new LineBreak();
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
            //Assert.IsTrue( !string.IsNullOrWhiteSpace( element.ID ) );
        }
    }
}
