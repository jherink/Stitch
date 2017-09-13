using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    public abstract class RelativeParser
    {
        protected byte[] Data;
        protected uint Offset;
        protected uint RelativeOffset;

        protected byte GetByte()
        {
            var v = ByteParser.GetByte( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( byte );
            return v;
        }

        protected char GetChar()
        {
            var v = ByteParser.GetChar( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( char );
            return v;
        }

        protected ushort GetUShort()
        {
            var v = ByteParser.GetUShort( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( ushort );
            return v;
        }

        protected short GetShort()
        {
            var v = ByteParser.GetShort( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( short );
            return v;
        }

        protected uint GetUInt()
        {
            var v = ByteParser.GetUInt( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( uint );
            return v;
        }

        protected int GetInt()
        {
            var v = ByteParser.GetInt( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( int );
            return v;
        }

        protected float GetFloat()
        {
            var v = ByteParser.GetFloat( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( float );
            return v;
        }

        protected float GetFixed()
        {
            var v = ByteParser.GetFixed( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( float );
            return v;
        }

        protected float GetF2Dot14()
        {
            return ( GetShort() / (float)16384.0 );

        }

        protected void Skip(uint size, uint amount = 0 )
        {
            if ( amount <= 0 ) amount = 1;
            RelativeOffset += (size * amount);
        }

        protected DateTime GetLongDateTime()
        {
            var v = ByteParser.GetDateTimeFromLong( Data, Offset + RelativeOffset );
            RelativeOffset += sizeof( long );
            return v;
        }

        protected string GetTag()
        {
            var v = ByteParser.GetTag( Data, Offset + RelativeOffset );
            RelativeOffset += 4;
            return v;
        }

        protected string GetString( uint size )
        {
            var v = ByteParser.GetString( Data, Offset + RelativeOffset, size );
            RelativeOffset += (uint)v.Length;
            return v;
        }
    }
}
