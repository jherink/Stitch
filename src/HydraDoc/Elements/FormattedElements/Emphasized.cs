using HydraDoc.Elements.Interface;

namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering emphasized formatted text.
    /// </summary>
    public class Emphasized : FormattedElement
    {
        public override string Tag { get { return "em"; } }

        /// <summary>
        /// Formatted text for emphasized text to be rendered.
        /// </summary>
        public Emphasized() { }

        /// <summary>
        /// Formatted text for emphasized text to be rendered.
        /// </summary>
        /// <param name="text">The text to render emphasized.</param>
        public Emphasized( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for emphasized text to be rendered.
        /// </summary>
        /// <param name="content">The content to render emphasized.</param>
        public Emphasized( IElement content ) : base( content ) { }

    }
}
