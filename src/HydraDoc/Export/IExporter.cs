using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Export
{
    public interface IExporter
    {
        byte[] Export( string content );
        void Export( string content, Stream outputStream );
    }
}
