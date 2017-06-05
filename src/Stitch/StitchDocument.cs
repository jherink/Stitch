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
using System.Collections;
using Stitch.Widgets;

namespace Stitch
{
    public class StitchDocument : IEnumerable<IPage>
    {
        private readonly IHtmlElement Page;

        private readonly ThemeCssResourceLoader themeResourceLoader = new ThemeCssResourceLoader();
        private readonly WidgetCssResourceLoader widgetResourceLoader = new WidgetCssResourceLoader();

        public Theme Theme { get; private set; }

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
        private IStyleElement PaperSizeStyle;

        public Margin Margin { get; set; }

        public string Language { get; set; }

        public StitchDocument()
        {
            Page = new Html();
            Page.Children.Add( new Head() ); // add a head
            Page.Children.Add( new Body() ); // and a body to the page.
            Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName; // set to current culture.
            Margin = new Margin( .5, .5, .5, .5 ) { MarginUnit = MarginUnit.Inches };
            
            // add mobile viewport meta tag.
            Head.Metas.Add( new Meta( "viewport", "width=device-width, initial-scale=1" ) );
            // add charset meta tag.
            Head.Metas.Add( new Meta( string.Empty, "text/html; charset=utf-8", "content-type" ) );

            // add w3 resource.
            Head.Styles.Add( new Style() { StyleSheet = themeResourceLoader.LoadTheme( "w3" ) } );
            // add paper size resource
            Head.Styles.Add( new Style() { StyleSheet = widgetResourceLoader.LoadWidget( "paper-sizes" ) } );

            SetTheme( Theme.Blue );

            // add a custom styles style sheet for user defined rules.
            CustomStyles = new Style { StyleSheet = new StyleSheet() };
            Head.Styles.Add( CustomStyles );

            CreatePage(); // create first page.

            // Setup defaults
            //Orientation = PageOrientation.Portrait;
        }

        #region Page Functions

        private List<IPage> Pages = new List<IPage>();

        private PaperSize _paperSize = PaperSize.ANSI_A;

        /// <summary>
        /// The paper size of the pages in this document.
        /// All pages created or inserted will get this paper size
        /// </summary>
        public PaperSize PaperSize
        {
            get { return _paperSize; }
            set
            {
                foreach (var page in this.Where(t => t.PageSize == _paperSize ))
                {
                    page.PageSize = value;   
                }
                _paperSize = value;
            }
        }

        /// <summary>
        /// Get the page with the specified page number.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public IPage this[int pageNumber]
        {
            get
            {
                foreach (var pg in Pages) if (pg.PageNumber == pageNumber) return pg;
                return default( IPage );
            }
        }

        /// <summary>
        /// The number of pages in the current document.
        /// </summary>
        public int PageCount { get { return Pages.Count; } }

        /// <summary>
        /// Create a page in the current document.
        /// </summary>
        /// <returns>The page.</returns>
        public IPage CreatePage()
        {
            var page = new Page() { PageNumber = Pages.Count + 1 };
            page.Margin = this.Margin.Clone() as Margin;
            page.PageSize = this.PaperSize;
            Body.Children.Add( page );
            Pages.Add( page );
            return page;
        }

        /// <summary>
        /// Inserts a page at the specified index.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageNumber"></param>
        public void InsertPage( IPage page, int pageNumber )
        {
            page.PageSize = this.PaperSize;
            page.Margin = this.Margin.Clone() as Margin;
            page.PageNumber = pageNumber; // make sure they match.

            // insert into page body.
            if (pageNumber <= 1)
            { // insert at front
                Pages.Insert( pageNumber - 1, page );
                Body.Children.Insert( pageNumber - 1, page );
            }
            else if (pageNumber >= PageCount)
            { // at end
                Pages.Add( page );
                Body.Children.Add( page );
            }
            else { // in middle
                for (int i = 0; i < Body.Children.Count; i++)
                {
                    if (Body.Children[i] is IPage &&
                       (Body.Children[i] as IPage).PageNumber == pageNumber)
                    { // if it's the correct page.
                        Body.Children.Insert( i, page );
                    }
                }
                Pages.Insert( pageNumber - 1, page );
            }
            
            for (int i = pageNumber; i < Pages.Count; i++)
            { // correct page numbering.
                Pages[i].PageNumber = pageNumber + 1;
            }

        }

        public void RemovePage( int pageNumber )
        {
            var pg = Pages.FirstOrDefault( t => t.PageNumber == pageNumber );
            if (pg != default( IPage ))
            {
                Pages.Remove( pg );
                Body.Children.Remove( pg );
            }
            for (int i = 1; i <= PageCount; i++)
            {
                Pages[i - 1].PageNumber = i; // correct page numberings.
            }
        }

        /// <summary>
        /// Add a page to the end of the document.
        /// </summary>
        /// <param name="page"></param>
        public void AddPage( IPage page )
        {
            InsertPage( page, Math.Max(PageCount, 1) );
        }

        //public IDivElement AddBodyContainer()
        //{
        //    var div = new Div( true );
        //    Body.Children.Add( div );
        //    return div;
        //}

        #endregion

        /// <summary>
        /// Add an element to the body of the document.
        /// </summary>
        /// <param name="element"></param>
        public void Add( params IElement[] elements )
        {
            //foreach (var element in elements) Body.Children.Add( element );
            var lp = this[PageCount];
            foreach (var element in elements)
            {
                if (element is IPage)
                {
                    AddPage( element as IPage );
                    lp = this[PageCount];
                }
                else {
                    if (lp == null) lp = CreatePage();
                    lp.Children.Add( element );
                }
            }
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

        //public void InsertPageBreak()
        //{
        //    var last = Body.Children.Last();
        //    if (last is IDivElement && last.ClassList.Contains( "w3-container" )) last = (last as IDivElement).Children.Last();
        //    InsertPageBreak( last );
        //}

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
            var sheet = new Parser().Parse( css );
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
            var newTheme = themeResourceLoader.LoadTheme( resource );

            SetTheme( newTheme );
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
                IndentChars = "\t",
                NewLineChars = "\n"
            };

            using (var writer = XmlWriter.Create( path, settings ))
            {
                xml.Save( writer );
            }
        }

        #region Export 

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

        #endregion

        #region IEnumerable Implementation

        public IEnumerator<IPage> GetEnumerator()
        {
            return Pages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Pages.GetEnumerator();
        }

        #endregion
    }
}
