using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Fosol.Core.ServiceModel.Behaviors
{
    public sealed class HttpHeaderBehaviorExtensionElement
        : BehaviorExtensionElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The behavior Type.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(HttpHeaderBehavior); }
        }

        /// <summary>
        /// get/set - The configuration section name for the behavior.
        /// </summary>
        [ConfigurationProperty("sectionName", DefaultValue = "httpHeaders", Options = ConfigurationPropertyOptions.None)]
        public string SectionName { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new ResponseFormatBehavior object.
        /// </summary>
        /// <returns>New ResponseFormatBehavior object.</returns>
        protected override object CreateBehavior()
        {
            var section_name = (string)this.ElementInformation.Properties["sectionName"].Value;
            if (!string.IsNullOrEmpty(section_name))
                return new HttpHeaderBehavior(section_name);
            else
                return new HttpHeaderBehavior();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
