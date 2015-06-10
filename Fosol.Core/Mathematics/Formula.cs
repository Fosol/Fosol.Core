using System;

namespace Fosol.Core.Mathematics
{
    /// <summary>
    /// Formula static class, provides helpful Math formulas.
    /// </summary>
    public static class Formula
    {
        #region Methods
        #region Ratio
        /// <summary>
        /// calculates the ratio difference between the two values.
        /// </summary>
        /// <param name="val1">Value one.</param>
        /// <param name="val2">Value two.</param>
        /// <returns>Ratio difference between two values.</returns>
        public static float Ratio(short val1, short val2)
        {
            return (float)val1 / val2;
        }

        /// <summary>
        /// calculates the ratio difference between the two values.
        /// </summary>
        /// <param name="val1">Value one.</param>
        /// <param name="val2">Value two.</param>
        /// <returns>Ratio difference between two values.</returns>
        public static float Ratio(int val1, int val2)
        {
            return (float)val1 / val2;
        }

        /// <summary>
        /// calculates the ratio difference between the two values.
        /// </summary>
        /// <param name="val1">Value one.</param>
        /// <param name="val2">Value two.</param>
        /// <returns>Ratio difference between two values.</returns>
        public static float Ratio(long val1, long val2)
        {
            return (float)val1 / val2;
        }

        /// <summary>
        /// calculates the ratio difference between the two values.
        /// </summary>
        /// <param name="val1">Value one.</param>
        /// <param name="val2">Value two.</param>
        /// <returns>Ratio difference between two values.</returns>
        public static double Ratio(double val1, double val2)
        {
            return (double)val1 / val2;
        }
        #endregion

        #region MinRatio
        /// <summary>
        /// Determine which value is the smallest ratio difference from 1.
        /// </summary>
        /// <example>
        /// (3/4, 2/5) = (15/20, 8/20) = (0.75, 0.4) = 0.75
        /// 
        /// (0.75, 1.3) = (75/100, 1 3/10) = (3/4, 13/10) = (30/40, 52/40) = (10, 12) = 10
        /// 1 - 0.75 = 0.25
        /// 1.3 - 1 = 0.3
        /// </example>
        /// <param name="ratio1">Ratio value one.</param>
        /// <param name="ratio2">Ratio value two.</param>
        /// <returns>The smaller ratio difference.</returns>
        public static float MinRatio(float ratio1, float ratio2)
        {
            // These ratios are smaller, so which ever is closer to 1.
            if (ratio1 < 1 && ratio2 < 1)
                return Math.Max(ratio1, ratio2);
            // Ratio1 is less than 1, ratio2 is greater or equal to 1.
            else if (ratio1 < 1)
            {
                var r1 = 1 - ratio1;
                var r2 = ratio2 - 1;
                var r = Math.Min(r1, r2);
                return (r == r1) ? ratio1 : ratio2;
            }
            // Ratio1 is less than 1, ratio2 is greater or equal to 1.
            else if (ratio2 < 1)
            {
                var r1 = ratio1 - 1;
                var r2 = 1 - ratio2;
                var r = Math.Min(r1, r2);
                return (r == r1) ? ratio1 : ratio2;
            }
            // Both ratios are greater then or equal to 1.
            else
                return Math.Min(ratio1, ratio2);
        }

        /// <summary>
        /// Determine which value is the smallest ratio difference from 1.
        /// </summary>
        /// <example>
        /// (3/4, 2/5) = (15/20, 8/20) = (0.75, 0.4) = 0.75
        /// 
        /// (0.75, 1.3) = (75/100, 1 3/10) = (3/4, 13/10) = (30/40, 52/40) = (10, 12) = 10
        /// 1 - 0.75 = 0.25
        /// 1.3 - 1 = 0.3
        /// </example>
        /// <param name="ratio1">Ratio value one.</param>
        /// <param name="ratio2">Ratio value two.</param>
        /// <returns>The smaller ratio difference from 1.</returns>
        public static double MinRatio(double ratio1, double ratio2)
        {
            // These ratios are smaller, so which ever is closer to 1.
            if (ratio1 < 1 && ratio2 < 1)
                return Math.Max(ratio1, ratio2);
            // Ratio1 is less than 1, ratio2 is greater or equal to 1.
            else if (ratio1 < 1)
            {
                var r1 = 1 - ratio1;
                var r2 = ratio2 - 1;
                var r = Math.Min(r1, r2);
                return (r == r1) ? ratio1 : ratio2;
            }
            // Ratio1 is less than 1, ratio2 is greater or equal to 1.
            else if (ratio2 < 1)
            {
                var r1 = ratio1 - 1;
                var r2 = 1 - ratio2;
                var r = Math.Min(r1, r2);
                return (r == r1) ? ratio1 : ratio2;
            }
            // Both ratios are greater then or equal to 1.
            else
                return Math.Min(ratio1, ratio2);
        }
        #endregion

        #region MaxRatio
        /// <summary>
        /// Determine which value is the greatest ratio difference from 1.
        /// </summary>
        /// <param name="ratio1">Ratio value one.</param>
        /// <param name="ratio2">Ratio value two.</param>
        /// <returns>The largest ratio difference from 1.</returns>
        public static float MaxRatio(float ratio1, float ratio2)
        {
            return MinRatio(ratio1, ratio2) == ratio1 ? ratio2 : ratio1;
        }

        /// <summary>
        /// Determine which value is the greatest ratio difference from 1.
        /// </summary>
        /// <param name="ratio1">Ratio value one.</param>
        /// <param name="ratio2">Ratio value two.</param>
        /// <returns>The largest ratio difference from 1.</returns>
        public static double MaxRatio(double ratio1, double ratio2)
        {
            return MinRatio(ratio1, ratio2) == ratio1 ? ratio2 : ratio1;
        }
        #endregion

        #region Resize
        /// <summary>
        /// Calculate the new size based on the ratio.
        /// </summary>
        /// <param name="value">Value to resize.</param>
        /// <param name="ratio">Ratio to use to calculate the new size.</param>
        /// <returns>New size based on ratio.</returns>
        public static float Resize(float value, float ratio)
        {
            return value * ratio;
        }

        /// <summary>
        /// Calculate the new size based on the ratio.
        /// </summary>
        /// <param name="value">Value to resize.</param>
        /// <param name="ratio">Ratio to use to calculate the new size.</param>
        /// <returns>New size based on ratio.</returns>
        public static double Resize(double value, float ratio)
        {
            return (double)(value * ratio);
        }

        /// <summary>
        /// Calculate the new size based on the ratio.
        /// </summary>
        /// <param name="value">Value to resize.</param>
        /// <param name="ratio">Ratio to use to calculate the new size.</param>
        /// <returns>New size based on ratio.</returns>
        public static double Resize(double value, double ratio)
        {
            return (double)(value * ratio);
        }

        /// <summary>
        /// Calculate the new size based on the ratio.
        /// </summary>
        /// <param name="value">Value to resize.</param>
        /// <param name="ratio">Ratio to use to calculate the new size.</param>
        /// <returns>New size based on ratio.</returns>
        public static int Resize(int value, float ratio)
        {
            return (int)(value * ratio);
        }

        /// <summary>
        /// Calculate the new size based on the ratio.
        /// </summary>
        /// <param name="value">Value to resize.</param>
        /// <param name="ratio">Ratio to use to calculate the new size.</param>
        /// <returns>New size based on ratio.</returns>
        public static long Resize(long value, float ratio)
        {
            return (long)(value * ratio);
        }
        #endregion

        #region Center
        /// <summary>
        /// Calculate the center point of the value (50%).
        /// </summary>
        /// <param name="value">Value to split.</param>
        /// <returns>50% of the value.</returns>
        public static short Center(short value)
        {
            return (short)(value / 2);
        }

        /// <summary>
        /// Calculate the center point of the value (50%).
        /// </summary>
        /// <param name="value">Value to split.</param>
        /// <returns>50% of the value.</returns>
        public static float Center(float value)
        {
            return (value / 2);
        }

        /// <summary>
        /// Calculate the center point of the value (50%).
        /// </summary>
        /// <param name="value">Value to split.</param>
        /// <returns>50% of the value.</returns>
        public static double Center(double value)
        {
            return (value / 2);
        }

        /// <summary>
        /// Calculate the center point of the value (50%).
        /// </summary>
        /// <param name="value">Value to split.</param>
        /// <returns>50% of the value.</returns>
        public static int Center(int value)
        {
            return (int)((float)value / 2);
        }

        /// <summary>
        /// Calculate the center point of the value (50%).
        /// </summary>
        /// <param name="value">Value to split.</param>
        /// <returns>50% of the value.</returns>
        public static long Center(long value)
        {
            return (int)((double)value / 2);
        }

        /// <summary>
        /// Calculate the center point of the value (50%).
        /// </summary>
        /// <param name="value">Value to split.</param>
        /// <returns>50% of the value.</returns>
        public static int Center(string value)
        {
            return (int)((float)value.Length / 2);
        }
        #endregion
        #endregion
    }
}
