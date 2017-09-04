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
using System.Collections;
using Stitch.Widgets;
using Stitch.Attributes;
using Stitch.Loaders;

namespace Stitch
{
    public class StitchDocument : IEnumerable<IPage>
    {
        private readonly IHtmlElement Root;

        private readonly ThemeCssResourceLoader themeResourceLoader = new ThemeCssResourceLoader();
        private readonly WidgetCssResourceLoader widgetResourceLoader = new WidgetCssResourceLoader();
        private readonly StitchCssResourceLoader stitchResourceLoader = new StitchCssResourceLoader();

        public Theme Theme { get; private set; }
        public IIDFactory IDFactory { get; set; } = new IDFactory();

        public IHeadElement Head
        {
            get { return Root.Head; }
        }

        public IBodyElement Body
        {
            get
            {
                return Root.Body;
            }
        }

        private TableOfContents _toc;
        public TableOfContents TableOfContents
        {
            get
            {
                if (_toc == null)
                {
                    _toc = AddTableOfContents();
                }
                return _toc;
            }
        }

        private IStyleElement CustomStyles;
        private IStyleElement ActiveTheme;

        public Margin Margin { get; set; }

        public string Language { get; set; }

        public StitchDocument()
        {
            Root = new Html();
            Root.Children.Add( new Head() ); // add a head
            Root.Children.Add( new Body() ); // and a body to the page.
            Body.Children.Add( PageContainer ); // add container for pages.
            Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName; // set to current culture.
            Margin = new Margin( .5, .5, .5, .5 ) { MarginUnit = MarginUnit.Inches };

            // add mobile viewport meta tag.
            Head.Metas.Add( new Meta( "viewport", "width=device-width, initial-scale=1" ) );
            // add charset meta tag.
            Head.Metas.Add( new Meta( string.Empty, "text/html; charset=utf-8", "content-type" ) );

            // add w3 resource.
            Head.Styles.Add( new Style() { StyleSheet = themeResourceLoader.LoadTheme( "w3" ) } );
            // add w3 resource.
            Head.Styles.Add( new Style() { StyleSheet = stitchResourceLoader.LoadTheme( "stitch" ) } );
            // add paper size resource
            Head.Styles.Add( new Style() { StyleSheet = widgetResourceLoader.LoadWidget( "widgets" ) } );

            SetTheme( Theme.Blue );

            // add a custom styles style sheet for user defined rules.
            CustomStyles = new Style { StyleSheet = new StyleSheet() };
            Head.Styles.Add( CustomStyles );

            CreatePage(); // create first page.
        }

        public TableOfContents AddTableOfContents()
        {
            if (_toc == null)
            {
                _toc = new TableOfContents()
                {
                    PageSize = PaperSize.ANSI_A,
                    ID = IDFactory.GetId()
                };
                InsertPage( _toc, 1 );
            }
            return _toc;
        }

        #region Page Functions

        private readonly List<IPage> Pages = new List<IPage>();
        private readonly IDivElement PageContainer = new Div() { ID = "pages" };

        /// <summary>
        /// The paper size of the pages in this document.
        /// All new pages created in this document will get this paper size.
        /// </summary>
        public PaperSize PaperSize { get; set; } = PaperSize.ANSI_A;

        /// <summary>
        /// Get the page with the specified page number.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public IPage this[int pageIndex]
        {
            get
            {
                return Pages[pageIndex];
            }
        }

        public IPage GetPage( int pageNumber )
        {
            return Pages.FirstOrDefault( t => t.PageNumber == pageNumber );
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
            var page = new Page() { PageNumber = Pages.Count + 1, PageSize = PaperSize.ANSI_A, ID = IDFactory.GetId() };
            AddPage( page );
            return page;
        }

        /// <summary>
        /// Inserts a page at the specified index.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageNumber"></param>
        public void InsertPage( IPage page, int pageNumber )
        {
            page.Margin = this.Margin.Clone() as Margin;
            page.PageNumber = pageNumber; // make sure they match.
            // pages need id's
            if (string.IsNullOrWhiteSpace( page.ID )) page.ID = IDFactory.GetId();
            var pageIndex = pageNumber - 1;

            Pages.Insert( pageIndex, page );
            PageContainer.Children.Insert( pageIndex, page );

            // remove page class on last page only & correct page numberings.
            for (int i = 0; i < Pages.Count; i++)
            {
                Pages[i].PageNumber = i + 1;
                if (i < (Pages.Count - 1))
                {
                    Pages[i].ClassList.Add( "page" );
                }
                else if ((i == (Pages.Count - 1)) && page.ClassList.Contains( "page" ))
                {
                    Pages[i].ClassList.Remove( "page" );
                }
            }


        }

        public void RemovePage( int pageNumber )
        {
            var pg = Pages.FirstOrDefault( t => t.PageNumber == pageNumber );
            if (pg != default( IPage ))
            {
                Pages.Remove( pg );
                PageContainer.Children.Remove( pg );
            }
            for (int i = 0; i < PageCount; i++)
            {
                Pages[i].PageNumber = i + 1; // correct page numberings.
            }
        }

        /// <summary>
        /// Add a page to the end of the document.
        /// </summary>
        /// <param name="page"></param>
        public void AddPage( IPage page )
        {
            InsertPage( page, PageCount + 1 );
        }

        #endregion

        /// <summary>
        /// Add an element to the body of the document.
        /// </summary>
        /// <param name="element"></param>
        public void Add( params IElement[] elements )
        {
            //foreach (var element in elements) Body.Children.Add( element );
            var lp = PageCount > 0 ? this[PageCount - 1] : null;
            foreach (var element in elements)
            {
                if (element is IPage)
                {
                    AddPage( element as IPage );
                    lp = this[PageCount - 1];
                }
                else
                {
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

        public void SetPageHeight( int pageHeightInPixels )
        {
            AddStyleRule( $".page {{ min-height: {pageHeightInPixels}px }}");
        }
         
        public void SetPageWidth( int pageWidthInPixels )
        {
            AddStyleRule( $".page {{ min-width: {pageWidthInPixels}px }}" );
        }

        public IElement Find( string id )
        {
            return Body.FindById( id );
        }

        public string Render()
        {
            var allNodes = Root.GetAllNodes();
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
