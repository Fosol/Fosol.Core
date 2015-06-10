using System;
using System.Runtime.Serialization;

namespace Fosol.Core.Validation.Exceptions
{
    /// <summary>
    /// ValueOutOfRangeException class provides a way to identify an exception that occurs due to invalid Value values.
    /// </summary>
    public class ValueOutOfRangeException : ArgumentOutOfRangeException
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a ValueOutOfRangeException.
        /// </summary>
        public ValueOutOfRangeException()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance of a ValueOutOfRangeException.
        /// </summary>
        /// <param name="ValueName">Name of the Value where the exception was thrown.</param>
        public ValueOutOfRangeException(string ValueName)
            : base(ValueName)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueOutOfRangeException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</para
        /// <param name="ValueName">Name of the Value where the exception was thrown.</param>m>
        public ValueOutOfRangeException(string ValueName, string message)
            : base(ValueName, message)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueOutOfRangeException.
        /// </summary>
        /// <param name="message">Error message to descripe exception.</param>
        /// <param name="innerException">Exception that originally raise this exception.</param>
        public ValueOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a ValueOutOfRangeException.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The object that describes the source or destination of the serialized data.</param>
        public ValueOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
