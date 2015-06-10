using Fosol.Core.ServiceModel.Configuration.WhiteList;
using Fosol.Core.ServiceModel.Helpers;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text.RegularExpressions;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// Message inpsector provides a way to ensure only configured IP addresses are allowed to access services and endpoints.
    /// </summary>
    public sealed class WhiteListMessageInspector 
        : IDispatchMessageInspector
    {
        #region Variables
        private const string SERVICE = "Service";
        private const string ENDPOINT = "Endpoint";
        private const string PARSE_REQUEST_URI = @"/(?<Service>\w+\.svc)/(?<Endpoint>\w+)/?";
        readonly static WhiteListSection m_ipAddressConfig;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize static variables.
        /// </summary>
        static WhiteListMessageInspector()
        {
            m_ipAddressConfig = (WhiteListSection)ConfigurationManager.GetSection("ipWhiteList");
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method is called after a request has been recieved.
        /// Determine the Service and the Endpoint being requested to find out if the request IP has access to it.
        /// If the requestor does not have access, throw a HttpStatusCode.Unauthorized response.
        /// </summary>
        /// <param name="request">Message received.</param>
        /// <param name="channel">IClientChannel object.</param>
        /// <param name="instanceContext">InstanceContext object.</param>
        /// <returns>CorrelationState object with service and endpoint information.</returns>
        public object AfterReceiveRequest(ref Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            var uri = request.Headers.To;
            Match match = Regex.Match(uri.AbsolutePath, PARSE_REQUEST_URI, RegexOptions.IgnoreCase);

            var endpoint = request.Properties["HttpOperationName"] as string;

            // Request contains Service and Endpoint information.
            // If the request IP is in the WhiteList 
            if (match.Success &&
                m_ipAddressConfig != null &&
                (m_ipAddressConfig.IpAddresses.Count > 0 || m_ipAddressConfig.Services.Count > 0))
            {
                var service = match.Groups[SERVICE].Value;

                // Get client ip
                var request_ips = WebOperationContextHelper.GetClientIPWithSource();

                // Get the IP Whitelist for this service and endpoint.
                var whitelist = m_ipAddressConfig.GetIpAddresses(service, endpoint);
                var allow = whitelist.Count == 0 ? true : whitelist.Where(a => request_ips.Values.Contains(a.Value)).Count() == 0 ? false : true;

                // If the IP isn't on the Whitelist, return a 401 response.
                // Log the illegal attempt to connect.
                if (!allow)
                {
                    /// TODO: AH, will there always be at least one ip in request_ips?
                    WebFaultContract.RaiseFault(
                        string.Format("IP address [{0}] from source [{1}] not authorized",
                            request_ips.First().Value.ToString(),
                            request_ips.First().Key.ToString()),
                        HttpStatusCode.Unauthorized);
                }
            }

            return null;
        }



        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <param name="reply">Response Message.</param>
        /// <param name="correlationState">CorrelationState object with information about the Request.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion
    }
}

