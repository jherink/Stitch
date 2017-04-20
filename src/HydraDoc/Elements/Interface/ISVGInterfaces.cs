using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Elements.Interface
{
    public interface ISVG : IParentElement
    {
        double Width { get; set; }
        double Height { get; set; }
    }

    public interface ISVGGroup : ISVGElement {
        IList<IElement> Children { get; }
        void Add( IElement element );
    }

    /// <summary>
    /// Base interface for SVG elements.
    /// </summary>
    public interface ISVGElement : IElement
    {
        /// <summary>
        /// Any CSS color value.
        /// </summary>
        string Fill { get; set; }
        /// <summary>
        /// Defines the "border" color of particular shapes and paths.
        /// </summary>
        string Stroke { get; set; }
        /// <summary>
        /// Defines the width of the strokes.
        /// </summary>
        int StrokeWidth { get; set; }
        /// <summary>
        /// Which shape the end of an open path will take and there are 
        /// four value options: butt, round, square, inherit.
        /// </summary>
        string StrokeLineCap { get; set; }
        /// <summary>
        /// How the corners of strokes will look on paths and basic shapes.
        /// </summary>
        string StrokeLineJoin { get; set; }
        /// <summary>
        /// When two lines meet at a sharp angle and are set to a stroke-linejoin="miter",
        /// this allows for the specification of how far this joint/corner extends.
        /// </summary>
        double StrokeMiterLimit { get; set; }
        /// <summary>
        /// The distance into the dash pattern to start the dash.
        /// </summary>
        int StrokeDashOffset { get; set; }
        /// <summary>
        /// Distances between dashes.
        /// </summary>
        int[] StrokeDashArray { get; set; }
        /// <summary>
        /// Set transparency level on strokes.
        /// </summary>
        double StrokeOpacity { get; set; }
        /// <summary>
        /// Set transparency level on fills.
        /// </summary>
        double FillOpacity { get; set; }
        /// <summary>
        /// Set svg elements opacity
        /// </summary>
        double Opacity { get; set; }
        /// <summary>
        /// The fill rule.
        /// </summary>
        string FillRule { get; set; }
        /// <summary>
        /// The transformation rule for this element.
        /// </summary>
        string Transform { get; set; }
    }

    public interface ISVGRectangle : ISVGElement
    {
        double X { get; set; }
        double Y { get; set; }
        double Height { get; set; }
        double Width { get; set; }
        double Rx { get; set; }
        double Ry { get; set; }
    }

    public interface ISVGEllipse : ISVGElement
    {
        /// <summary>
        /// Horizontal Radius
        /// </summary>
        double Rx { get; set; }
        /// <summary>
        /// Vertical Radius.
        /// </summary>
        double Ry { get; set; }
        /// <summary>
        /// Center X coordinate of ellipse.
        /// </summary>
        double Cx { get; set; }
        /// <summary>
        /// Center Y coordinate of ellipse.
        /// </summary>
        double Cy { get; set; }
    }

    public interface ISVGCircle : ISVGElement
    {
        /// <summary>
        /// Center X coordinate of circle.
        /// </summary>
        double Cx { get; set; }
        /// <summary>
        /// Center Y coordinate of circle.
        /// </summary>
        double Cy { get; set; }
        /// <summary>
        /// Radius of circle.
        /// </summary>
        double R { get; set; }
    }

    public interface ISVGLine : ISVGElement
    {
        /// <summary>
        /// The start of the line on the x-axis.
        /// </summary>
        double X1 { get; set; }
        /// <summary>
        /// The start of the line on the y-axis.
        /// </summary>
        double Y1 { get; set; }
        /// <summary>
        /// The end of the line on the x-axis.
        /// </summary>
        double X2 { get; set; }
        /// <summary>
        /// The end of the line on the y-axis.
        /// </summary>
        double Y2 { get; set; }
    }

    public interface ISVGPolyItem : ISVGElement
    {
        IList<SVGPoint> Points { get; }
        void Add( int x, int y );
        void Add( SVGPoint point );
    }

    public interface ISVGPolygon : ISVGPolyItem
    {
    }

    public interface ISVGPolyline : ISVGPolyItem
    {
    }

    public interface ISVGPath : ISVGElement
    {
        void MoveTo( double xTo, double yTo, bool absolute = true );
        void LineTo( double xTo, double yTo, bool absolute = true );
        void HorizontalLineTo( double xTo, double yTo, bool absolute = true );
        void VerticalLineTo( double xTo, double yTo, bool absolute = true );
        void CurveTo( double xTo, double yTo, bool absolute = true );
        void SmoothCurveTo( double xFrom, double yFrom, double xTo, double yTo, bool absolute = true );
        void QuadraticBezierCurve( double xFrom, double yFrom, double xTo, double yTo, bool absolute = true );
        void SmoothQuadraticBezierCurve( double xFrom, double yFrom, double xTo, double yTo, bool absolute = true );
        void EllipticalArc( double xTo, double yTo, bool absolute = true );
        void Clear();
        void ClosePath();
    }
        
    /// <summary>
    /// Interface for SVG Text elements.
    /// </summary>
    public interface ISVGText : ISVGElement
    {
        double X { get; set; }
        double Y { get; set; }
        double Dx { get; set; }
        double Dy { get; set; }
        void Append( string text );
        ISVGTspan AppendLine( string text );
        ISVGTspan AppendLine( double x, double y, string text );
    }

    public interface ISVGTspan : ISVGElement
    {
        double X { get; set; }
        double Y { get; set; }
    }
}
