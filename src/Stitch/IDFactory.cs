using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch
{

    public sealed class IDFactory
    {
        //private HashSet<Guid> Elements = new HashSet<Guid>();
        private int idIndex = 1;
        private const int elementBase = 26;
        private readonly int MaxLength = (int)Math.Ceiling( Math.Log( int.MaxValue, elementBase ) ); // should be 7.

        #region Singleton Implementation

        private static IDFactory _inst;

        private static IDFactory Inst
        {
            get
            {
                if (_inst == null) _inst = new IDFactory();
                return _inst;
            }
        }

        private IDFactory() { }

        #endregion

        //private string getElementGuid()
        //{
        //    // We use GUIDs as identifiers for our elements
        //    // however, HTML ids cannot start with a number.  
        //    // So, we will ensure that our identifiers start 
        //    // with the letter 'f'

        //    var distinct = false;
        //    var guid = default( Guid );

        //    while (!distinct)
        //    {
        //        guid = Guid.NewGuid();
        //        var bytes = guid.ToByteArray();

        //        if ((bytes[3] & 0xf0) < 0xa0)
        //        {
        //            bytes[3] |= 0xc0;
        //            guid = new Guid( bytes );
        //        }
        //        distinct = !Elements.Contains( guid );
        //    }
        //    Elements.Add( guid );
        //    return guid.ToString();
        //}

        //Changed to use a, b, c..., aa, ab, ac,... ba, bb ect.
        private string getElementId()
        {
            var id = new string( ' ', MaxLength ).ToCharArray();
            var current = idIndex;
            var offset = MaxLength;

            while (current > 0)
            {
                id[--offset] = (char)((--current % elementBase) + 'a');
                current /= elementBase;
            }

            idIndex++;
            return new string( id ).Trim();
        }

        /// <summary>
        /// Get a unique element id matching the assignment pattern a, b, c..., aa, ab... ba, bb, bc ect.
        /// </summary>
        /// <returns></returns>
        public static string GetElementId()
        {
            var id = string.Empty;
            lock(Inst)
            {
                id = Inst.getElementId();
            }
            return id;
        }
    }
}
