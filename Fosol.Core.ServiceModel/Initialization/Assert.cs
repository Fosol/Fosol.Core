using System;

namespace Fosol.Core.ServiceModel.Initialization
{
    public static class Assert
    {
        #region Convert
        /// <summary>
        /// Attempts to convert the value to the desired type and return the result.
        /// If it fails it resubmits the exception with a valid message which will contain the parameter name.
        /// </summary>
        /// <exception cref="WebFaultException of type ParameterFormatFault">Parameter 'value' must have a valid format to covert.</exception>
        /// <exception cref="WebFaultException of type ParameterInvalidCastFault">Parameter 'value' must be convertable to the desired type.</exception>
        /// <exception cref="WebFaultException of type ParameterNullFault">Parameter 'value' cannot be null.</exception>
        /// <exception cref="WebFaultException of type ParameterOverflowFault">Parameter 'value' must be within a valid range for the desired type.</exception>
        /// <typeparam name="T">Type you want to convert the value into.</typeparam>
        /// <param name="value">Value to convert from.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">Description of the error if it fails to convert the value.</param>
        public static T Convert<T>(object value, string paramName, string message = null)
        {
            try
            {
                return (T)System.Convert.ChangeType(value, typeof(T));
            }
            catch (ArgumentNullException)
            {
                throw new Faults.ParameterNullFault(paramName, string.Format(message ?? Resources.Multilingual.Initialization_Convert_ArgumentNullException, paramName)).Raise<Faults.ParameterNullFault>();
            }
            catch (InvalidCastException)
            {
                throw new Faults.ParameterInvalidCastFault(paramName, string.Format(message ?? Resources.Multilingual.Iniitialization_Convert_InvalidCastException, paramName, typeof(T).Name)).Raise<Faults.ParameterInvalidCastFault>();
            }
            catch (OverflowException)
            {
                throw new Faults.ParameterOverflowFault(paramName, string.Format(message ?? Resources.Multilingual.Initialization_Convert_OverflowException, paramName, typeof(T).Name)).Raise<Faults.ParameterOverflowFault>();
            }
            catch (FormatException)
            {
                throw new Faults.ParameterFormatFault(paramName, string.Format(message ?? Resources.Multilingual.Initialization_Convert_FormatException, paramName, typeof(T).Name)).Raise<Faults.ParameterFormatFault>();
            }
        }
        #endregion
    }
}
