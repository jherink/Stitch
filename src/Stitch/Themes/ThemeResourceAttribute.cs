using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Themes
{
    internal class ThemeResourceAttribute : Attribute
    {
        public string ResourceName { get; set; }

        public ThemeResourceAttribute( string resourceName ) { ResourceName = resourceName; }
        
    }
}
