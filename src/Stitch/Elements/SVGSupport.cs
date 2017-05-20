namespace Stitch.Elements
{
    public class SVGPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public SVGPoint() { }
        public SVGPoint( double x, double y )
        {
            X = x; Y = y;
        }
    }
}
