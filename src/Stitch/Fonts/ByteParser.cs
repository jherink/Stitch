using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    /// <summary>
    /// Static toolkit for parsing byte collections as .NET types
    /// </summary>
    internal class ByteParser
    {
        private static byte[] getBytes( byte[] buffer, uint offset, int size )
        {
            var bytes = new byte[size];
            for ( var i = offset; i < offset + size; i++ )
            {
                if ( BitConverter.IsLittleEndian )
                {
                    bytes[( size - 1 ) - ( i - offset )] = buffer[i];
                }
                else
                {
                    bytes[i - offset] = buffer[i];
                }
            }
            return bytes;
        }

        public static byte GetByte( byte[] buffer, uint offset )
        {
            return buffer[offset];
        }

        public static char GetChar( byte[] buffer, uint offset )
        {
            return BitConverter.ToChar( getBytes( buffer, offset, sizeof( char ) ), 0 );
        }

        public static ushort GetUShort( byte[] buffer, uint offset )
        {
            return BitConverter.ToUInt16( getBytes( buffer, offset, sizeof( ushort ) ), 0 );
        }

        public static short GetShort( byte[] buffer, uint offset )
        {
            return BitConverter.ToInt16( getBytes( buffer, offset, sizeof( short ) ), 0 );
        }

        public static int GetInt( byte[] buffer, uint offset )
        {
            return BitConverter.ToInt32( getBytes( buffer, offset, sizeof( int ) ), 0 );
        }

        public static uint GetUInt( byte[] buffer, uint offset )
        {
            return BitConverter.ToUInt32( getBytes( buffer, offset, sizeof( uint ) ), 0 );
        }

        public static long GetLong( byte[] buffer, uint offset )
        {
            return BitConverter.ToInt64( getBytes( buffer, offset, sizeof( long ) ), 0 );
        }

        public static ulong GetULong( byte[] buffer, uint offset )
        {
            return BitConverter.ToUInt64( getBytes( buffer, offset, sizeof( ulong ) ), 0 );
        }

        public static float GetFloat( byte[] buffer, uint offset )
        {
            return BitConverter.ToSingle( getBytes( buffer, offset, sizeof( float ) ), 0 );
        }

        public static float GetFixed( byte[] buffer, uint offset )
        {
            float _decimal = GetShort( buffer, offset );
            float _fraction = GetUShort( buffer, offset + sizeof( short ) );
            return _decimal + _fraction / ushort.MaxValue;
        }

        public static DateTime GetDateTimeFromLong( byte[] buffer, uint offset )
        {
            return new DateTime( GetLong( buffer, offset ) );
        }

        public static string GetTag( byte[] buffer, uint offset )
        {
            const int TAG_SIZE = 4;
            return GetString( buffer, offset, TAG_SIZE );
        }

        public static short[] GetShortList(byte[] buffer, uint offset, uint size )
        {
            var _list = new short[size];
            for (int i = 0; i < size; i++ )
            {
                _list[i] = GetShort( buffer, offset );
                offset += sizeof( short );
            }
            return _list;
        }

        public static byte[] GetByteList( byte[] buffer, uint offset, uint size )
        {
            var _list = new byte[size];
            for ( int i = 0; i < size; i++ )
            {
                _list[i] = GetByte( buffer, offset );
                offset += sizeof( byte );
            }
            return _list;
        }
        
        public static string GetString( byte[] buffer, uint offset, uint size )
        {
            string str = string.Empty;
            for ( var i = offset; i < offset + size; i++ )
            {
                if ( buffer[i] == 0 || buffer[i] == 1 )
                {
                    str += buffer[i];
                }
                else
                {
                    str += Convert.ToChar( buffer[i] );
                }
            }
            return str;
        }
    }
}
