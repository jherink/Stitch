using System;
using System.Collections.Generic;

namespace Stitch.Fonts.Tables
{
    internal sealed class LocaFontTable : FontTable
    {
        public readonly ushort NumGlyphs;
        private readonly Func<uint> ParseFunction = null;
        private readonly bool ShortVersion;

        public readonly List<uint> GlyphOffsets = new List<uint>();

        public LocaFontTable( byte[] data, uint offset, ushort numGlyphs, bool shortVersion ) : base( data, offset )
        {
            NumGlyphs = numGlyphs;
            ShortVersion = shortVersion;
            ParseFunction = shortVersion ? new Func<uint>( () => GetUShort() ) : new Func<uint>( () => GetUInt() );
            Parse();
        }

        protected override void Parse()
        {
            if ( ParseFunction != null )
            { // check so base class doesn't parse until we are ready
                for ( int i = 0; i < NumGlyphs; i++ )
                {
                    var offset = ParseFunction.Invoke();

                    // The short table version stores the actual offset divided by 2.
                    if ( ShortVersion ) offset *= 2;

                    GlyphOffsets.Add( offset );
                }
            }
        }
    }
}
