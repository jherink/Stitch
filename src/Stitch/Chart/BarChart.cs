using Stitch.Chart.Axis;
using Stitch.Chart.Axis.Algorithms;
using Stitch.Elements;
using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    };

    public class Bar : ICloneable
    {
        public string Label { get; set; }

        public string Color { get; set; }

        public double Value { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class BarGroup : List<Bar>, ICloneable
    {
        public readonly string Name;

        public string Color { get; set; }

        public void SetColor( string color )
        {
            Color = color;
            foreach (var bar in this)
            {
                bar.Color = color;
            }
        }

        public object Clone()
        {
            var clone = new BarGroup( Name );
            foreach (var v in this) clone.Add( v.Clone() as Bar );
            return clone;
        }

        public BarGroup( string name ) { Name = name; }
    }

    public class BarChart : AxisChart<string, double>
    {
        private const string DEFAULT_BAR_GROUP = "";

        //private readonly List<Bar> Bars = new List<Bar>();

        private Dictionary<string, BarGroup> BarGroups = new Dictionary<string, BarGroup>();

        public int BarGroupCount { get { return BarGroups.Count; } }

        public Orientation AxisOrientation { get; set; }

        #region Constructor

        public BarChart() : this( 900, 500 )
        {
        }

        public BarChart( int width, int height ) : base( width, height )
        {
            AxisOrientation = Orientation.Vertical;
            LabeledAxis.GridLines = false;
            LabeledAxisTickAlgorithm = null;
        }

        #endregion

        #region Public Methods

        public void AddBar( string label, double value, string color = "" )
        {
            AddBar( new Bar() { Label = label, Value = value, Color = color }, DEFAULT_BAR_GROUP );
        }

        public void AddToBarGroup( string barGroup, string label, double value )
        {
            AddBar( new Bar() { Label = label, Value = value }, barGroup );
        }

        //public void AddToBarGroup( string barGroup, params double[] values ) {
        //    foreach (var v in values) AddToBarGroup( barGroup, v );
        //}

        public void SetBarGroupColor( string barGroup, string color )
        {
            GetBarGroup( barGroup ).SetColor( color );
        }

        public void AddBar( Bar bar, string barGroup = "" )
        {
            var group = GetBarGroup( barGroup );
            group.Add( bar );
        }

        /// <summary>
        /// Gets the bar group by name, if none exists then it creates one.
        /// </summary>
        /// <param name="barGroup"></param>
        /// <returns></returns>
        public BarGroup GetBarGroup( string barGroup )
        {
            if (!BarGroups.ContainsKey( barGroup ))
            {
                BarGroups.Add( barGroup, new BarGroup( barGroup ) );
            }
            return BarGroups[barGroup];
        }

        #endregion

        protected override void RenderAxisChartImpl()
        {
            //LabeledAxis.SetTicks( Bars.Select( t => t.Label ) );
            if (BarGroups.Count == 1)
            {
                LabeledAxis.SetTicks( BarGroups.First().Value.Select( t => t.Label ) );
            }
            else {
                var ticks = new List<string>();
                foreach (var g in BarGroups)
                {
                    ticks.AddRange( g.Value.Select( t => t.Label ) );
                }
                LabeledAxis.SetTicks( ticks.Distinct() );
            }

            switch (AxisOrientation)
            {
                case Orientation.Horizontal:
                    RenderChartHorizontally();
                    break;
                case Orientation.Vertical:
                    RenderChartVertically();
                    break;

            }
        }

        private void RenderChartHorizontally()
        {
            // Bars going --->>
            // Measured Axis is on bottom. 

            // Render bars
            var horizontalAxisLocations = new double[] { };
            var verticalAxisLocations = new double[] { };
            var measuredIntervals = AxisHelper.SuggestIntervals( GetChartableAreaWidth() );
            var min = MeasuredAxis.IncludeDefault ? default( double ) : BarGroups.Min( t => t.Value.Min( p => p.Value ) );

            var set = new List<double>() { 0 };
            foreach (var group in BarGroups) set.AddRange( group.Value.Select( t => t.Value ) );
            MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( set, measuredIntervals ) );
            var labeledClone = LabeledAxis.Clone() as IAxis<string>;
            var ticks = labeledClone.Ticks.ToList();
            ticks.Insert( 0, string.Empty );
            ticks.Add( string.Empty );
            labeledClone.SetTicks( ticks );

            AxisHelper.RenderAxis( labeledClone, MeasuredAxis,
                                   GetChartableAreaWidth(),
                                   Height - BottomAxisOffset(),
                                   GetLegendLeftOffset(),
                                   GetTitleHeight() + GetLegendTopOffset(),
                                   VerticalAxisGroup, HorizontalAxisGroup, out verticalAxisLocations,
                                   out horizontalAxisLocations );

            var horizontalSpace = (horizontalAxisLocations.Max() - horizontalAxisLocations.Min()) / horizontalAxisLocations.Length;
            var verticalSpace = (verticalAxisLocations.Max() - verticalAxisLocations.Min()) / verticalAxisLocations.Length;
            var baseLineX = horizontalAxisLocations.Min();
            for (int i = 1; i < labeledClone.Ticks.Count - 1; i++)
            {
                var label = labeledClone.Ticks[i];

                var bars = new List<Bar>();
                var id = 1;
                foreach (var g in BarGroups) bars.AddRange( g.Value.Where( t => t.Label == label ) );
                
                var indWidth = verticalSpace / bars.Count;
                var barY = verticalAxisLocations[i] - (verticalSpace / 2); // center bar on grid line
                foreach (var bar in bars)
                {
                    var physicalChartSize = (horizontalAxisLocations.Max() - baseLineX);
                    var barWidth = bar.Value * physicalChartSize / MeasuredAxis.MaxValue;
                    var svgBar = new SVGRectangle
                    {
                        X = baseLineX,
                        Y = barY,
                        Width = barWidth,
                        Height = indWidth,
                        Fill = bar.Color,
                        StrokeOpacity = 0
                    };
                    svgBar.ClassList.Add( GetChartTheme( id++ ) );
                    barY += indWidth;

                    ChartGroup.Add( svgBar );
                }
            }
        }

        private void RenderChartVertically()
        {
            // Bars going   |
            //              |
            //              |
            //              V
            //              V
            // Labeled Axis is on bottom.

            var horizontalAxisLocations = new double[] { };
            var verticalAxisLocations = new double[] { };
            var intervals = AxisHelper.SuggestIntervals( Height );
            var min = MeasuredAxis.IncludeDefault ? default( double ) : BarGroups.Min( t => t.Value.Min( p => p.Value ) );

            var set = new List<double>() { 0 };
            foreach (var group in BarGroups) set.AddRange( group.Value.Select( t => t.Value ) );
            MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( set, intervals ) );

            // Because of how SVG renders we must reverse the measured values first.
            var measuredClone = MeasuredAxis.Clone() as IAxis<double>;
            measuredClone.SetTicks( measuredClone.Ticks.Reverse() );
            var horizontalClone = LabeledAxis.Clone() as IAxis<string>;
            var ticks = horizontalClone.Ticks.ToList();
            ticks.Insert( 0, string.Empty );
            horizontalClone.SetTicks( ticks );

            AxisHelper.RenderAxis( measuredClone, horizontalClone, GetChartableAreaWidth(),
                                   Height - GetLegendBottomOffset(),
                                   GetLegendLeftOffset(), GetTitleHeight() + GetLegendTopOffset(),
                                   VerticalAxisGroup, HorizontalAxisGroup,
                                   out verticalAxisLocations, out horizontalAxisLocations );

            var horizontalSpace = (horizontalAxisLocations.Max() - horizontalAxisLocations.Min()) / horizontalAxisLocations.Length;
            var baseLineX = horizontalAxisLocations.Min();

            // Render bars.
            var baseLineY = verticalAxisLocations.Max();
            for (int i = 1; i < horizontalClone.Ticks.Count; i++)
            {
                var label = horizontalClone.Ticks[i];
                var bars = new List<Bar>();
                var id = 1;
                foreach (var g in BarGroups) bars.AddRange( g.Value.Where( t => t.Label == label ) );
                
                var indWidth = horizontalSpace / bars.Count;
                var barX = horizontalAxisLocations[i] - horizontalSpace / 2; // center bar on grid line
                foreach (var bar in bars)
                {
                    var physicalChartSize = baseLineY - verticalAxisLocations.Min();
                    var barHeight = bar.Value * physicalChartSize / MeasuredAxis.MaxValue;
                    var svgBar = new SVGRectangle
                    {
                        X = barX,
                        Y = baseLineY - barHeight,
                        Height = barHeight,
                        Width = indWidth,
                        Fill = bar.Color,
                        StrokeOpacity = 0,
                    };
                    svgBar.ClassList.Add( GetChartTheme( id++ ) );
                    barX += indWidth;

                    ChartGroup.Add( svgBar );
                }
            }
        }

        public override double GetLegendLeftOffset()
        {
            if (LegendPosition == LegendPosition.Left)
            {
                var qString = string.Empty;
                qString = GetLegendItems().Select( t => t.Item1 ).OrderBy( t => t.Length ).First();
                qString = qString.Length < AxisHelper.MaxHorizontalAxisLength ? new string( 'w', AxisHelper.MaxHorizontalAxisLength ) : qString;
                return GraphicsHelper.MeasureStringWidth( qString, ChartTextStyle );
            }
            return 0;
        }

        public override double GetLegendRightOffset()
        {
            if (LegendPosition == LegendPosition.Right)
            {
                var qString = string.Empty;
                qString = GetLegendItems().Select( t => t.Item1 ).OrderBy( t => t.Length ).First();
                qString = qString.Length < AxisHelper.MaxHorizontalAxisLength ? new string( 'w', AxisHelper.MaxHorizontalAxisLength ) : qString;
                return GraphicsHelper.MeasureStringWidth( qString, ChartTextStyle );
            }
            return 0;
        }

        private double BottomAxisOffset()
        {
            if (LegendPosition == LegendPosition.Bottom)
            {
                var qString = string.Empty;
                switch (AxisOrientation)
                {
                    case Orientation.Vertical:
                        qString = AxisHelper.LongestTick( LabeledAxis );
                        qString = qString.Length < AxisHelper.MaxHorizontalAxisLength ? new string( 'w', AxisHelper.MaxHorizontalAxisLength ) : qString;
                        return LabeledAxis.Visible ? GraphicsHelper.MeasureStringWidth( qString, LabeledAxis.AxisTextStyle ) : 0;
                    case Orientation.Horizontal:
                        qString = AxisHelper.LongestTick( MeasuredAxis );
                        qString = qString.Length < AxisHelper.MaxHorizontalAxisLength ? new string( 'w', AxisHelper.MaxHorizontalAxisLength ) : qString;
                        return MeasuredAxis.Visible ? GraphicsHelper.MeasureStringWidth( qString, MeasuredAxis.AxisTextStyle ) : 0;
                }
            }
            return 0;
        }

        protected override double GetChartableAreaWidth()
        {
            var baseWidth = base.GetChartableAreaWidth();
            var qString = string.Empty;
            switch (AxisOrientation)
            {
                case Orientation.Horizontal:
                    qString = AxisHelper.LongestTick( LabeledAxis );
                    qString = qString.Length < AxisHelper.MaxHorizontalAxisLength ? new string( 'w', AxisHelper.MaxHorizontalAxisLength ) : qString;
                    return baseWidth - (LabeledAxis.Visible ? GraphicsHelper.MeasureStringWidth( qString, LabeledAxis.AxisTextStyle ) : 0);
                case Orientation.Vertical:
                    qString = AxisHelper.LongestTick( MeasuredAxis );
                    qString = qString.Length < AxisHelper.MaxHorizontalAxisLength ? new string( 'w', AxisHelper.MaxHorizontalAxisLength ) : qString;
                    return baseWidth - (MeasuredAxis.Visible ? GraphicsHelper.MeasureStringWidth( qString, MeasuredAxis.AxisTextStyle ) : 0);
            }
            return 0;
        }

        protected override IEnumerable<Tuple<string, string>> GetLegendItems()
        {
            return BarGroups.Select( t => new Tuple<string, string>( t.Value.Name, t.Value.Color ) );
        }

        public override object Clone()
        {
            var clone = base.Clone() as BarChart;
            clone.BarGroups = new Dictionary<string, BarGroup>();
            foreach (var g in BarGroups)
            {
                clone.BarGroups.Add( g.Key, g.Value.Clone() as BarGroup );
            }
            return clone;
        }

        public override string Render()
        {
            //if (BarGroupCount > 1)
            //{
            //    var lastGroup = BarGroups.First().Key;
            //    var ct = BarGroups.First().Value.Count;
            //    foreach (var group in BarGroups)
            //    {
            //        if (ct != group.Value.Count)
            //        {
            //            throw new InvalidOperationException( $"Mismatched Bar Group Exception: The bar group \"{lastGroup}\" has {ct} members while the bar group \"{group.Value.Name}\" has {group.Value.Count} members." );
            //        }
            //        ct = group.Value.Count;
            //        lastGroup = group.Key;
            //    }
            //}
            return base.Render();
        }
    }
}
