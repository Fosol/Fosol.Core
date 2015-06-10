using System.Configuration;
using System.Linq;

namespace Fosol.Core.ServiceModel.Configuration.WhiteList
{
    /// <summary>
    /// Configuration section to identify, control and add allowable IP addresses.
    /// This configuration provides a way to ensure only white listed IP addresses may access services and endpoints.
    /// </summary>
    public sealed class WhiteListSection 
        : ConfigurationSection
    {
        #region Properties
        /// <summary>
        /// get/set - Collection of headers to add to all service endpoints.
        /// </summary>
        [ConfigurationProperty("ipAddresses", IsRequired = false, IsKey = false)]
        public IpAddressCollection IpAddresses
        {
            get { return (IpAddressCollection)this["ipAddresses"]; }
            set { this["ipAddresses"] = value; }
        }

        /// <summary>
        /// get/set - Collection of services which will contain specific headers for them and their endpionts.
        /// </summary>
        [ConfigurationProperty("services", IsRequired = false, IsKey = false)]
        public ServiceCollection Services
        {
            get { return (ServiceCollection)this["services"]; }
            set { this["services"] = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the IpAddressCollection for the specified service and endpoint.
        /// This collection contains an aggregate of all ip addresses configured and correctly overrides those configured within each section.
        /// </summary>
        /// <param name="service">The name of the service (i.e. Mobile.svc).</param>
        /// <param name="endpoint">The name of the endpoint (i.e. Binary).</param>
        /// <returns>IpAddressCollection object with all ip addresses that need to be applied.</returns>
        public IpAddressCollection GetIpAddresses(string service, string endpoint)
        {
            IpAddressCollection addresses = new IpAddressCollection();

            ServiceElement services = Services.FirstOrDefault(s => s.Name.ToLower() == service.ToLower());
            EndpointElement endpoints = services != null ? services.Endpoints.FirstOrDefault(e => e.Name.ToLower() == endpoint.ToLower()) : null;

            // Add all the ip addresses for the specified endpoint.
            if (endpoints != null)
            {
                foreach (IpAddressElement ip in endpoints.IpAddresses)
                    addresses.Add(ip);
            }
            else
            {
                addresses.Clear();
                return addresses;
            }

            // Add all the ip addresses for the specified service that have not yet been added.
            if (services != null)
            {
                foreach (IpAddressElement ip in services.IpAddresses.Where(a => !addresses.Contains(a, new IpAddressElement.IpAddressComparer())))
                    addresses.Add(ip);
            }
            else
            {
                addresses.Clear();
                return addresses;
            }

            // Add all the ip addresses that have not already been added.
            foreach (IpAddressElement ip in this.IpAddresses.Where(a => !addresses.Contains(a, new IpAddressElement.IpAddressComparer())))
                addresses.Add(ip);

            return addresses;
        }
        #endregion
    }
}
