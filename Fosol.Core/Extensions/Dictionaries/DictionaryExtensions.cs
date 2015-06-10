using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Fosol.Core.Extensions.Dictionaries
{
    public static class DictionaryExtensions
    {
        #region StringDictionary
        /// <summary>
        /// Determine if the StringDictionary has the same key and value pairs.
        /// </summary>
        /// <param name="source">Source StringDictionary object.</param>
        /// <param name="compare">StringDictionary object to compare.</param>
        /// <returns>True if they are equal.</returns>
        public static bool AreEqual(this StringDictionary source, StringDictionary compare)
        {
            if (ReferenceEquals(source, compare))
                return true;

            if (source.Keys.Count != compare.Keys.Count)
                return false;

            foreach (string key in source.Keys)
            {
                if (!compare.ContainsKey(key))
                    return false;
                else if (ReferenceEquals(source[key], compare[key]))
                    continue;
                else if (!source[key].Equals(compare[key]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Aggregates the StringDictionary into a query string.
        /// </summary>
        /// <param name="source">StringDictionary object.</param>
        /// <returns>Query string.</returns>
        public static string ToQueryString(this StringDictionary source)
        {
            var builder = new StringBuilder();
            foreach (string key in source.Keys)
            {
                if (builder.Length > 0)
                    builder.Append("&");
                builder.Append(key + "=" + source[key]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Converts the StringDictionary into a generic Dictionary.
        /// </summary>
        /// <param name="source">StringDictionary object.</param>
        /// <returns>New Dictionary object.</returns>
        public static Dictionary<string, object> ToDictionary(this StringDictionary source)
        {
            if (source == null)
                return null;

            var dictionary = new Dictionary<string, object>();

            foreach (string key in source.Keys)
            {
                var v = source[key];
                dictionary.Add(key, source[key]);
            }

            return dictionary;
        }
        #endregion

        #region Dictionary<KT, VT>
        /// <summary>
        /// Determines if the Dictionary has the same key and value pairs.
        /// </summary>
        /// <typeparam name="KT">Key type of the dictionaries.</typeparam>
        /// <typeparam name="VT">Value type of the dicionaries.</typeparam>
        /// <param name="source">Source Dictionary object.</param>
        /// <param name="compare">Dictionary object to compare.</param>
        /// <returns>True if they are equal.</returns>
        public static bool AreEqual<KT, VT>(this Dictionary<KT, VT> source, Dictionary<KT, VT> compare)
        {
            if (ReferenceEquals(source, compare))
                return true;

            if (source.Keys.Count != compare.Keys.Count)
                return false;

            foreach (KT key in source.Keys)
            {
                if (!compare.ContainsKey(key))
                    return false;
                else if (ReferenceEquals(source[key], compare[key]))
                    continue;
                else if (!source[key].Equals(compare[key]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Aggregates the Dictionary into a query string.
        /// </summary>
        /// <param name="source">StringDictionary object.</param>
        /// <returns>Query string.</returns>
        public static string ToQueryString(this Dictionary<string, string> source)
        {
            var builder = new StringBuilder();
            foreach (string key in source.Keys)
            {
                if (builder.Length > 0)
                    builder.Append("&");
                builder.Append(key + "=" + source[key]);
            }
            return builder.ToString();
        }
        #endregion
    }
}
