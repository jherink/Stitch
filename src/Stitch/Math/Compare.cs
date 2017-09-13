using System;

namespace Stitch.StitchMath
{
    /// <summary>Class for numeric comparisons.</summary>
    internal static class Compare
    {
        /// <summary>Epsilon value for single precision values.</summary>
        public const float SingleEpsilon = 1e-7f;
        /// <summary>Epsilon value for double precision values.</summary>
        public const double DoubleEpsilon = 1e-15;

        #region Public Interface

        /// <summary>
        /// Compares a number to see it is zero.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the number is zero.</returns>
        public static bool IsZero( double value )
        {
            return IsZero( value, SingleEpsilon );
        }

        /// <summary>
        /// Compares a number to see it is zero.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the number is zero.</returns>
        public static bool IsZero( double value, double tolerance )
        {
            return AreEqual( value, 0, tolerance );
        }


        /// <summary>Compare two values for equality using given tolerance.</summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="tolerance">The comparison tolerance.</param>
        /// <returns>True if the difference between the values &lt;= the tolerance.</returns>
        public static bool AreEqual( int value1, int value2, int tolerance )
        {
            return Math.Abs( value1 - value2 ) <= tolerance;
        }

        /// <summary>Compare two values for equality using given tolerance.</summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>True if the difference between the values &lt;= the tolerance.</returns>
        public static bool AreEqual( float value1, float value2 )
        {
            return AreEqual( value1, value2, SingleEpsilon );
        }

        /// <summary>Compare two values for equality using given tolerance.</summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="tolerance">The comparison tolerance.</param>
        /// <returns>True if the difference between the values &lt;= the tolerance.</returns>
        public static bool AreEqual( float value1, float value2, float tolerance )
        {
            return Math.Abs( value1 - value2 ) <= tolerance;
        }

        /// <summary>Compare two values for equality using given tolerance.</summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>True if the difference between the values &lt;= the tolerance.</returns>
        public static bool AreEqual( double value1, double value2 )
        {
            return AreEqual( value1, value2, DoubleEpsilon );
        }

        /// <summary>Compare two values for equality using given tolerance.</summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="tolerance">The comparison tolerance.</param>
        /// <returns>True if the difference between the values &lt;= the tolerance.</returns>
        public static bool AreEqual( double value1, double value2, double tolerance )
        {
            return Math.Abs( value1 - value2 ) <= tolerance;
        }

        /// <summary>Test for greater than or equality.</summary>
        /// <param name="a">The value assumed to be greater.</param>
        /// <param name="b">The value assumed to be less.</param>
        /// <returns>True if the first value is greater than or equal to the second value.</returns>
        public static bool GreaterThanOrEqual( float a, float b )
        {
            return (a > b) || AreEqual( a, b );
        }

        /// <summary>Test for greater than or equality.</summary>
        /// <param name="a">The value assumed to be greater.</param>
        /// <param name="b">The value assumed to be less.</param>
        /// <param name="tolerance">The comparison tolerance.</param>
        /// <returns>True if the first value is greater than or equal to the second value.</returns>
        public static bool GreaterThanOrEqual( float a, float b, float tolerance )
        {
            return (a > b) || AreEqual( a, b, tolerance );
        }

        /// <summary>Test for greater than or equality.</summary>
        /// <param name="a">The value assumed to be greater.</param>
        /// <param name="b">The value assumed to be less.</param>
        /// <returns>True if the first value is greater than or equal to the second value.</returns>
        public static bool GreaterThanOrEqual( double a, double b )
        {
            return (a > b) || AreEqual( a, b );
        }

        /// <summary>Test for greater than or equality.</summary>
        /// <param name="a">The value assumed to be greater.</param>
        /// <param name="b">The value assumed to be less.</param>
        /// <param name="tolerance">The comparison tolerance.</param>
        /// <returns>True if the first value is greater than or equal to the second value.</returns>
        public static bool GreaterThanOrEqual( double a, double b, double tolerance )
        {
            return (a > b) || AreEqual( a, b, tolerance );
        }

        /// <summary>Test if integer is en.</summary>
        /// <param name="value">The value to test.</param>
        /// <returns>True if the value is even.</returns>
        public static bool IsEvenInteger( int value )
        {
            return (value == (value & ~1));
        }

        /// <summary>Test if integer is odd.</summary>
        /// <param name="value">The value to test.</param>
        /// <returns>True if the value is odd.</returns>
        public static bool IsOddInteger( int value )
        {
            return (value != (value & ~1));
        }

        /// <summary>Test for less than or equality.</summary>
        /// <param name="a">The value assumed to be less.</param>
        /// <param name="b">The value assumed to be greater.</param>
        /// <returns>True if the first value is less than or equal to the second value.</returns>
        public static bool LessThanOrEqual( float a, float b )
        {
            return (a < b) || AreEqual( a, b );
        }

        /// <summary>Test for less than or equality.</summary>
        /// <param name="a">The value assumed to be less.</param>
        /// <param name="b">The value assumed to be greater.</param>
        /// <param name="tolerance">The comparison tolerance.</param>
        /// <returns>True if the first value is less than or equal to the second value.</returns>
        public static bool LessThanOrEqual( float a, float b, float tolerance )
        {
            return (a < b) || AreEqual( a, b, tolerance );
        }

        /// <summary>Test for less than or equality.</summary>
        /// <param name="a">The value assumed to be less.</param>
        /// <param name="b">The value assumed to be greater.</param>
        /// <returns>True if the first value is less than or equal to the second value.</returns>
        public static bool LessThanOrEqual( double a, double b )
        {
            return (a < b) || AreEqual( a, b );
        }

        /// <summary>Test for less than or equality.</summary>
        /// <param name="a">The value assumed to less.</param>
        /// <param name="b">The value assumed to be greater.</param>
        /// <param name="tolerance">The comparison tolerance.</param>
        /// <returns>True if the first value is less than or equal to the second value.</returns>
        public static bool LessThanOrEqual( double a, double b, double tolerance )
        {
            return (a < b) || AreEqual( a, b, tolerance );
        }

        /// <summary>Calculate the percent error between a test value and the expected value.</summary>
        /// <param name="testValue">The value to test.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <returns>The percent error difference (ex. 0.02 means test value is 2% larger than expected value).</returns>
        public static float PercentError( float testValue, float expectedValue )
        {
            if (AreEqual( expectedValue, 0.0f ))
            {
                return (AreEqual( testValue, 0.0f ) ? 0 : 1);
            }

            return testValue / expectedValue - 1;
        }

        /// <summary>Calculate the percent error between a test value and the expected value.</summary>
        /// <param name="testValue">The value to test.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <returns>The percent error difference (ex. 0.02 means test value is 2% larger than expected value).</returns>
        public static double PercentError( double testValue, double expectedValue )
        {
            if (AreEqual( expectedValue, 0.0 ))
            {
                return (AreEqual( testValue, 0.0 ) ? 0 : 1);
            }
            return testValue / expectedValue - 1;
        }

        /// <summary>Calculate the percent error between a test value and the expected value.</summary>
        /// <param name="testValue">The value to test.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="differenceThreshold">Values are equal if their difference is less or equal to this value.</param>
        /// <returns>The percent error difference (ex. 0.02 means test value is 2% larger than expected value).</returns>
        public static float PercentError( float testValue, float expectedValue, float differenceThreshold )
        {
            if (Math.Abs( testValue - expectedValue ) < differenceThreshold)
            {
                return 0;
            }
            return PercentError( testValue, expectedValue );
        }

        /// <summary>Calculate the percent error between a test value and the expected value.</summary>
        /// <param name="testValue">The value to test.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="differenceThreshold">Values are equal if their difference is less or equal to this value.</param>
        /// <returns>The percent error difference (ex. 0.02 means test value is 2% larger than expected value).</returns>
        public static double PercentError( double testValue, double expectedValue, double differenceThreshold )
        {
            if (Math.Abs( testValue - expectedValue ) < differenceThreshold)
            {
                return 0;
            }
            return PercentError( testValue, expectedValue );
        }

        #endregion

        #region Private Interface

        const string NegativeValueError = "Value cannot be negative.";

        #endregion
    }
}
