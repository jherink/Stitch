using System;
using Stitch.Elements;
using Stitch.Elements.Interface;
using System.Collections.Generic;
using System.Text;

namespace Stitch
{
    public struct DOMString : IElement, ICloneable
    {
        public ClassList ClassList { get; set; }

        public StyleList StyleList { get; set; }

        public IDictionary<string, string> Attributes { get; private set; }

        public string ID { get; set; }

        public string Tag { get; set; }

        public string Title { get; set; }

        public string Text { get { return Render(); } }

        public IList<IElement> Elements { get; private set; }

        public string Render()
        {
            var builder = new StringBuilder();
            foreach (var element in Elements)
            {
                //builder.Append( XmlSafeRender( element.Render() ) );
                builder.Append( element.Render() );
            }
            return builder.ToString();
        }

        //private string XmlSafeRender( string renderedString )
        //{
        //    // GOAL: Loop through rendered string only once checking for 
        //    // special characters.

        //    //var tidier = new HtmlTidier();
        //    return tidier.Tidy( renderedString );

        //    const string amp = "&amp;";
        //    const string apos = "&apos;";
        //    const string quot = "&quot;";
        //    const string lt = "&lt;";
        //    const string gt = "&gt;";

        //    // start with a buffer the same size as the string being rendered.
        //    // StringBuilder will resize as necessary when swapping out values.
        //    var buffer = new StringBuilder( renderedString.Length );
        //    var holdBuffer = new StringBuilder();
        //    var inPossibleTag = false;

        //    for (int i = 0; i < renderedString.Length; i++)
        //    {
        //        switch (renderedString[i])
        //        {
        //            case '&':
        //                // If we encounter an ampersand then make sure it isn't part of 
        //                // a XML Safe character already.

        //                // POSSIBLE OPTIMIZATION: We can exit peek process if peek string
        //                // is not a substring of the special characters string.

        //                var peek = new string( '&', 1 ); // we know it starts with an ampersand.
        //                var next = false;
        //                for (int j = 1; j < apos.Length && i + j < renderedString.Length; j++)
        //                { // peek ahead 6 characters, maximum XML safe string length (apos is longest)
        //                    peek += renderedString[i + j];
        //                    switch (peek)
        //                    {
        //                        case amp:
        //                        case apos:
        //                        case quot:
        //                        case lt:
        //                        case gt:
        //                            // already XML Safe.  Add to buffer and move i counter ahead.
        //                            buffer.Append( peek );
        //                            i += j;
        //                            next = true;
        //                            break;
        //                    }
        //                    if (next) break;
        //                }

        //                if (!next) buffer.Append( amp );
        //                break;
        //            case '\'':  // Just swap
        //                buffer.Append( apos );
        //                break;
        //            case '"':   // Just swap
        //                buffer.Append( quot );
        //                break;
        //            case '<':   // Make sure its not a part of an HTML tag
        //                int k = 1;
        //                ValidateElementSequence: // for restarting validate only if validated exit char.
        //                while ((i + k < renderedString.Length) && ValidElementChar( renderedString[i + k] ))
        //                {
        //                    holdBuffer.Append( renderedString[i + k++] );
        //                }
        //                if (i + k < renderedString.Length)
        //                {
        //                    if (renderedString[i + k] == '>')
        //                    { // completes starting element tag.
        //                        buffer.Append( renderedString[i] );         // remember to add '<'
        //                        buffer.Append( holdBuffer.ToString() );
        //                        buffer.Append( renderedString[i + k] );     // add '>'
        //                        holdBuffer.Clear();
        //                        i += k;
        //                    }
        //                    else if (renderedString[i + k] == '/')
        //                    {   // this is valid if the previous character is a '<' (end tag like </div>)
        //                        // or the next char is a '>' tag (self contained tag like <hr />
        //                        if (renderedString[i + k - 1] == '<' ||
        //                           ((i + k + 1 < renderedString.Length) && renderedString[i + k + 1] == '>'))
        //                        {
        //                            holdBuffer.Append( renderedString[i + k++] ); // add '/' to hold buffer
        //                            goto ValidateElementSequence; // keep trying to validate.
        //                        }
        //                    }
        //                    else if (renderedString[i + k] == ' ')
        //                    {   // there can be any number of spaces between element text and a close element />
        //                        // like <hr />
        //                        while ((i + k < renderedString.Length) && renderedString[i + k] == ' ')
        //                        {
        //                            holdBuffer.Append( renderedString[i + k++] );
        //                        }
        //                        if (i + k < renderedString.Length)
        //                        {
        //                            if (renderedString[i + k] == '/')
        //                            {
        //                                holdBuffer.Append( renderedString[i + k++] );
        //                                if ((i + k < renderedString.Length) && renderedString[i + k] == '>')
        //                                { // valid
        //                                    buffer.Append( renderedString[i] );     // remember to add '<'
        //                                    buffer.Append( holdBuffer.ToString() ); // add hold
        //                                    buffer.Append( renderedString[i + k] ); // add '>'
        //                                    holdBuffer.Clear();
        //                                    i += k;
        //                                }
        //                                else
        //                                { // not valid
        //                                    buffer.Append( lt ); // swap out for lt
        //                                    if (holdBuffer.Length > 0)
        //                                    { // We checked some values.  Add them to buffer so we don't check them twice.
        //                                        buffer.Append( holdBuffer.ToString() );
        //                                        holdBuffer.Clear();
        //                                        i += k - 1;
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            { // not valid
        //                                buffer.Append( lt ); // swap out for lt
        //                                if (holdBuffer.Length > 0)
        //                                { // We checked some values.  Add them to buffer so we don't check them twice.
        //                                    buffer.Append( holdBuffer.ToString() );
        //                                    holdBuffer.Clear();
        //                                    i += k - 1;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {   // Read to the end of the string. Not an element.
        //                            buffer.Append( lt ); // swap out for lt
        //                            if (holdBuffer.Length > 0)
        //                            { // We checked some values.  Add them to buffer so we don't check them twice.
        //                                buffer.Append( holdBuffer.ToString() );
        //                                holdBuffer.Clear();
        //                                i += k - 1;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    { // Read to the end of the string. Not an element.
        //                        buffer.Append( lt ); // swap out for lt
        //                        if (holdBuffer.Length > 0)
        //                        { // We checked some values.  Add them to buffer so we don't check them twice.
        //                            buffer.Append( holdBuffer.ToString() );
        //                            holdBuffer.Clear();
        //                            i += k - 1;
        //                        }
        //                    }
        //                }
        //                else
        //                { // invalid tag.
        //                    buffer.Append( lt ); // swap out for lt
        //                    if (holdBuffer.Length > 0)
        //                    { // We checked some values.  Add them to buffer so we don't check them twice.
        //                        buffer.Append( holdBuffer.ToString() );
        //                        holdBuffer.Clear();
        //                        i += k - 1;
        //                    }
        //                }
        //                break;
        //            case '>':
        //                buffer.Append( gt ); // tag '>' cases are handled in '<' case -> just swap
        //                break;
        //            default: // not special -> put it in buffer.
        //                buffer.Append( renderedString[i] );
        //                break;
        //        }
        //    }

        //    return buffer.ToString();

        //    // TODO: Optimize & fix
        //    // Notes: Must do '&' first with string.replace so we don't overwrite
        //    // other valid XML elements.
        //    // Drawbacks: string.replace will break code that already has XML
        //    // safe elements in it.
        //    // Drawback: breaks elements like <big>.
        //    //return renderedString.Replace( "&", "&amp;" )
        //    //                     .Replace( "\"", "&quot;" )
        //    //                     .Replace( "'", "&apos;" )
        //    //                     .Replace( "<", "&lt;" )
        //    //                     .Replace( ">", "&gt;" );
        //}

        //private bool ValidElementChar( char c )
        //{
        //    return (c >= 0x30 && c <= 0x39) ||  // is digit
        //           (c >= 0x41 && c <= 0x5A) ||  // uppercase letter
        //           (c >= 0x61 && c <= 0x7A) ||  // lowercase letter
        //            c == '_' ||                // underscore
        //            c == '.' ||                // period
        //            c == '-';                   // hyphen
        //}

        public DOMString( string content ) : this( new PlainText( content ) )
        {
        }

        public DOMString( int value ) : this( value.ToString() ) { }

        public DOMString( double value ) : this( value.ToString() ) { }

        public DOMString( params IElement[] content )
        {
            ID = string.Empty;
            Tag = string.Empty;
            Title = string.Empty;
            ClassList = new ClassList();
            StyleList = new StyleList();
            Attributes = new Dictionary<string, string>();
            Elements = new List<IElement>();
            foreach (var item in content)
            {
                Elements.Add( item );
            }
        }

        //public static DOMString Create()
        //{
        //    var str = new DOMString();
        //    str.ID = str.Tag = str.Title = string.Empty;
        //    str.ClassList = new ClassList();
        //    str.Elements = new List<IElement>();
        //    return str;
        //}

        public void Append( string content )
        {
            AppendElement( new PlainText( content ) );
        }

        public void AppendElement( IElement content )
        {
            if (Elements == null) Elements = new List<IElement>();
            Elements.Add( content );
        }

        public void Clear()
        {
            Elements.Clear();

        }

        #region Implicit Operators With string

        public static implicit operator DOMString( string str )
        {
            return new DOMString( str );
        }

        public static implicit operator string( DOMString str )
        {
            return str.Render();

            // For whatever reason I got an Overflow exception
            // when checkin if str was null.  Do this and hope for the 
            // best...
            //return str != default( DOMString ) ? str.Render() : 
            //                                     default( string );
        }

        public static implicit operator DOMString( int value )
        {
            return new DOMString( value );
        }

        public static implicit operator DOMString( double value )
        {
            return new DOMString( value );
        }


        #endregion

        #region Operator Overloads

        public static DOMString operator +( DOMString left, DOMString right )
        {
            return new DOMString( left.Text + right.Text );
        }

        public static DOMString operator +( DOMString left, IElement right )
        {
            var result = (DOMString)left.Clone();
            result.Elements.Add( right );
            return result;
        }

        public static bool operator ==( DOMString left, string right )
        {
            return left.Render() == right;
        }

        public static bool operator !=( DOMString left, string right )
        {
            return left.Render() != right;
        }

        public static bool operator ==( string left, DOMString right )
        {
            return left == right.Render();
        }

        public static bool operator !=( string left, DOMString right )
        {
            return left != right.Render();
        }

        #endregion

        #region object overrides

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals( object obj )
        {
            try
            {
                var o = (DOMString)obj;
                if (o != null)
                {
                    return o.Text.Equals( Text );
                }
            }
            catch (InvalidCastException) { }

            var o2 = obj as string;
            if (o2 != null)
            {
                return o2.Equals( Text );
            }

            if (obj != null)
            {
                var o3 = obj.ToString();
                return o3.Equals( Text );
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        #endregion

        #region ICloneable Implementation

        public object Clone()
        {
            // this can happen if initialized from default struct constructor. :(
            if (Elements == null)
            {
                Elements = new List<IElement>();
            }
            if (ClassList == null)
            {
                ClassList = new ClassList();
            }

            var domString = (DOMString)MemberwiseClone();
            domString.ID = string.Empty;
            domString.Elements = new List<IElement>();
            domString.ClassList = new ClassList();
            foreach (var element in Elements)
            {
                domString.Elements.Add( element.Clone() as IElement );
            }
            foreach (var cls in ClassList)
            {
                domString.ClassList.Add( cls );
            }
            return domString;
        }

        #endregion
    }

}
