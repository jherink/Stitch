using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts.Tables
{
    internal sealed class KernFontTable : FontTable
    {
        public IReadOnlyDictionary<string, short> KernTable;

        public KernFontTable( byte[] data, uint offset ) : base( data, offset )
        {
        }

        protected override void Parse()
        {
            var tableVersion = GetUShort();
            if (tableVersion == 0 )
            { // Windows Kern table.
                KernTable = ParseWindowsKernTable();
            }
            else if (tableVersion == 1 )
            { // MAC Kern table.
                KernTable = ParseMacKernTable();
            }
            else
            {
                throw new Exception( $"Unsupported kern table version ( {tableVersion} )" );
            }
        }

        private Dictionary<string, short> ParseWindowsKernTable()
        {
            var pairs = new Dictionary<string, short>();

            // Skip nTables.
            Skip( sizeof( ushort ) );
            var subTableVersion = GetUShort();
            if ( subTableVersion != 0 ) throw new Exception( "Unsupported kern sub-table version." );

            // Skip subtableLength, subtableCoverage
            Skip( sizeof( ushort ), 2 );
            var nPairs = GetUShort();

            // Skip searchRange, entrySelector, rangeShift.
            Skip( sizeof( ushort ), 3 );
            for (int i = 0; i < nPairs; i++ )
            {
                var leftIdx = GetUShort();
                var rightIdx = GetUShort();
                var value = GetShort();
                pairs.Add( $"{leftIdx},{rightIdx}", value );
            }

            return pairs;
        }

        private Dictionary<string, short> ParseMacKernTable()
        {
            var pairs = new Dictionary<string, short>();

            // The Mac kern table stores the version as a fixed (32 bits) but we only loaded the first 16 bits.
            // Skip the rest.
            Skip( sizeof( ushort ) );
            var nTables = GetUInt();

            Skip( sizeof( uint ) );
            var converage = GetUShort();
            var subtableVersion = converage & 0xFF;

            Skip( sizeof( ushort ) );
            if ( subtableVersion == 0 )
            {
                var nPairs = GetUShort();

                // Skip searchRange, entrySelector, rangeShift.
                Skip( sizeof( ushort ), 3 );
                for ( int i = 0; i < nPairs; i++ )
                {
                    var leftIdx = GetUShort();
                    var rightIdx = GetUShort();
                    var value = GetShort();
                    pairs.Add( $"{leftIdx},{rightIdx}", value );
                }
            }

            return pairs;
        }
    }
}
