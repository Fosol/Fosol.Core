
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// Provides a way to ensure the response format is controlled by the request Accept Header or the Query string parameter.
    /// </summary>
    public sealed class ResponseFormatBehavior
        : IServiceBehavior, IEndpointBehavior, IDispatchMessageInspector
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The default WebMessageFormat to use if one isn't requested.
        /// </summary>
        public WebMessageFormat DefaultFormat { get; set; }

        /// <summary>
        /// get/set - The query string parameter name that specifies the requested format.
        /// </summary>
        public string QueryParamName { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ResponseFormatBehavior object.
        /// </summary>
        public ResponseFormatBehavior()
            : this(WebMessageFormat.Xml, "format")
        {
        }

        /// <summary>
        /// Creates a new instance of a ResponseFormatBehavior object.
        /// </summary>
        /// <param name="defaultFormat">The default WebMessageFormat to use if the request doesn't specify one.</param>
        /// <param name="queryParamName">The query string parameter name that may contain the requested format.</param>
        public ResponseFormatBehavior(WebMessageFormat defaultFormat, string queryParamName)
        {
            this.DefaultFormat = defaultFormat;
            this.QueryParamName = queryParamName;
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
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Apply a MessageInspector to every endpoint so that they all use this ResponseFormatBehavior
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
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Apply a MessageInspector to the endpoint to apply the desired response format.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
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

        #region IDispatchMessageInspector Methods
        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            Fosol.Core.ServiceModel.Helpers.WebOperationContextHelper.AutoResponseFormat(DefaultFormat, QueryParamName);
            return null;
        }

        /// <summary>
        /// Set the response format based on the Accept Header, or the Query string parameter, or the default format.
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion

    }
}