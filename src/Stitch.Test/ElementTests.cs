using System;
using Stitch.Elements;
using Stitch.Elements.Interface;
using System.Reflection;
using System.Linq;
using Xunit;
using System.IO;
using Stitch.CSS;

namespace Stitch.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ElementTests
    {
        [Fact( DisplayName = "StyleFromUri" )]
        public void StyleFromUri()
        {
            var bootstrapCss = new Uri( "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" );
            var style = new Style( bootstrapCss );

            Assert.True( style.StyleSheet.Rules.Count > 0 );
        }

        [Fact( DisplayName = "TestIElementDeepCloneImplementations" )]
        public void TestIElementDeepCloneImplementations()
        {
            var type = typeof( IElement );
            var assem = Assembly.GetAssembly( type );
            var elementTypes = assem.GetTypes().Where( p => type.IsAssignableFrom( p ) && !p.IsAbstract && !p.ContainsGenericParameters );
            foreach (var elementType in elementTypes)
            {
                try
                {
                    var inst = (IElement)Activator.CreateInstance( elementType, null, null );

                    // if it is a struct we need to manually create class list.  As user would as well.
                    if (elementType.IsValueType) inst.ClassList = new ClassList();
                    inst.ClassList.Add( "something" );
                    inst.ID = "tempID";
                    inst.Title = "should be same";

                    var clone = (IElement)inst.Clone();
                    Assert.NotEqual( inst.ID, clone.ID );
                    Assert.True( string.IsNullOrWhiteSpace( clone.ID ) );
                    Assert.Equal( inst.Title, clone.Title );
                    Assert.Equal( inst.ClassList.Count, clone.ClassList.Count );

                    clone.ID = "newID";
                    clone.ClassList.Clear();

                    Assert.NotEqual( inst.ID, clone.ID );
                    Assert.Equal( inst.Title, clone.Title ); // should still be the same.
                    Assert.NotEqual( inst.ClassList.Count, clone.ClassList.Count );
                }
                catch (Exception e)
                {
                    throw new Exception( $"Could not deep clone type: {elementType.ToString()}.  Exception Message: {e.Message}" );
                }
            }
        }

        [Fact( DisplayName = "RenderElementBenchmark" )]
        public void RenderElementBenchmark()
        {
            var element = new Table();
            for (int i = 0; i < 500; i++)
            {
                var str = new DOMString();
                str.AppendElement( new Table( IntegrationHelpers.GetSampleTableData() ) );
                element.CreateRow( str );
            }
            var rendering = element.Render();
        }

        [Theory( DisplayName = "GetAndEmbedLocalResources" )]
        [InlineData( "..\\..\\Resources\\ResourceResolutionResolve\\css\\regressiontest.css" )]
        public void GetAndEmbedLocalResources( string uriSource )
        {
            // try using given (probably relative) file path.
            var uri = new Uri( uriSource, UriKind.Relative );
            var resolver = new CSSResourceResolver( uri, true );

            uri = new Uri( uriSource, UriKind.RelativeOrAbsolute );
            resolver = new CSSResourceResolver( uri, true );

            // now try using the full file path.
            var fullUri = Path.GetFullPath( uriSource );
            uri = new Uri( fullUri );
            resolver = new CSSResourceResolver( uri, true );
        }

        [Theory( DisplayName = "GetAndEmbedRemoteResources" )]
        [InlineData( "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" )]
        public void GetAndEmbedRemoteResources( string uriSource )
        {
            var uri = new Uri( uriSource );
            var resolver = new CSSResourceResolver( uri, true );
        }

        [Fact( DisplayName = "StyleListTests" )]
        public void StyleListTests()
        {
            var styleList = new StyleList();
            styleList.Add( "font-size", "300%" );
            styleList.Add( "background-color:powderblue" );
            styleList.Add( "color:blue;" );
            styleList.Add( "font-family:verdana;text-align:center;" );

            var answer = "style=\"font-size:300%;background-color:powderblue;color:blue;font-family:verdana;text-align:center;\"";
            Assert.Equal( answer, styleList.GetStyleString() );
        }

        [Theory( DisplayName = "ImageElementDownload" )]
        [InlineData( "..\\..\\Resources\\tiger.jpg", false, "imgRenderTest1" )] /* Relative path a time of rendering. */
        [InlineData( "tiger.jpg", true, "imgRenderTest2" )] /* Relative path at time of release (reference) */
        [InlineData( "http://placehold.it/350x150.png", false, "imgRenderTest3" )]
        [InlineData( "http://placehold.it/350x150.png", true, "imgRenderTest4" )]
        public void ImageElementDownload( string file, bool reference, string name )
        {
            var img = new ImageElement( file );
            img.ReferenceImage = reference;
            if (reference)
            {
                Assert.True( img.Render().Contains( file ) );
            }
            else {
                Assert.NotNull( img.GetBase64EncodedImage() );
                Assert.True( !img.Render().Contains( file ) );
            }
        }

        [Theory( DisplayName = "TestForcedReferenceImage" )]
        [InlineData( "http://google.com/image_that_does_not_exist.png" )]
        public void TestForcedReferenceImage( string file )
        {
            var img = new ImageElement( file );
            img.ReferenceImage = false;
            Assert.True( img.Render().Contains( file ) );
            Assert.True( img.ReferenceImage ); // should be true now.
        }
    }
}
