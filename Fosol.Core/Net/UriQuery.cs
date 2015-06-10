using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Fosol.Core.Net
{
    /// <summary>
    /// UriQuery sealed class, provides a way to maintain a collection of query string parameters and their values.
    /// - Handles query parameters that have multiple values.
    /// </summary>
    public sealed class UriQuery
        : IEnumerable<UriQueryParam>
    {
        #region Variables
        private const string _FormatBoundary = @"\A({0})\Z";
#if WINDOWS_APP || WINDOWS_PHONE_APP
        private static readonly Regex _QueryRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.QueryRegex), RegexOptions.None);
#else
        private static readonly Regex _QueryRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.QueryRegex), RegexOptions.Compiled);
#endif
        private readonly Dictionary<string, UriQueryParam> _Parameters;
        #endregion

        #region Properties
        /// <summary>
        /// get - A collection of UriQueryParam objects that contain the query parameter value.
        /// </summary>
        public ICollection<UriQueryParam> Parameters
        {
            get { return _Parameters.Select(v => v.Value).ToList(); }
        }

        /// <summary>
        /// get/set - The UriQueryParam object with the specified key name.
        /// </summary>
        /// <param name="key">Name to identify the query parameter.</param>
        /// <returns>QueryParam object for the specified key name.</returns>
        public UriQueryParam this[string key]
        {
            get
            {
                return _Parameters[key];
            }
            set
            {
                _Parameters[key] = value;
            }
        }

        /// <summary>
        /// get - Collection of key names.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return _Parameters.Keys; }
        }

        /// <summary>
        /// get - Number of QueryParam objects in the collection.
        /// </summary>
        public int Count
        {
            get { return _Parameters.Count; }
        }

        /// <summary>
        /// get - Whether this collection is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UriQuery class.
        /// </summary>
        public UriQuery()
        {
            _Parameters = new Dictionary<string, UriQueryParam>();
        }

        /// <summary>
        /// Creates a new instance of a UriQuery class.
        /// </summary>
        /// <param name="queryString">Initialize the query string parameters with this value.</param>
        /// <param name="decode">Whether the key value pairs should be URL decoded.</param>
        public UriQuery(string queryString, bool decode = false)
            : this()
        {
            var values = UriQuery.ParseQueryStringToKeyValuePair(queryString, decode);

            foreach (var kv in values)
            {
                this.Add(kv);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the enumerator for the collection of UriQueryParam objects.
        /// </summary>
        /// <returns>Enumerator for the collection of UriQueryParam objects.</returns>
        public IEnumerator<UriQueryParam> GetEnumerator()
        {
            var parameters = _Parameters.ToArray();

            foreach (var p in parameters)
            {
                yield return p.Value;
            }
        }

        /// <summary>
        /// Get the enumerator for the collection of UriQueryParam objects.
        /// </summary>
        /// <returns>Enumerator for the collection of UriQueryParam objects.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Parameters.GetEnumerator();
        }

        /// <summary>
        /// Add the UriQueryParam object to the collection.
        /// If the collection already contains a UriQueryParam with the specified key name it will be overwritten by this one.
        /// </summary>
        /// <param name="item">UriQueryParam object to add to the collection.</param>
        public void Add(UriQueryParam item)
        {
            Validation.Argument.Assert.IsNotNull(item, nameof(item));

            if (this.ContainsKey(item.Name))
                this[item.Name] = item;
            else
                _Parameters.Add(item.Name, item);
        }

        /// <summary>
        /// Add the query parameter to the collection.
        /// If the collection already contains a QueryParam with the specified key name it will add this value to it.
        /// </summary>
        /// <param name="item">KeyValuePair item to add to the collection.</param>
        public void Add(KeyValuePair<string, string> item)
        {
            Validation.Argument.Assert.IsNotNull(item, nameof(item));

            if (this.ContainsKey(item.Key))
                this[item.Key].Add(item.Value);
            else
                _Parameters.Add(item.Key, new UriQueryParam(item.Key, item.Value));
        }

        /// <summary>
        /// Add the key and value to the query parameter collection.
        /// If the collection already contains a QueryParam with the specified key name it will add this value to it.
        /// </summary>
        /// <param name="key">Name to identify the query parameter.</param>
        /// <param name="value">Value of the query parameter.</param>
        public void Add(string key, string value)
        {
            Validation.Argument.Assert.IsNotNull(key, nameof(key));
            Validation.Argument.Assert.IsNotNull(value, nameof(value));

            if (this.ContainsKey(key))
                this[key].Add(value);
            else
                this.Add(new UriQueryParam(key, value));
        }

        /// <summary>
        /// Remove the UriQueryParam that has the specified key name.
        /// </summary>
        /// <param name="key">Name to identify the query parameter.</param>
        /// <returns>True if the item was removed from the collection.</returns>
        public bool Remove(string key)
        {
            return _Parameters.Remove(key);
        }

        /// <summary>
        /// Attempt to find the query parameter with the specified key name and return it.
        /// </summary>
        /// <param name="key">Name to identify the query parameter.</param>
        /// <param name="value">UriQueryParam object with the specified key name.</param>
        /// <returns>True if the query parameter was found.</returns>
        public bool TryGetValue(string key, out UriQueryParam value)
        {
            return _Parameters.TryGetValue(key, out value);
        }

        /// <summary>
        /// Clear all of the query parameters from the collection.
        /// </summary>
        public void Clear()
        {
            _Parameters.Clear();
        }

        /// <summary>
        /// Determine if the collection contains a query parameter with the specified key name.
        /// </summary>
        /// <param name="key">Name to identify the query parameter.</param>
        /// <returns>True if the collection contains a query parameter with the specified key name.</returns>
        public bool Contains(string key)
        {
            Validation.Argument.Assert.IsNotNull(key, nameof(key));
            return _Parameters.ContainsKey(key);
        }

        /// <summary>
        /// Determines if the collection contains the specified key name.
        /// </summary>
        /// <param name="key">Name to identify the query parameter.</param>
        /// <returns>True if the collection contains a query parameter with the specified key name.</returns>
        public bool ContainsKey(string key)
        {
            Validation.Argument.Assert.IsNotNull(key, nameof(key));
            return _Parameters.ContainsKey(key);
        }

        /// <summary>
        /// Copy te contents of the collection into the specified array.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">Index position to start copying within the destination array.</param>
        public void CopyTo(UriQueryParam[] array, int arrayIndex)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(array, nameof(array));
            Validation.Argument.Assert.IsValidIndexPosition(arrayIndex, array, nameof(arrayIndex));
            Validation.Argument.Assert.IsMaximum(array.Length - arrayIndex, _Parameters.Count, nameof(array), "Parameter 'array' must have a length greater than equal to the number of items in this collection.");

            var index = 0;
            foreach (var value in _Parameters)
            {
                array[arrayIndex + index++] = new UriQueryParam(value.Key, value.Value.Values);
            }
        }

        /// <summary>
        /// Returns the full query string with all key value pairs.
        /// </summary>
        /// <returns>Full query string with all key value pairs.</returns>
        public override string ToString()
        {
            if (_Parameters.Count == 0)
                return String.Empty;

            return _Parameters.Select(v => v.Value.ToString()).Aggregate((a, b) => a + "&" + b);
        }

        /// <summary>
        /// Parse the specified query string into a UriQuery object.
        /// </summary>
        /// <param name="queryString">Raw query string value.</param>
        /// <param name="decode">Whether the key value pairs should be decoded.</param>
        /// <returns>A new instance of a QueryParamCollection object.</returns>
        public static UriQuery ParseQueryString(string queryString, bool decode = true)
        {
            return new UriQuery(queryString, decode);
        }

        /// <summary>
        /// Parse the specified query string into a NameValueCollection object.
        /// </summary>
        /// <param name="queryString">Raw query string value.</param>
        /// <param name="decode">Whether the key value pairs should be decoded.</param>
        /// <returns>A new instance of a NameValueCollection object.</returns>
        public static NameValueCollection ParseQueryStringToNameValueCollection(string queryString, bool decode = true)
        {
            var result = new NameValueCollection();
            var values = UriQuery.ParseQueryStringToKeyValuePair(queryString, decode);

            foreach (var value in values)
            {
                result.Add(value.Key, value.Value);
            }

            return result;
        }

        /// <summary>
        /// Parse the specified query string into a collection of KeyValuePair objects.
        /// Parses from the first character or the first questionmark character '?'.
        /// Parses to the last character or the first pound character '#'.
        /// </summary>
        /// <param name="queryString">Raw query string value.</param>
        /// <param name="decode">Whether the key value pairs should be decoded.</param>
        /// <returns>A new instance of a Collection of KeyValuePair objects.</returns>
        public static List<KeyValuePair<string, string>> ParseQueryStringToKeyValuePair(string queryString, bool decode = false)
        {
            var keys = new List<KeyValuePair<string, string>>();

            if (String.IsNullOrEmpty(queryString))
                return keys;

            // Find the first questionmark character.
            var index_of_q = queryString.IndexOf('?');
            if (index_of_q != -1)
            {
                if (index_of_q + 1 >= queryString.Length)
                    return keys;

                queryString = queryString.Substring(index_of_q + 1);
            }

            // Find the first pound character.
            var index_of_pound = queryString.IndexOf('#');
            if (index_of_pound != -1)
            {
                queryString = queryString.Substring(0, index_of_pound);
            }

            if (String.IsNullOrEmpty(queryString))
                return keys;

            // Replace spaces with UrlEncoded spaces.
            queryString = UriBuilder.ReplaceWhitespaces(queryString);

            // Validate the queryString.
            var match = _QueryRegex.Match(queryString);
            if (!match.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                throw new FormatException("Query string value has invalid characters.");
#else
                throw new UriFormatException("Query string value has invalid characters.");
#endif

            var k_start = 0;
            for (var i = 0; i < queryString.Length; i++)
            {
                if (queryString[i] == '&')
                {
                    keys.Add(UriQuery.ParseKeyValuePair(queryString.Substring(k_start, i - k_start), decode));

                    i++;
                    k_start = i;
                }
            }

            // There was only one key value pair.
            if (k_start == 0
                && keys.Count == 0)
                keys.Add(UriQuery.ParseKeyValuePair(queryString, decode));
            else if (k_start > 0)
                // This is the last key value pair in the query string.
                keys.Add(UriQuery.ParseKeyValuePair(queryString.Substring(k_start), decode));

            return keys;
        }

        /// <summary>
        /// Parse the key value pair into a KeyValuePair object.
        /// </summary>
        /// <param name="keyAndValue">Raw key value pair text value.</param>
        /// <param name="decode">Whether the key value pair should be URL decoded.</param>
        /// <returns>A new instance of a KeyValuePair object.</returns>
        private static KeyValuePair<string, string> ParseKeyValuePair(string keyAndValue, bool decode = false)
        {
            // This is an empty key value.
            if (keyAndValue.Equals("="))
                return new KeyValuePair<string, string>(keyAndValue, String.Empty);

            for (var i = 0; i < keyAndValue.Length; i++)
            {
                // Found the separater for the key value pair.
                if (keyAndValue[i] == '=')
                {
                    // This one has a key value.
                    if (i > 0)
                    {
                        var key = decode ? WebUtility.UrlDecode(keyAndValue.Substring(0, i)) : keyAndValue.Substring(0, i);
                        var value = decode ? WebUtility.UrlDecode(keyAndValue.Substring(i + 1)) : keyAndValue.Substring(i + 1);
                        return new KeyValuePair<string, string>(key, value);
                    }
                    else
                    {
                        // This one has no key value.
                        var value = decode ? WebUtility.UrlDecode(keyAndValue.Substring(i + 1)) : keyAndValue.Substring(i + 1);
                        return new KeyValuePair<string, string>(String.Empty, value);
                    }
                }
            }

            // The whole keyAndValue is only a key.
            return new KeyValuePair<string, string>((decode ? WebUtility.UrlDecode(keyAndValue) : keyAndValue), String.Empty);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
