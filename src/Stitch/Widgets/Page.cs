using System;
using Stitch.Elements;
using Stitch.Elements.Interface;

namespace Stitch
{
    public interface IPage : IParentElement {
        int PageNumber { get; set; }
        string PageID { get; }
    }

    public class Page : Div, IPage
    {
        public int PageNumber { get; set; }

        public string PageID
        {
            get
            {
                return _pageID;
            }
        }

        private readonly string _pageID;

        public Page()
        {
            _pageID = IDFactory.GetElementGuid();
            ClassList.Add( "page", "w3-container" );           
        }
        
    }
}
