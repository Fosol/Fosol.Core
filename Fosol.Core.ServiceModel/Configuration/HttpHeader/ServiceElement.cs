using System;
using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.HttpHeader
{
    /// <summary>
    /// Configuration for the specified service.  
    /// This will ensure this service and all its endpionts will contain the following HTTP headers.
    /// </summary>
    public sealed class ServiceElement : ConfigurationElement
    {
        #region Properties
        /// <summary>
        /// get/set - The name of the service (i.e. Mobile.svc).
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - Collection of HTTP headers to add to the response of all endpoints within this service.
        /// </summary>
        [ConfigurationProperty("headers", IsRequired = false, IsKey = false)]
        public HeaderCollection Headers
        {
            get { return (HeaderCollection)this["headers"]; }
            set { this["headers"] = value; }
        }

        /// <summary>
        /// get/set - Collection of endpoint to configure individually.
        /// </summary>
        [ConfigurationProperty("endpoints", IsRequired = false, IsKey = false)]
        public EndpointCollection Endpoints
        {
            get { return (EndpointCollection)this["endpoints"]; }
            set { this["endpoints"] = value; }
        }
        #endregion
    }
}
