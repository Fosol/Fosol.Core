using System;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// UnhandledErrorBehavior provides a way to capture unhandled exceptions that occur within a service or a specific endpoint.
    /// Any unhandled exception will be captured and wrapped into a default WebFaultContract.
    /// The HTTP status code will be changed to 500.
    /// If an Action is provided it will send the unhandled exception to the action, otherwise it will only Debug.WriteLine(Exception).
    /// </summary>
    public sealed class UnhandledErrorBehavior
        : IServiceBehavior, IEndpointBehavior
    {
        #region Variables
        Func<Exception, bool> _ErrorHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an UnhandledErrorBehavior object.
        /// </summary>
        internal UnhandledErrorBehavior()
        {

        }

        /// <summary>
        /// Creates a new instance of an UnhandledErrorBehavior object.
        /// </summary>
        /// <param name="handler">Method to perform when an error occurs.</param>
        public UnhandledErrorBehavior(Func<Exception, bool> handler)
        {
            _ErrorHandler = handler;
        }
        #endregion

        #region Methods
        #region IServiceBehavior
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
        /// Apply the UnhandledErrorHandler to each ChannelDispatcher within the serviceHostBase.
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                cd.ErrorHandlers.Add(new Exceptions.UnhandledErrorHandler(_ErrorHandler));
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

        #region IEndpointBehavior
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
        /// Apply the UnhandledErrorHandler to the endpointDispatcher.ChannelDispatcher.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new Exceptions.UnhandledErrorHandler(_ErrorHandler));
        }

        /// <summary>
        /// No behavior added.
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
