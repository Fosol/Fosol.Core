using System;
using System.Configuration;

namespace Fosol.Core.Configuration.Validators
{
    /// <summary>
    /// UriValidator sealed class, provides a way to validate a URI string value.
    /// </summary>
    public sealed class UriValidator
        : ConfigurationValidatorBase
    {
        #region Variables
        public const int UriMaxLength = 65520;
        private UriKind _UriKind;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UriValidator class.
        /// </summary>
        /// <param name="uriKind">The kind of URI that will be valid.</param>
        public UriValidator(UriKind uriKind = UriKind.RelativeOrAbsolute)
        {
            _UriKind = uriKind;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines if the type can be used to validate.
        /// </summary>
        /// <param name="type">Object type being validated.</param>
        /// <returns>True if it can be validated.</returns>
        public override bool CanValidate(Type type)
        {
            return (type == typeof(string));
        }

        /// <summary>
        /// Validate whether the object and value are a valid URI.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'value' must be of type String and cannot be empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'value' cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'value' cannot have a length greater than 65520.</exception>
        /// <exception cref="System.UriFormatException">Parameter 'value' must be a valid URI.</exception>
        /// <param name="value">Value to validate.</param>
        public override void Validate(object value)
        {
            Validation.Argument.Assert.IsType(value, typeof(string), nameof(value));
            var url = value as string;
            Validation.Argument.Assert.IsNotNullOrWhitespace(url, nameof(value));
            Validation.Argument.Assert.IsMaximum(url.Length, UriValidator.UriMaxLength, "value.Length");

            Uri uri;
            if (!Uri.TryCreate(url, _UriKind, out uri))
                throw new UriFormatException(Resources.Multilingual.Validators_UriValidator_InvalidUri);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
