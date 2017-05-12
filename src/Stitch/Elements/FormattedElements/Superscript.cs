using Stitch.Elements.Interface;

namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering superscript formatted text.
    /// </summary>
    public class Superscript : FormattedElement
    {
        public override string Tag { get { return "sup"; } }

        /// <summary>
        /// Formatted text for superscript text to be rendered.
        /// </summary>
        public Superscript() { }

        /// <summary>
        /// Formatted text for superscript text to be rendered.
        /// </summary>
        /// <param name="text">The text to render as superscript.</param>
        public Superscript( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for superscript text to be rendered.
        /// </summary>
        /// <param name="content">The content to render as superscript.</param>
        public Superscript( IElement content ) : base( content ) { }

    }
}
