using System;
using System.Collections.Generic;
using HydraDoc.Elements.Interface;
using HydraDoc.Elements;
using HydraDoc.CSS;
using System.Data;
using System.Linq;
using System.Text;

namespace HydraDoc
{
    public sealed class ElementFactory
    {
        private HashSet<Guid> Elements = new HashSet<Guid>();
        private int idIndex = 1;
        private const int elementBase = 26;
        private readonly int MaxLength = (int)Math.Ceiling( Math.Log( int.MaxValue, elementBase ) ); // should be 7.

        private string GetElementGuid()
        {
            // We use GUIDs as identifiers for our elements
            // however, HTML ids cannot start with a number.  
            // So, we will ensure that our identifiers start 
            // with the letter 'f'

            var distinct = false;
            var guid = default( Guid );

            while (!distinct)
            {
                guid = Guid.NewGuid();
                var bytes = guid.ToByteArray();

                if ((bytes[3] & 0xf0) < 0xa0)
                {
                    bytes[3] |= 0xc0;
                    guid = new Guid( bytes );
                }
                distinct = !Elements.Contains( guid );
            }
            Elements.Add( guid );
            return guid.ToString();
        }

        // Changed to use a, b, c..., aa, ab, ac,... bb ect.

        private string GetElementId()
        {
            var id = new string( ' ', MaxLength ).ToCharArray();
            var current = idIndex;
            var offset = MaxLength;

            while (current > 0)
            {
                id[--offset] = (char)((--current % 26) + 'a');
                current /= elementBase;
            }

            idIndex++;
            return new string( id ).Trim();
        }

        #region Singleton Implementation

        private static ElementFactory _inst;

        private static ElementFactory Inst
        {
            get
            {
                if (_inst == null) _inst = new ElementFactory();
                return _inst;
            }
        }

        private ElementFactory() { }

        #endregion

        #region Label

        ///// <summary>
        ///// Create a label element. 
        ///// </summary>
        ///// <param name="labelText">The text to put inside the element.</param>
        ///// <param name="id">Give the element an auto generated id.</param>
        ///// <returns></returns>
        //public static ILabelElement CreateLabel( DOMString labelText = default( DOMString ), bool id = true )
        //{
        //    return CreateLabel( labelText, id ? Inst.GetElementId() : string.Empty );
        //}

        ///// <summary>
        ///// Create a label element.
        ///// </summary>
        ///// <param name="labelText">The text to put inside the label element.</param>
        ///// <param name="id">The id of the element.</param>
        ///// <returns></returns>
        //public static ILabelElement CreateLabel( string id, DOMString labelText )
        //{
        //    return new Label
        //    {
        //        ID = id,
        //        Text = labelText
        //    };
        //}

        ///// <summary>
        ///// Create a label element.
        ///// </summary>
        ///// <param name="_for">The element the label is for.</param>
        ///// <param name="labelText">The text to put inside the label.</param>
        ///// <param name="id">The id to give the label.</param>
        ///// <returns></returns>
        //public static ILabelElement CreateLabel( IElement _for, DOMString labelText = default( DOMString ), bool id = true )
        //{
        //    var label = CreateLabel( _for, labelText, id ? Inst.GetElementId() : string.Empty );
        //    return label;
        //}

        //public static ILabelElement CreateLabel( IElement _for, string id, DOMString labelText = default( DOMString ) )
        //{
        //    var label =  CreateLabel( labelText, id );
        //    label.For = _for.ID;
        //    return label;
        //}

        public static ILabelElement CreateLabel( IElement _for, DOMString labelText = default( DOMString ) )
        {
            var label = CreateLabel( labelText );
            label.For = _for.ID;

            return label;
        }

        public static ILabelElement CreateLabel( DOMString labelText = default( DOMString ) )
        {
            return new Label
            {
                ID = Inst.GetElementId(),
                Text = labelText
            };
        }

        #endregion

        #region Paragraph

        public static IParagraphElement CreateParagraph( DOMString text = default( DOMString ) )
        {
            return new Paragraph
            {
                ID = Inst.GetElementId(),
                Content = text
            };
        }

        #endregion

        #region Div

        public static IDivElement CreateDiv( bool container = false )
        {
            var div = new Div
            {
                ID = Inst.GetElementId()
            };

            if (container)
            {
                div.ClassList.Add( "w3-container" );
            }

            return div;
        }

        public static IDivElement CreateDiv( bool container, params IElement[] content )
        {
            var div = CreateDiv( container );
            foreach (var item in content) div.Children.Add( item );
            return div;
        }

        public static IDivElement CreateDiv( params IElement[] contents )
        {
            return CreateDiv( false, contents );
        }

        #endregion

        #region Pre

        public static IPreElement CreatePre( DOMString text = default( DOMString ) )
        {
            return new Pre
            {
                ID = Inst.GetElementId(),
                Text = text
            };
        }

        #endregion

        #region Line Break & Horizontal Rule

        public static IHorizontalRuleElement CreateHorizontalRule( bool assignId = false )
        {
            return new HorizontalRule
            {
                ID = assignId ? Inst.GetElementId() : string.Empty
            };
        }

        public static ILineBreakElement CreateLineBreak( bool assignId = false )
        {
            return new LineBreak
            {
                ID = assignId ? Inst.GetElementId() : string.Empty
            };
        }

        #endregion

        #region Meta

        public static IMetaElement CreateMeta( string name, string content, string httpEquiv = "" )
        {
            var m = CreateMeta( name );
            m.Content = content;
            m.HttpEquiv = httpEquiv;
            return m;
        }

        public static IMetaElement CreateMeta( string name )
        {
            var m = CreateMeta();
            m.Name = name;
            return m;
        }

        public static IMetaElement CreateMeta()
        {
            return new Meta() { };
        }

        #endregion

        #region Styles

        //public static IStyleElement CreateStyleFromTheme( string theme )
        //{
        //    var resourceLoader = new CssThemeResourceLoader();

        //    return CreateStyle( resourceLoader.LoadTheme( theme ) );
        //}

        //public static IStyleElement CreateStyleFromResource( string resource )
        //{
        //    var loader = new CssThemeResourceLoader();
        //    return CreateStyle( loader.LoadCssResource( resource ) );
        //}

        //public static IStyleElement CreateStyle( StyleSheet stylesheet )
        //{
        //    return new Style() { StyleSheet = stylesheet };
        //    //var style = new Style();
        //    //stylesheet.
        //    //foreach (var rule in rules) style.Rules.Add( rule );
        //    //return style;
        //}

        #endregion

        #region Table

        public static ITableElement CreateTable( DataTable dt )
        {
            var table = CreateTable();
            var head = new TableHeader();
            var headerRow = CreateRow();
            foreach (DataColumn column in dt.Columns)
            {
                headerRow.Children.Add( CreateCell( column.ColumnName ) );
            }
            head.Children.Add( headerRow );
            table.TableHead = head;

            var body = table.AddBody();
            foreach (DataRow row in dt.Rows)
            {
                body.Children.Add( CreateRow( row ) );
            }
            //table.TableBodies.Add( body );
            return table;
        }

        public static ITableElement CreateTable( params string[] headers )
        {
            var table = new Table( headers ) { ID = Inst.GetElementId() };
            table.ClassList.Add( "w3-table" );
            return table;
        }

        public static ITableElement CreateTable()
        {
            return CreateTable( new string[] { } );
        }

        public static ITableRowElement CreateRow()
        {
            return new TableRow() { ID = Inst.GetElementId() };
        }

        public static ITableRowElement CreateRow( ITableElement table )
        {
            var lastRow = table.Rows.Last();
            throw new NotImplementedException();
        }

        public static ITableRowElement CreateRow( params DOMString[] data )
        {
            var tableRow = CreateRow();
            foreach (var item in data)
            {
                tableRow.Children.Add( CreateCell( item ) );
            }
            return tableRow;
        }

        public static ITableRowElement CreateRow( DataRow row )
        {
            var row2 = row.ItemArray.Select( t => new DOMString( new PlainText( t.ToString() ) ) ).ToArray();
            return CreateRow( row2 );
        }

        public static ITableCellElement CreateCell( DOMString cellData = default( DOMString ) )
        {
            return new TableCell
            {
                ID = Inst.GetElementId().ToString(),
                Content = cellData
            };
        }

        #endregion

        #region Lists

        public static IUnorderedListElement CreateUnorderedList()
        {
            return new UnorderedListElement() { ID = Inst.GetElementId() };
        }

        public static IUnorderedListElement CreateUnorderedList( IEnumerable<object> data, UnorderedListStyleType style = UnorderedListStyleType.Disc )
        {
            var list = CreateUnorderedList();
            list.StyleType = style;
            foreach (var d in data)
            {
                list.Children.Add( CreateListItem( d.ToString() ) );
            }
            return list;
        }

        public static IOrderedListElement CreateOrderedList()
        {
            return new OrderedListElement { ID = Inst.GetElementId() };
        }

        public static IOrderedListElement CreateOrderedList( IEnumerable<object> data, OrderedListStyleType style = OrderedListStyleType.Numbered )
        {
            var list = CreateOrderedList();
            list.StyleType = style;
            foreach (var d in data)
            {
                list.Children.Add( CreateListItem( d.ToString() ) );
            }
            return list;
        }

        public static IListItemElement CreateListItem( DOMString content = default( DOMString ) )
        {
            return new ListItemElement
            {
                ID = Inst.GetElementId(),
                Content = content
            };
        }

        public static IDescriptionTermElement CreateDescriptionTermItem( DOMString content = default( DOMString ) )
        {
            return new DescriptionTermElement
            {
                ID = Inst.GetElementId(),
                Content = content
            };
        }

        public static IDescriptionDescribeElement CreateDescriptionDescribeItem( DOMString content = default( DOMString ) )
        {
            return new DescriptionDescribeElement
            {
                ID = Inst.GetElementId(),
                Content = content
            };
        }


        #endregion

        #region Headings

        public static IHeadingElement CreateHeadingElement( HeadingLevel level = HeadingLevel.H1, DOMString content = default( DOMString ), bool assignId = false )
        {
            var h = new Heading( level ) { Content = content };
            if (assignId) h.ID = Inst.GetElementId();

            return h;
        }

        #endregion

        #region Header

        public static IHeaderElement CreateHeader()
        {
            return new Header() { ID = Inst.GetElementId() };
        }

        public static IHeaderElement CreateHeader( params IElement[] elements )
        {
            var header = CreateHeader();
            foreach (var element in elements)
            {
                header.Children.Add( element );
            }
            return header;
        }

        #endregion

        #region Image

        public static IImageElement CreateImage()
        {
            return new ImageElement { ID = Inst.GetElementId() };
        }

        public static IImageElement CreateImage( Uri src, string alt = "", int width = -1, int height = -1 )
        {
            var img = CreateImage();
            img.Src = src;
            img.Alt = alt;
            img.Width = width;
            img.Height = height;
            return img;
        }

        public static IImageElement CreateImage( string src, string alt = "", int width = -1, int height = -1 )
        {
            var uri = default( Uri );
            // first try absolute and releative
            if (!Uri.TryCreate( src, UriKind.Absolute, out uri ) &&
                !Uri.TryCreate( src, UriKind.Relative, out uri ))
            {
                // if those didnt work try a hail mary. If that fails let the exceptions rain...
                uri = new Uri( src, UriKind.RelativeOrAbsolute );
            }
            return CreateImage( uri, alt, width, height );
        }

        #endregion
    }
}
