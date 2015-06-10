using Fosol.Core.Text.Parsers;
using System;
using System.ComponentModel;

namespace Fosol.Core.Text.Converters
{
    /// <summary>
    /// ElementCollectionConverter class, provides a way to convert a special formatted string into a ElementCollection object (or vise-versa).
    /// </example>
    public class ElementCollectionConverter
        : TypeConverter
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Can convert the following types;
        /// - string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Can convert to the following types;
        /// - string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Convert from a specially formatted string value to a Format.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                return new ElementParser().Parse((string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Convert to a specially formatted string value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value.GetType() == typeof(ElementParser))
                {
                    var val = value as ElementParser;
                    return val.ToString();
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
