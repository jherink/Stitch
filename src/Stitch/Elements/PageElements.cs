using Stitch.Elements.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace Stitch.Elements
{
    #region Base Classes For Elements and Parent Elements

    public abstract class BaseElement : IElement
    {
        public ClassList ClassList { get; set; } = new ClassList();

        public StyleList StyleList { get; set; } = new StyleList();

        public IDictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();

        public string ID { get; set; }

        public string Title { get; set; }

        public abstract string Tag { get; }

        public override string ToString()
        {
            return Render();
        }

        public abstract string Render();

        protected void AppendIdAndClassInfoToTag( StringBuilder builder )
        {
            if (!string.IsNullOrWhiteSpace( ID )) builder.Append( $" id=\"{ID}\"" );
            if (ClassList.Any()) builder.Append( $" {ClassList.GetClassString()}" );
            if (StyleList.Count > 0) builder.Append( $" {StyleList.GetStyleString()}" );
            if (Attributes.Count > 0) foreach (var att in Attributes) builder.Append( $" {att.Key}=\"{att.Value}\"" );
        }

        public virtual object Clone()
        {
            var clone = (IElement)MemberwiseClone();
            clone.ClassList = new ClassList();
            clone.StyleList = new StyleList();
            (clone as BaseElement).Attributes = new Dictionary<string, string>();
            clone.ID = string.Empty;
            foreach (var cls in ClassList)
            {
                clone.ClassList.Add( cls );
            }
            foreach (var style in StyleList)
            {
                clone.StyleList.Add( style );
            }
            foreach (var att in Attributes)
            {
                clone.Attributes.Add( att.Key, att.Value );
            }
            return clone;
        }
    }

    public abstract class BaseParentElement : BaseElement, IParentElement
    {
        public virtual IList<IElement> Children { get; set; } = new List<IElement>();

        public IElement FindById( string id )
        {

            foreach (var child in Children)
            {
                if (child.ID == id) { return child; }
                else
                {
                    if (child is INodeQueryable)
                    {
                        var r = (child as INodeQueryable).FindById( id );
                        if (r != default( INodeQueryable ))
                        {
                            return r; // exit if this node or one of it's children is it. Otherwise continue.
                        }
                    }
                }
            }
            return default( IElement );
        }

        public virtual IEnumerable<IElement> GetAllNodes()
        {
            var elements = new List<IElement>();
            //System.Diagnostics.Debug.WriteLine( $"Getting all nodes: {Tag}, Children count: {Children.Count}" );
            foreach (var child in Children)
            {
                elements.Add( child );
                if (child is INodeQueryable)
                {
                    elements.AddRange( (child as INodeQueryable).GetAllNodes() );
                }
            }
            return elements;
        }

        public virtual IEnumerable<IElement> GetNodes( string tagFilter )
        {
            var elements = new List<IElement>();
            foreach (var child in Children)
            {
                if (child.Tag == tagFilter) elements.Add( child );

                if (child is INodeQueryable)
                {
                    elements.AddRange( (child as INodeQueryable).GetNodes( tagFilter ) );
                }
            }
            return elements;
        }

        protected string RenderChildren()
        {
            var childrenString = new StringBuilder();
            foreach (var child in Children)
            {
                childrenString.Append( child.Render() );
            }
            return childrenString.ToString();
        }

        public override object Clone()
        {
            var parentClone = (IParentElement)base.Clone();
            parentClone.Children = new List<IElement>();
            foreach (var child in Children)
            {
                parentClone.Children.Add( (IElement)child.Clone() );
            }
            return parentClone;
        }
    }

    #endregion

    public class Heading : BaseElement, IHeadingElement
    {
        public DOMString Content { get; set; }

        public HeadingLevel Level { get; set; }

        public override string Tag
        {
            get
            {
                return Level.ToString().ToLowerInvariant();
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );

            AppendIdAndClassInfoToTag( builder );

            builder.Append( $">{Content}</{Tag}>" );
            return builder.ToString();
        }

        public Heading() { }
        public Heading( HeadingLevel level ) { Level = level; }
        public Heading( HeadingLevel level, DOMString content ) : this( level )
        {
            Content = content;
        }
    }

    public class Header : BaseParentElement, IHeaderElement
    {
        public override string Tag
        {
            get
            {
                return "header";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( ">" );
            builder.Append( RenderChildren() );
            builder.Append( $"</{Tag}" );

            return builder.ToString();
        }
    }

    public class Label : BaseElement, ILabelElement
    {
        public override string Tag { get { return "label"; } }
        public string For { get; set; }
        public DOMString Text { get; set; }

        public Label() { }

        public Label( DOMString labelText )
        {
            Text = labelText;
        }

        public Label( IElement _for ) : this( _for, string.Empty ) { }

        public Label( IElement _for, DOMString labelText ) : this( labelText )
        {
            For = _for.ID;
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} for=\"{For}\"" );

            AppendIdAndClassInfoToTag( builder );

            builder.Append( $">{Text}</{Tag}>" );
            return builder.ToString();
        }
    }

    public class Paragraph : BaseElement, IParagraphElement
    {
        public override string Tag { get { return "p"; } }

        public DOMString Content { get; set; }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );

            AppendIdAndClassInfoToTag( builder );

            builder.Append( $">{Content.Render()}</{Tag}>" );
            return builder.ToString();
        }
        public Paragraph() : this( string.Empty )
        {
        }

        public Paragraph( string content )
        {
            Content = new DOMString( content );
        }

        public IElement FindById( string id )
        {
            foreach (var element in Content.Elements)
            {
                if (element.ID == id) { return element; }
                else
                {
                    if (element is INodeQueryable)
                    {
                        var r = (element as INodeQueryable).FindById( id );
                        if (r != default( INodeQueryable ))
                        {
                            return r; // exit if this node or one of it's children is it. Otherwise continue.
                        }
                    }
                }
            }
            return default( IElement );
        }

        public IEnumerable<IElement> GetAllNodes()
        {
            var elements = new List<IElement>();
            foreach (var element in Content.Elements)
            {
                elements.Add( element );
                if (element is INodeQueryable)
                {
                    elements.AddRange( (element as INodeQueryable).GetAllNodes() );
                }
            }
            return elements;
        }

        public IEnumerable<IElement> GetNodes( string tagFilter )
        {
            var elements = new List<IElement>();
            foreach (var element in Content.Elements)
            {
                if (element.Tag == tagFilter) elements.Add( element );

                if (element is INodeQueryable)
                {
                    elements.AddRange( (element as INodeQueryable).GetNodes( tagFilter ) );
                }
            }
            return elements;
        }
    }

    public class Div : BaseParentElement, IDivElement
    {
        public override string Tag { get { return "div"; } }
        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( $">" );
            builder.Append( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }

        public Div() : this( false ) { }

        public Div( bool container )
        {
            if (container)
            {
                ClassList.Add( "w3-container" );
            }
        }

        public Div( bool container, params IElement[] children ) : this( container )
        {
            Add( children );
        }

        public Div( params IElement[] children ) : this( false, children )
        {
        }

        public void Add( params IElement[] children )
        {
            foreach (var child in children) { Children.Add( child ); }
        }
    }

    public class Span : BaseParentElement, ISpanElement
    {
        public override string Tag { get { return "span"; } }
        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( $">" );
            builder.Append( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }

        public Span() { }

        public Span( params IElement[] children )
        {
            foreach (var child in children) { Children.Add( child ); }
        }
    }

    public class Pre : BaseElement, IPreElement
    {
        public override string Tag { get { return "pre"; } }
        public DOMString Text { get; set; }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( $">{Text}</{Tag}>" );
            return builder.ToString();
        }
    }

    public class LineBreak : BaseElement, ILineBreakElement
    {
        public override string Tag { get { return "br"; } }
        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} " );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( "/>" );
            return builder.ToString();
        }
    }

    public class HorizontalRule : BaseElement, IHorizontalRuleElement
    {
        public override string Tag { get { return "hr"; } }
        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} " );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( "/>" );
            return builder.ToString();
        }
    }

    public class AnchorLinkElement : BaseParentElement, IAnchorElement
    {
        public DOMString Href { get; set; }

        public string Name { get; set; }

        public override string Tag
        {
            get
            {
                return "a";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} " );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( $" href=\"{Href}\">" );
            builder.AppendLine( RenderChildren() );
            builder.Append( $"{Name}</{Tag}>" );
            return builder.ToString();
        }
    }
}
