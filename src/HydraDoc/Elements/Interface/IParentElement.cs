using System.Collections.Generic;

namespace HydraDoc.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implementations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    public interface INodeQueryable
    {
        IElement FindById( string id );

        IEnumerable<IElement> GetAllNodes();

        IEnumerable<IElement> GetNodes( string tagFilter );
    }

    public interface IParentElement : IElement, INodeQueryable
    {
        ICollection<IElement> Children { get; set; }
    }

}
