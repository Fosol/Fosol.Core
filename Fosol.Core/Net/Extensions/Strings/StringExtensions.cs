using Fosol.Core.Extensions.Strings;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Fosol.Core.Net.Extensions.Strings
{
    /// <summary>
    /// StringExtensions static class, provides extensions methods for net related tasks.
    /// </summary>
    public static class StringExtensions
    {
        #region HTML
        /// <summary>
        /// Deals with characters outside of the allowable range and encodes them.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">String value to HTML encode.</param>
        /// <returns>HTML encoded string.</returns>
        public static string HtmlEncode(this string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return string.Join("", value.ToCharArray().Select(c => (int)c > 127 ? "&#" + (int)c + ";" : c.ToString()).ToArray());
            //return System.Net.WebUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Decode the value back to the original string.
        /// </summary>
        /// <param name="encodedValue">Encoded string value.</param>
        /// <returns>HTML decoded value.</returns>
        public static string HtmlDecode(this string encodedValue)
        {
            return System.Net.WebUtility.HtmlDecode(encodedValue);
        }

        /// <summary>
        /// Removes all HTML tags from the string value.
        /// This method also HtmlDecodes the string value.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">String to parse HTML from</param>
        /// <returns>String value without HTML tags</returns>
        public static string RemoveHtml(this string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return System.Text.RegularExpressions.Regex.Replace(System.Net.WebUtility.HtmlDecode(value), @"<(.|\n)*?>", string.Empty);
        }
        #endregion

        #region URIs
        /// <summary>
        /// Encode the specified URL.
        /// </summary>
        /// <param name="url">URL to be encoded.</param>
        /// <returns>Encoded URL.</returns>
        public static string UrlEncode(this string url)
        {
            return System.Net.WebUtility.UrlEncode(url);
        }

        /// <summary>
        /// Decodes the specified encoded URL.
        /// </summary>
        /// <param name="encodedUrl">Encoded URL to decode.</param>
        /// <returns>Decoded URL.</returns>
        public static string UrlDecode(this string encodedUrl)
        {
            return System.Net.WebUtility.UrlDecode(encodedUrl);
        }

        /// <summary>
        /// Parse the query string into a NameValueCollection object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "query" cannot be null.</exception>
        /// <param name="query">The query part of the URL.</param>
        /// <returns>A new instance of a NameValueCollection object.</returns>
        public static NameValueCollection ParseQueryString(this string query)
        {
            if (query.StartsWith("?"))
                query = query.Substring(1);
            return query.SplitToNameValueCollection("&", "=", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified URL is relative.
        /// </summary>
        /// <param name="url">URL value to test.</param>
        /// <returns>True if the URL is relative.</returns>
        public static bool IsRelative(this string url)
        {
            return !url.HasSchema() && !url.IsRooted();
        }

        /// <summary>
        /// Determines whether the specified URL has a schema defined.
        /// </summary>
        /// <param name="url">URL value to test.</param>
        /// <returns>True if the URL has a schema.</returns>
        public static bool HasSchema(this string url)
        {
            var colon = url.IndexOf(':');
            if (colon == -1)
                return false;

            int slash = url.IndexOf('/');
            return slash == -1 || colon < slash;
        }

        /// <summary>
        /// Determines whether the specified URL is rooted.
        /// </summary>
        /// <param name="url">URL value to test.</param>
        /// <returns>True if the URL is rooted.</returns>
        public static bool IsRooted(this string url)
        {
            return string.IsNullOrEmpty(url) || url[0] == '/' || url[0] == '\\';
        }
        #endregion
    }
}
