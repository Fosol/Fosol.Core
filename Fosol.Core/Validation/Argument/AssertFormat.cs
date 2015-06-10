using System;

namespace Fosol.Core.Validation.Argument
{
    /// <summary>
    /// AssertFormat static class is a collection of string validation functions.
    /// </summary>
    public static class AssertFormat
    {
        #region Methods
        /// <summary>
        /// Assert the specified value is a valid email.
        /// Maximum size of an email is 254 characters.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid email.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">The value to test.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        /// <param name="innerException">Original exception that raised this exception.</param>
        public static void IsEmail(string value, string paramName, string message = null, Exception innerException = null)
        {
            Assert.IsNotNull(value, paramName, message, innerException);
            if (!Fosol.Core.Validation.AssertFormat.IsEmail(value))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_AssertFormat_IsEmail, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the specified value is a number.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid number.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">The value to test.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        /// <param name="innerException">Original exception that raised this exception.</param>
        public static void IsNumber(string value, string paramName, string message = null, Exception innerException = null)
        {
            Assert.IsNotNull(value, paramName, message, innerException);
            if (!Fosol.Core.Validation.AssertFormat.IsNumber(value))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_AssertFormat_IsNumber, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the specified valud is a valid URI.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid Uri.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">The value to test.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        /// <param name="innerException">Original exception that raised this exception.</param>
        public static void IsUri(string value, string paramName, UriKind uriKind = UriKind.RelativeOrAbsolute, string message = null, Exception innerException = null)
        {
            Assert.IsNotNull(value, paramName, message, innerException);
            if (!Fosol.Core.Validation.AssertFormat.IsUri(value, uriKind))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_AssertFormat_IsUri, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the specified value is a valid postal code.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid postal code.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">The value to test.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        /// <param name="innerException">Original exception that raised this exception.</param>
        public static void IsPostalCode(string value, string paramName, string message = null, Exception innerException = null)
        {
            Assert.IsNotNull(value, paramName, message, innerException);
            if (!Fosol.Core.Validation.AssertFormat.IsPostalCode(value))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_AssertFormat_IsPostalCode, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the specified valud is a valid postal code FSA.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid FSA code.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">The value to test.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        /// <param name="innerException">Original exception that raised this exception.</param>
        public static void IsFSA(string value, string paramName, string message = null, Exception innerException = null)
        {
            Assert.IsNotNull(value, paramName, message, innerException);
            if (!Fosol.Core.Validation.AssertFormat.IsFSA(value))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_AssertFormat_IsFSA, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the specified value is a valid postal code LDU.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid LDU code.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">The value to test.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        /// <param name="innerException">Original exception that raised this exception.</param>
        public static void IsLDU(string value, string paramName, string message = null, Exception innerException = null)
        {
            Assert.IsNotNull(value, paramName, message, innerException);
            if (!Fosol.Core.Validation.AssertFormat.IsLDU(value))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_AssertFormat_IsLDU, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert that the specified valud is a valid ZIP code.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid ZIP code.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">The value to test.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        /// <param name="innerException">Original exception that raised this exception.</param>
        public static void IsZIPCode(string value, string paramName, string message = null, Exception innerException = null)
        {
            Assert.IsNotNull(value, paramName, message, innerException);
            if (!Fosol.Core.Validation.AssertFormat.IsZIPCode(value))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_AssertFormat_IsZIPCode, paramName), paramName, innerException);
        }
        #endregion
    }
}
