using System;
using System.Runtime.Serialization;

namespace Fosol.Core.Validation.Exceptions
{
    /// <summary>
    /// PropertyNullException class provides a way to identify an exception that occurs due to a null property values.
    /// </summary>
    public class PropertyNullException : ArgumentNullException
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyNullException.
        /// </summary>
        public PropertyNullException()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance of a PropertyNullException.
        /// </summary>
        /// <param name="propertyName">Name of the property where the exception was thrown.</param>
        public PropertyNullException(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyNullException.
        /// </summary>
        /// <param name="propertyName">Name of the property where the exception was thrown.</param>
        /// <param name="message">Error message to descripe exception.</param>
        public PropertyNullException(string propertyName, string message)
            : base(propertyName, message)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyNullException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public PropertyNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyNullException.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The object that describes the source or destination of the serialized data.</param>
        public PropertyNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
