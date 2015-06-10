using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fosol.Core.Extensions.Enums
{
    /// <summary>
    /// EnumExtensions static class, provides extension methods for Enum objects.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Creates a Dictionary with the specified Enum type and the descriptions for each enum value.
        /// The descriptions must be specified with the DescriptionAttribute keyword within the Enum's declaration.
        /// </summary>
        /// <example>
        /// public enum Transmission
        /// {
        ///     [DescriptionAttribute("None")]
        ///     None = 0,
        ///     
        ///     [DescriptionAttribute("Telephone")]
        ///     Phone = 2,
        ///     
        ///     [DescriptionAttribute("E-Mail Attachement")]
        ///     Mail = 4
        /// }
        /// </example>
        /// <exception cref="System.ArgumentException">Parameter "enumType" must be of type enum.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "enumType" cannot be null.</exception>
        /// <param name="enumType">Type of Enum you want to create a Dictionary for.</param>
        /// <param name="includeDescription">Determines whether description information is included in the dictionary.</param>
        /// <returns>Dictionary with each enum value and description.</returns>
        public static IDictionary<object, string> ToDictionary(this Type enumType, bool includeDescription = true)
        {
            Validation.Argument.Assert.IsNotNull(enumType, nameof(enumType));
            Validation.Argument.Assert.IsValid(enumType.IsEnum, nameof(enumType), "Parameter \"{0}.IsEnum\" must be true.");

            var type_list = new Dictionary<object, string>();

            foreach (var value in Enum.GetValues(enumType))
            {
                if (includeDescription)
                {
                    var field_info = enumType.GetField(value.ToString());
                    var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field_info, typeof(DescriptionAttribute));

                    if (attr != null)
                        type_list.Add(value, attr.Description);
                    else
                        type_list.Add(value, null);
                }
                else
                    type_list.Add(value, null);
            }

            return type_list;
        }

        /// <summary>
        /// Returns a collection of string values that represent the Enum.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "enumType" must be of type enum.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "enumType" cannot be null.</exception>
        /// <param name="enumType">Type of Enum you want to create a collection from.</param>
        /// <returns>Collection of string values.</returns>
        public static IEnumerable<string> GetNames(this Type enumType)
        {
            Validation.Argument.Assert.IsNotNull(enumType, nameof(enumType));
            Validation.Argument.Assert.IsValid(enumType.IsEnum, nameof(enumType), "Parameter \"{0}.IsEnum\" must be true.");

            return (
                from f in enumType.GetFields(BindingFlags.Public | BindingFlags.Static)
                where f.IsLiteral
                select f.Name);
        }

        /// <summary>
        /// Returns a collection of enum values from the specified Enum.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "enumType" must be of type enum.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "enumType" cannot be null.</exception>
        /// <param name="enumType">Type of Enum you want to create a collection from.</param>
        /// <returns>Collection of enum values.</returns>
        public static IEnumerable<object> GetEnums(this Type enumType)
        {
            Validation.Argument.Assert.IsNotNull(enumType, nameof(enumType));
            Validation.Argument.Assert.IsValid(enumType.IsEnum, nameof(enumType), "Parameter \"{0}.IsEnum\" must be true.");

            return (
                from f in enumType.GetFields(BindingFlags.Public | BindingFlags.Static)
                where f.IsLiteral
                select f);
        }

        /// <summary>
        /// Returns the description found within the DescriptionAttribute or the enum.ToString() value.
        /// </summary>
        /// <typeparam name="T">Type of enumeration.</typeparam>
        /// <param name="value">Enum value you want the description for.</param>
        /// <returns>Description or name of the enumeration value.</returns>
        public static string GetDescription<T>(this T value)
            where T : struct
        {
            var type = value.GetType();
            Validation.Argument.Assert.IsValid(type.IsEnum, nameof(value), "Parameter \"{0}\" must be an enum.");

            var mi = type.GetMember(value.ToString());
            if (mi != null && mi.Length > 0)
            {
                var attrs = mi[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return value.ToString();
        }
    }
}
