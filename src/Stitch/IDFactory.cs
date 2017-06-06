using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch
{
    public interface IIDFactory
    {
        string GetId();
        void Reset();
    }

    public sealed class GuidIDFactor : IIDFactory
    {
        private HashSet<Guid> Elements = new HashSet<Guid>();
        private readonly object _lock = new object();

        public string GetId()
        {
            // We use GUIDs as identifiers for our elements
            // however, HTML ids cannot start with a number.  
            // So, we will ensure that our identifiers start 
            // with the letter 'f'
            var distinct = false;
            var guid = default( Guid );

            lock (_lock)
            {
                while (!distinct)
                {
                    guid = Guid.NewGuid();
                    var bytes = guid.ToByteArray();

                    if ((bytes[3] & 0xf0) < 0xa0)
                    {
                        bytes[3] |= 0xc0;
                        guid = new Guid( bytes );
                    }
                    distinct = !Elements.Contains( guid );
                }
                Elements.Add( guid );
            }
            return guid.ToString();
        }

        public void Reset() { Elements.Clear(); }
    }

    public sealed class IDFactory : IIDFactory
    {
        private int idIndex = 1;
        private const int elementBase = 26;
        private readonly int MaxLength = (int)Math.Ceiling( Math.Log( int.MaxValue, elementBase ) ); // should be 7.
        private readonly object _lock = new object();

        public string GetId()
        {
            var id = new string( ' ', MaxLength ).ToCharArray();
            lock (_lock)
            {
                var current = idIndex;
                var offset = MaxLength;

                while (current > 0)
                {
                    id[--offset] = (char)((--current % elementBase) + 'a');
                    current /= elementBase;
                }

                idIndex++;
            }
            return new string( id ).Trim();
        }

        public void Reset()
        {
            idIndex = 1;
        }
    }
}
