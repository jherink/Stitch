using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Elements
{
    public abstract class SVGBaseElement : BaseElement, ISVGElement
    {
        public string Fill { get; set; }

        public string Stroke { get; set; }

        public int[] StrokeDashArray { get; set; }

        public int StrokeDashOffset { get; set; }

        public string StrokeLineCap { get; set; }

        public string StrokeLineJoin { get; set; }

        public double StrokeMiterLimit { get; set; }

        /// <summary>
        /// Set the opacity of just the stroke.
        /// </summary>
        public double StrokeOpacity { get; set; } = 1;

        public double FillOpacity { get; set; } = 1;

        #region Opacity 

        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = StrokeOpacity = FillOpacity = value;
            }
        }

        private double _opacity = 1;

        #endregion

        public int StrokeWidth { get; set; }

        public string FillRule { get; set; }

        public string Transform { get; set; }

        protected string PreRenderSVGStyleString()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace( Fill ))
            {
                StyleList.Add( "fill", Fill );
            }
            if (!string.IsNullOrWhiteSpace( Stroke ))
            {
                StyleList.Add( "stroke", Stroke );
            }
            if (StrokeDashArray != default( int[] ) && StrokeDashArray.Length > 0)
            {
                StyleList.Add( "stroke-dasharray", string.Join( ",", StrokeDashArray ) );
            }
            if (StrokeDashOffset > 0)
            {
                StyleList.Add( "stroke-dashoffset", StrokeDashOffset.ToString() );
            }
            if (!string.IsNullOrWhiteSpace( StrokeLineCap ))
            {
                StyleList.Add( "stroke-linecap", StrokeLineCap );
            }
            if (!string.IsNullOrWhiteSpace( StrokeLineJoin ))
            {
                StyleList.Add( "stroke-linejoin", StrokeLineJoin );
            }
            if (StrokeMiterLimit >= 1)
            {
                StyleList.Add( "stroke-miterlimit", StrokeMiterLimit.ToString() );
            }

            if (Math.Abs( StrokeOpacity - Opacity ) < .001 &&
                Math.Abs( FillOpacity - Opacity ) < .001 &&
                Opacity >= 0 && Opacity < 1)
            {
                StyleList.Add( "opacity", Opacity.ToString( "0.00" ) );
            }
            else
            {
                if (StrokeOpacity >= 0 && StrokeOpacity < 1)
                {
                    StyleList.Add( "stroke-opacity", StrokeOpacity.ToString( "0.00" ) );
                }
                if (FillOpacity >= 0 && FillOpacity < 1)
                {
                    StyleList.Add( "fill-opacity", FillOpacity.ToString( "0.00" ) );
                }
            }
            if (StrokeWidth > 0)
            {
                StyleList.Add( "stroke-width", StrokeWidth.ToString() );
            }
            if (!string.IsNullOrWhiteSpace( FillRule ))
            {
                StyleList.Add( "fill-rule", FillRule );
            }
            if (!string.IsNullOrWhiteSpace( Transform ))
            {
                builder.Append( $" transform=\"{Transform}\"" );
            }
            return builder.ToString();
        }
    }

    public class SVG : BaseParentElement, ISVG
    {
        public double Height { get; set; }

        public override string Tag
        {
            get
            {
                return "svg";
            }
        }

        public double Width { get; set; }

        public void Add( IElement svgElement )
        {
            Children.Add( svgElement );
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( $" height=\"{Height}\" width=\"{Width}\"" );
            builder.AppendLine( ">" );
            builder.AppendLine( RenderChildren() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }

        #region Constructors 

        public SVG()
        {
        }

        public SVG( int height, int width )
        {
            Height = height;
            Width = width;
        }

        #endregion
    }

    public class SVGRectangle : SVGBaseElement, ISVGRectangle
    {
        public double Height { get; set; }

        public double Rx { get; set; }

        public double Ry { get; set; }

        public override string Tag
        {
            get
            {
                return "rect";
            }
        }

        public double Width { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            if (Width > 0) builder.Append( $" width=\"{Width}\"" );
            if (Height > 0) builder.Append( $" height=\"{Height}\"" );
            builder.Append( $" x=\"{X}\"" );
            builder.Append( $" y=\"{Y}\"" );
            if (Rx > 0) builder.Append( $" rx=\"{Rx}\"" );
            if (Ry > 0) builder.Append( $" ry=\"{Ry}\"" );

            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );

            builder.AppendLine( "/>" );
            return builder.ToString();

        }
    }

    public class SVGCircle : SVGBaseElement, ISVGCircle
    {
        public double Cx { get; set; } = 0;

        public double Cy { get; set; } = 0;

        public double R { get; set; }

        public override string Tag
        {
            get
            {
                return "circle";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            if (Cx > 0) builder.Append( $" cx=\"{Cx}\"" );
            if (Cy > 0) builder.Append( $" cy=\"{Cy}\"" );
            builder.Append( $" r=\"{R}\"" );
            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( "/>" );
            return builder.ToString();
        }

        public SVGCircle() { }

        public SVGCircle( double cx, double cy, double r ) { Cx = cx; Cy = cy; R = r; }

        public SVGCircle( SVGPoint center, double r ) : this( center.X, center.Y, r ) { }
    }

    public class SVGEllipse : SVGBaseElement, ISVGEllipse
    {

        public double Rx { get; set; } = 0;

        public double Ry { get; set; } = 0;

        public double Cx { get; set; } = 0;

        public double Cy { get; set; } = 0;

        public override string Tag
        {
            get
            {
                return "ellipse";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            builder.Append( $" cx=\"{Cx}\"" );
            builder.Append( $" cy=\"{Cy}\"" );
            if (Rx > 0) builder.Append( $" rx=\"{Rx}\"" );
            if (Ry > 0) builder.Append( $" ry=\"{Ry}\"" );
            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( "/>" );
            return builder.ToString();
        }

        #region Constructors

        public SVGEllipse() { }

        public SVGEllipse( int cx, int cy, int rx, int ry )
        {
            Cx = cx; Cy = cy; Rx = rx; Ry = ry;
        }

        #endregion
    }

    public class SVGLine : SVGBaseElement, ISVGLine
    {
        public override string Tag
        {
            get
            {
                return "line";
            }
        }

        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            builder.Append( $" x1=\"{X1}\"" );
            builder.Append( $" y1=\"{Y1}\"" );
            builder.Append( $" x2=\"{X2}\"" );
            builder.Append( $" y2=\"{Y2}\"" );
            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( "/>" );
            return builder.ToString();
        }

        #region Constructors

        public SVGLine() { }

        public SVGLine( double x1, double y1, double x2, double y2 )
        {
            X1 = x1; Y1 = y1; X2 = x2; Y2 = y2;
        }

        public SVGLine( SVGPoint startPoint, SVGPoint endPoint ) : this( startPoint.X, startPoint.Y, endPoint.X, endPoint.Y ) { }

        #endregion
    }

    public abstract class SVGPolyItem : SVGBaseElement, ISVGPolyItem
    {
        #region Points

        private List<SVGPoint> points = new List<SVGPoint>();
        public IList<SVGPoint> Points
        {
            get
            {
                return points;
            }
        }

        #endregion

        public void Add( SVGPoint point )
        {
            points.Add( point );
        }

        public void Add( int x, int y )
        {
            Add( new SVGPoint( x, y ) );
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );

            // build points
            builder.Append( " points=\"" );
            for (int i = 0; i < points.Count; i++)
            { // use for loop to address fencepost problem
                if (i == 0) builder.Append( $"{points[i].X},{points[i].Y}" );
                else builder.Append( $" {points[i].X},{points[i].Y}" );
            }
            builder.Append( "\"" ); // end points

            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( "/>" );
            return builder.ToString();
        }
    }

    public class SVGPolygon : SVGPolyItem, ISVGPolygon
    {
        public override string Tag
        {
            get
            {
                return "polygon";
            }
        }
    }

    public class SVGPolyline : SVGPolyItem, ISVGPolyline
    {
        public override string Tag
        {
            get
            {
                return "polyline";
            }
        }
    }

    public class SVGGroup : SVGBaseElement, ISVGGroup, IParentElement, INodeQueryable
    {
        public IList<IElement> Children { get; set; } = new List<IElement>();

        public void Add( IElement element ) { Children.Add( element ); }

        public override string Tag
        {
            get
            {
                return "g";
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( ">" );
            foreach (var child in Children)
            {
                builder.AppendLine( child.Render() );
            }
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }

        public IElement FindById( string id )
        {

            foreach (var child in Children)
            {
                if (child.ID == id) { return child; }
                else
                {
                    if (child is INodeQueryable)
                    {
                        var r = (child as INodeQueryable).FindById( id );
                        if (r != default( INodeQueryable ))
                        {
                            return r; // exit if this node or one of it's children is it. Otherwise continue.
                        }
                    }
                }
            }
            return default( IElement );
        }

        public IEnumerable<IElement> GetAllNodes()
        {
            System.Diagnostics.Debug.WriteLine( $"Getting all nodes: {Tag}, Child count: {Children.Count}" );

            var elements = new List<IElement>();
            foreach (var child in Children)
            {
                elements.Add( child );
                if (child is INodeQueryable)
                {
                    elements.AddRange( (child as INodeQueryable).GetAllNodes() );
                }
            }
            return elements;
        }

        public IEnumerable<IElement> GetNodes( string tagFilter )
        {
            var elements = new List<IElement>();
            foreach (var child in Children)
            {
                if (child.Tag == tagFilter) elements.Add( child );

                if (child is INodeQueryable)
                {
                    elements.AddRange( (child as INodeQueryable).GetNodes( tagFilter ) );
                }
            }
            return elements;
        }
    }

    public class SVGPath : SVGBaseElement, ISVGPath
    {
        private StringBuilder _path = new StringBuilder();

        public override string Tag
        {
            get
            {
                return "path";
            }
        }

        public void ClosePath()
        {
            _path.Append( " Z" );
        }

        public void CurveTo( double xTo, double yTo, bool absolute = true )
        {
            Append( "C", xTo, yTo, absolute );
        }

        public void EllipticalArc( double xTo, double yTo, bool absolute = true )
        {
            Append( "A", xTo, yTo, absolute );
        }

        public void EllipticalArc( double arcXRadius, double arcYRadius, bool largeArcFlag, bool sweepFlag, double arcDirection, double xTo, double yTo )
        {
            _path.Append( $" A{arcXRadius} {arcYRadius} {Convert.ToDouble( sweepFlag )} {Convert.ToDouble( sweepFlag )} {arcDirection} {xTo} {yTo}" );
        }

        public void HorizontalLineTo( double xTo, double yTo, bool absolute = true )
        {
            Append( "H", xTo, yTo, absolute );
        }

        public void LineTo( double xTo, double yTo, bool absolute = true )
        {
            Append( "L", xTo, yTo, absolute );
        }

        public void MoveTo( double xTo, double yTo, bool absolute = true )
        {
            Append( "M", xTo, yTo, absolute );
        }

        public void QuadraticBezierCurve( double xFrom, double yFrom, double xTo, double yTo, bool absolute = true )
        {
            Append( "Q", xFrom, yFrom, absolute );
            _path.Append( $" {xTo} {yTo}" );
        }

        public void SmoothCurveTo( double xFrom, double yFrom, double xTo, double yTo, bool absolute = true )
        {
            Append( "S", xFrom, yFrom, absolute );
            _path.Append( $" {xTo} {yTo}" );
        }

        public void SmoothQuadraticBezierCurve( double xFrom, double yFrom, double xTo, double yTo, bool absolute = true )
        {
            Append( "T", xFrom, yFrom, absolute );
            _path.Append( $" {xTo} {yTo}" );
        }

        public void VerticalLineTo( double xTo, double yTo, bool absolute = true )
        {
            Append( "V", xTo, yTo, absolute );
        }

        public void Clear() { _path.Clear(); }

        public override object Clone()
        {
            var baseAttempt = base.Clone() as SVGPath;
            baseAttempt._path = new StringBuilder();
            baseAttempt._path.Append( _path.ToString() );
            return baseAttempt;
        }

        private void Append( string command, double xTo, double yTo, bool absolute )
        {
            if (_path.Length > 0) command = " " + command;
            command = (absolute ? command.ToUpper() : command.ToLower());
            _path.Append( $"{command}{xTo} {yTo}" );
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} d=\"{_path.ToString()}\"" );
            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.AppendLine( "/>" );
            return builder.ToString();
        }
    }

    public class SVGText : SVGBaseElement, ISVGText
    {
        public override string Tag
        {
            get
            {
                return "text";
            }
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Dx { get; set; }

        public double Dy { get; set; }

        public DOMString Text { get; set; } = new DOMString();

        public string GetContent()
        {
            return Text.ToString();
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} x=\"{X}\" y=\"{Y}\"" );
            if (Dx > 0 || Dx < 0) builder.Append( $" dx=\"{Dx}\"" );
            if (Dy > 0 || Dy < 0) builder.Append( $" dy=\"{Dy}\"" );
            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( ">" );
            builder.Append( Text.Render() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }

        public void Append( string text )
        {
            Text.Append( text );
        }

        public ISVGTspan AppendLine( string text )
        {
            var tspan = new SVGTspan( text );
            Text.AppendElement( tspan );
            return tspan;
        }

        public ISVGTspan AppendLine( double x, double y, string text )
        {
            var tspan = AppendLine( text );
            tspan.X = x;
            tspan.Y = y;
            return tspan;
        }

        public SVGText() : this( string.Empty )
        {
        }

        public SVGText( params string[] text )
        {
            //ClassList.Add( "stitch-theme" );
            if (text.Length >= 1)
            {
                Text = new DOMString( text[0] );
                if (text.Length > 1)
                {
                    for (int i = 1; i < text.Length; i++)
                    {
                        AppendLine( text[i] );
                    }
                }
            }
            else
            {
                Text = new DOMString( string.Empty );
            }
        }

        public SVGText( double x, double y, params string[] text ) : this( text )
        {
            X = x;
            Y = y;
        }
    }

    public class SVGTspan : SVGBaseElement, ISVGTspan
    {
        public override string Tag
        {
            get
            {
                return "tspan";
            }
        }

        public DOMString Text { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag} x=\"{X}\" y=\"{Y}\"" );
            builder.Append( $" {PreRenderSVGStyleString()}" );
            AppendIdAndClassInfoToTag( builder );
            builder.Append( ">" );
            builder.Append( Text.Render() );
            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }

        public SVGTspan() : this( string.Empty ) { }

        public SVGTspan( string text )
        {
            Text = new DOMString( text );
        }

        public SVGTspan( double x, double y, string text ) : this( text )
        {
            X = x;
            Y = y;
        }
    }

}
