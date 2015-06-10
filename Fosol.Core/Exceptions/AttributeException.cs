using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Exceptions
{
    /// <summary>
    /// AttributeException class, provides a way to capture Attribute related exceptions.
    /// </summary>
    public class AttributeException
        : Exception
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The Attribute Type required.
        /// </summary>
        public Type AttributeType { get; private set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new AttributeException object.
        /// </summary>
        public AttributeException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new AttributeException object.
        /// </summary>
        /// <param name="attributeType">Attribute Type required.</param>
        public AttributeException(Type attributeType)
            : base(attributeType.GetType().Name)
        {
            this.AttributeType = attributeType;
        }

        /// <summary>
        /// Creates a new AttributeException object.
        /// </summary>
        /// <param name="message">Description of error.</param>
        public AttributeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new AttributeException object.
        /// </summary>
        /// <param name="attributeType">Attribute Type required.</param>
        /// <param name="innerException">Exception that caused this exception.</param>
        public AttributeException(Type attributeType, Exception innerException)
            : base(attributeType.GetType().Name, innerException)
        {
            this.AttributeType = attributeType;
        }

        /// <summary>
        /// Creates a new AttributeException object.
        /// </summary>
        /// <param name="message">Description of error.</param>
        /// <param name="innerException">Exception that caused this exception.</param>
        public AttributeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new AttributeException object.
        /// </summary>
        /// <param name="info">The object that holds all the exception details.</param>
        /// <param name="context">Contains contextual information about the source and destination.</param>
        public AttributeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
