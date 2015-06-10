using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Fosol.Core.ServiceModel.Extensions.OperationContexts
{
    /// <summary>
    /// Helpful extensions for OperationContext.
    /// </summary>
    public static class OperationContextExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the OperationDescription for the specified OperationContext object.
        /// </summary>
        /// <param name="context">OperationContext object.</param>
        /// <returns>OperationDescription object.</returns>
        public static OperationDescription OperationDescription(this OperationContext context)
        {
            ServiceEndpoint endpoint = context.Host.Description.Endpoints.Find(context.EndpointDispatcher.EndpointAddress.Uri);
            DispatchOperation dispatchOperation = context.EndpointDispatcher.DispatchRuntime.Operations.Where(op => op.Action == context.IncomingMessageHeaders.Action).First();
            return endpoint.Contract.Operations.Find(dispatchOperation.Name);
        }

        /// <summary>
        /// get - Provides the OperationContext.Current.RequestContext.RequestMessage.Properties["Via"] value
        /// </summary>
        public static Uri RequestUri(this OperationContext context)
        {
            Uri uri = context.RequestContext.RequestMessage.Properties.Via;

            // For some reason in Staging and Production IIS provides the machine domain name (i.e. wpgccweb30.canada.com) instead of the requested Host name
            uri = new Uri(new Uri(string.Format("{0}://{1}", uri.Scheme, context.RequestAuthority())), uri.PathAndQuery);
            return uri;
        }

        /// <summary>
        /// get - Provides the OperationContext.Current.RequestContext.RequestMessage.Properties.Via.Host value
        /// </summary>
        public static string RequestHost(this OperationContext context)
        {
            return context.RequestContext.RequestMessage.Properties.Via.Host;
        }

        /// <summary>
        /// get - Provides the OperationContext.Current.RequestContext.RequestMessage.Properties["httpRequest"].Headers["Host"] value
        /// </summary>
        public static string RequestAuthority(this OperationContext context)
        {
            return ((HttpRequestMessageProperty)context.RequestContext.RequestMessage.Properties["httpRequest"]).Headers["Host"];
        }

        /// <summary>
        /// get - Provides the OperationContext.Current.RequestContext.RequestMessage.Propertiess["httpRequest"].Headers["User-Agent"] value
        /// </summary>
        public static string RequestUserAgent(this OperationContext context)
        {
            return ((HttpRequestMessageProperty)context.RequestContext.RequestMessage.Properties["httpRequest"]).Headers["User-Agent"];
        }

        /// <summary>
        /// Returns client ip
        /// Look up is in following sequence
        /// 1. X-Postmedia-True-Client-IP
        /// 2. True-Client-IP
        /// 3. X-Forwarded-For
        /// 4. Request Address (Dns resoloved)
        /// </summary>
        /// <param name="context">OperationContext object.</param>
        /// <returns>list of ip addresses for the client</returns>
        public static List<string> GetClientIP(this OperationContext context)
        {
            // list to hold request ips
            var request_ips = new List<string>();


            // get http request header object
            var requestMessage = context.RequestContext.RequestMessage;
            HttpRequestMessageProperty mp = (HttpRequestMessageProperty)requestMessage.Properties["httpRequest"];
            var oh = mp.Headers;

            // X-Postmedia-True-Client-IP is a custom header added by our proxy scripts
            // for requests sent to our app services where the app services need to work with the actual
            // client IP as opposed to the IP address provided by Akamai in the True-Client-IP 
            // (which would be the IP address of our server that initiated the programmatic 
            // requests that we send back to our own sites/services). 
            if (!string.IsNullOrEmpty(mp.Headers["X-Postmedia-True-Client-IP"]))
                request_ips.Add(mp.Headers["X-Postmedia-True-Client-IP"]);

            // True-Client-IP is provided by Akamai
            else if (!string.IsNullOrEmpty(mp.Headers["True-Client-IP"]))
                request_ips.Add(mp.Headers["True-Client-IP"]);

            // the following checks (HTTP_CLIENT_IP, X-Forwarded-For, request_address)
            // would only be done for sites not sitting behind Akamai

            /// TODO: AH, confirm with Mark if HTTP_CLIENT_IP is still required in wcf
            //else if (!string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_CLIENT_IP"]))
            //    ip = context.Request.ServerVariables["HTTP_CLIENT_IP"];

            else if (!string.IsNullOrEmpty(mp.Headers["X-Forwarded-For"]) && mp.Headers["X-Forwarded-For"] != "unknown")
                request_ips.Add(mp.Headers["X-Forwarded-For"]);

            else
            {
                // RemoteEndpointMessageProperty new in 3.5 allows us 
                // to get the remote endpoint address.

                var remote_endpoint = requestMessage.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                // Parse the request IP so that it can be compared to the Whitelist.
                IPAddress request_address = IPAddress.Parse(remote_endpoint.Address);
                foreach (var ip in Dns.GetHostAddresses(request_address.ToString()))
                {
                    // Only use the IP address if it's IPv4 or IPv6.
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                        || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        request_ips.Add(ip.ToString());
                }
            }

            return request_ips;
        }

        /// <summary>
        /// Returns client ip with source
        /// Look up is in following sequence
        /// 1. X-Postmedia-True-Client-IP
        /// 2. True-Client-IP
        /// 3. X-Forwarded-For
        /// 4. Request Address (Dns resoloved)
        /// </summary>
        /// <param name="context">OperationContext object.</param>
        /// <returns>dictionary list of ip addresses for the client with source. Key = source, Value = ip</returns>
        public static Dictionary<string, string> GetClientIPWithSource(this OperationContext context)
        {
            // list to hold request ips
            var request_ips = new Dictionary<string, string>();


            // get http request header object
            var requestMessage = context.RequestContext.RequestMessage;
            HttpRequestMessageProperty mp = (HttpRequestMessageProperty)requestMessage.Properties["httpRequest"];
            var oh = mp.Headers;

            // X-Postmedia-True-Client-IP is a custom header added by our proxy scripts
            // for requests sent to our app services where the app services need to work with the actual
            // client IP as opposed to the IP address provided by Akamai in the True-Client-IP 
            // (which would be the IP address of our server that initiated the programmatic 
            // requests that we send back to our own sites/services). 
            if (!string.IsNullOrEmpty(mp.Headers["X-Postmedia-True-Client-IP"]))
                request_ips.Add("X-Postmedia-True-Client-IP", mp.Headers["X-Postmedia-True-Client-IP"]);

            // True-Client-IP is provided by Akamai
            else if (!string.IsNullOrEmpty(mp.Headers["True-Client-IP"]))
                request_ips.Add("True-Client-IP", mp.Headers["True-Client-IP"]);

            // the following checks (HTTP_CLIENT_IP, X-Forwarded-For, request_address)
            // would only be done for sites not sitting behind Akamai

            /// TODO: AH, confirm with Mark if HTTP_CLIENT_IP is still required in wcf
            //else if (!string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_CLIENT_IP"]))
            //    ip = context.Request.ServerVariables["HTTP_CLIENT_IP"];

            else if (!string.IsNullOrEmpty(mp.Headers["X-Forwarded-For"]) && mp.Headers["X-Forwarded-For"] != "unknown")
                request_ips.Add("X-Forwarded-For", mp.Headers["X-Forwarded-For"]);

            else
            {
                // RemoteEndpointMessageProperty new in 3.5 allows us 
                // to get the remote endpoint address.

                var remote_endpoint = requestMessage.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                // Parse the request IP so that it can be compared to the Whitelist.
                IPAddress request_address = IPAddress.Parse(remote_endpoint.Address);
                foreach (var ip in Dns.GetHostAddresses(request_address.ToString()))
                {
                    // Only use the IP address if it's IPv4 or IPv6.
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                        || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        request_ips.Add("Remote Address", ip.ToString());
                }
            }

            return request_ips;
        }
        #endregion
    }
}
