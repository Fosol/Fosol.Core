using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// GuidElement sealed class, provides a way to render a guid into the message.
    /// </summary>
    [Element("guid")]
    public sealed class GuidElement
        : DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The format string for the Guid.
        /// </summary>
        [DefaultValue("N")]
        [ElementProperty("format", new string[] { "f" })]
        public string Format { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a GuidElement.
        /// </summary>
        /// <param name="attribute">StringDictionary object.</param>
        public GuidElement(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a Guid id value.
        /// </summary>
        /// <param name="data">Object data to be used by the dynamic keyword.</param>
        /// <returns>Guid id value.</returns>
        public override string Render(object data)
        {
            return Guid.NewGuid().ToString(this.Format);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
