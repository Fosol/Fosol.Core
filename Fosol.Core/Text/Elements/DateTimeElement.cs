using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// DateTimeElement class, provides a way to dynamically generate a string to represent the current DateTime.
    /// </summary>
    [Element("datetime")]
    public class DateTimeElement
        : DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The format string for the DateTime value.
        /// </summary>
        [DefaultValue("G")]
        [ElementProperty("format", new string[] { "f" })]
        public string Format { get; set; }

        /// <summary>
        /// get/set - Whether to display the ticks value instead of the DateTime.
        /// </summary>
        [ElementProperty("ticks")]
        public bool Ticks { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DateTimeElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public DateTimeElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Render the DateTime value.
        /// </summary>
        /// <param name="data">Object contain information for dynamic keywords.</param>
        /// <returns>DateTime value as a string with the configured format.</returns>
        public override string Render(object data)
        {
            if (this.Ticks)
                return Optimization.FastDateTime.Now.Ticks.ToString(this.Format);
            else
                return Optimization.FastDateTime.Now.ToString(this.Format);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
