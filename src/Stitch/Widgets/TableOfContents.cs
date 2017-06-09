using Stitch.Elements;
using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Widgets
{
    //public interface ITOCLine
    //{
    //    string Label { get; set; }

    //    List<ITOCLine> Lines { get; }

    //    int Offset { get; set; }
    //}

    //public class TOCPageLink : Div, ITOCLine
    //{
    //    public string Label { get; set; }

    //    public List<ITOCLine> Lines { get; } = new List<ITOCLine>();

    //    public int Offset { get; set; }

    //    public readonly IPage PageReference;
    //    private readonly IAnchorElement Link = new AnchorLinkElement();
    //    private readonly ISpanElement PageNumber = new Span();

    //    public TOCPageLink( string label, IPage pageReference )
    //    {
    //        Label = label;
    //        PageReference = pageReference;

    //        ClassList.Add( "page-width", "toc-link" );
    //        PageNumber.StyleList.Add( "float", "right" );
    //        Children.Add( Link );
    //    }

    //    public TOCPageLink( IPage pageReference ) : this( $"Page {pageReference.PageNumber}", pageReference )
    //    {
    //    }

    //    public override string Render()
    //    {
    //        Link.Href = $"#{PageReference.ID}";
    //        Link.Children.Clear();
    //        PageNumber.Children.Clear();
    //        Link.Children.Add( new PlainText( Label ) );
    //        Link.Children.Add( PageNumber );
    //        PageNumber.Children.Add( new PlainText( PageReference.PageNumber ) );
    //        return base.Render();
    //    }
    //}

    //public class TOCCategory : Div, ITOCLine
    //{
    //    public string Label { get; set; }

    //    public List<ITOCLine> Lines { get; } = new List<ITOCLine>();

    //    public int Offset { get; set; }
    //}


    //public class TOCLine : Div
    //{
    //    public string LineText { get; set; }
    //    public readonly IPage PageReference;
    //    private readonly IAnchorElement Link = new AnchorLinkElement();
    //    private readonly ISpanElement PageNumber = new Span();
    //    private readonly List<TOCLine> SubLines = new List<TOCLine>();

    //    public TOCLine()
    //    {

    //    }

    //    public TOCLine( IPage page ) : base()
    //    {
    //        PageReference = page;
    //        Children.Add( Link );



    //        LineText = ;
    //    }

    //    public TOCLine( string pageLabel, IPage page ) : this( page )
    //    {
    //        LineText = pageLabel;
    //    }

    //    public override string Render()
    //    {

    //        return base.Render();
    //    }
    //}

    public class TableOfContentsCategory : OrderedList
    {
        public readonly string Label;

        private readonly IElement LabelElement;

        public TableOfContentsCategory( string label )
        {
            Label = label;
            StyleType = OrderedListStyleType.None;
            ID = $"toc-category-{label.Replace( " ", "-" )}";
            LabelElement = new ListItemElement( label );
            Children.Add( LabelElement );
        }

        public void AddTOCLink( IPage page )
        {
            AddTOCLink( new TableOfContentsLink( page ) );
        }

        public void AddTOCLink( string label, IPage page )
        {
            AddTOCLink( new TableOfContentsLink( label, page ) );
        }

        public void AddTOCLink( TableOfContentsLink link )
        {
            var li = new ListItemElement();
            li.Children.Add( link );
            Children.Add( li );
        }
    }

    public class TableOfContentsLink : ListItemElement
    {
        public string Label { get; set; }
        public readonly IPage PageReference;
        private readonly IAnchorElement Link = new AnchorLinkElement();
        private readonly ISpanElement PageNumber = new Span();

        public TableOfContentsLink( string label, IPage pageReference )
        {
            Label = label;
            PageReference = pageReference;

            ClassList.Add( "page-width", "toc-link" );
            PageNumber.StyleList.Add( "float", "right" );
            Children.Add( Link );
        }

        public TableOfContentsLink( IPage pageReference ) : this( $"Page {pageReference.PageNumber}", pageReference )
        {
        }

        public override string Render()
        {
            Link.Href = $"#{PageReference.ID}";
            Link.Children.Clear();
            PageNumber.Children.Clear();
            Link.Children.Add( new PlainText( Label ) );
            Link.Children.Add( PageNumber );
            PageNumber.Children.Add( new PlainText( PageReference.PageNumber ) );
            return base.Render();
        }
    }

    public class TableOfContents : Page
    {
        private readonly IOrderedListElement _toc = new OrderedList();
        public string TOCTitle { get { return _tocTitle.Content; } set { _tocTitle.Content = value; } }
        private readonly Heading _tocTitle = new Heading( HeadingLevel.H4 );

        public OrderedListStyleType StyleType { get { return _toc.StyleType; } set { _toc.StyleType = value; } }

        public TableOfContents()
        {
            TOCTitle = "Table of Contents";
            _tocTitle.StyleList.Add( "width: 100%;" );
            _tocTitle.StyleList.Add( "text-align: center" );            
            Children.Add( _tocTitle );
            Children.Add( _toc );
        }

        public void AddTOCLink( IPage page )
        {
            AddTOCLink( new TableOfContentsLink( page ) );
        }

        public void AddTOCLink( string label, IPage page )
        {
            AddTOCLink( new TableOfContentsLink( label, page ) );
        }

        public void AddTOCLink( TableOfContentsLink link )
        {
            var li = new ListItemElement();
            li.Children.Add( link );
            _toc.Children.Add( li );
        }

        public void AddTOCLinkToCategory( string categoryName, TableOfContentsLink link )
        {
            var category = FindById( $"toc-category-{categoryName.Replace( " ", "-" )}" );
            if (category != null)
            {
                var pCategory = category as TableOfContentsCategory;
                pCategory.AddTOCLink( link );
            }
        }

        public void AddTOCLinkToCategory( string categoryName, string label, IPage page )
        {
            AddTOCLinkToCategory( categoryName, new TableOfContentsLink( label, page ) );
        }

        public void AddTOCLinkToCategory( string categoryName, IPage page )
        {
            AddTOCLinkToCategory( categoryName, new TableOfContentsLink( page ) );
        }

        public void AddTOCCategory( TableOfContentsCategory category )
        {
            category.StyleType = StyleType;
            _toc.Children.Add( category );
        }

        public void AddTOCCategory( string category )
        {
            AddTOCCategory( new TableOfContentsCategory( category ) );
        }

        //public void AddTOCLine( string pageLabel, IPage page )
        //{
        //    Children.Add( new TOCLine( pageLabel, page ) );
        //}

        //public void AddTOCLine( IPage page )
        //{
        //    Children.Add( new TOCLine( page ) );
        //}
    }

}
