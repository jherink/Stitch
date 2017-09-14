namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// The OS/2 table consists of a set of metrics and other data that are required in OpenType fonts
    /// https://www.microsoft.com/typography/otspec/os2.htm
    /// </summary>
    internal class OS2FontTable : FontTable
    {
        public ushort Version { get; private set; }
        public short xAvgCharWidth { get; private set; }
        public ushort usWeightClass { get; private set; }
        public ushort usWidthClass { get; private set; }
        public ushort fsType { get; private set; }
        public short ySubscriptXSize { get; private set; }
        public short ySubscriptYSize { get; private set; }
        public short ySubscriptXOffset { get; private set; }
        public short ySubscriptYOffset { get; private set; }
        public short ySuperscriptXSize { get; private set; }
        public short ySuperscriptYSize { get; private set; }
        public short ySuperscriptXOffset { get; private set; }
        public short ySuperscriptYOffset { get; private set; }
        public short yStrikeoutSize { get; private set; }
        public short yStrikeoutPosition { get; private set; }
        public short sFamilyClass { get; private set; }
        public byte[] Panose { get; private set; } = new byte[10];
        public uint ulUnicodeRange1 { get; private set; }
        public uint ulUnicodeRange2 { get; private set; }
        public uint ulUnicodeRange3 { get; private set; }
        public uint ulUnicodeRange4 { get; private set; }
        public string achVendID { get; private set; }
        public ushort fsSelection { get; private set; }
        public ushort usFirstCharIndex { get; private set; }
        public ushort usLastCharIndex { get; private set; }
        public short sTypoAscender { get; private set; }
        public short sTypoDescender { get; private set; }
        public short sTypoLineGap { get; private set; }
        public ushort usWinAscent { get; private set; }
        public ushort usWinDescent { get; private set; }
        public uint ulCodePageRange1 { get; private set; }
        public uint ulCodePageRange2 { get; private set; }
        public short sxHeight { get; private set; }
        public short sCapHeight { get; private set; }
        public ushort usDefaultChar { get; private set; }
        public ushort usBreakChar { get; private set; }
        public ushort usMaxContext { get; private set; }
        public ushort usLowerOpticalPointSize { get; private set; }
        public ushort usUpperOpticalPointSize { get; private set; }


        public OS2FontTable( byte[] data, uint offset ) : base( data, offset ) { }

        protected override void Parse()
        {
            Version = GetUShort();
            xAvgCharWidth = GetShort();
            usWeightClass = GetUShort();
            usWidthClass = GetUShort();
            fsType = GetUShort();
            ySubscriptXSize = GetShort();
            ySubscriptYSize = GetShort();
            ySubscriptXOffset = GetShort();
            ySubscriptYOffset = GetShort();
            ySuperscriptXSize = GetShort();
            ySuperscriptYSize = GetShort();
            ySuperscriptXOffset = GetShort();
            ySuperscriptYOffset = GetShort();
            yStrikeoutSize = GetShort();
            yStrikeoutPosition = GetShort();
            sFamilyClass = GetShort();
            for ( var i = 0; i < Panose.Length; i++ )
            {
                Panose[i] = GetByte();
            }

            ulUnicodeRange1 = GetUInt();
            ulUnicodeRange2 = GetUInt();
            ulUnicodeRange3 = GetUInt();
            ulUnicodeRange4 = GetUInt();
            achVendID = GetTag();
            fsSelection = GetUShort();
            usFirstCharIndex = GetUShort();
            usLastCharIndex = GetUShort();
            sTypoAscender = GetShort();
            sTypoDescender = GetShort();
            sTypoLineGap = GetShort();
            usWinAscent = GetUShort();
            usWinDescent = GetUShort();
            if ( Version >= 1 )
            {
                ulCodePageRange1 = GetUInt();
                ulCodePageRange2 = GetUInt();
            }

            if ( Version >= 2 )
            {
                sxHeight = GetShort();
                sCapHeight = GetShort();
                usDefaultChar = GetUShort();
                usBreakChar = GetUShort();
                usMaxContext = GetUShort();
            }
        }
    }
}
