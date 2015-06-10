using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;

namespace Fosol.Core.Data.Converters
{
    /// <summary>
    /// SqlDbTypeConverter sealed class, provides a way to convert SqlDbType values into native .NET types.
    /// </summary>
    public sealed class SqlDbTypeConverter
        : TypeConverter
    {
        #region Variables
        private static Dictionary<SqlDbType, Type> type_map;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Determine if the 'sourceType' is valid conversion option.
        /// Valid 'sourceType' values are; SqlDbType, String.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(SqlDbType))
                return true;
            else if (sourceType == typeof(String))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Determine if the 'destinationType' is valid conversion option.
        /// Valid 'destinationType' values are; String, SqlDbType, Type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;
            else if (destinationType == typeof(SqlDbType))
                return true;
            else if (destinationType == typeof(Type))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Convert the 'value' to a native .NET type.
        /// Valid 'value' types are; String, SqlDbType.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(String))
                return SqlDbTypeConverter.GetNativeType((System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), (string)value, true));
            else if (value.GetType() == typeof(SqlDbType))
                return SqlDbTypeConverter.GetNativeType((SqlDbType)value);

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Convert the 'value' to the 'destinationType'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value.GetType() == typeof(SqlDbType) && destinationType == typeof(String))
                return ((SqlDbType)value).ToString("g");
            else if (value.GetType() == typeof(string) && destinationType == typeof(SqlDbType))
                return (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), (string)value, true);
            else if (destinationType == typeof(Type))
            {
                if (value.GetType() == typeof(string))
                    return SqlDbTypeConverter.GetNativeType((System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), (string)value, true));
                else if (value.GetType() == typeof(SqlDbType))
                    return SqlDbTypeConverter.GetNativeType((SqlDbType)value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Get the native .NET type for the specified SqlDbType.
        /// By default the following SqlDbTypes return a String; Char, NChar, VarChar, NVarchar, Text, NText.
        /// If you require those SqlDbTypes to return a Char[] type instead use the overridden method.
        /// </summary>
        /// <param name="type">SqlDbType value.</param>
        /// <returns>The native .NET type for the specified SqlDbType.</returns>
        public static Type GetNativeType(SqlDbType type)
        {
            if (type_map == null)
            {
                type_map = new Dictionary<SqlDbType, Type>();
                type_map.Add(SqlDbType.Binary, typeof(Byte[]));
                type_map.Add(SqlDbType.VarBinary, typeof(Byte[]));
                //type_map.Add(SqlDbType.VarBinary, typeof(Byte[])); // FILESTREAM attribute (varbinary(max))
                type_map.Add(SqlDbType.Image, typeof(Byte[]));
                type_map.Add(SqlDbType.Timestamp, typeof(Byte[]));
                type_map.Add(SqlDbType.Date, typeof(DateTime));
                type_map.Add(SqlDbType.DateTime, typeof(DateTime));
                type_map.Add(SqlDbType.DateTime2, typeof(DateTime));
                type_map.Add(SqlDbType.DateTimeOffset, typeof(DateTimeOffset));
                type_map.Add(SqlDbType.Time, typeof(TimeSpan));
                type_map.Add(SqlDbType.TinyInt, typeof(Byte));
                type_map.Add(SqlDbType.SmallInt, typeof(Int16));
                type_map.Add(SqlDbType.Int, typeof(Int32));
                type_map.Add(SqlDbType.BigInt, typeof(Int64));
                type_map.Add(SqlDbType.SmallMoney, typeof(Decimal));
                type_map.Add(SqlDbType.Decimal, typeof(Decimal));
                type_map.Add(SqlDbType.Money, typeof(Decimal));
                type_map.Add(SqlDbType.Real, typeof(Single));
                type_map.Add(SqlDbType.Float, typeof(Double));
                type_map.Add(SqlDbType.Char, typeof(String)); // Char[]
                type_map.Add(SqlDbType.NChar, typeof(String)); // Char[]
                type_map.Add(SqlDbType.VarChar, typeof(String)); // Char[]
                type_map.Add(SqlDbType.NVarChar, typeof(String)); // Char[]
                type_map.Add(SqlDbType.Text, typeof(String)); // Char[]
                type_map.Add(SqlDbType.NText, typeof(String)); // Char[]
                type_map.Add(SqlDbType.Bit, typeof(Boolean));
                type_map.Add(SqlDbType.UniqueIdentifier, typeof(Guid));
                type_map.Add(SqlDbType.Variant, typeof(Object));
                type_map.Add(SqlDbType.Xml, typeof(SqlXml));
            }

            return type_map[type];
        }

        /// <summary>
        /// Get the native .NET type for the specified SqlDbType.
        /// Return type Char[] if the 'columnMaximumLength' is less than or equal to the 'maxCharLegnth'.
        /// </summary>
        /// <param name="type">SqlDbType of the column.</param>
        /// <param name="columnMaximumLength">The CHARACTER_MAXIMUM_LENGTH value of the column.</param>
        /// <param name="maxCharLength">Maximum character length of the column that should use type Char[].</param>
        /// <returns>The native .NET type for the SqlDbType.</returns>
        public static Type GetNativeType(SqlDbType type, int columnMaximumLength, int maxCharLength)
        {
            if ((type == SqlDbType.Char
                || type == SqlDbType.NChar
                || type == SqlDbType.VarChar
                || type == SqlDbType.NVarChar)
                && columnMaximumLength <= maxCharLength)
                return typeof(Char[]);

            return GetNativeType(type);
        }

        /// <summary>
        /// Get the native .NET type for the specified SqlDbType.
        /// By default the following SqlDbTypes return a String; Char, NChar, VarChar, NVarchar, Text, NText.
        /// If you require those SqlDbTypes to return a Char[] type instead use the overridden method.
        /// </summary>
        /// <param name="type">SqlDbType value.</param>
        /// <returns>The native .NET type for the specified SqlDbType.</returns>
        public static Type GetNativeType(string sqlDbType)
        {
            return GetNativeType((System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), (string)sqlDbType, true));
        }

        /// <summary>
        /// Get the native .NET type for the specified SqlDbType.
        /// Return type Char[] if the 'columnMaximumLength' is less than or equal to the 'maxCharLegnth'.
        /// </summary>
        /// <param name="type">SqlDbType of the column.</param>
        /// <param name="columnMaximumLength">The CHARACTER_MAXIMUM_LENGTH value of the column.</param>
        /// <param name="maxCharLength">Maximum character length of the column that should use type Char[].</param>
        /// <returns>The native .NET type for the SqlDbType.</returns>
        public static Type GetNativeType(string sqlDbType, int columnMaximumLength, int maxCharLength)
        {
            return GetNativeType((System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), (string)sqlDbType, true), columnMaximumLength, maxCharLength);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
