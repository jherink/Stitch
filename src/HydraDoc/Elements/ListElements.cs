using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HydraDoc.Elements.Interface;

namespace HydraDoc.Elements
{
    public class ListItemElement : BaseParentElement, IListItemElement
    {
        public DOMString Content { get; set; }

        public override string Tag
        {
            get
            {
                return "li";
            }
        }

        public override ICollection<IElement> Children
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

        public ListItemElement()
        {
            Content = new DOMString( string.Empty );
        }
    }

    /// <summary>
    /// A term/name in a description list
    /// </summary>
    public class DescriptionTermElement : ListItemElement, IDescriptionTermElement
    {
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
    }

    public class UnorderedListElement : BaseListElement, IUnorderedListElement
    {
        public UnorderedListStyleType StyleType { get; set; } = UnorderedListStyleType.Disc;

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
            StyleList.Add( "list-style-type", StyleTypeHelper.GetStyleType( StyleType ) );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( ">" );
            //builder.AppendLine( $" style=\"list-style-type:{StyleTypeHelper.GetStyleType( StyleType )}\">" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }
    }

    public class OrderedListElement : BaseListElement, IOrderedListElement
    {
        public OrderedListStyleType StyleType { get; set; } = OrderedListStyleType.Numbered;

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
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( $" type=\"{StyleTypeHelper.GetStyleType( StyleType )}\">" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }
    }

    /// <summary>
    /// A description list, with terms and descriptions
    /// </summary>
    public class DescriptionListElement : BaseListElement, IDescriptionList
    {
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
