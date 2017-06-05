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

        public Margin Margin { get; set; }

        private PaperSize _pageSize;

        public PaperSize PageSize
        {
            get { return _pageSize; }
            set
            {
                var currClass = PaperSizeHelper.GetClass( _pageSize, PageOrientation );
                ClassList.Remove( currClass );
                _pageSize = value;
                var newClass = PaperSizeHelper.GetClass( _pageSize, PageOrientation );
                ClassList.Add( newClass );
            }
        }

        public PageOrientation PageOrientation { get; set; } = PageOrientation.Portrait;

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

    }
}
