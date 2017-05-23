using System;

namespace Stitch.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implementations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    public interface IImageElement : IElement
    {
        string Alt { get; set; }
        int Height { get; set; }
        Uri Src { get; set; }
        int Width { get; set; }
        bool ReferenceImage { get; set; }
        string GetBase64EncodedImage();
    }
}
