using System;
using Stitch.Elements;
using Xunit;

namespace Stitch.Tests
{
    public class DOMStringTests
    {
        [Fact]
        public void ImplicitStringOperatorsTest()
        {
            DOMString domstr = "test";
            string regstring = domstr;
            Assert.True( domstr == regstring );
        }

        [Fact]
        public void SpecialTextCombine()
        {
            var sp = new Big( "big!!!" );
            DOMString dom = "This was ";
            Assert.True( "This was <big>big!!!</big>" == dom + sp );
            Assert.Equal( dom + sp, "This was <big>big!!!</big>" );
        }

        [Fact]
        public void MultipleAppendsTest()
        {
            var text = new DOMString();
            text.Append( "This is a " );
            text += new Bold( "red" );
            text += " paragraph";
            text.Append( "!!!!" );

            Assert.True( "This is a <b>red</b> paragraph!!!!" == text );
        }
    }
}
