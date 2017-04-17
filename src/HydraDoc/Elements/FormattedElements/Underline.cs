using HydraDoc.Elements.Interface;

namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering underlined formatted text.
    /// </summary>
    public class Underline : FormattedElement
    {
        public override string Tag { get { return "u"; } }

        /// <summary>
        /// Formatted text for underlined text to be rendered.
        /// </summary>
        public Underline() { }

        /// <summary>
        /// Formatted text for underlined text to be rendered.
        /// <param name="text">The text to render as underlined.</param>
        /// </summary>
        public Underline( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for underlined text to be rendered.
        /// <param name="content">The content to render as underlined.</param>
        /// </summary>
        public Underline( IElement content ) : base( content ) { }
    }
}
