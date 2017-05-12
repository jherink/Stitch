using Stitch.CSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Elements.Interface
{
    // All definitions were taken from actual DOM HTML element implemtations 
    // found here: https://www.w3.org/TR/DOM-Level-1/level-one-html.html
    // except for IHeadInterface.  I made that one up to group metadata and 
    // style information in a header.   

    public interface IHeadElement : IElement
    {
        ICollection<IMetaElement> Metas { get; set; }
        ICollection<IStyleElement> Styles { get; set; }
        string Render( IEnumerable<string> tags, IEnumerable<string> classes );
    }

    public interface IMetaElement : IElement
    {
        string HttpEquiv { get; set; }
        string Content { get; set; }
        string Name { get; set; }
    }

    public interface IStyleElement : IElement
    {
        string Media { get; set; }
        bool Disabled { get; set; }
        string Type { get; set; }   
        StyleSheet StyleSheet { get; set; }
        string Render( IEnumerable<string> tags, IEnumerable<string> classes );
    }
}
