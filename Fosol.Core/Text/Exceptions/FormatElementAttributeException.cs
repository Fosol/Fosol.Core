using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Text.Exceptions
{

    /// <summary>
    /// FormatAttributeException sealed class, provides a way to identify an error which occured while parsing the FormatKeyword objects from the formatted string and a required parameter is missing.
    /// </summary>
    public sealed class FormatAttributeException
        : Core.Exceptions.AttributeException
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The keyword that caused the error.
        /// </summary>
        public string Keyword { get; private set; }

        /// <summary>
        /// get - The parameter name that caused the error.
        /// </summary>
        public string Parameter { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a FormatAttributeException.
        /// </summary>
        public FormatAttributeException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatAttributeException.
        /// </summary>
        /// <param name="message">A message to describe the error.</param>
        public FormatAttributeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatAttributeException.
        /// </summary>
        /// <param name="message">A message to describe the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public FormatAttributeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatAttributeException.
        /// </summary>
        /// <param name="keyword">The name of the keyword that cause the error.</param>
        /// <param name="parameter">The name of the paramter that caused the error.</param>
        public FormatAttributeException(string keyword, string parameter)
            : base(String.Format("Format keyword \"{0}\" attribute \"{1}\" is required.", keyword, parameter))
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatAttributeException.
        /// </summary>
        /// <param name="keyword">The name of the keyword that cause the error.</param>
        /// <param name="parameter">The name of the paramter that caused the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public FormatAttributeException(string keyword, string parameter, Exception innerException)
            : base(String.Format("Format keyword \"{0}\" attribute \"{1}\" is required.", keyword, parameter), innerException)
        {
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
