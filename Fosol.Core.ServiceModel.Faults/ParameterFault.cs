using System;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;

namespace Fosol.Core.ServiceModel.Faults
{
    /// <summary>
    /// ParameterFault provides a way to describe an exception that occurred due to an invalid parameter value.
    /// </summary>
    [DataContract(Name = "ParameterFault", Namespace = "http://team.fosol.ca")]
    public class ParameterFault
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Name of the parameter that this fault is related to.
        /// </summary>
        [DataMember(Name = "parameter", IsRequired = true, Order = 1)]
        public string Parameter { get; set; }

        /// <summary>
        /// get/set - Description of the reason this fault occured.
        /// </summary>
        [DataMember(Name = "message", IsRequired = true, Order = 2)]
        public string Message { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParameterFault object.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that this fault is related to.</param>
        /// <param name="message">Description of reason this fault occured.</param>
        public ParameterFault(string parameterName, string message)
        {
            this.Parameter = parameterName;
            this.Message = message;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a new WebFaultException
        /// Sets the HTTP status code to BadRequest.
        /// </summary>
        /// <returns>A new instance of a WebFaultException of type ParameterFault.</returns>
        public virtual WebFaultException<ParameterFault> Raise()
        {
            return this.Raise(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Returns a new WebFaultException for this ParameterFault.
        /// </summary>
        /// <exception cref="WebFaultException">Always throws exception.</exception>
        /// <param name="statusCode">HttpStatusCode value.</param>
        /// <returns>A new instance of a WebFaultException of type ParameterFault.</returns>
        public virtual WebFaultException<ParameterFault> Raise(HttpStatusCode statusCode)
        {
            return new WebFaultException<ParameterFault>(this, statusCode);
        }

        /// <summary>
        /// Returns a new WebFaultException for this ParameterFault.
        /// Sets the HTTP status code to BadRequest.
        /// </summary>
        /// <typeparam name="T">Type of fault contract to include with the WebFaultException.</typeparam>
        /// <returns>A new instance of a WebFaultException of type T.</returns>
        public virtual WebFaultException<T> Raise<T>()
            where T : ParameterFault
        {
            return this.Raise<T>(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Returns a new WebFaultException for this ParameterFault.
        /// </summary>
        /// <typeparam name="T">Type of fault contract to include with the WebFaultException.</typeparam>
        /// <param name="statusCode">HttpStatusCode value.</param>
        /// <returns>A new instance of a WebFaultException of type T.</returns>
        public virtual WebFaultException<T> Raise<T>(HttpStatusCode statusCode)
            where T : ParameterFault
        {
            return new WebFaultException<T>((T)this, statusCode);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
