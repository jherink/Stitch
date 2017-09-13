namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// The "hhea" table.
    /// This table contains information for horizontal layout. The values in the minRightSidebearing, minLeftSideBearing and xMaxExtent should be computed using only glyphs that have contours. Glyphs with no contours should be ignored for the purposes of these calculations. All reserved areas must be set to 0.
    /// https://www.microsoft.com/typography/otspec/hhea.htm
    /// </summary>
    internal sealed class HheaFontTable : FontTable
    {
        public ushort MajorVersion { get; private set; }
        public ushort MinorVersion { get; private set; }
        public short Ascender { get; private set; }
        public short Descender { get; private set; }
        public short LineGap { get; private set; }
        public ushort AdvanceWidthMax { get; private set; }
        public short MinLeftSideBearing { get; private set; }
        public short MinRightSideBearing { get; private set; }
        public short xMaxExtent { get; private set; }
        public short CaretSlopeRise { get; private set; }
        public short CaretSlopeRun { get; private set; }
        public short CaretOffset { get; private set; }
        public short MetricDataFormat { get; private set; }
        /// <summary>
        /// Number of hMetric entries in 'hmtx' table
        /// </summary>
        public ushort NumberOfHMetrics { get; private set; }

        public HheaFontTable( byte[] data, uint offset ) : base( data, offset )
        {
        }

        protected override void Parse()
        {
            MajorVersion = GetUShort();
            MinorVersion = GetUShort();
            Ascender = GetShort();
            Descender = GetShort();
            LineGap = GetShort();
            AdvanceWidthMax = GetUShort();
            MinLeftSideBearing = GetShort();
            MinRightSideBearing = GetShort();
            xMaxExtent = GetShort();
            CaretSlopeRise = GetShort();
            CaretSlopeRun = GetShort();
            CaretOffset = GetShort();
            // 4 "reserved" places
            RelativeOffset += ( 4 * sizeof( short ) );
            MetricDataFormat = GetShort();
            NumberOfHMetrics = GetUShort();
        }
    }
}
