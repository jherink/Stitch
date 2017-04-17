using System;
using System.Collections.Generic;

namespace HydraDoc.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implemtations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    /// <summary>
    /// Interface for all element objects.
    /// </summary>
    public interface IElement : ICloneable
    {
        /// <summary>
        /// The ID of the element.
        /// </summary>
        string ID { get; set; }
        /// <summary>
        /// The title of the element.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// The class definitions of the element.
        /// </summary>
        ClassList ClassList { get; set; }
        /// <summary>
        /// The style rules of the element.
        /// </summary>
        StyleList StyleList { get; set; }
        /// <summary>
        /// The tag that this element uses.
        /// </summary>
        string Tag { get; }
        /// <summary>
        /// A dictionary of custom element attributes.
        /// </summary>
        IDictionary<string, string> Attributes { get; }
        /// <summary>
        /// Render the HTML for this element.
        /// </summary>
        /// <returns>The HTML for this element.</returns>
        string Render();
    }
}
