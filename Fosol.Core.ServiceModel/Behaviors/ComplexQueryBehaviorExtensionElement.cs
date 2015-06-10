using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// Provides a way to configure an endpoint behavior to handle complex query string parameters (i.e. arrays and collections of objects).
    /// </summary>
    /// <example>
    ///     <behaviors>
    ///         <endpointBehaviors>
    ///             <behavior name="ServiceEndpointBehavior">
    ///                 <complexQueryBehavior keyValueDelimiter="=" helpEnabled="true" />
    ///              </behavior>
    ///         </endpointBehaviors>
    ///     </behaviors>
    ///     <extensions>
    ///         <behaviorExtensions>
    ///             <add name="complexQueryBehavior" type="Postmedia.AppServices.Behaviors.ComplexQueryBehaviorExtension, Postmedia.AppServices"/>
    ///         </behaviorExtensions>
    ///     </extensions>
    /// </example>
    public sealed class ComplexQueryBehaviorExtensionElement 
        : BehaviorExtensionElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - Type of ComplexQueryBehavior.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(ComplexQueryBehavior); }
        }

        /// <summary>
        /// get/set - The delimiter used within complex querystring values ([tag]=[key][KeyValueDelimiter][value]).
        /// This property is only used when the QueryString results contains KeyValuePairs.
        /// </summary>
        /// <example>tag=key=value,key=value</example>
        [ConfigurationProperty("keyValueDelimiter", DefaultValue = ",", Options = ConfigurationPropertyOptions.None)]
        public string KeyValueDelimiter { get; set; }

        /// <summary>
        /// get/set - Whether or not to provide the Help endpoint for this server.
        /// </summary>
        [ConfigurationProperty("helpEnabled", DefaultValue = false, Options = ConfigurationPropertyOptions.None)]
        public bool HelpEnabled { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Creates a ComplexQueryBehavior object.
        /// Initializes the behavior with attribute values within the configuration file.
        /// </summary>
        /// <returns>ComplexQueryBehavior object.</returns>
        protected override object CreateBehavior()
        {
            var behavior = new ComplexQueryBehavior();
            behavior.HelpEnabled = (bool)this.ElementInformation.Properties["helpEnabled"].Value;
            behavior.KeyValueDelimiter = (string)this.ElementInformation.Properties["keyValueDelimiter"].Value;
            return behavior;
        }
        #endregion

        #region Events
        #endregion
    }
}
