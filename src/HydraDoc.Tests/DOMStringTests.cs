using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydraDoc.Elements;

namespace HydraDoc.Tests
{
    [TestClass]
    public class DOMStringTests
    {
        [TestMethod]
        public void ImplicitStringOperatorsTest()
        {
            DOMString domstr = "test";
            string regstring = domstr;
            Assert.IsTrue( domstr == regstring );
        }

        [TestMethod]
        public void SpecialTextCombine()
        {
            var sp = new Big( "big!!!" );
            DOMString dom = "This was ";
            Assert.IsTrue( "This was <big>big!!!</big>" == dom + sp );
            Assert.AreEqual( dom + sp, "This was <big>big!!!</big>" );
        }

        [TestMethod]
        public void MultipleAppendsTest()
        {
            var text = new DOMString();
            text.Append( "This is a " );
            text += new Bold( "red" );
            text += " paragraph";
            text.Append( "!!!!" );

            Assert.IsTrue( "This is a <b>red</b> paragraph!!!!" == text );
        }
    }
}
