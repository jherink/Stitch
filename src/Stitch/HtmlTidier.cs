//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Stitch
//{
//    internal sealed class HtmlTidier
//    {
//        // HTML parsing specs taken from: https://html.spec.whatwg.org/multipage/syntax.html#attributes-2

//        const string HtmlAmpersand = "&amp;";
//        const string HtmlApostrophe = "&apos;";
//        const string HtmlQuote = "&quot;";
//        const string HtmlLessThan = "&lt;";
//        const string HtmlGreaterThan = "&gt;";

//        const char UTFLessThan = '\u003C';
//        const char UTFSolidus = '\u002F';
//        const char UTFGreaterThan = '\u003E';
//        const char UTFAmpersand = '\u0026';
//        const char UTFQuote = '\u0022';
//        //const char UTFLeftQuote = '\u201C';
//        //const char UTFRightQuote = '\u201D';
//        const char UTFApostrophe = '\u0027';
//        // AASCII Whitespace definition: https://infra.spec.whatwg.org/#ascii-whitespace
//        readonly char[] ASCIIWhitespace = { '\u0009', '\u000A', '\u000C', '\u000D', '\u0020' }; // TAB, LF, FF, CR, SPACE

//        private readonly StringBuilder Buffer = new StringBuilder();

//        private bool IsDigit( char c ) { return c >= 0x30 && c <= 0x39; }
//        private bool IsUppercase( char c ) { return c >= 0x41 && c <= 0x5A; }
//        private bool IsLowercase( char c ) { return c >= 0x61 && c <= 0x7A; }
//        private bool IsElementChar( char c )
//        {
//            return IsDigit( c ) || IsUppercase( c ) || IsLowercase( c ) ||
//                   c == '_' || c == '.' || c == '-';
//        }

//        private bool IsControlChar( char c )
//        {
//            return (c >= '\u0000' && c <= '\u001F') ||
//                   (c >= '\u007F' && c <= '\u009F') ||
//                   (c >= '\u0030' && c <= '\u0039') ||
//                   (c >= '\u0041' && c <= '\u0046') ||
//                   (c >= '\u0061' && c <= '\u0066') ||
//                   (c >= '\u0041' && c <= '\u005A') ||
//                   (c >= '\u0061' && c <= '\u007A');
//        }

//        public string Tidy( string input )
//        {
//            Buffer.Clear();
//            Buffer.Clear();
//            Buffer.EnsureCapacity( input.Length );

//            // Loop through only once.
//            for (int i = 0; i < input.Length; i++)
//            {
//                switch (input[i])
//                {
//                    case '&':
//                        Buffer.Append( ParseAmpersand( input, ref i ) );
//                        break;
//                    case '\'':
//                        Buffer.Append( HtmlApostrophe );
//                        break;
//                    case '"':
//                        Buffer.Append( HtmlQuote );
//                        break;
//                    case '<':
//                        Buffer.Append( ParseLessThan( input, ref i ) );
//                        break;
//                    case '>':
//                        //Buffer.Append( ParseGreaterThan( input, ref i ) );
//                        Buffer.Append( HtmlGreaterThan );
//                        break;
//                    default:
//                        Buffer.Append( input[i] );
//                        break;
//                }
//            }

//            return Buffer.ToString();
//        }

//        private string ParseAmpersand( string input, ref int index )
//        {
//            // If we encounter an ampersand then make sure it isn't part of 
//            // a XML Safe character already.

//            var peek = new string( '&', 1 ); // we know it starts with an ampersand.
//            var valid = false;
//            int j = 1;
//            for (j = 1; j < HtmlApostrophe.Length && index + j < input.Length; j++)
//            {
//                peek += input[index + j];
//                if (HtmlLessThan.StartsWith( peek ) ||
//                    HtmlGreaterThan.StartsWith( peek ) ||
//                    HtmlQuote.StartsWith( peek ) ||
//                    HtmlApostrophe.StartsWith( peek ) ||
//                    HtmlAmpersand.StartsWith( peek ))
//                {
//                    valid = HtmlLessThan.Equals( peek ) ||
//                            HtmlGreaterThan.Equals( peek ) ||
//                            HtmlQuote.Equals( peek ) ||
//                            HtmlApostrophe.Equals( peek ) ||
//                            HtmlAmpersand.Equals( peek );
//                    if (valid) break;
//                }
//                else
//                {
//                    valid = false;
//                    break;
//                }
//            }
//            index += j;
//            if (valid)
//            {
//                return peek;
//            }
//            else
//            {
//                return HtmlAmpersand + peek.Substring( 1 );
//            }
//        }

//        //private string ParseApostrophe( string input, ref int index )
//        //{
//        //    return input[index];
//        //}

//        //private string ParseQuote( string input, ref int index )
//        //{
//        //    return string.Empty;
//        //}

//        private string ParseLessThan( string input, ref int index )
//        {
//            // It hits the fan here... TODO.
//            var buffer = new StringBuilder();
//            var holdBuffer = new StringBuilder();
//            var k = 1;
//ValidateElementSequence:
//            while ((index + k < input.Length) && IsElementChar( input[index + k] ))
//            { // while in element tag.
//                holdBuffer.Append( input[index + k++] );
//            }

//            if (index + k < input.Length)
//            {
//                var curr = input[index + k];
//                if (curr == '>')
//                { // completes starting element tag.
//                    buffer.Append( input[index] ); // remember to add '<'
//                    buffer.Append( holdBuffer.ToString() );
//                    buffer.Append( input[index + k] ); // add '>'
//                    holdBuffer.Clear();
//                    index += k;
//                }
//                else if (curr == '/')
//                {   // this is valid if the previous character is a '<' (end tag like </div>)
//                    // or the next char is a '>' tag (self contained tag like <hr />
//                    if (input[index + k - 1] == '<' ||
//                       ((index + k + 1 < input.Length) && input[index + k + 1] == '>'))
//                    {
//                        holdBuffer.Append( input[index + k++] );
//                        goto ValidateElementSequence;
//                    }
//                }
//                else if (curr == ' ')
//                {
//                    while ((index + k < input.Length) && input[index + k] == ' ')
//                    {
//                        holdBuffer.Append( input[index + k++] );
//                    }
//                    if (index + k < input.Length)
//                    {
//                        if (input[index + k] == '/')
//                        {
//                            holdBuffer.Append( input[index + k++] );
//                            if ((index + k < input.Length) && input[index + k] == '>')
//                            {
//                                buffer.Append( input[index] );          // remember to add '<'
//                                buffer.Append( holdBuffer.ToString() ); // add hold
//                                buffer.Append( input[index + k] );      // add '>'
//                                holdBuffer.Clear();
//                                index += k;
//                            }
//                            else
//                            { // invalid
//                                FinishLessThanState( buffer, holdBuffer, ref index, ref k );
//                            }
//                        }
//                        else
//                        { // invalid
//                            FinishLessThanState( buffer, holdBuffer, ref index, ref k );
//                        }
//                    }
//                    else
//                    { // EOS
//                        FinishLessThanState( buffer, holdBuffer, ref index, ref k );
//                    }
//                }
//                else
//                { // invalid
//                    FinishLessThanState( buffer, holdBuffer, ref index, ref k );
//                }
//            }
//            else
//            { // EOS
//                FinishLessThanState( buffer, holdBuffer, ref index, ref k );
//            }

//            //if (holdBuffer.Length > 0)
//            //{ // there was element text
//            //    index += k;

//            //    while (index < input.Length && input[index] != '>')
//            //    { //
//            //        holdBuffer.Append( ParseProposedAttribute( input, ref index ) );
//            //    }

//            //    if (index > input.Length)
//            //    {
//            //        buffer.Append( HtmlLessThan );
//            //        if (holdBuffer.Length > 0)
//            //        {
//            //            buffer.Append( holdBuffer.ToString() );
//            //            //index += k - 1;
//            //        }
//            //    }
//            //    else {
//            //        buffer.Append( '<' );
//            //        buffer.Append( holdBuffer );
//            //    }
//            //}
//            //else {
//            //    Buffer.Append( HtmlLessThan );
//            //    index++;
//            //}

//            return buffer.ToString();
//        }
        

//        private void FinishLessThanState( StringBuilder buffer, StringBuilder holdBuffer, ref int i, ref int k )
//        {
//            buffer.Append( HtmlLessThan );
//            if (holdBuffer.Length > 0)
//            {
//                buffer.Append( holdBuffer.ToString() );
//                holdBuffer.Clear();
//                i += k - 1;
//            }
//        }

//        private string ParseProposedAttribute( string input, ref int i )
//        {
//            var buffer = new StringBuilder();
//            var inQuote = false;
//            var exit = false;
//            for (int j = 0; (j + i) < input.Length; j++)
//            {
//                switch (input[i + j])
//                {
//                    case ' ':
//                        buffer.Append( input[i + j] );
//                        break;
//                    case '>':
//                        if (!inQuote)
//                        {
//                            exit = true;
//                            j--;
//                        }
//                        break;
//                    case '"':
//                        buffer.Append( input[i + j] );
//                        if (inQuote)
//                        {
//                            exit = true;
//                        }
//                        inQuote = !inQuote;
//                        break;
//                    default:
//                        if (inQuote)
//                        {
//                            buffer.Append( input[i + j] );
//                        }
//                        else
//                        {
//                            if (IsElementChar( input[i + j] ))
//                            {
//                                buffer.Append( input[i + j] );
//                            }
//                            else if (input[i + j] == '=')
//                            {
//                                buffer.Append( input[i + j] );
//                            }
//                            else
//                            {
//                                exit = true;
//                            }
//                        }
//                        break;
//                }
//                if (exit)
//                {
//                    i += j + 1;
//                    break;
//                }
//            }
//            return buffer.ToString();
//        }

//        //private string ParseGreaterThan( string input, ref int index )
//        //{
//        //    return string.Empty;
//        //}

//        private string ParseElementAttribute( string input, ref int index )
//        {
//            return string.Empty;
//        }
//    }
//}
