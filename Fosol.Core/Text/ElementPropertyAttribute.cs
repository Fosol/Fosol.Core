using Fosol.Core.Extensions.Types;
using System;
using System.ComponentModel;

namespace Fosol.Core.Text
{
    /// <summary>
    /// ElementPropertyAttribute sealed class, provides a way to define a property as an Element's attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ElementPropertyAttribute
        : Attribute
    {
        #region Variables
        private readonly string _Name;
        private readonly string[] _Abbr;
        private readonly TypeConverter _Converter;
        #endregion

        #region Properties
        /// <summary>
        /// get - Unique name to identify this attribute in a formatted string.
        /// </summary>
        public string Name { get { return _Name; } }

        /// <summary>
        /// get - Unique abbreviations that can be used instead of the full name.
        /// Be careful when assigning abbreviations to ensure they are not used by another parameter for the same keyword.
        /// </summary>
        public string[] Abbreviations { get { return _Abbr; } }

        /// <summary>
        /// get - Type of TypeConverter to use when setting the property of field.
        /// </summary>
        public TypeConverter Converter
        {
            get { return _Converter; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        public ElementPropertyAttribute(string name)
            : this(name, new string[0])
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="converterType">TypeConverter to use to convert configuration values.</param>
        /// <param name="converterArgs">Arguments to supply to the TypeConverter.</param>
        public ElementPropertyAttribute(string name, Type converterType, params object[] converterArgs)
            : this(name, null, converterType, converterArgs)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="abbrev">Parameter name abbreviations.</param>
        public ElementPropertyAttribute(string name, string[] abbrev)
            : this(name, abbrev, null)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="abbrev">Parameter name abbreviations.</param>
        /// <param name="converter">TypeConverter to use to convert configuration values.</param>
        /// <param name="converterArgs">Arguments to supply to the TypeConverter.</param>
        public ElementPropertyAttribute(string name, string[] abbrev, Type converterType, params object[] converterArgs)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(name, nameof(name));
            _Name = name;
            _Abbr = abbrev;

            if (converterType != null)
            {
                if (converterArgs != null && converterArgs.Length > 0)
                    _Converter = (TypeConverter)Activator.CreateInstance(converterType, converterArgs);
                else if (converterType.HasEmptyConstructor())
                    _Converter = (TypeConverter)Activator.CreateInstance(converterType);
            }
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
