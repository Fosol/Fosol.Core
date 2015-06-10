using System;

namespace Fosol.Core.Extensions.Shorts
{
    /// <summary>
    /// ShortExtensions static class, provides extension methods for integers.
    /// </summary>
    public static class ShortExtensions
    {
        /// <summary>
        /// Converts a number into a hex value.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">Number to convert.</param>
        /// <returns>Hex value that represents the number.</returns>
        public static string ToHex(this short value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return Convert.ToString(value, 16);
        }
    }
}
