using HydraDoc.Elements.Interface;
using System;
using System.Text;

namespace HydraDoc.Elements
{
    public class ImageElement : BaseElement, IImageElement
    {
        public string Alt { get; set; }

        /// <summary>
        /// The height of the image in pixels. -1 indicates auto.
        /// </summary>
        public int Height { get; set; } = -1;

        
        public Uri Src { get; set; }

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
            var src = Src.IsAbsoluteUri ? Src.AbsoluteUri : Src.OriginalString;
            builder.Append( $"<{Tag} src=\"{src}\"" ); // there must be a source.
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
