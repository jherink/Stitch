using Stitch.Elements.Interface;
using System;
using System.Text;

namespace Stitch.Elements
{
    public class ImageElement : BaseElement, IImageElement
    {
        public string Alt { get; set; }

        /// <summary>
        /// The height of the image in pixels. -1 indicates auto.
        /// </summary>
        public int Height { get; set; } = -1;
                
        public Uri Src { get; set; }

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

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} " ); // there must be a source.
            if (Src != null)
            {
                var src = Src.IsAbsoluteUri ? Src.AbsoluteUri : Src.OriginalString;
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
    }
}
