using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Fosol.Core.ServiceModel
{
    /// <summary>
    /// WebFault provides a common DataContract for WebFaultException responses.
    /// </summary>
    [DataContract(Name = "WebFaultContract", Namespace = "http://www.fosol.ca")]
    [Description("Web service fault DataContract.")]
    public sealed class WebFaultContract
    {
        #region Variables
        private string _Message;
        private HttpStatusCode _StatusCode;
        private Exception _InnerException;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A description of the fault that was raise.
        /// </summary>
        [DataMember(Name = "Message", IsRequired = true)]
        [Description("A description of the fault that was raise.")]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        /// <summary>
        /// get/set - The HTTP status code for the response.
        /// </summary>
        [DataMember(Name = "StatusCode", IsRequired = true)]
        [Description("The HTTP status code for the response.")]
        public HttpStatusCode StatusCode
        {
            get { return _StatusCode; }
            set { _StatusCode = value; }
        }

        /// <summary>
        /// get - The inner exception that was the origin of this WebFaultContract.
        /// </summary>
        public Exception InnerException
        {
            get { return _InnerException; }
            private set { _InnerException = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a WebFaultContract object.
        /// </summary>
        /// <param name="message">A description of the fault that was raised.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        public WebFaultContract(string message, HttpStatusCode statusCode)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(message, "message");

            _Message = message;
            _StatusCode = statusCode;
        }

        /// <summary>
        /// Creates a new instance of a WebFaultContract object.
        /// </summary>
        /// <param name="message">A description of the fault that was raised.</param>
        /// <param name="innerException">Origin of the exception that threw this WebFault.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        public WebFaultContract(string message, Exception innerException, HttpStatusCode statusCode)
            : this(message, statusCode)
        {
            _InnerException = innerException;
        }

        /// <summary>
        /// Creates a new instance of a WebFaultContract object.
        /// Sets the StatusCode to HttpStatusCode.InternalServerError.
        /// </summary>
        /// <param name="exception">Exception that occured to raise this WebFault.</param>
        public WebFaultContract(Exception exception)
        {
            Validation.Assert.IsNotNull(exception, "exception");

            _InnerException = exception;
            _Message = exception.Message;
            _StatusCode = HttpStatusCode.InternalServerError;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Throws a WebFaultException for this WebFaultContract object.
        /// </summary>
        /// <exception cref="System.ServiceModel.Web.WebFaultException">Returns this WebFaultContract in the response.</exception>
        public void RaiseFault()
        {
            throw new WebFaultException<WebFaultContract>(this, this.StatusCode);
        }

        /// <summary>
        /// Creates a new instance of a WebFaultContract object and throws a WebFaultException for the WebFaultContract object.
        /// </summary>
        /// <exception cref="System.ServiceModel.Web.WebFaultException">Returns a WebFaultContract in the response.</exception>
        /// <param name="message">Description of the fault that will be raised.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        public static void RaiseFault(string message, HttpStatusCode statusCode)
        {
            throw new WebFaultException<WebFaultContract>(new WebFaultContract(message, statusCode), statusCode);
        }

        /// <summary>
        /// Creates a new instance of a WebFaultContract object and throws a WebFaultException for the WebFaultContract object.
        /// </summary>
        /// <exception cref="System.ServiceModel.Web.WebFaultException">Returns a WebFaultContract in the response.</exception>
        /// <param name="message">Description of the fault that will be raised.</param>
        /// <param name="innerException">Origin of the exception that threw this WebFaultContract.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        public static void RaiseFault(string message, Exception innerException, HttpStatusCode statusCode)
        {
            throw new WebFaultException<WebFaultContract>(new WebFaultContract(message, innerException, statusCode), statusCode);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Casts the WebFaultContract into a FaultException.
        /// </summary>
        /// <param name="fault">WebFaultContract object.</param>
        /// <returns>A new instance of a FaultException.</returns>
        public static explicit operator FaultException<WebFaultContract>(WebFaultContract fault)
        {
            var reason = new FaultReason(fault.Message);
            var code = new FaultCode(fault.StatusCode.ToString("g"));
            return new FaultException<WebFaultContract>(fault, reason, code);
        }

        /// <summary>
        /// Casts the WebFaultContract into a WebFaultException.
        /// </summary>
        /// <param name="fault">WebFaultContract object.</param>
        /// <returns>A new instance of a WebFaultException.</returns>
        public static explicit operator WebFaultException<WebFaultContract>(WebFaultContract fault)
        {
            return new WebFaultException<WebFaultContract>(fault, fault.StatusCode);
        }
        #endregion

        #region Events
        #endregion
    }
}
