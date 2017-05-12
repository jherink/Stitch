using Stitch.Elements.Interface;

namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering plain text.
    /// </summary>
    public class PlainText : FormattedElement
    {
        public override string Tag
        {
            get
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Formatted text for plain text to be rendered.
        /// </summary>
        public PlainText() { }

        /// <summary>
        /// Formatted text for plain text to be rendered.
        /// <param name="text">The text to render.</param>
        /// </summary>
        public PlainText( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for plain text to be rendered.
        /// <param name="element">The element to render as plain text.</param>
        /// </summary>
        public PlainText( IElement element ) : base( element ) { }

        public override string Render()
        {
            return Text;
        }
    }
}
