using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Fonts
{
    public class Contour : List<PointData> { }

    public class Path
    {
        public readonly IReadOnlyList<PointData> Points;
        

        public Path(IReadOnlyList<PointData> points )
        {
            Points = points;

            // For our purposes I don't think this is needed.

            //var contours = GetContours();
            //for (int contourIndex = 0; contourIndex < contours.Count; contourIndex++ )
            //{
            //    var contour = contours[contourIndex];
            //    var prev = default( Contour );
            //    var curr = contour[contour.Count - 1];
            //    var next = contour[0];
            //    if ( curr.OnCurve )
            //    {

            //    }
            //}
        }

        public List<Contour> GetContours()
        {
            var contours = new List<Contour>();
            var currentContour = new Contour();
            foreach (var pt in Points )
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
