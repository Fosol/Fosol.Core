using System;
using System.Linq;

namespace Fosol.Core.ServiceModel.Validation
{
    public static class Assert
    {
        #region IsNotNull
        /// <summary>
        /// If the value is null it will throw a WebFaultException of type ParameterNullFault.
        /// </summary>
        /// <exception cref="WebFaultException of type ParameterNullFault">Parameter 'value' cannot be null.</exception>
        /// <param name="value">Value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">Error message describing the exception.</param>
        public static void IsNotNull(object value, string paramName, string message = null)
        {
            if (message != null)
                message = string.Format(message, paramName);
            
            if (value == null)
                throw new Faults.ParameterNullFault(paramName, string.Format(message ?? Resources.Multilingual.Validation_Assert_IsNotNull, paramName)).Raise();
        }
        #endregion

        #region IsTrue
        /// <summary>
        /// Assert that the parameter is true.
        /// If not throw WebFaultException of type ParameterFault.
        /// </summary>
        /// <exception cref="WebFaultException of type ParameterFault">Parameter "value" returned false.</exception>
        /// <param name="value">Parameter value.</param>
        /// <param name="paramName">Name of the parameter being tested.</param>
        /// <param name="message">Error message describing the exception</param>
        public static void IsTrue(bool value, string paramName, string message = null)
        {
            if (message != null)
                message = string.Format(message, paramName);

            if (!value)
                throw new Faults.ParameterFault(message ?? String.Format(Resources.Multilingual.Validation_Assert_IsTrue, paramName), paramName).Raise();
        }
        #endregion

        #region IsValue
        /// <summary>
        /// If the value does not exist in the valid values array it will throw WebFaultException of type ParameterOutOfRangeFault.
        /// </summary>
        /// <exception cref="WebFaultException of type ParameterOutOfRangeFault">Parameter "value" is must be a valid value.</exception>
        /// <typeparam name="T">Type of value being tested.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="validValues">An array of valid values to compare against.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">Error message describing the exception.</param>
        public static void IsValue<T>(T value, T[] validValues, string paramName, string message = null)
        {
            if (message != null)
                message = string.Format(message, paramName);

            if (!validValues.Contains(value))
                throw new Faults.ParameterOutOfRangeFault(paramName, string.Format(message ?? Resources.Multilingual.Validation_Assert_IsValue, paramName)).Raise();
        }

        /// <summary>
        /// If the value does not equal the valid value it will throw WebFaultException of type ParameterOutOfRangeFault.
        /// This method is most effective when ensuring a parameter property is appropriate.
        /// </summary>
        /// <exception cref="WebFaultException of type ParameterOutOfRangeFault">Parameter "value" is must be a valid value.</exception>
        /// <typeparam name="T">Type of value being tested.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="validValue">The only valid value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">Error message describing the exception.</param>
        public static void IsValue<T>(T value, T validValue, string paramName, string message = null)
        {
            if (message != null)
                message = string.Format(message, paramName);

            if (!value.Equals(validValue))
                throw new Faults.ParameterOutOfRangeFault(message ?? string.Format(Resources.Multilingual.Validation_Assert_IsValue, paramName), paramName).Raise();
        }
        #endregion
    }
}
