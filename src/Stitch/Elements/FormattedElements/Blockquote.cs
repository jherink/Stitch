namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering block quotes.
    /// </summary>
    public class BlockQuote : FormattedElement
    {
        public override string Tag { get { return "blockquote"; } }

        /// <summary>
        /// The cite of the block quote.
        /// </summary>
        public string Cite { get; set; } = string.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BlockQuote() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">The quoted text.</param>
        /// <param name="cite">The URL of the document where the quoted text originated from.</param>
        public BlockQuote( string text, string cite ) { Text = text; Cite = cite; }

        public override string Render()
        {
            return $"<{Tag} cite=\"{Cite}\">{Text}</{Tag}>";
        }
    }
}
