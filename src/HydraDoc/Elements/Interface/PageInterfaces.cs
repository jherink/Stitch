namespace HydraDoc.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implementations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    public interface IHtmlElement : IParentElement
    {
        IHeadElement Head { get; }
        IBodyElement Body { get; }
    }

    public interface IBodyElement : IParentElement
    {
    }

    public interface IHeadingElement : IElement
    {
        HeadingLevel Level { get; set; }
        
        DOMString Content { get; set; }
    }

    public interface IHeaderElement : IParentElement
    {
    }

    public interface ILabelElement : IElement
    {
        string For { get; set; }
        DOMString Text { get; set; }
    }

    public interface IParagraphElement : IElement, INodeQueryable
    {
        DOMString Content { get; set; }
    }

    public interface IDivElement : IParentElement
    {
    }

    public interface ISpanElement : IParentElement
    {
    }

    public interface IPreElement : IElement
    {
    }

    public interface ILineBreakElement : IElement
    {
    }

    public interface IHorizontalRuleElement : IElement
    {
    }
}
