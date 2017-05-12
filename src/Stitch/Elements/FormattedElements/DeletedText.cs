using Stitch.Elements.Interface;

namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering deleted formatted text.
    /// </summary>
    public class DeletedText : FormattedElement
    {
        public override string Tag { get { return "del"; } }

        /// <summary>
        /// Formatted text for deleted text to be rendered.
        /// </summary>
        public DeletedText() { }

        /// <summary>
        /// Formatted text for deleted text to be rendered.
        /// </summary>
        /// <param name="text">The text to render as deleted.</param>
        public DeletedText( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for deleted text to be rendered.
        /// </summary>
        /// <param name="content">The content to render as deleted.</param>
        public DeletedText( IElement content ) : base( content ) { }

    }
}
