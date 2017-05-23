using Stitch.Elements.Interface;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stitch.Elements
{
    public class ImageElement : BaseElement, IImageElement
    {
        public string Alt { get; set; }

        /// <summary>
        /// The height of the image in pixels. -1 indicates auto.
        /// </summary>
        public int Height { get; set; } = -1;

        private Uri _src;
        private Task _downloadTask;

        public Uri Src
        {
            get
            {
                return _src;
            }
            set
            {
                _src = value;
                if (!ReferenceImage && _src != default( Uri ))
                {
                    var mimeType = Helpers.GetMIMEType( _src.IsAbsoluteUri ? _src.AbsoluteUri : _src.OriginalString );
                    _downloadTask = Task.Factory.StartNew( () =>
                     {
                         _base64Image = $"data:{mimeType};base64,{Convert.ToBase64String( DownloadImage() )}";

                     } );
                    _downloadTask.ContinueWith( t => t.Dispose() );
                    //_downloadTask = DownloadImage();
                    //_downloadTask.ContinueWith( t =>
                    //{
                    //    if (t.Result != null && t.Result.Length > 0) _base64Image = Convert.ToBase64String( t.Result );
                    //} );
                }
            }
        }

        private string _base64Image;

        public bool ReferenceImage { get; set; } = false;

        public ImageElement() { }

        public ImageElement( Uri src, string alt, int width = -1, int height = -1 ) : this()
        {
            Src = src;
            Alt = alt;
            Width = width;
            Height = height;
        }

        public ImageElement( string src, string alt = "", int width = -1, int height = -1 ) : this( default( Uri ), alt, width, height )
        {
            var isB = IsBase64String( src );

            var uri = default( Uri );
            // first try absolute and releative
            if (!Uri.TryCreate( src, UriKind.Absolute, out uri ) &&
                !Uri.TryCreate( src, UriKind.Relative, out uri ))
            {
                // if those didnt work try a hail mary. If that fails let the exceptions rain...
                uri = new Uri( src, UriKind.RelativeOrAbsolute );
            }
            Src = uri;
        }

        public override string Tag
        {
            get
            {
                return "img";
            }
        }

        /// <summary>
        /// The width of the image in pixels. -1 indicates auto.
        /// </summary>
        public int Width { get; set; } = -1;

        private byte[] DownloadImage()
        {
            var data = new byte[] { };
            if (_src.IsAbsoluteUri && !_src.IsFile)
            {
                using (var client = new WebClient())
                {
                    data = client.DownloadData( _src );
                }
            }
            else
            {
                if (System.IO.File.Exists( _src.OriginalString )) data = System.IO.File.ReadAllBytes( _src.OriginalString );
            }
            return data;
        }

        private bool IsBase64String( string input )
        {
            var regex = new Regex( "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$" );
            return regex.IsMatch( input );
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} " ); // there must be a source.
            if (Src != null)
            {
                var src = string.Empty;
                if (ReferenceImage)
                {
                    src = Src.IsAbsoluteUri ? Src.AbsoluteUri : Src.OriginalString;
                }
                else {
                    src = GetBase64EncodedImage();
                }
                builder.Append( $"src =\"{src}\"" );
            }
            if (!string.IsNullOrWhiteSpace( Alt )) builder.Append( $" alt=\"{Alt}\"" );
            AppendIdAndClassInfoToTag( builder );

            if (Height > 0 && Width > 0)
            {
                builder.Append( $" style=\"width:{Width}px;height:{Height}px;\"" );
            }
            else if (Width > 0)
            {
                builder.Append( $" style=\"width:{Width}px;\"" );
            }
            else if (Height > 0)
            {
                builder.Append( $" style=\"height:{Height}px;\"" );
            }
            builder.Append( " />" ); // require this close tag for XML parsing.

            return builder.ToString();
        }

        public override object Clone()
        {
            var imgClone = (IImageElement)base.Clone();
            if (Src != null) imgClone.Src = new Uri( Src.IsAbsoluteUri ? Src.AbsoluteUri : Src.OriginalString );
            return imgClone;
        }

        public string GetBase64EncodedImage()
        {
            if (_downloadTask != null && _downloadTask.Status != TaskStatus.RanToCompletion)
            {
                _downloadTask.Wait();
            }
            return _base64Image;
        }
    }
}
