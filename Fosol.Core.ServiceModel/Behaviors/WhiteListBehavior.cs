using System.ServiceModel.Description;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// This EndpointBehaviour provides a way to restrict what IPs may request specific services and endpoints.
    /// </summary>
    public sealed class WhiteListBehavior
        : IServiceBehavior, IEndpointBehavior
    {
        #region Properties
        #endregion

        #region IServiceBehavior Implementation
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
        /// Applys this WhiteListBehavior to all endpoints within this service.
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (var endpoint in serviceDescription.Endpoints)
            {
                endpoint.EndpointBehaviors.Add(this);
            }
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

        #region IEndpointBehavior Implementation
        /// <summary>
        /// Apply dispatch behavior to this endpoint.
        /// Adds the WhiteListMessageInspector to the endpoint so that white list message inspector is added.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            WhiteListMessageInspector inspector = new WhiteListMessageInspector();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

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
        #endregion
    }
}
