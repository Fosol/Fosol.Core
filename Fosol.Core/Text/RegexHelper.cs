using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fosol.Core.Text
{
    /// <summary>
    /// RegexHelper static class, provides methods to help with regular expressions.
    /// </summary>
    public static class RegexHelper
    {
        #region Variables
        const string _EscapeCharacters = @"\#$^*()+[]{}|.? ";
        #endregion

        #region Methods
        /// <summary>
        /// Due to the fact that Regex.Escape doesn't escape closing brackets this method will escape every required characters.
        /// Escapes the following characters in the specified string value.
        /// Escape characters: "\#$^*()+[]{}|.? " (not including the double quotes).
        /// </summary>
        /// <param name="regex">Regex object.</param>
        /// <param name="value">Value to escape.</param>
        /// <returns>Updated value with escaped characters.</returns>
        public static string EscapeAll(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            var result = new StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                if (_EscapeCharacters.Contains(value[i]))
                {
                    if (char.IsWhiteSpace(value[i]))
                        result.Append(@"\s");
                    else
                        result.Append('\\');
                }

                result.Append(value[i]);

            }
            return result.ToString();
        }
        #endregion
    }
}
