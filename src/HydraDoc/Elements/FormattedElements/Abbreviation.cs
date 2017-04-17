namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering abbreviations.
    /// </summary>
    public class Abbreviation : FormattedElement
    {
        public override string Tag { get { return "abbr"; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Abbreviation() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">The abbreviation text.</param>
        /// <param name="title">The text the abbreviation stands for.</param>
        public Abbreviation( string text, string title ) { Text = new DOMString( text ); Title = title; }

        public override string Render()
        {
            return $"<{Tag} title=\"{Title}\">{Text}</{Tag}>";
        }
    }
}
