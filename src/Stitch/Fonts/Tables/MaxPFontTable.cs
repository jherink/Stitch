namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// This table establishes the memory requirements for this font. Fonts with CFF data must use Version 0.5 of this table, specifying only the numGlyphs field. Fonts with TrueType outlines must use Version 1.0 of this table, where all data is required.
    /// https://www.microsoft.com/typography/otspec/maxp.htm
    /// </summary>
    internal sealed class MaxPFontTable : FontTable
    {
        public float Version { get; private set; }
        public ushort NumGlyphs { get; private set; }
        public ushort MaxPoints { get; private set; }
        public ushort MaxContours { get; private set; }
        public ushort MaxCompositePoints { get; private set; }
        public ushort MaxCompositeContours { get; private set; }
        public ushort MaxZones { get; private set; }
        public ushort MaxTwilightPoints { get; private set; }
        public ushort MaxStorage { get; private set; }
        public ushort MaxFunctionDefs { get; private set; }
        public ushort MaxInstructionDefs { get; private set; }
        public ushort MaxStackElements { get; private set; }
        public ushort MaxSizeOfInstructions { get; private set; }
        public ushort MaxComponentElements { get; private set; }
        public ushort MaxComponentDepth { get; private set; }


        public MaxPFontTable( byte[] data, uint offset ) : base( data, offset )
        {
        }

        protected override void Parse()
        {
            Version = GetFixed();
            NumGlyphs = GetUShort();
            if ( Version == 1.0 )
            {
                MaxPoints = GetUShort();
                MaxContours = GetUShort();
                MaxCompositePoints = GetUShort();
                MaxCompositeContours = GetUShort();
                MaxZones = GetUShort();
                MaxTwilightPoints = GetUShort();
                MaxStorage = GetUShort();
                MaxFunctionDefs = GetUShort();
                MaxInstructionDefs = GetUShort();
                MaxStackElements = GetUShort();
                MaxSizeOfInstructions = GetUShort();
                MaxComponentElements = GetUShort();
                MaxComponentDepth = GetUShort();
            }
        }
    }
}
