using Stitch.Elements;
using Stitch.Fonts.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    public class PointData
    {
        public bool OnCurve;
        public bool LastPointOfContour;
        public float X;
        public float Y;
    }

    public class Component
    {
        public ushort GlyphIndex;
        public float xScale;
        public float Scale01;
        public float Scale10;
        public float yScale;
        public int Dx;
        public int Dy;
        public Tuple<ushort, ushort> MatchedPoints;
    }

    public class Contour : List<PointData> { }

    public sealed class Glyph : RelativeParser
    {
        public uint Index { get; private set; }

        public string Name { get; private set; }

        public uint Unicode { get; private set; }

        public short NumberOfContours { get; private set; }

        public List<uint> Unicodes { get; private set; } = new List<uint>();

        public short XMin { get; private set; }

        public short YMin { get; private set; }

        public short XMax { get; private set; }

        public short YMax { get; private set; }
        
        public List<ushort> EndPointIndicies;
        public byte[] Instructions;
        public List<PointData> Points;
        public List<Component> Components = new List<Component>();

        public bool IsComposite { get; private set; }

        public ushort AdvanceWidth { get; internal set; }

        public short LeftSideBearing { get; internal set; }

        public BoundingBox BoundingBox { get; internal set; }

        public Glyph(uint index, byte[] data, uint offset)
        {
            Index = index;
            base.Data = data;
            Offset = offset;
            RelativeOffset = 0;
            Parse();
        }

        public void AddUnicode(uint unicode)
        {
            if (Unicodes.Count == 0 )
            {
                Unicode = unicode;
            }
            Unicodes.Add( unicode );
        }
                
        private int ParseGlyphCoordinate(byte flag, int previousValue, byte shortVectorBitMask, byte sameBitMask )
        {
            int v = 0;
            if ((flag & shortVectorBitMask) > 0 )
            {
                // The coordinate is 1 byte long.
                v = GetByte();
                // The `same` bit is re-used for short values to signify the sign of the value.
                if ( (flag & sameBitMask) == 0 )
                {
                    v -= v;
                }

                v = previousValue + v;
            }
            else
            {
                // The coordinate is 2 bytes long.
                // If the `same` bit is set, the coordinate is the same as the previous coordinate.
                if ((flag & sameBitMask) > 0 )
                {
                    v = previousValue;
                } else
                {
                    // Parse the coordinate as a signed 16-bit delta value.
                    v = previousValue + GetShort();
                }
            }

            return v;
        }

        public void Parse()
        {
            NumberOfContours = GetShort();
            XMin = GetShort();
            YMin = GetShort();
            XMax = GetShort();
            YMax = GetShort();

            if (NumberOfContours > 0 )
            {
                EndPointIndicies = new List<ushort>( NumberOfContours );
                for (int i = 0; i < NumberOfContours; i++ )
                {
                    EndPointIndicies.Add( GetUShort() );
                }

                var numInstructions = GetUShort();
                Instructions = new byte[numInstructions];
                for (int i = 0; i < numInstructions; i++ )
                {
                    Instructions[i] = GetByte();
                }

                var numCoordinates = EndPointIndicies[EndPointIndicies.Count - 1] + 1;
                var flags = new List<byte>();
                for (int i = 0; i < numCoordinates; i++ )
                {
                    var flag = GetByte();
                    flags.Add( flag );
                    if ((flag & 8) > 0 )
                    { // If bit 3 is set, we repeat this flag n times, where n is the next byte.
                        var repeatCount = GetByte();
                        for (int j = 0; j < repeatCount; j++ )
                        {
                            flags.Add( flag );
                            i++;
                        }
                    }
                }

                if ( flags.Count != numCoordinates ) { throw new Exception( "Bad flags." ); } 

                if (EndPointIndicies.Count > 0 )
                {
                    var points = new List<PointData>();
                    if (numCoordinates > 0 )
                    {
                        for (int i = 0; i < numCoordinates; i++ )
                        {
                            var flag = flags[i];
                            points.Add( new PointData
                            {
                                OnCurve = (( flag & 1 ) == 1), // TODO: CHECK
                                LastPointOfContour = EndPointIndicies.IndexOf((ushort)i) >= 0
                            } );
                        }

                        var px = 0;
                        for (int i = 0; i < numCoordinates; i++ )
                        {
                            var flag = flags[i];
                            var point = points[i];
                            point.X = ParseGlyphCoordinate( flag, px, 2, 16 );
                            px = (int)point.X;
                        }

                        var py = 0;
                        for (int i = 0; i < numCoordinates; i++ )
                        {
                            var flag = flags[i];
                            var point = points[i];
                            point.Y = ParseGlyphCoordinate( flag, py, 4, 32 );
                            py = (int)point.Y;
                        }
                    }
                    Points = points;
                }
                else
                {
                    Points = new List<PointData>();
                }
            }
            else if(NumberOfContours == 0 )
            {
                Points = new List<PointData>();
            }
            else
            {
                IsComposite = true;
                Points = new List<PointData>();

                var moreComponents = true;
                ushort flags = default( ushort );
                while ( moreComponents )
                {
                    flags = GetUShort();
                    var component = new Component
                    {
                        GlyphIndex = GetUShort(),
                        xScale = 1,
                        Scale01 = 0,
                        Scale10 = 0,
                        yScale = 1,
                        Dx = 0,
                        Dy = 0
                    };

                    if ((flags & 1) > 0 )
                    { // The arguments are words
                        if ((flags & 2) > 0 )
                        {
                            // values are offset
                            component.Dx = GetShort();
                            component.Dy = GetShort();
                        }
                        else
                        {  // values are matched points
                            component.MatchedPoints = new Tuple<ushort, ushort>( GetUShort(), GetUShort() );
                        }
                    }
                    else
                    { // The arguments are bytes
                        if ((flags & 2) > 0)
                        {
                            component.Dx = GetByte();
                            component.Dy = GetByte();
                        }
                        else
                        {  // values are matched points
                            component.MatchedPoints = new Tuple<ushort, ushort>( GetUShort(), GetUShort() );
                        }
                    }

                    if ( ( flags & 8 ) > 0 )
                    { // We have a scale
                        component.xScale = component.yScale = GetF2Dot14();
                    }
                    else if ( ( flags & 64 ) > 0 )
                    { // We have an X / Y scale
                        component.xScale = GetF2Dot14();
                        component.yScale = GetF2Dot14();
                    }
                    else if ( ( flags & 128 ) > 0 )
                    { // We have a 2x2 transformation
                        component.xScale = GetF2Dot14();
                        component.Scale01 = GetF2Dot14();
                        component.Scale10 = GetF2Dot14();
                        component.yScale = GetF2Dot14();
                    }

                    Components.Add( component );
                    moreComponents = ( flags & 32 ) == 32; // TODO: CHECK
                }
                if ( (flags & 0x100) == 1 )
                { // We have instructions
                    var instructionLength = GetUShort();
                    Instructions = new byte[instructionLength];
                    for (int i = 0; i < instructionLength; i++ )
                    {
                        Instructions[i] = GetByte();
                    }
                }
            }

            BoundingBox = GetBoundingBox();
        }

        private BoundingBox GetBoundingBox()
        {
            if ( IsComposite )
            {
                // TODO
            }
            

            var contours = GetContours();

            var startX = 0.0f;
            var startY = 0.0f;
            var prevX = 0.0f;
            var prevY = 0.0f;

            var box = new BoundingBox();
            for (int i = 0; i < contours.Count; i++ )
            {
                var contour = contours[i];
                var prev = default(PointData);
                var curr = contour[contour.Count - 1];
                var next = contour[0];
                
                if ( curr.OnCurve )
                {
                    box.AddPoint( curr.X, curr.Y );
                    startX = prevX = curr.X;
                    startY = prevY = curr.Y;
                }
                else
                {
                    if ( next.OnCurve )
                    {
                        box.AddPoint( next.X, next.Y );
                        startX = prevX = next.X;
                        startY = prevY = next.Y;
                    }
                    else
                    {
                        var sStartX = 0.5f * ( curr.X + next.X );
                        var sStartY = 0.5f * ( curr.Y + next.Y );
                        box.AddPoint( sStartX, sStartY );
                        startX = prevX = sStartX;
                        startY = prevY = sStartY;
                    }
                }

                for (int j = 0; j < contour.Count; j++ )
                {
                    prev = curr;
                    curr = next;
                    next = contour[( i + 1 ) % contour.Count];
                    
                    if ( curr.OnCurve )
                    {
                        box.AddPoint( curr.X, curr.Y );
                        prevX = curr.X;
                        prevY = curr.Y;
                    }
                    else
                    {
                        var prev2 = prev;
                        var next2 = next;

                        if ( !prev.OnCurve )
                        {
                            prev2.X = 0.5f * ( curr.X + prev.X );
                            prev2.Y = 0.5f * ( curr.Y + prev.Y );
                            box.AddPoint( prev2.X, prev2.Y );
                            prevX = prev2.X;
                            prevY = prev2.Y;
                        }

                        if ( !next.OnCurve )
                        {
                            next2.X = 0.5f * ( curr.X + next.X );
                            next2.Y = 0.5f * ( curr.Y + next.Y );
                        }

                        box.AddPoint( prev2.X, prev2.Y );
                        prevX = prev2.X;
                        prevY = prev2.Y;

                        box.AddQuad( prevX, prevY, curr.X, curr.Y, next2.X, next2.Y );
                        prevX = next2.Y;
                        prevY = next2.Y;
                    }                    
                }
            }

            return box;
        }

        public List<Contour> GetContours()
        {
            var contours = new List<Contour>();
            var currentContour = new Contour();
            foreach ( var pt in Points )
            {
                currentContour.Add( pt );
                if ( pt.LastPointOfContour )
                {
                    contours.Add( currentContour );
                    currentContour = new Contour();
                }
            }

            if ( currentContour.Count != 0 ) throw new Exception( "There are still points left in the currenc contour." );
            return contours;
        }
    }
}
