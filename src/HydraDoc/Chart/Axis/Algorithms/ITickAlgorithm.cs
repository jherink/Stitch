using System;
using System.Collections.Generic;

namespace HydraDoc.Chart.Axis.Algorithms
{
    /// <summary>
    /// Interface for Axis algorithms.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITickAlgorithm<T> where T : IComparable<T>
    {
        /// <summary>
        /// Suggest possible tick values based upon the number of desired intervals and the set of values.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="intervals"></param>
        /// <returns></returns>
        IEnumerable<T> SuggestTicks( IEnumerable<T> set, int intervals );

        /// <summary>
        /// Compute a numeric distance between the two values a,b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        double Subtract( IEnumerable<T> set, T a, T b );

        /// <summary>
        /// Compute a numeric value greater than or equal to 0 and less than or equal to 1 that
        /// represents the percentage of the value on the range [limitMinimum, limitMaximum]
        /// </summary>
        /// <param name="limitMinimum">The minimum limit value.</param>
        /// <param name="limitMaximum">The maximum limit value.</param>
        /// <param name="value">The value to evaluate.</param>
        /// <returns></returns>
        double Percentage( IEnumerable<T> range, T value );

        T Min( IEnumerable<T> set );
        T Max( IEnumerable<T> set );
    }
}
