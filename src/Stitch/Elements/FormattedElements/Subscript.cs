using Stitch.Elements.Interface;

namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering subscripted formatted text.
    /// </summary>
    public class Subscript : FormattedElement
    {
        public override string Tag { get { return "sub"; } }

        /// <summary>
        /// Formatted text for bold text to be rendered.
        /// </summary>
        public Subscript() { }

        /// <summary>
        /// Formatted text for bold text to be rendered.
        /// </summary>
        /// <param name="text">The text to render in bold.</param>
        public Subscript( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for bolt text to be rendered.
        /// </summary>
        /// <param name="content">The content to render as subscript.</param>
        public Subscript( IElement content ) : base( content ) { }

    }
}
