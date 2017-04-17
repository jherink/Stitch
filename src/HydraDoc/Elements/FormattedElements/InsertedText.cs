namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering inserted blocks of text.
    /// </summary>
    public class InsertedText : FormattedElement
    {
        public override string Tag { get { return "ins"; } }

        /// <summary>
        /// Formatted text to render as an inserted block of text.
        /// </summary>
        public InsertedText() { }

        /// <summary>
        /// Formatted text to render as an inserted block of text.
        /// <param name="text">The text to render.</param>
        /// </summary>
        public InsertedText( string text ) : base( text ) { }

    }
}
