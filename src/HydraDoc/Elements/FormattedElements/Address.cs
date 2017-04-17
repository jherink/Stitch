using System;

namespace HydraDoc.Elements
{
    /// <summary>
    /// Class for rendering addresses.
    /// </summary>
    public class Address : FormattedElement
    {
        public override string Tag { get { return "address"; } }

        /// <summary>
        /// Formatted text for addresses to be rendered.
        /// </summary>
        public Address() { }

        /// <summary>
        /// Formatted text for addresses to be rendered.
        /// </summary>
        /// <param name="text">The address to render.</param>
        public Address( string text ) { Text = new DOMString( text ); }

        /// <summary>
        /// Append an address line.
        /// </summary>
        /// <param name="line">The address line to append.</param>
        public void Append( string line )
        {
            Text += new DOMString( line ) + new LineBreak() + Environment.NewLine;
        }
    }
}
