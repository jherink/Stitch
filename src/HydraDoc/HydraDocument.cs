using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HydraDoc.CSS;
using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
using System.Globalization;
using System.Xml;
using HydraDoc.Export;

namespace HydraDoc
{
    public class HydraDocument
    {
        public readonly IHtmlElement Page;

        public IHeadElement Head
        {
            get { return Page.Head; }
        }

        public IBodyElement Body
        {
            get
            {
                return Page.Body;
            }
        }

        private IStyleElement CustomStyles;

        public string Language { get; set; }

        public HydraDocument()
        {
            Page = new Html();
            Page.Children.Add( new Head() ); // add a head
            Page.Children.Add( new Body() ); // and a body to the page.
            Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName; // set to current culture.

            // add mobile viewport meta tag.
            Head.Metas.Add( ElementFactory.CreateMeta( "viewport", "width=device-width, initial-scale=1" ) );
            // add charset meta tag.
            Head.Metas.Add( ElementFactory.CreateMeta( string.Empty, "text/html; charset=utf-8", "content-type" ) );

            // add w3 resource.
            Head.Styles.Add( ElementFactory.CreateStyleFromResource( "w3" ) );
            // add graph resource.
            Head.Styles.Add( ElementFactory.CreateStyleFromResource( "graph" ) );

            // add a custom styles style sheet for user defined rules.
            CustomStyles = ElementFactory.CreateStyle( new StyleSheet() );
            Head.Styles.Add( CustomStyles );

            //var assembly = typeof( StyleSheet ).Assembly;
            //var name = "HydraDoc.CSS.Themes.w3.css";
            //using (var stream = assembly.GetManifestResourceStream( name ))
            //{
            //    using (var reader = new StreamReader( stream ))
            //    {
            //        var parser = new Parser();
            //        Head.Styles.Add( ElementFactory.CreateStyle( parser.Parse( reader.ReadToEnd() ) ) );
            //    }
            //}

            // load w3 style.
            //var loader = new CssThemeLoader();
            //loader.LoadThemeResource( "w3" );
            //Head.Styles.Add( ElementFactory.CreateStyle(loader.StyleSheet) );
        }

        public IDivElement AddBodyContainer()
        {
            var div = ElementFactory.CreateDiv( true );
            Body.Children.Add( div );
            return div;
        }

        /// <summary>
        /// Add an element to the body of the document.
        /// </summary>
        /// <param name="element"></param>
        public void Add( IElement element )
        {
            Body.Children.Add( element );
        }

        public bool Remove( IElement element )
        {
            return Body.Children.Remove( element );
        }

        public void AddStyle( IStyleElement style )
        {
            Head.Styles.Add( style );
        }

        public void AddMeta( IMetaElement meta )
        {
            Head.Metas.Add( meta );
        }

        /// <summary>
        /// Add a CSS rule.
        /// </summary>
        /// <param name="css">The CSS rule</param>
        public void AddStyleRule( string css )
        {
            var parser = new Parser();
            var sheet = parser.Parse( css );
            foreach (var rule in sheet.Rules)
            {
                CustomStyles.StyleSheet.Rules.Add( rule );
            }
        }

        public IElement Find( string id )
        {
            return Body.FindById( id );
        }

        public string Render()
        {
            var allNodes = Page.GetAllNodes();
            var nodes = allNodes.Select( t => t.Tag ).Distinct();
            var classes = new List<string>();
            foreach (var node in allNodes)
            {
                classes.AddRange( node.ClassList.Where( t => true ) );
            }
            var filteredClasses = classes.Distinct();

            var builder = new StringBuilder();
            builder.AppendLine( $"<html lang=\"{Language}\">" );
            builder.AppendLine( Head.Render( nodes, filteredClasses ) );
            builder.AppendLine( Body.Render() );
            builder.AppendLine( "</html>" );
            return builder.ToString();
        }

        public void Save( string path )
        {
            var xml = new XmlDocument();
            xml.LoadXml( Render() );

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t"
            };

            var writer = XmlWriter.Create( path, settings );
            xml.Save( writer );
        }

        public void ExportToPdf( string path )
        {
            var pdfExporter = new PDFExporter();
            using (var output = File.Create( path ))
            {
                Export( pdfExporter, output );
            }
        }

        public byte[] Export( IExporter exporter )
        {
            return exporter.Export( Render() );
        }

        public void Export( IExporter exporter, Stream exportTo )
        {
            exporter.Export( Render(), exportTo );
        }
    }
}
