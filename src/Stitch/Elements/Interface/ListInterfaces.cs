using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implementations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    public interface IListElement : IParentElement
    {
        IEnumerable<IListItemElement> Items { get; }
        ListStyleType StyleType { get; set; }
    }

    public interface IUnorderedListElement : IListElement
    {
    }

    public interface IOrderedListElement : IListElement
    {
        
    }

    public interface IDescriptionList : IParentElement
    {
    }

    public interface IListItemElement : IParentElement
    {
        DOMString Content { get; set; }
    }

    public interface IDescriptionTermElement : IListItemElement { }

    public interface IDescriptionDescribeElement : IListItemElement { }
}
