using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch
{
    internal sealed class HtmlTidier
    {
        const string HtmlAmpersand = "&amp;";
        const string HtmlApostrophe = "&apos;";
        const string HtmlQuote = "&quot;";
        const string HtmlLessThan = "&lt;";
        const string HtmlGreaterThan = "&gt;";

        private readonly StringBuilder Buffer = new StringBuilder();
        private readonly StringBuilder HoldBuffer = new StringBuilder();

        private bool IsDigit( char c ) { return c >= 0x30 && c <= 0x39; }
        private bool IsUppercase( char c ) { return c >= 0x41 && c <= 0x5A; }
        private bool IsLowercase( char c ) { return c >= 0x61 && c <= 0x7A; }
        private bool IsElementChar( char c )
        {
            return IsDigit( c ) || IsUppercase( c ) || IsLowercase( c ) ||
                   c == '_' || c == '.' || c == '-';
        }

        public string Tidy( string input )
        {
            Buffer.Clear();
            Buffer.Clear();
            Buffer.EnsureCapacity( input.Length );

            // Loop through only once.
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '&':
                        Buffer.Append( ParseAmpersand( input, ref i ) );
                        break;
                    case '\'':
                        //Buffer.Append( ParseApostrophe( input, ref i ) );
                        Buffer.Append( input[i] );
                        break;
                    case '"':
                        //Buffer.Append( ParseQuote( input, ref i ) );
                        Buffer.Append( input[i] );
                        break;
                    case '<':
                        Buffer.Append( ParseLessThan( input, ref i ) );
                        break;
                    case '>':
                        //Buffer.Append( ParseGreaterThan( input, ref i ) );
                        Buffer.Append( input[i] );
                        break;
                    default:
                        Buffer.Append( input[i] );
                        break;
                }
            }

            return Buffer.ToString();
        }

        private string ParseAmpersand( string input, ref int index )
        {
            // If we encounter an ampersand then make sure it isn't part of 
            // a XML Safe character already.
            
            var peek = new string( '&', 1 ); // we know it starts with an ampersand.
            int j = 1;
            for (j = 1; j < HtmlApostrophe.Length && index + j < input.Length; j++)
            {
                peek += input[index + j];
                if (HtmlLessThan.StartsWith( peek ) ||
                    HtmlGreaterThan.StartsWith( peek ) ||
                    HtmlQuote.StartsWith( peek ) ||
                    HtmlApostrophe.StartsWith( peek ) ||
                    HtmlAmpersand.StartsWith( peek ))
                {
                    break;
                }
            }
            index += j;
            return peek;
        }

        //private string ParseApostrophe( string input, ref int index )
        //{
        //    return input[index];
        //}

        //private string ParseQuote( string input, ref int index )
        //{
        //    return string.Empty;
        //}

        private string ParseLessThan( string input, ref int index )
        {
            // It hits the fan here... TODO.
            return string.Empty;
        }

        //private string ParseGreaterThan( string input, ref int index )
        //{
        //    return string.Empty;
        //}
    }
}
