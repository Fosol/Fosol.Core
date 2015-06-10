using System;
using System.ServiceModel.Configuration;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// This EndpointBehaviour provides a way to restrict what IPs may request specific services and endpoints.
    /// </summary>
    /// <example>
    ///     <behaviors>
    ///         <endpointBehaviors>
    ///             <behavior name="ServiceEndpointBehavior">
    ///                 <whiteListBehavior keyValueDelimiter="=" helpEnabled="true" />
    ///              </behavior>
    ///         </endpointBehaviors>
    ///     </behaviors>
    ///     <extensions>
    ///         <behaviorExtensions>
    ///             <add name="whiteListBehavior" type="Postmedia.AppServices.Behaviors.WhiteListBehaviorExtension, Postmedia.AppServices"/>
    ///         </behaviorExtensions>
    ///     </extensions>
    /// </example>
    public sealed class WhiteListBehaviorExtensionElement 
        : BehaviorExtensionElement
    {
        #region Properties
        /// <summary>
        /// get - The type of behavior.
        /// </summary>
        public override System.Type BehaviorType
        {
            get { return typeof(WhiteListBehavior); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new WhiteListBehavior object.
        /// </summary>
        /// <returns>WhiteListBehavior object.</returns>
        protected override object CreateBehavior()
        {
            return new WhiteListBehavior();
        }
        #endregion
    }
}
