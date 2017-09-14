using System;
using System.Collections.Generic;
using System.Text;

namespace Stitch.Fonts.Tables
{
    internal sealed class LtagFontTable : FontTable
    {
        public IReadOnlyList<string> Tags { get; private set; }

        public LtagFontTable( byte[] data, uint offset ) : base( data, offset )
        {
        }

        protected override void Parse()
        {
            var start = Offset;
            var tableVersion = GetUInt();
            if ( tableVersion != 1 ) throw new Exception( "Unsupported ltag table version." );

            // The 'ltag' specification does not define any flags; skip the field.
            Skip( sizeof( uint ) );
            var numTags = GetUInt();

            var tags = new List<string>();
            var tag = new StringBuilder();
            for (var i = 0; i < numTags; i++ )
            {
                tag.Clear();
                var offset = start + GetUShort();
                var length = GetUShort();
                for (var j = offset; j < offset + length; j++ )
                {
                    tag.Append( GetChar() );
                }
                tags.Add( tag.ToString() );
            }

            Tags = tags;
        }
    }
}
