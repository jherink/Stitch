using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stitch.CSS;
using Stitch.Elements;
using Stitch.Elements.Interface;
using System.Globalization;
using System.Xml;
using Stitch.Export;
using Stitch.Themes;

namespace Stitch
{
    public class StitchDocument
    {
        public readonly IHtmlElement Page;

        private readonly ThemeCssResourceLoader resourceLoader = new ThemeCssResourceLoader();

        public Theme Theme { get; private set; }

        #region Orientation

        private PageOrientation _orientation;

        public PageOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                Body.StyleList.Add( "max-width", $"{Helpers.TranslateOrientation( _orientation )}px !important" );
            }
        }

        #endregion

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
        private IStyleElement ActiveTheme;

        public string Language { get; set; }

        public StitchDocument()
        {
            Page = new Html();
            Page.Children.Add( new Head() ); // add a head
            Page.Children.Add( new Body() ); // and a body to the page.
            Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName; // set to current culture.

            // add mobile viewport meta tag.
            //Head.Metas.Add( ElementFactory.CreateMeta( "viewport", "width=device-width, initial-scale=1" ) );
            Head.Metas.Add( new Meta( "viewport", "width=device-width, initial-scale=1" ) );
            // add charset meta tag.
            //Head.Metas.Add( ElementFactory.CreateMeta( string.Empty, "text/html; charset=utf-8", "content-type" ) );
            Head.Metas.Add( new Meta( string.Empty, "text/html; charset=utf-8", "content-type" ) );


            // add w3 resource.
            //Head.Styles.Add( ElementFactory.CreateStyleFromResource( "w3" ) );
            Head.Styles.Add( new Style() { StyleSheet = resourceLoader.LoadTheme( "w3" ) } );

            SetTheme( Theme.Blue );

            // add a custom styles style sheet for user defined rules.
            CustomStyles = new Style { StyleSheet = new StyleSheet() };
            Head.Styles.Add( CustomStyles );

            // Setup defaults
            Orientation = PageOrientation.Portrait;
            Margin = 25;
        }

        public IDivElement AddBodyContainer()
        {
            var div = new Div( true );
            Body.Children.Add( div );
            return div;
        }

        /// <summary>
        /// Add an element to the body of the document.
        /// </summary>
        /// <param name="element"></param>
        public void Add( params IElement[] elements )
        {
            foreach (var element in elements) Body.Children.Add( element );
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
        /// The width in pixels of the left and right margins.
        /// </summary>
        public int Margin
        {
            get
            {
                var margin = Body.StyleList["margin-left"];
                return int.Parse( margin.Substring( 0, margin.Length - 2 ) );
            }
            set
            {
                Body.StyleList["margin-left"] = Body.StyleList["margin-right"] = $"{value}px";
            }
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

        public void InsertPageBreak()
        {
            var last = Body.Children.Last();
            if (last is IDivElement && last.ClassList.Contains( "w3-container" )) last = (last as IDivElement).Children.Last();
            InsertPageBreak( last );
        }

        public void SetTheme( StyleSheet newTheme )
        {
            if (newTheme != null)
            {
                if (ActiveTheme != null)
                {
                    ActiveTheme.StyleSheet = newTheme;
                }
                else
                {
                    ActiveTheme = new Style() { StyleSheet = newTheme };
                    Head.Styles.Add( ActiveTheme );
                }
            }
        }

        public void SetTheme( string themePath )
        {
            var css = File.ReadAllText( themePath );
            var sheet = new Parser().Parse(css);
            SetTheme( sheet );
        }

        public void SetTheme( Theme theme )
        {
            Theme = theme;

            // extract resource name.
            var type = typeof( Theme );
            var memInfo = type.GetMember( theme.ToString() );
            var attributes = memInfo[0].GetCustomAttributes( typeof( ThemeResourceAttribute ), false );
            var resource = ((ThemeResourceAttribute)attributes[0]).ResourceName;
            var newTheme = resourceLoader.LoadTheme( resource );

            SetTheme( newTheme );
        }

        public IElement InsertPageBreak( IElement element )
        {
            element.StyleList.Add( "page-break-after", "always" );
            return element;
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

        public void Write( Stream writeableStream )
        {
            var writer = new StreamWriter( writeableStream );
            writer.Write( Render() );
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

            using (var writer = XmlWriter.Create( path, settings ))
            {
                xml.Save( writer );
            }
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
