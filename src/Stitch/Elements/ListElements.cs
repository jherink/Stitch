using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stitch.Elements.Interface;
using Stitch.Attributes;

namespace Stitch.Elements
{
    public class ListItemElement : BaseParentElement, IListItemElement
    {
        public DOMString Content { get; set; }

        public ListItemElement() {
            Content = new DOMString( string.Empty );
        }

        public ListItemElement( DOMString content )
        {
            Content = content;
        }

        public override string Tag
        {
            get
            {
                return "li";
            }
        }

        public override IList<IElement> Children
        {
            get
            {
                return Content.Elements;
            }

            set
            {
                Content.Elements.Clear();
                foreach (var e in value)
                {
                    Content.Elements.Add( e );
                }
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.AppendLine( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( $">{Content}</{Tag}>" );
            return builder.ToString();
        }
    }

    /// <summary>
    /// A term/name in a description list
    /// </summary>
    public class DescriptionTermElement : ListItemElement, IDescriptionTermElement
    {
        public DescriptionTermElement() : base() { }

        public DescriptionTermElement( DOMString content ) : base( content ) { }

        public override string Tag
        {
            get
            {
                return "dt";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DescriptionDescribeElement : ListItemElement, IDescriptionDescribeElement
    {

        public DescriptionDescribeElement() : base() { }

        public DescriptionDescribeElement( DOMString content ) : base( content ) { }

        public override string Tag
        {
            get
            {
                return "dd";
            }
        }
    }

    public abstract class BaseListElement : BaseParentElement, IListElement
    {
        public IEnumerable<IListItemElement> Items
        {
            get
            {
                return Children.Where( c => (c as IListItemElement) != null ).Select( c => (IListItemElement)c );
            }
        }

        public BaseListElement() { }

        public BaseListElement( IEnumerable<object> data )
        {
            foreach (var d in data)
            {
                Children.Add( CreateListItem( d.ToString() ) );
            }
        }

        protected abstract IListItemElement CreateListItem( DOMString content );
    }

    public class UnorderedList : BaseListElement, IUnorderedListElement
    {
        public UnorderedListStyleType StyleType { get; set; } = UnorderedListStyleType.Disc;

        public UnorderedList()
        {

        }

        public UnorderedList( IEnumerable<object> data ) : base( data )
        {

        }

        public UnorderedList( UnorderedListStyleType style )
        {
            StyleType = style;
        }

        public UnorderedList( IEnumerable<object> data, UnorderedListStyleType style ) : this( data )
        {
            StyleType = style;
        }

        protected override IListItemElement CreateListItem( DOMString content )
        {
            return new ListItemElement() { Content = content };
        }

        public override string Tag
        {
            get
            {
                return "ul";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            //StyleList.Add( "list-style-type", StyleTypeHelper.GetStyleType( StyleType ) );
            ClassList.Add( StitchCssResourceHelper.GetClass( StyleType ) );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( ">" );
            //builder.AppendLine( $" style=\"list-style-type:{StyleTypeHelper.GetStyleType( StyleType )}\">" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }
    }

    public class OrderedList : BaseListElement, IOrderedListElement
    {
        public OrderedListStyleType StyleType { get; set; } = OrderedListStyleType.Numbered;

        public OrderedList()
        {

        }

        public OrderedList( IEnumerable<object> data ) : base( data )
        {

        }

        public OrderedList( OrderedListStyleType style )
        {
            StyleType = style;
        }

        public OrderedList( IEnumerable<object> data, OrderedListStyleType style ) : this( data )
        {
            StyleType = style;
        }

        protected override IListItemElement CreateListItem( DOMString content )
        {
            return new ListItemElement() { Content = content };
        }

        public override string Tag
        {
            get
            {
                return "ol";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            ClassList.Add( StitchCssResourceHelper.GetClass( StyleType ) );
            AppendIdAndClassInfoToTag( builder );
            //builder.AppendLine( $" type=\"{StyleTypeHelper.GetStyleType( StyleType )}\">" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }
    }

    /// <summary>
    /// A description list, with terms and descriptions
    /// </summary>
    public class DescriptionList : BaseListElement, IDescriptionList
    {
        public DescriptionList()
        {

        }

        public DescriptionList( IEnumerable<object> data ) : base( data )
        {

        }

        protected override IListItemElement CreateListItem( DOMString content )
        {
            return new DescriptionTermElement( content );
        }

        public override string Tag
        {
            get
            {
                return "dl";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( ">" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }
    }

}
