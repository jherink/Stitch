using HydraDoc.Elements.Interface;

namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering strong formatted text.
    /// </summary>
    public class Strong : FormattedElement
    {
        public override string Tag { get { return "strong"; } }

        /// <summary>
        /// Formatted text for strong text to be rendered.
        /// </summary>
        public Strong() { }

        /// <summary>
        /// Formatted text for strong text to be rendered.
        /// </summary>
        /// <param name="text">The text to render strong.</param>
        public Strong( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for strong text to be rendered.
        /// </summary>
        /// <param name="content">The content to render strong</param>
        public Strong( IElement content ) : base( content ) { }

    }
}
