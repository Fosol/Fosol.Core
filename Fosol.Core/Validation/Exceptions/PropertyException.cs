using System;

namespace Fosol.Core.Validation.Exceptions
{
    /// <summary>
    /// PropertyException class provides a way to identify an exception that occurs due to invalid property values.
    /// </summary>
    public class PropertyException : ArgumentException
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyException.
        /// </summary>
        public PropertyException()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance of a PropertyException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        public PropertyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="propertyName">Name of the property where the exception was thrown.</param>
        public PropertyException(string message, string propertyName)
            : base(message, propertyName)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public PropertyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="propertyName">Name of the property where the exception was thrown.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public PropertyException(string message, string propertyName, Exception innerException)
            : base(message, propertyName, innerException)
        {
        }
        #endregion
    }
}
