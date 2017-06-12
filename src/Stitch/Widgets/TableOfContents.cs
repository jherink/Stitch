using Stitch.Elements;
using Stitch.Elements.Interface;

namespace Stitch.Widgets
{
    public class TableOfContentsCategory : Div
    {
        public readonly string Label;

        private readonly IElement LabelElement;
        public readonly IPage PageReference;
        private readonly IAnchorElement Link;
        private readonly ISpanElement PageNumber;
        private readonly IOrderedListElement CategoryList = new OrderedList();
        public OrderedListStyleType StyleType { get { return CategoryList.StyleType; } set { CategoryList.StyleType = value; } }

        public TableOfContentsCategory( string label )
        {
            Label = label;
            StyleType = OrderedListStyleType.None;
            ID = $"toc-category-{label.Replace( " ", "-" )}";
            LabelElement = new PlainText( label );
            ClassList.Add( "page-width", "toc-link" );
            Children.Add( CategoryList );
        }

        public TableOfContentsCategory( string label, IPage page ) : this( label )
        {
            if (page != null)
            {
                Children.Remove( LabelElement );
                Link = new AnchorLinkElement();
                PageNumber = new Span();
                PageNumber.StyleList.Add( "float", "right" );
                Children.Insert( 0, Link );
                PageReference = page;
            }
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
            CategoryList.Children.Add( link );
        }

        public override string Render()
        {
            if (PageReference != null)
            {
                Link.Href = $"#{PageReference.ID}";
                Link.Children.Clear();
                PageNumber.Children.Clear();
                Link.Children.Add( LabelElement );
                Link.Children.Add( PageNumber );
                PageNumber.Children.Add( new PlainText( PageReference.PageNumber ) );
            }
            return base.Render();
        }
    }

    public class TableOfContentsLink : AnchorLinkElement
    {
        public string Label { get; set; }
        public readonly IPage PageReference;
        private readonly IListItemElement Item = new ListItemElement();
        private readonly ISpanElement PageNumber = new Span();

        public TableOfContentsLink( string label, IPage pageReference )
        {
            Label = label;
            PageReference = pageReference;

            ClassList.Add( "page-width", "toc-link" );
            Children.Add( Item );
        }

        public TableOfContentsLink( IPage pageReference ) : this( $"Page {pageReference.PageNumber}", pageReference )
        {
        }

        public override string Render()
        {
            Href = $"#{PageReference.ID}";
            Item.Children.Clear();
            PageNumber.Children.Clear();
            PageNumber.Children.Add( new PlainText( PageReference.PageNumber ) );
            Item.Children.Add( new Span( new PlainText( Label ) ) );
            Item.Children.Add( PageNumber );
            return base.Render();
        }
    }

    public class TableOfContents : Page
    {
        private readonly IDivElement _toc = new Div();
        public string TOCTitle { get { return _tocTitle.Content; } set { _tocTitle.Content = value; } }
        private readonly Heading _tocTitle = new Heading( HeadingLevel.H4 );

        public OrderedListStyleType DefaultStyleType = OrderedListStyleType.None;

        public TableOfContents()
        {
            TOCTitle = "Table of Contents";
            _tocTitle.StyleList.Add( "width: 100%;" );
            _tocTitle.StyleList.Add( "text-align: center" );
            Children.Add( _tocTitle );
            Children.Add( _toc );
        }

        public void SetStyleType( OrderedListStyleType styleType )
        {
            foreach (var category in GetNodes( "div" ))
            {
                var tcc = category as TableOfContentsCategory;
                if (tcc != null)
                {
                    tcc.StyleType = styleType;
                }
            }
            DefaultStyleType = styleType;
        }

        public TableOfContentsCategory GetCategory( string categoryName )
        {
            var e = FindById( $"toc-category-{categoryName.Replace( " ", "-" )}" );
            return e != default( IElement ) ? e as TableOfContentsCategory : default(TableOfContentsCategory);
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
            var category = GetCategory(categoryName);
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
            category.StyleType = DefaultStyleType;
            _toc.Children.Add( category );
        }

        public void AddTOCCategory( string category )
        {
            AddTOCCategory( new TableOfContentsCategory( category ) );
        }

        public void AddTOCCategory( string category, IPage page )
        {
            AddTOCCategory( new TableOfContentsCategory( category, page ) );
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
