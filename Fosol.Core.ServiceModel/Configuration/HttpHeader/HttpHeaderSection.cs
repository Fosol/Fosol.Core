using System;
using System.Configuration;
using System.Linq;

namespace Fosol.Core.ServiceModel.Configuration.HttpHeader
{
    /// <summary>
    /// Configuration section to identify, control and add HTTP headers to service endpoints.
    /// Headers defined within this section will be overridden by those in the Services and Endpoints sections.
    /// </summary>
    public sealed class HttpHeaderSection : ConfigurationSection
    {
        #region Properties
        /// <summary>
        /// get/set - Collection of headers to add to all service endpoints.
        /// </summary>
        [ConfigurationProperty("headers", IsRequired = false, IsKey = false)]
        public HeaderCollection Headers
        {
            get { return (HeaderCollection)this["headers"]; }
            set { this["headers"] = value; }
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
        /// Returns the HeaderCollection for the specified service and endpoint.
        /// This collection contains an aggregate of all headers configured and correctly overrides those configured within each section.
        /// </summary>
        /// <param name="service">The name of the service (i.e. Mobile.svc).</param>
        /// <param name="endpoint">The name of the endpoint (i.e. Binary).</param>
        /// <returns>HeaderCollection object with all headers that need to be applied.</returns>
        public HeaderCollection GetHeaders(string service, string endpoint)
        {
            HeaderCollection headers = new HeaderCollection();

            ServiceElement services = Services.FirstOrDefault(s => s.Name.ToLower() == service.ToLower());
            EndpointElement endpoints = services != null ? services.Endpoints.FirstOrDefault(e => e.Name.ToLower() == endpoint.ToLower()) : null;

            // Add all the headers for the specified endpoint.
            if (endpoints != null)
                foreach (HeaderElement header in endpoints.Headers)
                    headers.Add(header);

            // Add all the headers for the specified service that have not yet been added.
            if (services != null)
                foreach (HeaderElement header in services.Headers.Where(h => !headers.Contains(h, new HeaderElement.HeaderComparer())))
                    headers.Add(header);

            // Add all the headers that have not already been added.
            foreach (HeaderElement header in this.Headers.Where(h => !headers.Contains(h, new HeaderElement.HeaderComparer())))
                headers.Add(header);

            return headers;
        }
        #endregion
    }
}
