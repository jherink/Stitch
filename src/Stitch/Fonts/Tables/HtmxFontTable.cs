using System.Collections.Generic;

namespace Stitch.Fonts.Tables
{
    /// <summary>
    /// Glyph metrics used for horizontal text layout include glyph advance widths, side bearings and X-direction min and max values (xMin, xMax). These are derived using a combination of the glyph outline data ('glyf', 'CFF ' or 'CFF2') and the horizontal metrics table. The horizontal metrics ('hmtx') table provides glyph advance widths and left side bearings.
    /// https://www.microsoft.com/typography/otspec/hmtx.htm
    /// </summary>
    internal sealed class HtmxFontTable : FontTable
    {
        public readonly int NumMetrics;
        public readonly int NumGlyphs;
        private readonly bool _parseFlag = false;
        private readonly Dictionary<uint, Glyph> GlyphSet;

        internal class LongHorMetric
        {
            ushort AdvanceWidth { get; set; }
            short LeftSideBearing { get; set; }
        }

        public HtmxFontTable( byte[] data, uint offset, int numMetrics, int numGlyphs, Dictionary<uint, Glyph> glyphSet ) : base( data, offset )
        {
            NumMetrics = numMetrics;
            NumGlyphs = numGlyphs;
            GlyphSet = glyphSet;
            _parseFlag = true;
            Parse();
        }

        protected override void Parse()
        {
            if ( _parseFlag )
            {
                ushort advanceWidth = 0;
                short leftSideBearing = 0;
                for (uint i = 0; i < NumGlyphs; i++ )
                {
                    if (i < NumMetrics )
                    {
                        advanceWidth = GetUShort();
                        leftSideBearing = GetShort();
                    }

                    var glyph = GlyphSet[i];
                    glyph.AdvanceWidth = advanceWidth;
                    glyph.LeftSideBearing = leftSideBearing;
                }
            }
        }
    }
}
