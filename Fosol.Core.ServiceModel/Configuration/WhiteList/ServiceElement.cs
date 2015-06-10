using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.WhiteList
{
    /// <summary>
    /// Configuration for the specified service.  
    /// This will ensure this service and all its endpionts are only accessible by these IP addresses.
    /// </summary>
    public sealed class ServiceElement 
        : ConfigurationElement
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
        /// get/set - Collection of IP addresses to add to the response of all endpoints within this service.
        /// </summary>
        [ConfigurationProperty("ipAddresses", IsRequired = false, IsKey = false)]
        public IpAddressCollection IpAddresses
        {
            get { return (IpAddressCollection)this["ipAddresses"]; }
            set { this["ipAddresses"] = value; }
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
