using System;
using System.Runtime.Serialization;

namespace Fosol.Core.Validation.Exceptions
{
    /// <summary>
    /// ValueNullException class provides a way to identify an exception that occurs due to a null Value values.
    /// </summary>
    public class ValueNullException : ArgumentNullException
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a ValueNullException.
        /// </summary>
        public ValueNullException()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance of a ValueNullException.
        /// </summary>
        /// <param name="ValueName">Name of the Value where the exception was thrown.</param>
        public ValueNullException(string ValueName)
            : base(ValueName)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueNullException.
        /// </summary>
        /// <param name="ValueName">Name of the Value where the exception was thrown.</param>
        /// <param name="message">Error message to descripe exception.</param>
        public ValueNullException(string ValueName, string message)
            : base(ValueName, message)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueNullException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public ValueNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueNullException.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The object that describes the source or destination of the serialized data.</param>
        public ValueNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
