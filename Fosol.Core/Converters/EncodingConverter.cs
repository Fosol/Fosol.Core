using System;
using System.ComponentModel;
using System.Text;

namespace Fosol.Core.Converters
{
    /// <summary>
    /// EncodingConverter sealed class, provides a way to convert string values to an Encoding object.
    /// </summary>
    public sealed class EncodingConverter
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
        /// Can convert from the following types;
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
        /// Convert the value to an Encoding object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid Encoding.</exception>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                var val = value as string;
                if (val == null)
                    return null;
                switch (val.ToLower())
                {
                    case ("default"):
                        return Encoding.Default;
                    case ("utf7"):
                        return Encoding.UTF7;
                    case ("utf8"):
                        return Encoding.UTF8;
                    case ("utf32"):
                        return Encoding.UTF32;
                    default:
                        return Encoding.GetEncoding(val);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts an Encoding object to the specified destination Type.
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
                var val = value as Encoding;
                if (val == null)
                    return null;
                return val.EncodingName;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion

        #region Events
        #endregion
    }
}
