using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Fosol.Core.Extensions.NameValueCollections
{
    /// <summary>
    /// NameValueCollectionExtensions static class, provides extension methods for NameValueCollection objects.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Determines if the NameValueCollection has the same key and value pairs.
        /// </summary>
        /// <param name="obj">Original NameValueCollection object.</param>
        /// <param name="compare">NameValueCollection to compare with.</param>
        /// <returns>True if they are equal.</returns>
        public static bool IsEqual(this NameValueCollection obj, NameValueCollection compare)
        {
            return obj.AllKeys.OrderBy(k => k)
                .SequenceEqual(compare.AllKeys.OrderBy(k => k))
                && obj.AllKeys.All(k => obj[k] == compare[k]);
        }

        /// <summary>
        /// Aggregates the NameValueCollection into a query string.
        /// </summary>
        /// <param name="obj">NameValueCollection object.</param>
        /// <returns>Query string.</returns>
        public static string ToQueryString(this NameValueCollection obj)
        {
            var builder = new StringBuilder();
            foreach (var key in obj.AllKeys)
            {
                if (builder.Length > 0)
                    builder.Append("&");
                builder.Append(key + "=" + obj[key]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Converts a NameValueCollection object into a StringDictionary.
        /// </summary>
        /// <param name="obj">NameValueCollection object.</param>
        /// <returns>New StringDictionary object.</returns>
        public static StringDictionary ToStringDictionary(this NameValueCollection obj)
        {
            if (obj == null)
                return null;

            var dictionary = new StringDictionary();

            foreach (var key in obj.AllKeys)
            {
                dictionary.Add(key, obj[key]);
            }

            return dictionary;
        }

        /// <summary>
        /// Converts a NameValueCollection object into a Dictionary.
        /// </summary>
        /// <param name="obj">NameValueCollection object.</param>
        /// <returns>New Dictionary object.</returns>
        public static Dictionary<string, object> ToDictionary(this NameValueCollection obj)
        {
            if (obj == null)
                return null;

            var dictionary = new Dictionary<string, object>();

            foreach (var key in obj.AllKeys)
            {
                dictionary.Add(key, obj[key]);
            }

            return dictionary;
        }
    }
}
