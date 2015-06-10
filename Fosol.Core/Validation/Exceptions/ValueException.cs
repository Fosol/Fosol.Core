using System;

namespace Fosol.Core.Validation.Exceptions
{
    /// <summary>
    /// ValueException class provides a way to identify an exception that occurs due to invalid Value values.
    /// </summary>
    public class ValueException : ArgumentException
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a ValueException.
        /// </summary>
        public ValueException()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance of a ValueException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        public ValueException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="ValueName">Name of the Value where the exception was thrown.</param>
        public ValueException(string message, string ValueName)
            : base(message, ValueName)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public ValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="ValueName">Name of the Value where the exception was thrown.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public ValueException(string message, string ValueName, Exception innerException)
            : base(message, ValueName, innerException)
        {
        }
        #endregion
    }
}
