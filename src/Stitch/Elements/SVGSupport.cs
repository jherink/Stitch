namespace Stitch.Elements
{
    public class SVGPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public SVGPoint() { }
        public SVGPoint( int x, int y )
        {
            X = x; Y = y;
        }
    }
}
