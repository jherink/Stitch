using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Elements
{
    /// <summary>
    /// Class for rendering italic formatted text.
    /// </summary>
    public class Italic : FormattedElement
    {
        public override string Tag { get { return "i"; } }
        /// <summary>
        /// Formatted text for italic text to be rendered.
        /// </summary>
        public Italic() { }
        /// <summary>
        /// Formatted text for italic text to be rendered.
        /// </summary>
        /// <param name="text">The text to render in italic.</param>
        public Italic( string text ) : base( text ) { }

        /// <summary>
        /// Formatted text for italic text to be rendered.
        /// </summary>
        /// <param name="content">The element to render in italic.</param>
        public Italic( IElement content ) : base( content ) { }

        /// <summary>
        /// Initialize an italic member with a class list.  This is useful
        /// since many frameworks like font awesome and bootstrap render UTF 
        /// image characters in them.
        /// </summary>
        /// <param name="text">The text to render inside the italic.</param>
        /// <param name="classes">The classes to initialize the italics with.</param>
        public Italic( string text, params string[] classes ) : base( text )
        {
            ClassList.Add( classes );
        }

        public Italic( IElement content, params string[] classes ) : base( content )
        {
            ClassList.Add( classes );
        }
    }
}
