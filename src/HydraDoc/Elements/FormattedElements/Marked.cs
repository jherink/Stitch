using HydraDoc.Elements.Interface;

namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering marked formatted text.
    /// </summary>
    public class Marked : FormattedElement
    {
        public override string Tag { get { return "mark"; } }

        /// <summary>
        /// Formatted text for marked text to be rendered.
        /// </summary>
        public Marked() { }

        /// <summary>
        /// Formatted text for marked text to be rendered.
        /// </summary>
        /// <param name="text">The text to render marked.</param>
        public Marked( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for marked text to be rendered.
        /// </summary>
        /// <param name="content">The content to render marked.</param>
        public Marked( IElement content ) : base( content ) { }

    }
}
