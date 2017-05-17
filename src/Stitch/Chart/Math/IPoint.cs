using System;

namespace Stitch.Chart
{
    /// <summary>
    /// Interface for spatial point objects.
    /// </summary>
    internal interface IPoint
    {
        double Distance( IPoint point );

    }
}
