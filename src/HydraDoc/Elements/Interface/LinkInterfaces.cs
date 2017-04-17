using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implemtations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    /// <summary>
    /// Interface for link elements.
    /// </summary>
    public interface ILinkElement : IElement
    {
        //bool Disabled { get; set; }
        DOMString Href { get; set; }
        //string HrefLanguage { get; set; }
        //string Charset { get; set; }
    }

    /// <summary>
    /// Interface for anchor links.
    /// </summary>
    public interface IAnchorElement : ILinkElement, IParentElement
    {
        string Name { get; set; }
        //string Rel { get; set; }
        //string Rev { get; set; }
        //long TabIndex { get; set; }
    }
}
