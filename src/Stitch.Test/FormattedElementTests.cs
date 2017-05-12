using Stitch.Elements;
using Stitch.Elements.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace Stitch.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class FormattedElementTests
    {
        public static IEnumerable<object[]> SpecialTextTestData
        {
            get
            {
                return new[] {
                    new object[] { new Subscript("test"), "<sub>test</sub>" },
                    new object[] { new Superscript("test"), "<sup>test</sup>" },
                    new object[] { new Big("test"), "<big>test</big>" },
                    new object[] { new Small("test"), "<small>test</small>" },
                    new object[] { new Italic("test"), "<i>test</i>" },
                    new object[] { new Bold("test"), "<b>test</b>" },
                    new object[] { new Underline("test"), "<u>test</u>" },
                };
            }
        }

        [Theory( DisplayName = "SpecialTextTest" ), MemberData( nameof( SpecialTextTestData ) )]
        public void TestMethod1( FormattedElement text, string answer )
        {
            Assert.Equal( text.ToString(), answer );
        }

        [Fact( DisplayName = "TestAddressElement" )]
        public void TestAddressElement()
        {
            var address = new Address();
            address.Append( "Written by John Doe." );
            address.Append( "Visit us at:" );
            address.Append( "Example.com" );
            address.Append( "Box 564, Disneyland" );
            address.Append( "USA" );

            var addr = address.Render();
            Assert.Equal( "<address>Written by John Doe.<br />\r\nVisit us at:<br />\r\nExample.com<br />\r\nBox 564, Disneyland<br />\r\nUSA<br />\r\n</address>", addr );
        }
    }
}
