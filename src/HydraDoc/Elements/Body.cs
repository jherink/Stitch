using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HydraDoc.Elements.Interface;
using System.Windows.Media;

namespace HydraDoc.Elements
{
    public class Html : BaseParentElement, IHtmlElement
    {
        public override string Tag { get { return "html"; } }

        public IHeadElement Head
        {
            get
            {
                return Children.Where( t => t is IHeadElement )
                              .Select( t => t as IHeadElement )
                              .FirstOrDefault();
            }
        }

        public IBodyElement Body
        {
            get
            {
                return Children.Where( t => t is IBodyElement )
                               .Select( t => t as IBodyElement )
                               .FirstOrDefault();
            }
        }

        public override IEnumerable<IElement> GetAllNodes()
        {
            var allNodes = new List<IElement>();
            allNodes.Add( this );
            allNodes.AddRange( base.GetAllNodes() );
            return allNodes;
        }

        public override IEnumerable<IElement> GetNodes( string tagFilter )
        {
            var allNodes = new List<IElement>();
            if (Tag == tagFilter) allNodes.Add( this );
            allNodes.AddRange( base.GetAllNodes() );
            return allNodes;
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            base.AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( ">" );
            builder.Append( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }
    }

    public class Body : BaseParentElement, IBodyElement
    {
        public override string Tag { get { return "body"; } }

        public Uri Background { get; set; }

        public Color BackgroundColor { get; set; }

        public string LinkColor { get; set; }

        public string TextColor { get; set; }

        public string VisitedLinkColor { get; set; }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( "<body" );
            base.AppendIdAndClassInfoToTag( builder );
            builder.Append( ">" );
            builder.Append( RenderChildren() );
            builder.AppendLine( "</body>" );
            return builder.ToString();
        }
    }
}
