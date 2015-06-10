using Fosol.Core.Extensions.Types;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Fosol.Core.Extensions.FieldInfos
{
    /// <summary>
    /// FieldInfoExtensions static class, provides extension methods for FieldInfo objects.
    /// </summary>
    public static class FieldInfoExtensions
    {
        /// <summary>
        /// Sets the field value of the specifed object.
        /// Handles basic conversion of the value to the object Type.
        /// Handles Nullable types.
        /// </summary>
        /// <param name="info">FieldInfo object.</param>
        /// <param name="obj">The object whose field will be set.</param>
        /// <param name="value">The new field value.</param>
        public static void SetValue2(this FieldInfo info, object obj, object value)
        {
            var target_type = info.FieldType.IsNullableType()
                ? Nullable.GetUnderlyingType(info.FieldType)
                : info.FieldType;

            info.SetValue(obj, Convert.ChangeType(value, target_type));
        }

        /// <summary>
        /// Sets the field value of the specifed object.
        /// Uses the converter if possible to convert the value to the appropriate Type.
        /// Handles basic conversion of the value to the object Type.
        /// Handles Nullable types.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "converter" cannot be null.</exception>
        /// <param name="info">FieldInfo object.</param>
        /// <param name="obj">The object whose field will be set.</param>
        /// <param name="value">The new field value.</param>
        /// <param name="converter">TypeConverter object.</param>
        public static void SetValue2(this FieldInfo info, object obj, object value, TypeConverter converter)
        {
            Validation.Argument.Assert.IsNotNull(converter, nameof(converter));

            // Same type is easy.
            if (info.FieldType == value.GetType())
                info.SetValue(obj, value);
            // Converter can converter, use it.
            else if (converter.CanConvertFrom(value.GetType()))
                info.SetValue(obj, converter.ConvertFrom(value));
            // Use the good ole college try.
            else
                SetValue2(info, obj, value);
        }
    }
}
