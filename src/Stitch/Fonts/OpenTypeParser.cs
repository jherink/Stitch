namespace Stitch.Fonts
{
    internal sealed class OpenTypeParser : FontParser
    {
        // https://www.microsoft.com/en-us/Typography/OpenTypeSpecification.aspx
        // Based on opentype.js https://github.com/nodebox/opentype.js
        protected override TableEntry[] ParseFontTables( byte[] buffer, int numTables )
        {
            TableEntry[] tableEntries = new TableEntry[numTables];
            uint p = 12; // offset to the first table directory entry.

            for (int i = 0; i < numTables; i++ )
            {
                tableEntries[i] = new TableEntry
                {
                    Tag = ByteParser.GetTag( buffer, p ),
                    CheckSum = ByteParser.GetUInt( buffer, p + 4 ),
                    Offset = ByteParser.GetUInt( buffer, p + 8 ),
                    Length = ByteParser.GetUInt( buffer, p + 12 ),
                    Compression = false
                };
                p += 16;
            }

            return tableEntries;
        }
    }
}
