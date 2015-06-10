using System;
using System.ServiceModel.Description;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// Provides a way to override the default limitation of parameter types within the WebHttpBehavior.
    /// This new behavior will now accept arrays, collections and Nullable fields (i.e. DateTime?).
    /// Update the Converters.ComplexQueryConvert if you want to add more available types.
    /// You will not be able to use this behavior with the standard WebHttpBehavior if your endpoint contains non-standard parameters.
    /// <example>
    ///     <behaviors>
    ///         <endpointBehaviors>
    ///             <behavior name="ServiceEndpointBehavior">
    ///                 <complexQueryBehavior keyValueDelimiter="=" helpEnabled="true" />
    ///                 <httpHeaderBehavior />
    ///                 <whiteListBehavior />
    ///             </behavior>
    ///         </endpointBehaviors>
    ///     </behaviors>
    /// </example>
    /// </summary>
    public sealed class ComplexQueryBehavior 
        : WebHttpBehavior
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The delimiter used within complex querystring values ([tag]=[key][KeyValueDelimiter][value]).
        /// This property is only used when the QueryString results contains KeyValuePairs.
        /// </summary>
        /// <example>tag=key=value,key=value</example>
        public string KeyValueDelimiter { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Provides a way to handle complex QueryString parameters.
        /// </summary>
        /// <param name="operationDescription">OperationDescription object.</param>
        /// <returns>ComlexQueryConverter object.</returns>
        protected override System.ServiceModel.Dispatcher.QueryStringConverter GetQueryStringConverter(OperationDescription operationDescription)
        {
            return new Converters.ComplexQueryConverter(this.KeyValueDelimiter);
        }
        #endregion

        #region Events
        #endregion
    }
}

