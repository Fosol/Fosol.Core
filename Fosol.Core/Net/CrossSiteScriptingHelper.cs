using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Net
{
    /// <summary>
    /// CrossSiteScriptingHelper static class provides a way to test string values for cross site scripting syntax.
    /// </summary>
    public static class CrossSiteScriptingHelper
    {
        #region Variables
        static readonly char[] _StartingChars = new char[] { '<', '&' };
        #endregion

        #region Methods
        /// <summary>
        /// Determine if the character is a letter from A-Z or a-z.
        /// </summary>
        /// <param name="value">Character to test.</param>
        /// <returns>True if the letter is alphabetic.</returns>
        internal static bool IsAlphabetic(char value)
        {
            return (value >= 'a' && value <= 'z') || (value >= 'A' && value <= 'Z');
        }

        /// <summary>
        /// Determine if the value specified is a dangerous cross site script.
        /// </summary>
        /// <param name="value">String value to test.</param>
        /// <returns>True if the value is a dangerous cross site script.</returns>
        public static bool IsDangerousString(string value)
        {
            int pos;

            return CrossSiteScriptingHelper.IsDangerousString(value, out pos);
        }

        /// <summary>
        /// Determine if the value specified is a dangerous cross site script.
        /// </summary>
        /// <param name="value">String value to test.</param>
        /// <param name="matchIndex">Index position of the dangerous character.</param>
        /// <returns>True if the value is a dangerous cross site script.</returns>
        public static bool IsDangerousString(string value, out int matchIndex)
        {
            matchIndex = -1;

            if (String.IsNullOrEmpty(value))
                return false;

            var index = 0;

            while (true)
            {
                var pos = value.IndexOfAny(_StartingChars, index);

                if (pos < 0)
                    break;

                if (pos == value.Length - 1)
                    return false;

                matchIndex = pos;

                var c = value[pos];

                if (c != '&')
                {
                    if (c == '<' && (CrossSiteScriptingHelper.IsAlphabetic(value[pos + 1])
                        || value[pos + 1] == '!'
                        || value[pos + 1] == '/'
                        || value[pos + 1] == '?'))
                        return true;
                }
                else
                {
                    if (value[pos + 1] == '#')
                        return true;
                }
                index = pos + 1;
            }

            return false;
        }

        /// <summary>
        /// Determine if the value specified is a dangerous cross site script Url.
        /// </summary>
        /// <param name="value">Url value to test.</param>
        /// <returns>True if the value is a dangerous cross site script.</returns>
        public static bool IsDangerousUrl(string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;

            value = value.Trim();

            var length = value.Length;

#if WINDOWS_APP || WINDOWS_PHONE_APP
            var string_comparison = StringComparison.CurrentCultureIgnoreCase;
#else
            var string_comparison = StringComparison.InvariantCultureIgnoreCase;
#endif

            if ((length > 4 && value.StartsWith("http:", string_comparison))
                || (length > 5 && value.StartsWith("https:", string_comparison)))
                return false;

            var query_pos = value.IndexOf('?');
            var invalid_char_pos = value.IndexOf('&');

            if (invalid_char_pos > -1
                && (query_pos > invalid_char_pos || query_pos == -1))
                return true;

            var pos = value.IndexOf(':');
            return pos != -1;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
