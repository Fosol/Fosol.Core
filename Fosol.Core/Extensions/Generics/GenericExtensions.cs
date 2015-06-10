using System;
using System.Collections;
using System.Linq;

namespace Fosol.Core.Extensions.Generics
{
    /// <summary>
    /// GenericExtensions is a static class containing extension methods for generic objects.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Get the generic element type of the specified enumerable type.
        /// </summary>
        /// <typeparam name="T">Type of object which must be of IEnumerable.</typeparam>
        /// <param name="obj">Object must be of IEnumerable.</param>
        /// <returns>The element type of the specified type.</returns>
        public static Type GetElementType<T>(this T obj)
            where T : IEnumerable
        {
            return Fosol.Core.Extensions.Types.TypeExtensions.GetElementType(typeof(T));
        }

        /// <summary>
        /// Determine if the specified object type has an attribute of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <param name="obj">Object to test.</param>
        /// <param name="attributeType">Attribute type to look for.</param>
        /// <param name="inherit">Whether to look in the ancestor objects for the attribute type.</param>
        /// <returns>True if the object has the specified attribute.</returns>
        public static bool HasAttribute<T>(this T obj, Type attributeType, bool inherit = false)
        {
            if (typeof(T) is Type)
                return Fosol.Core.Extensions.Types.TypeExtensions.HasAttribute(typeof(T), attributeType, inherit);
            else
                return Fosol.Core.Extensions.Types.TypeExtensions.HasAttribute(obj.GetType(), attributeType, inherit);
        }

        /// <summary>
        /// Determines if the specified object is one of the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <param name="obj">Object to test.</param>
        /// <param name="validValues">An array of valid values.</param>
        /// <returns>True if the specified object is one of the valid values.</returns>
        public static bool In<T>(this T obj, params T[] validValues)
        {
            return validValues.Contains(obj);
        }
    }
}
