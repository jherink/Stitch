using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    internal class PathCommand
    {
        public string Command;
        public double X;
        public double Y;
    }

    internal sealed class Path : Queue<PathCommand>
    {

    }
}
