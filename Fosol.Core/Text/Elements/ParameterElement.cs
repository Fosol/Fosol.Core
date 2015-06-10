using Fosol.Core.Text.Converters;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// ParameterElement sealed class, provides a dynamic way to apply static parameter names while including attributes for further logic.
    /// It is useful for database parameters (i.e. {parameter?name=@Id&SqlDbType=NVarChar}).
    /// You can use the shortcut syntax too (i.e. {@Id?value={message}}} or {@Id={message}}}.
    /// Note that the Value property/attribute can contain keywords, just be sure to escape the end boundary if it is next to the parameter keyword end boundary.
    /// </summary>
    [Element("parameter")]
    public sealed class ParameterElement
        : DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The parameter name.
        /// </summary>
        [ElementProperty("name")]
        [Required(AllowEmptyStrings = false)]
        public string ParameterName { get; set; }

        /// <summary>
        /// get/set - The parameter value.
        /// </summary>
        [ElementProperty("value", typeof(ElementCollectionConverter))]
        [Required(AllowEmptyStrings = false)]
        public ElementCollection Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParameterElement.
        /// </summary>
        /// <param name="attributes">StringDictionary containing attribute values.</param>
        public ParameterElement(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return the parameter name.
        /// If the data is a string value it will be treated as a string format (i.e. {0}={1} will result in the Name=Value).
        /// </summary>
        /// <param name="data">Data to be used in the result.</param>
        /// <returns>String value.</returns>
        public override string Render(object data)
        {
            // Check if a format string was supplied.
            var format = data as string;
            if (format != null && format.Contains("{0}"))
            {
                return string.Format(format, this.ParameterName, this.Value.Render(data));
            }
            return this.ParameterName;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
