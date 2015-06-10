using System;

namespace Fosol.Core.ServiceModel.Exceptions
{
    /// <summary>
    /// This provides a standard exception object for Wcf FaultExceptions that occur due to the framework not correctly serializing DataContracts.
    /// </summary>
    /// <typeparam name="TServiceFault">Type of Service Fault being thrown.</typeparam>
    public sealed class ServiceException<TServiceFault> 
        : Exception
        where TServiceFault : class
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - 
        /// </summary>
        public TServiceFault Fault { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ServiceException object.
        /// </summary>
        /// <param name="fault">Initialize Fault property.</param>
        public ServiceException(TServiceFault fault)
            : base(Resources.Multilingual.Exceptions_ServiceException)
        {
            this.Fault = fault;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
