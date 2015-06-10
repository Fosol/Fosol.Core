using Fosol.Core.Extensions.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Fosol.Core.ServiceModel.Converters
{
    /// <summary>
    /// Provides a way to convert QueryString values into complex types (i.e. arrays and collections of objects).
    /// </summary>
    public sealed class ComplexQueryConverter 
        : QueryStringConverter
    {
        #region Variables
        /// <summary>
        /// By default .NET uses a comma when grouping an array of QueryString parameters into a single string value.
        /// </summary>
        private const string DEFAULT_KEYVALUE_DELIMITER = ",";
        private string _KeyValueDelimiter = DEFAULT_KEYVALUE_DELIMITER;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The delimiter used within complex querystring values ([tag]=[key][KeyValueDelimiter][value]).
        /// This property is only used when the QueryString results contains KeyValuePairs.
        /// </summary>
        /// <example>tag=key=value,key=value</example>
        private string KeyValueDelimiter
        {
            get { return _KeyValueDelimiter; }
            set { _KeyValueDelimiter = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ComplexQueryConverter object.
        /// </summary>
        public ComplexQueryConverter()
        {

        }

        /// <summary>
        /// Creates a new instance of a ComplexQueryConverter object.
        /// </summary>
        /// <param name="keyValueDelimiter">Initial KeyValueDelimiter property value.</param>
        public ComplexQueryConverter(string keyValueDelimiter)
        {
            this.KeyValueDelimiter = keyValueDelimiter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check to see if the specified type can be converted by this Converter.
        /// </summary>
        /// <param name="type">Value type.</param>
        /// <returns>True if the type is convertable.</returns>
        public override bool CanConvert(Type type)
        {
            // We can convert arrays and List objects.
            if (type.IsArray || typeof(System.Collections.IList).IsAssignableFrom(type))
            {
                var element_type = type.GetElementType();
                return type.GetProperty("Item", typeof(KeyValuePair<string, string>)) != null || element_type == typeof(string);
            }
            else
                return type == typeof(DateTime?) || base.CanConvert(type);
        }

        /// <summary>
        /// Convert a string into an object.
        /// Handles Nullable DateTime objects, String arrays and Collections of KeyValuePairs.
        /// </summary>
        /// <param name="parameter">String value to convert into the desired object.</param>
        /// <param name="parameterType">Parameter object type.</param>
        /// <returns>Object after being converted.</returns>
        public override object ConvertStringToValue(string parameter, Type parameterType)
        {
            // Convert the string into an array or a collection.
            if (parameterType.IsArray || typeof(System.Collections.IList).IsAssignableFrom(parameterType))
            {
                if (parameter == null)
                    return null;

                Type element_type = parameterType.GetElementType();

                // This is an array of string.
                if (element_type == typeof(string))
                {
                    var values = parameter.Split(DEFAULT_KEYVALUE_DELIMITER, StringComparison.InvariantCultureIgnoreCase);
                    Array result = Array.CreateInstance(element_type, values.Length);
                    for (int i = 0; i < values.Length; i++)
                        result.SetValue(base.ConvertStringToValue(values[i], element_type), i);

                    return result;
                }
                // This is an array of KeyValuePair objects.
                else if (element_type == typeof(KeyValuePair<string, string>))
                {
                    var values = parameter.SplitToKeyValuePairs(DEFAULT_KEYVALUE_DELIMITER, this.KeyValueDelimiter, StringComparison.InvariantCultureIgnoreCase);
                    Array result = Array.CreateInstance(typeof(KeyValuePair<string, string>), values.Count);
                    int i = 0;
                    foreach (var value in values)
                    {
                        result.SetValue(value, i++);
                    }

                    return result;
                }
                // This is a List with collection objects of type KeyValuePair<string, string>.
                else if (parameterType.GetProperty("Item", typeof(KeyValuePair<string, string>)) != null)
                {
                    var values = parameter.SplitToKeyValuePairs(DEFAULT_KEYVALUE_DELIMITER, this.KeyValueDelimiter, StringComparison.InvariantCultureIgnoreCase);
                    var result = new List<KeyValuePair<string, string>>();
                    foreach (var value in values)
                        result.Add(value);

                    return result;
                }
            }
            // Provides a way to return nullable dates.
            else if (parameterType == typeof(DateTime?))
            {
                DateTime date;
                if (DateTime.TryParse(parameter ?? string.Empty, out date))
                    return (DateTime?)date;

                return null;
            }

            // Convert all other string to standard objects.
            return base.ConvertStringToValue(parameter, parameterType);
        }

        /// <summary>
        /// Converts a value to a string.
        /// </summary>
        /// <param name="parameter">Object to convert to a string.</param>
        /// <param name="parameterType">Parameter object type.</param>
        /// <returns>String value.</returns>
        public override string ConvertValueToString(object parameter, Type parameterType)
        {
            if (parameterType.IsArray || typeof(System.Collections.IList).IsAssignableFrom(parameterType))
            {
                if (parameter == null)
                    return null;

                Type element_type = parameterType.GetElementType();

                // This is an array of string.
                if (element_type == typeof(string))
                {
                    return ((string[])parameter).Aggregate((a, b) => a + DEFAULT_KEYVALUE_DELIMITER + b);
                }
                // This is an array of KeyValuePair objects.
                else if (element_type == typeof(KeyValuePair<string, string>))
                {
                    var result = new StringBuilder();
                    foreach (var tag in (KeyValuePair<string, string>[])parameter)
                    {
                        if (result.Length > 0)
                            result.Append(DEFAULT_KEYVALUE_DELIMITER);

                        result.Append(tag.Key + this.KeyValueDelimiter + tag.Value);
                    }

                    return result.ToString();
                }
                // This is a List with collection objects of type KeyValuePair<string, string>.
                else if (parameterType.GetProperty("Item", typeof(KeyValuePair<string, string>)) != null)
                {
                    var result = new StringBuilder();
                    foreach (var tag in (List<KeyValuePair<string, string>>)parameter)
                    {
                        if (result.Length > 0)
                            result.Append(DEFAULT_KEYVALUE_DELIMITER);

                        result.Append(tag.Key + this.KeyValueDelimiter + tag.Value);
                    }

                    return result.ToString();
                }
            }
            // Provides a way to return nullable dates.
            else if (parameterType == typeof(DateTime?))
            {
                return ((DateTime?)parameter).ToString();
            }

            return base.ConvertValueToString(parameter, parameterType);
        }
        #endregion

        #region Events
        #endregion
    }
}

