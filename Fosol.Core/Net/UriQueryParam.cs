using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Core.Net
{
    /// <summary>
    /// UriQueryParam sealed class, provides a way to manage a single query parameter key and all it's values.
    /// </summary>
    public sealed class UriQueryParam
    {
        #region Variables
        private string _Name;
        private UriQueryParamValue _Values;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The value of this parameter at the specified index position.
        /// </summary>
        /// <param name="index">Index position.</param>
        /// <returns>Value at the specified index position.</returns>
        public string this[int index]
        {
            get { return _Values[index]; }
            set { _Values[index] = value; }
        }

        /// <summary>
        /// get - The name to identify this query string parameter.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        /// <summary>
        /// get - Array of values for this query string paramter.
        /// </summary>
        public string[] Values
        {
            get
            {
                return _Values.Select(v => v.ToString()).ToArray();
            }
        }

        /// <summary>
        /// get - Whether this query string parameter has multiple values.
        /// </summary>
        public bool IsMultiValue
        {
            get { return _Values.Count > 1; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UriQueryParam class.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'name' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Paramter 'name' cannot be null.</exception>
        /// <param name="name">Name to identify this query parameter.</param>
        public UriQueryParam(string name)
            : this(name, String.Empty)
        {
        }

        /// <summary>
        /// Creates a new instance of a UriQueryParam class.
        /// </summary>
        /// <param name="name">Name to identify this query parameter.</param>
        /// <param name="value">Initial value of the query parameter.</param>
        public UriQueryParam(string name, string value)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(name, nameof(name));
            Validation.Argument.Assert.IsNotNullOrEmpty(value, nameof(value));
            _Name = name;
            _Values = new UriQueryParamValue(value);
        }

        /// <summary>
        /// Creates a new instance of a UriQueryParam class.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'values' cannot be an empty 0 length array.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'values' cannot be null.</exception>
        /// <param name="name">Name to identify this query parameter.</param>
        /// <param name="values">Initial array of values for this query parameter.</param>
        public UriQueryParam(string name, string[] values)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(name, nameof(name));
            Validation.Argument.Assert.IsNotNullOrEmpty(values, nameof(values));
            Validation.Argument.Assert.IsNotNullOrEmpty(values.Aggregate((a, b) => a + b), nameof(values));

            _Name = name;
            _Values = new UriQueryParamValue(values);
        }

        /// <summary>
        /// Creates a new instance of a UriQueryParam class.
        /// </summary>
        /// <exception cref="Systme.ArgumentNullException">Parameter 'parameter' cannot be null.</exception>
        /// <param name="parameter">KeyValuePair object.</param>
        public UriQueryParam(KeyValuePair<string, string> parameter)
        {
            Validation.Argument.Assert.IsNotNull(parameter, nameof(parameter));
            Validation.Argument.Assert.IsNotNullOrEmpty(parameter.Key, nameof(parameter));
            Validation.Argument.Assert.IsNotNullOrEmpty(parameter.Value, nameof(parameter));

            _Name = parameter.Key;
            _Values = new UriQueryParamValue(parameter.Value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a full query string parameter key value pair (i.e. key=value).
        /// If the key contains mulitple value it will aggregate them (i.e. key=value1&key=value2&key=value3).
        /// </summary>
        /// <returns>Query string key value pair.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (var value in _Values)
            {
                // Don't add blanks.
                if (String.IsNullOrEmpty(this.Name)
                    && String.IsNullOrEmpty(value))
                    continue;

                if (result.Length > 0)
                    result.Append("&");

                if (!String.IsNullOrEmpty(value))
                    result.Append(String.Format("{0}={1}", this.Name, value));
                else
                    result.Append(this.Name);
            }
            return result.ToString();
        }

        /// <summary>
        /// Add a new value to the query parameter.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <param name="allowDuplicates">Whether or not to allow duplicate values to be added.</param>
        public void Add(string value, bool allowDuplicates = true)
        {
            if (allowDuplicates
                || (!allowDuplicates && !_Values.Contains(value)))
                _Values.Add(value);
        }

        /// <summary>
        /// Add each value in the array to the query parameter.
        /// </summary>
        /// <param name="values">Array of values to add.</param>
        /// <param name="allowDuplicates">Whether or not to allow duplicate values to be added.</param>
        public void Add(string[] values, bool allowDuplicates = true)
        {
            foreach (var value in values)
            {
                this.Add(value, allowDuplicates);
            }
        }

        /// <summary>
        /// Removes all copies of the value from the query parameter.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        /// <returns>True if the value was found and removed.</returns>
        public bool Remove(string value)
        {
            return _Values.Remove(value);
        }

        /// <summary>
        /// Clears all the values from the query parameter.
        /// </summary>
        public void Clear()
        {
            _Values.Clear();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
