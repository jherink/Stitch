using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// Base class for Font Tables.
    /// </summary>
    public abstract class FontTable : RelativeParser
    {
        protected FontTable( byte[] data, uint offset )
        {
            Data = data;
            Offset = offset;
            RelativeOffset = 0;
            Parse();
        }

        protected abstract void Parse();

    }
}
