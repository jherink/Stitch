using System;
using Stitch.Elements;
using Stitch.Elements.Interface;

namespace Stitch
{
    public interface IPage : IDivElement
    {
        int PageNumber { get; set; }
        string PageID { get; }
        PaperSize PageSize { get; set; }
        PageOrientation PageOrientation { get; set; }
        Margin Margin { get; set; }
    }

    public class Page : Div, IPage
    {
        private const string PAGE_NUMBER_ATT = "page-number";
        public int PageNumber
        {
            get
            {
                return int.Parse( Attributes[PAGE_NUMBER_ATT] );
            }
            set
            {
                if (value > 0) Attributes[PAGE_NUMBER_ATT] = value.ToString();
            }
        }

        public string PageID
        {
            get
            {
                return ID;
            }
        }

        public Margin Margin { get; set; } = new Margin();

        private PaperSize _pageSize = PaperSize.ANSI_A;

        public PaperSize PageSize
        {
            get { return _pageSize; }
            set
            {
                var currClass = PaperSizeHelper.GetClass( _pageSize, _pageOrientation );
                var newClass = PaperSizeHelper.GetClass( value, _pageOrientation );
                ClassList.Remove( currClass ).Add( newClass );
                _pageSize = value;
            }
        }

        private PageOrientation _pageOrientation = PageOrientation.Portrait;

        public PageOrientation PageOrientation
        {
            get
            {
                return _pageOrientation;
            }
            set
            {
                var currClass = PaperSizeHelper.GetClass( _pageSize, _pageOrientation );
                var newClass = PaperSizeHelper.GetClass( _pageSize, value );
                ClassList.Remove( currClass ).Add( newClass );
                _pageOrientation = value;
            }
        }

        public Page()
        {
            ID = IDFactory.GetElementId();
            ClassList.Add( "page" );
            Attributes.Add( "page-number", "0" );
        }

        public override string Render()
        {
            Margin.Apply( this );
            return base.Render();
        }

        public override object Clone()
        {
            var bClone = base.Clone() as Page;
            bClone.Margin = this.Margin.Clone() as Margin;
            return bClone;
        }
    }
}
