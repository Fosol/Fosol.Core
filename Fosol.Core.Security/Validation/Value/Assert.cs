using Fosol.Core.Validation.Exceptions;
using System;

namespace Fosol.Core.Security.Validation.Value
{
    /// <summary>
    /// Assert static class provides a way to ensure passwords meet the specified requirements.
    /// </summary>
    public static class Assert
    {
        #region Methods
        /// <summary>
        /// Assert the specified password to determine if it adheres to the specified regular expression.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.ValueException">Parameter 'passwordStrengthRegularExpression' cannot be empty or white space.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.ValueNullException">Parameters 'password', and 'passwordStrengthRegularExpression' cannot be null.</exception>
        /// <param name="password">The value to check.</param>
        /// <param name="passwordStrengthRegularExpression">Regular expression to test the strength of the password.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        public static void IsValidPassword(string password, string passwordStrengthRegularExpression, string paramName, string message = null, Exception innerException = null)
        {
            if (!PasswordHelper.IsValid(password, passwordStrengthRegularExpression))
                throw new ValueException(String.Format(message ?? Resources.Multilingual.Validation_Value_Assert_IsValidPassword, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the specified password to determine if it adheres to the specified regular expression.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.ValueException">Parameter 'passwordStrengthRegularExpression' cannot be empty or white space.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.ValueNullException">Parameters 'password', and 'requirements' cannot be null.</exception>
        /// <param name="password">The value to check.</param>
        /// <param name="requirements">PasswordRequirement object.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">A message to describe the exception.</param>
        public static void IsValidPassword(string password, Security.PasswordRequirement requirements, string paramName, string message = null, Exception innerException = null)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNull(requirements, nameof(requirements));

            var util = new PasswordHelper(requirements);
            if (!util.Validate(password))
                throw new ValueException(String.Format(message ?? Resources.Multilingual.Validation_Value_Assert_IsValidPassword, paramName), paramName, innerException);
        }
        #endregion
    }
}
