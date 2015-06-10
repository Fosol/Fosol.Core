using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.WhiteList
{
    /// <summary>
    /// Configuration to ensure this endpoint is only accessible by these IP addresses.
    /// </summary>
    public sealed class EndpointElement 
        : ConfigurationElement
    {
        #region Properties
        /// <summary>
        /// get/set - The name of the endpoint.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - IP Address collection
        /// </summary>
        [ConfigurationProperty("ipAddresses", IsRequired = false, IsKey = false)]
        public IpAddressCollection IpAddresses
        {
            get { return (IpAddressCollection)this["ipAddresses"]; }
            set { this["ipAddresses"] = value; }
        }
        #endregion
    }
}
