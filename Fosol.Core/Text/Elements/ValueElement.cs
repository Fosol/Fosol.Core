using System;
using System.Collections.Specialized;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// ValueElement sealed class, provides a way to dynamically format the passed data and convert it into a string value.
    /// </summary>
    [Element("value")]
    public sealed class ValueElement
        : DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The formatting for the data.
        /// </summary>
        [ElementProperty("format", new string[] { "f" })]
        public string Format { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ValueElement.
        /// </summary>
        /// <param name="attributes">Configuration for this keyword.</param>
        public ValueElement(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return a formatted string value.
        /// </summary>
        /// <param name="data">Data to use when generating the string result.</param>
        /// <returns>Formatted string value.</returns>
        public override string Render(object data)
        {
            if (string.IsNullOrEmpty(this.Format))
                return data.ToString();
            else
                return string.Format(this.Format, data);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
