using System.Collections.Generic;

namespace HydraDoc.Elements.Interface
{

    // All definitions were taken from actual DOM HTML element implementations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html

    public interface ITableElement : IParentElement
    {
        ITableCaptionElement TableCaption { get; set; }
        IEnumerable<ITableRowElement> Rows { get; } 
        ITableSectionElement TableHead { get; set; }
        ITableSectionElement TableFooter { get; set; }
        IEnumerable<ITableSectionElement> TableBodies { get; }
        ITableRowElement CreateRow();
        ITableRowElement CreateRow( params DOMString[] data );
        ITableSectionElement AddBody();              
    }

    public interface ITableCaptionElement : IElement
    {
        string Caption { get; set; }
    }

    public interface ITableSectionElement : IParentElement
    {
        IEnumerable<ITableRowElement> Rows { get; }
    }

    public interface ITableColumnElement : IElement
    {
        int Span { get; set; }
    }

    public interface ITableRowElement : IParentElement
    {
        ITableCellElement this[int index] { get; }
        long RowIndex { get; set; }
        long SectionRowIndex { get; set; }
        IEnumerable<ITableCellElement> Cells { get; }
    }

    public interface ITableCellElement : IElement
    {
        long CellIndex { get; set; }
        long RowSpan { get; set; }
        long ColSpan { get; set; }
        DOMString Content { get; set; }
    }
}
