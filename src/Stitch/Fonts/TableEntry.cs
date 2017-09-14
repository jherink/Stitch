using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    internal sealed class TableEntry
    {
        public string Tag { get; set; }

        public uint CheckSum { get; set; }

        public uint Offset { get; set; }

        public uint Length { get; set; }

        public bool Compression { get; set; }
    }
}
