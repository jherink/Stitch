using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    public sealed class GlyphSet : Dictionary<uint, Glyph>
    {
        private Dictionary<uint, Glyph> UnicodeMap = new Dictionary<uint, Glyph>();

        public Glyph LookupGlyph( uint unicode )
        {
            if ( UnicodeMap.Count < Count )
            {
                FormUnicodeMap();
            }
            return UnicodeMap[unicode];
        }

        private void FormUnicodeMap()
        {
            UnicodeMap.Clear();
            foreach ( var pair in this )
            {
                foreach ( var unicode in pair.Value.Unicodes )
                {
                    UnicodeMap.Add( unicode, pair.Value );
                }
            }
        }
    }
}
