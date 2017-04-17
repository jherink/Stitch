using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.CSS
{
    public class CSSResourceResolver
    {
        public readonly Uri ResolveUriSourceFile;
        public readonly StyleSheet StyleSheet;

        public CSSResourceResolver( Uri resolveUriSourceFile, bool autoResolve = true )
        {
            ResolveUriSourceFile = resolveUriSourceFile;
            StyleSheet = new Parser().Parse( GetCSS() );
            if (autoResolve) ResolveFonts();
        }

        public string GetCSS()
        {
            var css = string.Empty;
            if (ResolveUriSourceFile.IsAbsoluteUri && !ResolveUriSourceFile.IsFile)
            {
                using (var client = new WebClient())
                {
                    css = client.DownloadString( ResolveUriSourceFile );
                }
            }
            else
            {
                css = File.ReadAllText( ResolveUriSourceFile.OriginalString );
            }
            return css;
        }

        public byte[] GetContent( Uri uri )
        {
            byte[] content;
            if (uri.IsAbsoluteUri && !uri.IsFile)
            {
                using (var client = new WebClient())
                {
                    content = client.DownloadData( uri );
                }
            }
            else
            {
                content = File.ReadAllBytes( uri.OriginalString );
            }
            return content;
        }

        public void ResolveFonts()
        {
            foreach (var fontReference in StyleSheet.FontFaceDirectives)
            {
                foreach (var declaration in fontReference.Declarations.Where( t => t.Name.Equals( "src", StringComparison.InvariantCultureIgnoreCase ) ))
                {
                    ProcessFontTerm( declaration.Term );
                }
            }
        }

        private void ProcessFontTerm( Term term )
        {
            if (term.GetType() == typeof( TermList ))
            {
                foreach (var _term in (term as TermList))
                {
                    ProcessFontTerm( _term );
                }
            }
            else if (term.GetType() == typeof( PrimitiveTerm ))
            {
                var _term = term as PrimitiveTerm;
                var path = _term.Value.ToString(); // will be uri.
                Uri uri;
                string extension;

                if (ResolveUriSourceFile.IsAbsoluteUri && !ResolveUriSourceFile.IsFile)
                {
                    uri = new Uri( ResolveUriSourceFile, path );
                    extension = Path.GetExtension( uri.AbsolutePath );
                }
                else
                {
                    path = path.Replace( '/', '\\' ); // clean up uri source to a file path address if there is one.
                    uri = new Uri( Path.Combine( Path.GetDirectoryName( ResolveUriSourceFile.OriginalString ), path ), UriKind.Relative );
                    extension = Path.GetExtension( uri.OriginalString );
                }

                var data = Convert.ToBase64String( GetContent( uri ) );
                var mime = InterperateMimeType( extension );
                _term.Value = $"data:{mime};base64,{data}";
            }
        }

        private string InterperateMimeType( string extension )
        {
            if (string.Equals( extension, ".svg", StringComparison.InvariantCultureIgnoreCase ))
            {
                return "image/svg+xml";
            }
            if (string.Equals( extension, ".ttf", StringComparison.InvariantCultureIgnoreCase ))
            {
                return "application/x-font-truetype";
            }
            if (string.Equals( extension, ".otf", StringComparison.InvariantCultureIgnoreCase ))
            {
                return "application/x-font-opentype";
            }
            if (string.Equals( extension, ".woff", StringComparison.InvariantCultureIgnoreCase ))
            {
                return "application/font-woff";
            }
            if (string.Equals( extension, ".woff2", StringComparison.InvariantCultureIgnoreCase ))
            {
                return "application/font-woff2";
            }
            if (string.Equals( extension, ".eot", StringComparison.InvariantCultureIgnoreCase ))
            {
                return "application/vnd.ms-fontobject";
            }
            if (string.Equals( extension, ".sfnt", StringComparison.InvariantCultureIgnoreCase ))
            {
                return "application/font-sfnt";
            }
            return "mime/unknown";
        }
    }
}
