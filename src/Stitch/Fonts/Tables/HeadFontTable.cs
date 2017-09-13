using System;

namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// Class for font "head" table which contains various information
    /// about the font.
    /// https://www.microsoft.com/typography/otspec/head.htm
    /// </summary>
    internal sealed class HeadFontTable : FontTable
    {
        public int MajorVersion { get; private set; }
        public int MinorVersion { get; private set; }
        public float FontRevision { get; private set; }
        public uint CheckSumAdjustment { get; private set; }
        public uint MagicNumber { get; private set; }
        public ushort Flags { get; private set; }
        public ushort UnitsPerEm { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Modified { get; private set; }
        public short XMin { get; private set; }
        public short YMin { get; private set; }
        public short XMax { get; private set; }
        public short YMax { get; private set; }
        public ushort MacStyle { get; private set; }
        public ushort LowestRecPPEM { get; private set; }
        public short FontDirectionHint { get; private set; }
        public short IndexToLocFormat { get; private set; }
        public short GlyphDataFormat { get; private set; }

        public HeadFontTable( byte[] data, uint offset ) : base( data, offset )
        {
        }

        protected override void Parse()
        {
            MajorVersion = GetUShort();
            MinorVersion = GetUShort();
            FontRevision = (float)( Math.Round( GetFixed() * 1000.0 ) / 1000.0 );
            CheckSumAdjustment = GetUInt();
            MagicNumber = GetUInt();
            if ( MagicNumber != 0x5F0F3CF5 ) { throw new Exception( "Magic number is not correct" ); }
            Flags = GetUShort();
            UnitsPerEm = GetUShort();
            Created = GetLongDateTime();
            Modified = GetLongDateTime();
            XMin = GetShort();
            YMin = GetShort();
            XMax = GetShort();
            YMax = GetShort();
            MacStyle = GetUShort();
            LowestRecPPEM = GetUShort();
            FontDirectionHint = GetShort();
            IndexToLocFormat = GetShort();
            GlyphDataFormat = GetShort();
        }
    }
}
