using Stitch.Elements.Interface;

namespace Stitch.Elements
{

    /// <summary>
    /// Class for rendering Bold formatted text.
    /// </summary>
    public class Bold : FormattedElement
    {
        public override string Tag { get { return "b"; } }

        /// <summary>
        /// Formatted text for bold text to be rendered.
        /// </summary>
        public Bold() { }

        /// <summary>
        /// Formatted text for bold text to be rendered.
        /// </summary>
        /// <param name="text">The text to render in bold.</param>
        public Bold( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for bold text to be rendered.
        /// </summary>
        /// <param name="content">The content to render in bold.</param>
        public Bold( IElement content ) : base( content ) { }
    }
}
