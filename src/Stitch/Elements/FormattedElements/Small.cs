using Stitch.Elements.Interface;

namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering small formatted text.
    /// </summary>
    public class Small : FormattedElement
    {
        public override string Tag { get { return "small"; } }

        /// <summary>
        /// Formatted text for small text to be rendered.
        /// </summary>
        public Small() { }

        /// <summary>
        /// Formatted text for small text to be rendered.
        /// </summary>
        /// <param name="text">The text to render small.</param>
        public Small( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for small text to be rendered.
        /// </summary>
        /// <param name="content">The content to render small.</param>
        public Small( IElement content ) : base( content ) { }

    }
}
