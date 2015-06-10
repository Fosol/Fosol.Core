using static Fosol.Core.Mathematics.Formula;
using System;
using System.Drawing;

namespace Fosol.Core.Drawing.Mathematics
{
    /// <summary>
    /// Formula static class, provides helpful math formula.
    /// </summary>
    public static class Formula
    {
        #region OffsetCenter
        /// <summary>
        /// Calculate the offset of the center based on the offset point.
        /// The offset parameter is a coordinate ratio that places it within the value at the specified point.
        /// An offset of '-1' will always return a value of '1'.
        /// An offset of '1' will always return the value as it is.
        /// An offset of '0.5' will always return the center of the value.
        /// </summary>
        /// <param name="value">Value to offset.</param>
        /// <param name="offset">Offset ratio value.</param>
        /// <returns>The offset from center.</returns>
        public static float OffsetCenter(float value, float offset)
        {
            Validation.Argument.Assert.IsInRange(offset, -1, 1, nameof(offset));

            if (value == 0)
                return value;

            var center = Center(value);

            // Return the true center.
            if (offset == 0)
                return center;
            else if (offset == -1)
                return 0;
            else if (offset == 1)
                return value;
            // 100, 0 = 50
            // 100, 0.5 = (0.5 * 50) = 100 - 25 = 75
            else if (offset > 0)
                return value - Math.Abs(center * offset);
            // 100, -0.5 = (-0.5 * 50) = 50 - 25 = 25
            else
                return center - Math.Abs(center * offset);
        }

        /// <summary>
        /// Calculate the offset of the center based on the offset point.
        /// The offset parameter is a coordinate ratio that places it within the value at the specified point.
        /// An offset of '-1' will always return a value of '1'.
        /// An offset of '1' will always return the value as it is.
        /// An offset of '0.5' will always return the center of the value.
        /// </summary>
        /// <param name="value">Value to offset.</param>
        /// <param name="offset">Offset ratio value.</param>
        /// <returns>The offset from center.</returns>
        public static double OffsetCenter(double value, float offset)
        {
            Validation.Argument.Assert.IsInRange(offset, -1, 1, nameof(offset));

            if (value == 0)
                return value;

            var center = Center(value);
            if (offset == 0)
                return center;
            else if (offset == -1)
                return 0;
            else if (offset == 1)
                return value;
            // 100, 0 = 50
            // 100, 0.5 = (0.5 * 50) = 100 - 25 = 75
            else if (offset > 0)
                return value - Math.Abs(center * offset);
            // 100, -0.5 = (-0.5 * 50) = 50 - 25 = 25
            else
                return center - Math.Abs(center * offset);
        }
        #endregion

        #region Scale
        /// <summary>
        /// Calculate the destination rectangle that will host the object in the new size.
        /// </summary>
        /// <param name="size">Original size of the object.</param>
        /// <param name="resize">Desired size of the new object.</param>
        /// <param name="offset">Offset point within the new size.</param>
        /// <param name="allowWhitespace">
        ///     When 'true' it will ensure the object is not cropped.  
        ///     When 'false' the object will fill the new size and crop anything extending beyond the new size.
        /// </param>
        /// <returns>Destination rectangle.</returns>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        public static Rect Scale(Size size, Size resize, Fosol.Common.CenterPoint offset, bool allowWhitespace = true)
#else
        public static Rectangle Scale(Size size, Size resize, CenterPoint offset, bool allowWhitespace = true)
#endif
        {
            return Scale(size, resize, offset.X, offset.Y, allowWhitespace);
        }

        /// <summary>
        /// Calculate the destination rectangle that will host the object in the new size.
        /// </summary>
        /// <param name="size">Original size of the object.</param>
        /// <param name="resize">Desired size of the new object.</param>
        /// <param name="xOffset">Horizontal x-axis offset point within the new size.</param>
        /// <param name="yOffset">Vertical y-axis offset point within the new size.</param>
        /// <param name="allowWhitespace">
        ///     When 'true' it will ensure the object is not cropped.  
        ///     When 'false' the object will fill the new size and crop anything extending beyond the new size.
        /// </param>
        /// <returns>Destination rectangle.</returns>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        public static Rect Scale(Size size, Size resize, float xOffset = 0f, float yOffset = 0f, bool allowWhitespace = true)
#else
        public static Rectangle Scale(Size size, Size resize, float xOffset = 0f, float yOffset = 0f, bool allowWhitespace = true)
#endif
        {
            Validation.Argument.Assert.IsNotNull(size, nameof(size));
            Validation.Argument.Assert.IsNotNull(resize, nameof(resize));
            Validation.Argument.Assert.IsInRange(xOffset, -1, 1, nameof(xOffset));
            Validation.Argument.Assert.IsInRange(yOffset, -1, 1, nameof(yOffset));

#if WINDOWS_APP || WINDOWS_PHONE_APP
            var dest = new Rect(0, 0, resize.Width, resize.Height);
#else
            var dest = new Rectangle(0, 0, resize.Width, resize.Height);
#endif

            if (size.Equals(resize))
                return dest;

            var wr = Ratio(resize.Width, size.Width);
            var hr = Ratio(resize.Height, size.Height);

            var min = Math.Min(wr, hr);
            var max = Math.Max(wr, hr);

            // Using the minimum ratio will result in whitespace.
            var ratio = allowWhitespace ? min : max;

            dest.Width = Resize(size.Width, ratio);
            dest.Height = Resize(size.Height, ratio);

            // Restricts scaling to remain the boundaries of resize.
            if (allowWhitespace)
            {
                // Center the object within the resize.
                dest.X = (int)OffsetCenter(resize.Width - dest.Width, xOffset);
                dest.Y = (int)OffsetCenter(resize.Height - dest.Height, yOffset * -1);
            }
            // The object will scale using the max ratio, which means it will be cropped.
            else
            {
                dest.X = (int)OffsetCenter(dest.Width - resize.Width, xOffset) * -1;
                dest.Y = (int)OffsetCenter(dest.Height - resize.Height, yOffset * -1) * -1;
            }

            return dest;
        }
        #endregion
    }
}
