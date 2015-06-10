using Fosol.Core.ServiceModel.Configuration.HttpHeader;
using System;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text.RegularExpressions;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// Provides a way to automatically include Http Headers in WCF responses based on a configuration file.
    /// By default the configuration section name is "httpHeaders".  You can override this.
    /// </summary>
    public sealed class HttpHeaderBehavior
        : IServiceBehavior, IEndpointBehavior, IDispatchMessageInspector
    {
        #region Variables
        private const string ConfigSectionName = "httpHeaders";
        private const string ServiceName = "Service";
        private const string EndpointName = "Endpoint";
        private const string RequestUriRegex = @"/(?<Service>\w+(\.svc)?)/(?<Endpoint>(\w+/?)+)";
        private static HttpHeaderSection _HttpHeaderConfiguration;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a HttpHeaderBehavior object.
        /// </summary>
        public HttpHeaderBehavior()
            : this(ConfigSectionName)
        {
        }

        /// <summary>
        /// Creates a new instance of a HttpHeaderBehavior object.
        /// </summary>
        /// <param name="configSectionName">The configuration section name.</param>
        public HttpHeaderBehavior(string configSectionName)
        {
            // Only initialize once.
            if (_HttpHeaderConfiguration == null)
                _HttpHeaderConfiguration = (HttpHeaderSection)ConfigurationManager.GetSection(ConfigSectionName);
        }
        #endregion

        #region IServiceBehavior Methods
        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Apply a MessageInspector to every endpoint so that they all use this HttpHeaderBehavior.
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
                foreach (EndpointDispatcher ed in cd.Endpoints)
                    ed.DispatchRuntime.MessageInspectors.Add(this);
        }

        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }
        #endregion

        #region IEndpointBehavior Methods
        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Add the HttpHeader MessageInspector to the endpoint.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
        }

        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion

        #region IDispatchMessageInspector
        /// <summary>
        /// Extract which service and endpoint this particular request is for.
        /// This information will be used to add Http Headers to the response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns>CorrelationState object containing request information.</returns>
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            var uri = request.Headers.To;
            Match match = Regex.Match(uri.AbsolutePath, RequestUriRegex, RegexOptions.IgnoreCase);
            var state = new CorrelationState();
            var endpoint = request.Properties["HttpOperationName"] as string;

            // Request contains Service and Endpoint information.  This information must be passed to the BeforeSendReply event.
            if (match.Success)
            {
                state[ServiceName] = match.Groups[ServiceName].Value;
                state[EndpointName] = endpoint;
            }

            return state;
        }

        /// <summary>
        /// Apply Http Headers to the response if the configuration has headers for the specified request.
        /// </summary>
        /// <param name="reply">System.ServiceModel.Chennels.Message object.</param>
        /// <param name="correlationState">CorrelationState object created in the AfterReceiveRequest method.</param>
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            // No configuration file present.
            if (_HttpHeaderConfiguration == null)
                return;

            HttpResponseMessageProperty httpResponseMessage;
            object httpResponseMessageObject;
            var state = correlationState as CorrelationState;

            // If the reply message already contains the HttpResponse property, add the following headers.  Otherwise add the HttpResponse to the Message Properties.
            if (reply.Properties.TryGetValue(HttpResponseMessageProperty.Name, out httpResponseMessageObject))
            {
                httpResponseMessage = httpResponseMessageObject as HttpResponseMessageProperty;

                // Only add the header if it does not exist, or is null or empty.
                if (state.ContainsKey(ServiceName) && state.ContainsKey(EndpointName))
                    foreach (HeaderElement header in _HttpHeaderConfiguration.GetHeaders(state[ServiceName] as string, state[EndpointName] as string))
                        if (string.IsNullOrEmpty(httpResponseMessage.Headers[header.Name]))
                            httpResponseMessage.Headers[header.Name] = header.Value;
            }
            else
            {
                httpResponseMessage = new HttpResponseMessageProperty();

                // Add all the headers to the message.
                if (state.ContainsKey(ServiceName) && state.ContainsKey(EndpointName))
                    foreach (HeaderElement header in _HttpHeaderConfiguration.GetHeaders(state[ServiceName] as string, state[EndpointName] as string))
                        httpResponseMessage.Headers.Add(header.Name, header.Value);

                reply.Properties.Add(HttpResponseMessageProperty.Name, httpResponseMessage);
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
