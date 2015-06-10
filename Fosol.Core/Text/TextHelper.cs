using Fosol.Core.Extensions.Strings;
using System;

namespace Fosol.Core.Text
{
    /// <summary>
    /// TextHelper static class, provides useful settings for text.
    /// </summary>
    public static class TextHelper
    {
        #region Variables
        /// <summary>
        /// Lowercase characters.
        /// </summary>
        public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Uppercase characters.
        /// </summary>
        public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Digit characters.
        /// </summary>
        public const string Digit = "0123456789";

        /// <summary>
        /// All lowercase and uppercase characters.
        /// </summary>
        public const string Letter = Lowercase + Uppercase;

        /// <summary>
        /// All lowercase, uppercase and digit characters.
        /// </summary>
        public const string Alphanumeric = Letter + Digit;

        /// <summary>
        /// Digit characters, negative symbol and decimal point.
        /// </summary>
        public const string Decimals = Digit + "-.";

        /// <summary>
        /// Digit characters and negative symbol.
        /// </summary>
        public const string Integers = "-" + Digit;
        #endregion

        #region Methods
        /// <summary>
        /// Counts the number of words in the text.
        /// </summary>
        /// <param name="text">Text to count words in.</param>
        /// <returns>Number of words.</returns>
        public static int WordCount(string text)
        {
            return text.WordCount();
        }
        #endregion
    }
}
