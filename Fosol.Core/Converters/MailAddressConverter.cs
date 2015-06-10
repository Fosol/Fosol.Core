using System;
using System.ComponentModel;
using System.Net.Mail;

namespace Fosol.Core.Converters
{
    /// <summary>
    /// MailAddressConverter sealed class, provides a way to convert strings into a  MailAddress objects.
    /// </summary>
    public sealed class MailAddressConverter
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
        /// Convert the value to an MailAddress object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a valid MailAddress.</exception>
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

                return new MailAddress(val);
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts an MailAddress object to the specified destination Type.
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
                var val = value as MailAddress;
                if (val == null)
                    return null;
                return val.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion

        #region Events
        #endregion
    }
}
