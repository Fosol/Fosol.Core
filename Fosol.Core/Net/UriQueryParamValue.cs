using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Core.Net
{
    /// <summary>
    /// UriQueryParamValue sealed class, provides a way to maintain multiple values for a single query parameter key.
    /// </summary>
    public sealed class UriQueryParamValue
        : IEnumerable<string>, ICollection<string>
    {
        #region Variables
        private const int _DefaultArraySize = 1;
        private const int _DefaultArrayGrowSize = 5;
        private string[] _Values;
        private int _Count;
        private int _Size;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The value at the specified index position.
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">Parameter 'index' must be a valid position within the collection.</exception>
        /// <param name="index">Index position within the collection.</param>
        /// <returns>String value at the specified index position.</returns>
        public string this[int index]
        {
            get
            {
                Validation.Argument.Assert.IsValidIndexPosition(index, _Count, nameof(index));
                return _Values[index];
            }
            set
            {
                Validation.Argument.Assert.IsValidIndexPosition(index, _Count, nameof(index));
                _Values[index] = value;
            }
        }

        /// <summary>
        /// get - Number of values within the collection.
        /// </summary>
        public int Count
        {
            get { return _Count; }
        }

        /// <summary>
        /// get - Whether the collection is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UriQueryParamValue class.
        /// </summary>
        public UriQueryParamValue()
            : this(_DefaultArraySize)
        {
        }

        /// <summary>
        /// Creates a new instance of a UriQueryParamValue class.
        /// </summary>
        /// <param name="value">Initial query parameter value.</param>
        public UriQueryParamValue(string value)
            : this(new[] { value })
        {

        }

        /// <summary>
        /// Creates a new instance of a UriQueryParamValue class.
        /// </summary>
        /// <param name="initialSize">Size of the collection.</param>
        public UriQueryParamValue(int initialSize)
        {
            _Values = new string[initialSize];
            _Size = initialSize;
        }

        /// <summary>
        /// Creates a new instance of a UriQueryParamValue class.
        /// </summary>
        /// <exception cref="System.ArgumentException">Paramter 'values' cannot be an array of 0 length.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'values' cannot be null.</exception>
        /// <param name="values">Initialization values for the collection.</param>
        public UriQueryParamValue(string[] values)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(values, nameof(values));

            _Values = new string[values.Length];
            _Size = values.Length;
            var position = 0;
            foreach (var value in values)
            {
                if (value != null)
                {
                    _Values[position++] = value;
                    _Count = position;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>IEnumerator of type string.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            var index = 0;
            foreach (var value in _Values)
            {
                if (index++ >= _Count)
                    break;

                yield return value;
            }
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>IEnumerator of type string.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns every value separated by a comma.
        /// </summary>
        /// <returns>Every value separated by a comma.</returns>
        public override string ToString()
        {
            return this.ToString(",");
        }

        /// <summary>
        /// Returns every value separated by the specified delimiter.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'delimiter' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'delimiter' cannot be null.</exception>
        /// <param name="delimiter">The text used to separate each value.</param>
        /// <returns>Every value separated by the delimiter.</returns>
        public string ToString(string delimiter)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(delimiter, nameof(delimiter));
            return _Values.Aggregate((a, b) => a + delimiter + b);
        }

        /// <summary>
        /// Adds the string value to the collection.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'value' cannot be null.</exception>
        /// <param name="value">Value to add to the collection.</param>
        public void Add(string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));

            // If the array size is too small to accept a new value, increase the array size.
            if (_Size <= _Count)
            {
                // Increase the array size.
                var values = new string[_Size + _DefaultArrayGrowSize];
                Array.Copy(_Values, values, _Count);
                _Values = values;
                _Size = _Values.Length;

            }

            _Values[_Count++] = value;
        }

        /// <summary>
        /// Clears everything out of the collection.
        /// </summary>
        public void Clear()
        {
            Array.Clear(_Values, 0, _Count);
            _Count = 0;
        }

        /// <summary>
        /// Determines if the collection contains the specified value.
        /// </summary>
        /// <param name="value">Value to look for within the collection.</param>
        /// <returns>True of the value is in the collection.</returns>
        public bool Contains(string value)
        {
            foreach (var v in _Values)
            {
                if (value.Equals(v))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copy this collection into the destination array at the specified array index.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'array' cannot be an array with 0 length.</exception>
        /// <exception cref="System.ArgumentException">Parameter 'array' must be large enough to receive this collection of values.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'array' cannot be null.</exception>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">Index position to start copying values into the destination array.</param>
        public void CopyTo(string[] array, int arrayIndex)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(array, nameof(array));
            Validation.Argument.Assert.IsValidIndexPosition(arrayIndex, array, nameof(arrayIndex));
            Validation.Argument.Assert.IsMaximum((array.Length - arrayIndex), _Count, nameof(array), "Parameter '{0}' is not large enough to be copied to.");

            for (var i = 0; i < _Count; i++)
            {
                array[i + arrayIndex] = _Values[i];
            }
        }

        /// <summary>
        /// Removes the specified value (all copies) from the collection.
        /// </summary>
        /// <param name="value">Value to remove from the collection.</param>
        /// <returns>True if the value was removed from the collection.</returns>
        public bool Remove(string value)
        {
            // Copy all other items in the array to a new array.
            var remove_success = false;
            for (var i = 0; i < _Count; i++)
            {
                if (_Values[i].Equals(value))
                {
                    _Values[i] = null;
                    remove_success = true;
                }
            }

            // Resize the array by removing the null values.
            if (remove_success)
            {
                _Values = _Values.Where(v => v != null).ToArray();
                _Size = _Values.Length;
                _Count = _Size;
            }

            return remove_success;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
