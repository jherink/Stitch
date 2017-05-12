using Stitch.Elements.Interface;

namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering big formatted text.
    /// </summary>
    public class Big : FormattedElement
    {
        public override string Tag { get { return "big"; } }

        /// <summary>
        /// Formatted text for big text to be rendered.
        /// </summary>
        public Big() { }

        /// <summary>
        /// Formatted text for big text to be rendered.
        /// </summary>
        /// <param name="text">The text to render big.</param>
        public Big( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for big text to be rendered.
        /// </summary>
        /// <param name="content">The content to render big.</param>
        public Big( IElement content ) : base( content ) { }

    }
}
