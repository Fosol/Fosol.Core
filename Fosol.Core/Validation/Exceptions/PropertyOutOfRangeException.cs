using System;
using System.Runtime.Serialization;

namespace Fosol.Core.Validation.Exceptions
{
    /// <summary>
    /// PropertyOutOfRangeException class provides a way to identify an exception that occurs due to invalid property values.
    /// </summary>
    public class PropertyOutOfRangeException : ArgumentOutOfRangeException
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyOutOfRangeException.
        /// </summary>
        public PropertyOutOfRangeException()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance of a PropertyOutOfRangeException.
        /// </summary>
        /// <param name="propertyName">Name of the property where the exception was thrown.</param>
        public PropertyOutOfRangeException(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyOutOfRangeException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</para
        /// <param name="propertyName">Name of the property where the exception was thrown.</param>m>
        public PropertyOutOfRangeException(string propertyName, string message)
            : base(propertyName, message)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyOutOfRangeException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public PropertyOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyOutOfRangeException.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The object that describes the source or destination of the serialized data.</param>
        public PropertyOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
