using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Stitch.Elements
{
    public class Table : BaseParentElement, ITableElement
    {
        public override string Tag { get { return "table"; } }
        public IEnumerable<ITableRowElement> Rows
        {
            get
            {
                var rows = new List<ITableRowElement>();
                if (TableHead != null) rows.AddRange( TableHead.Rows );

                foreach (var body in TableBodies)
                {
                    rows.AddRange( body.Rows );
                }

                if (TableFooter != null) rows.AddRange( TableFooter.Rows );
                return rows;
            }
        }

        public IEnumerable<ITableSectionElement> TableBodies
        {
            get
            {
                return Children.Where( t => t is ITableSectionElement &&
                                            t.Tag == "tbody" )
                               .Select( t => t as ITableSectionElement );
            }
        }

        public ITableCaptionElement TableCaption { get; set; }

        private ITableSectionElement _tableFooter { get; set; }

        public ITableSectionElement TableFooter
        {
            get { return _tableFooter; }
            set
            {
                if (_tableFooter == null)
                {
                    _tableFooter = value;
                    Children.Add( _tableFooter );
                }
                else
                {
                    if (Children.Remove( _tableFooter ))
                    {
                        _tableFooter = value;
                        Children.Add( _tableFooter );
                    }
                }
            }
        }

        private ITableSectionElement _tableHead { get; set; }

        public ITableSectionElement TableHead
        {
            get { return _tableHead; }
            set
            {
                if (_tableHead == null)
                {
                    _tableHead = value;
                    Children.Add( _tableHead );
                }
                else
                {
                    if (Children.Remove( _tableHead ))
                    {
                        _tableHead = value;
                        Children.Add( _tableHead );
                    }
                }
            }
        }

        public override IList<IElement> Children
        {
            get
            {
                if (TableHead != null && TableHead.Rows.Any())
                {
                    TableHead.Rows.First().ClassList.Add( "stitch-theme" );
                }
                return base.Children;
            }

            set
            {
                base.Children = value;
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );

            builder.AppendLine( ">" ); // close <table>
            builder.AppendLine( RenderChildren() );

            builder.AppendLine( $"</{Tag}>" ); // close table
            return builder.ToString();
        }
        
        public ITableSectionElement AddBody()
        {
            var body = new TableBody();
            Children.Remove( TableFooter );
            Children.Add( body );
            if (TableFooter != null) Children.Add( TableFooter );
            return body;
        }

        public Table()
        {
            TableCaption = new TableCaption();
            Children.Add( TableCaption );
            TableHead = new TableHeader();
            Children.Add( new TableBody() );
            TableFooter = new TableFooter();
            ClassList.Add( "w3-table", "w3-table-all" );
            
        }

        public Table( DataTable table ) : this( )
        {
            var row = new TableRow();
            foreach (DataColumn column in table.Columns)
            {
                row.Children.Add( new TableCell( new DOMString( new PlainText( column.ColumnName ) ) ) );
            }
            if (row.Children.Any()) TableHead.Children.Add( row );

            var body = AddBody();
            foreach (DataRow tableRow in table.Rows)
            {
                CreateRow( tableRow );
            }
        }

        public Table( params string[] headings ) : this()
        {
            var tableRow = new TableRow();
            foreach (var heading in headings)
            {
                var cell = new TableCell();
                cell.Content.Append( heading );
                tableRow.Children.Add( cell );
            }
            if (tableRow.Children.Any()) TableHead.Children.Add( tableRow );
        }

        public ITableRowElement CreateRow()
        {
            var row = new TableRow();
            TableBodies.Last().Children.Add( row );
            return row;
        }

        public ITableRowElement CreateRow( DataRow row )
        {
            var row2 = row.ItemArray.Select( t => new DOMString( new PlainText( t.ToString() ) ) ).ToArray();
            return CreateRow( row2 );
        }

        public ITableRowElement CreateRow( params DOMString[] data )
        {
            var row = new TableRow();
            foreach (var item in data)
            {
                row.Children.Add( new TableCell( item ) );
            }
            TableBodies.Last().Children.Add( row );
            return row;
        }
    }

    public abstract class TableSectionElement : BaseParentElement, ITableSectionElement
    {
        public IEnumerable<ITableRowElement> Rows
        {
            get
            { // every time this is fetched re-number the rows in the section.
                var i = 0;
                var _rows = Children.Where( t => t is ITableRowElement ).Select( t => t as ITableRowElement ).ToList();
                foreach (var row in _rows)
                {
                    row.SectionRowIndex = i++;
                }
                return _rows;
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.AppendLine( $"<{Tag}>" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }
    }

    public class TableHeader : TableSectionElement
    {
        public override string Tag { get { return "thead"; } }
        public override string Render()
        {
            var builder = new StringBuilder();
            builder.AppendLine( "<thead>" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( "</thead>" );
            return builder.ToString().Replace( "<td", "<th" ).Replace( "</td>", "</th>" );
        }
        public TableHeader() { }

        public TableHeader( params string[] headers )
        {
            Children.Add( new TableRow( headers.Select( h => new DOMString( h ) ).ToArray() ) );
        }
    }

    public class TableBody : TableSectionElement
    {
        public override string Tag { get { return "tbody"; } }
    }

    public class TableFooter : TableSectionElement
    {
        public override string Tag { get { return "tfoot"; } }
    }

    public class TableCaption : BaseElement, ITableCaptionElement
    {
        public string Caption { get; set; }
        public override string Tag { get { return "caption"; } }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( $">{Caption}</{Tag}>" );
            return builder.ToString();
        }

        public TableCaption() { }

        public TableCaption( string caption ) { Caption = caption; }
    }

    public class TableRow : BaseParentElement, ITableRowElement
    {
        public override string Tag { get { return "tr"; } }

        public IEnumerable<ITableCellElement> Cells
        {
            get
            {
                return Children.Where( t => t is ITableCellElement ).Select( t => t as ITableCellElement );
                //var lastCell = default( ITableCellElement );
                //foreach (var cell in _cells)
                //{
                //    cell.CellIndex = lastCell == null ? 0 : lastCell.CellIndex + lastCell.ColSpan;
                //    lastCell = cell;
                //}
                //return _cells;
            }
        }

        public long RowIndex { get; set; }

        public long SectionRowIndex { get; set; }

        public ITableCellElement this[int index]
        {
            get
            {
                return Cells.ElementAt( index );
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( "<tr" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( ">" );
            builder.AppendLine( RenderChildren() );
            //foreach (var cell in Cells)
            //{
            //    builder.AppendLine( cell.Render() );
            //}
            builder.AppendLine( "</tr>" );
            return builder.ToString();
        }

        //public IElement FindById( string id )
        //{
        //    return Cells.FirstOrDefault( cell => cell.ID == id );
        //}

        //public IEnumerable<IElement> GetAllNodes()
        //{
        //    return Cells;
        //}

        //public IEnumerable<IElement> GetNodes( string tagFilter )
        //{
        //    return Cells.Where( t => t.Tag == tagFilter );
        //}

        public TableRow() { }

        public TableRow( params DOMString[] cells )
        {
            foreach (var cellData in cells) Children.Add( new TableCell( cellData ) );
        }

        //public override object Clone()
        //{
        //var baseClone = (ITableRowElement)base.Clone();
        //baseClone.Cells = new List<ITableCellElement>();
        //foreach (var cell in _cells)
        //{
        //    baseClone.Cells.Add( (ITableCellElement)cell.Clone() );
        //}

        //return baseClone;
        //}
    }

    public class TableCell : BaseElement, ITableCellElement
    {
        public override string Tag { get { return "td"; } }

        public long CellIndex { get; set; }
        public long RowSpan { get; set; } = 1;
        public long ColSpan { get; set; } = 1;

        public DOMString Content { get; set; }

        public TableCell() : this( new DOMString( string.Empty ) ) { }

        public TableCell( DOMString content ) { Content = content; }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            if (RowSpan > 1) builder.Append( $" rowspan=\"{RowSpan}\"" );
            if (ColSpan > 1) builder.Append( $" colspan=\"{ColSpan}\"" );
            builder.AppendLine( $">{Content.ToString()}</{Tag}>" );
            return builder.ToString();
        }

        public override object Clone()
        {
            var baseClone = (ITableCellElement)base.Clone();
            baseClone.Content = (DOMString)Content.Clone();

            return baseClone;
        }
    }

    public class HeaderCell : TableCell
    {
        public override string Tag { get { return "th"; } }
    }
}
