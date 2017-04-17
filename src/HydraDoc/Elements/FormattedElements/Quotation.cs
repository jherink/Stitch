
namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering quotations.
    /// </summary>
    public class Quotation : FormattedElement
    {
        public override string Tag { get { return "q"; } }

        /// <summary>
        /// Constructor for quotations.
        /// </summary>
        public Quotation() { }

        /// <summary>
        /// Constructor for quotations.
        /// </summary>
        /// <param name="text"></param>
        public Quotation( string text ) : base( text ) { }

    }
}
