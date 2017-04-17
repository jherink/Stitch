using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implementations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    public interface IListElement : IParentElement
    {
        IEnumerable<IListItemElement> Items { get; }
    }

    public interface IUnorderedListElement : IListElement
    {
        UnorderedListStyleType StyleType { get; set; }
    }

    public interface IOrderedListElement : IListElement
    {
        OrderedListStyleType StyleType { get; set; }
    }

    public interface IDescriptionList : IListElement
    {
    }

    public interface IListItemElement : IElement
    {
        DOMString Content { get; set; }
    }

    public interface IDescriptionTermElement : IListItemElement { }

    public interface IDescriptionDescribeElement : IListItemElement { }
}
