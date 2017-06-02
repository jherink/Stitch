using System;
using System.Collections.Generic;

namespace Stitch.Chart.Axis.Algorithms
{
    /// <summary>
    /// Interface for Axis algorithms.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITickAlgorithm<T> : IComparer<T> where T : IComparable<T>
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

        /// <summary>
        /// Identify the minimum value in the set.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>The set's minimum value.</returns>
        T Min( IEnumerable<T> set );

        /// <summary>
        /// Identify the maximum value in the set.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>The set's maximum value.</returns>
        T Max( IEnumerable<T> set );
    }
}
