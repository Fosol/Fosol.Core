using System;
using System.ComponentModel;

namespace Fosol.Core.Converters
{
    /// <summary>
    /// AssemblyTypeConverter sealed class, provides a way to convert a string value into the specified Type.
    /// </summary>
    public sealed class AssemblyTypeConverter
        : TypeConverter
    {
        #region Methods
        /// <summary>
        /// Can convert from the following types;
        /// - string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns>True if the sourceType can be converted.</returns>
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
        /// <returns>True if the destinationType can be converted.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Convert from the current value to the specified Type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns>A new instance of the type specified by the value parameter.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string) && !string.IsNullOrEmpty((string)value))
            {
                return Type.GetType((string)value, true, true);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// If the current value is a Type object it will return the full name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns>A new instance of the specified destinationType.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value.GetType() == typeof(Type))
            {
                return ((Type)value).FullName;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion
    }
}
