using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fosol.Core.Extensions.Types
{
    /// <summary>
    /// TypeExtensions is a static class containing extension methods for Type objects.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determine if the object type has a constructor that takes no parameters or only has optional parameters.
        /// </summary>
        /// <param name="type">Type of object to test.</param>
        /// <returns>True if the type has an empty constructor.</returns>
        public static bool HasEmptyConstructor(this Type type)
        {
            return (type.GetConstructor(Type.EmptyTypes) != null
                || type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Any(c => c.GetParameters().All(p => p.IsOptional)));
        }

        /// <summary>
        /// Determine if the constructor with the specified parameter type exists.
        /// </summary>
        /// <param name="type">Type of object to test.</param>
        /// <param name="paramType">Parameter type to test for.</param>
        /// <returns>True if the type has a valid constructor.</returns>
        public static bool HasConstructor(this Type type, Type paramType)
        {
            return HasConstructor(type, new Type[] { paramType });
        }

        /// <summary>
        /// Determine if the constructor with the specified parameter types exists.
        /// The order of the parameter types matters.
        /// </summary>
        /// <param name="type">Type of object to test.</param>
        /// <param name="paramTypes">An array of parameter types to test for.</param>
        /// <returns>True if the type has a valid constructor.</returns>
        public static bool HasConstructor(this Type type, params Type[] paramTypes)
        {
            foreach (var con in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
            {
                var p = con.GetParameters();

                if (p.Length != paramTypes.Length)
                    continue;

                for (var i = 0; i < paramTypes.Length; i++)
                {
                    if (p[i].ParameterType != paramTypes[i])
                        return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determine if the specified type is nullable.
        /// </summary>
        /// <param name="type">Type of object to test.</param>
        /// <returns>True if the type is nullable.</returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType
                && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        /// <summary>
        /// Determine if the type has the specified attribute defined.
        /// </summary>
        /// <param name="type">Type of object to test.</param>
        /// <param name="attributeType">Attribute type to test for.</param>
        /// <param name="inherit">Whether to search ancestors of the object for the attribute.</param>
        /// <returns>True if the type has the attribute defined.</returns>
        public static bool HasAttribute(this Type type, Type attributeType, bool inherit = false)
        {
            return Attribute.IsDefined(type, attributeType, inherit);
        }

        /// <summary>
        /// Get the specified attribut from the type.
        /// </summary>
        /// <typeparam name="T">Attribute type to return.</typeparam>
        /// <param name="type">Type of object to search.</param>
        /// <param name="inherit">Whether to search ancestors of the object for the attribute.</param>
        /// <returns>The attribute if it exists.</returns>
        public static T GetCustomAttribute<T>(this Type type, bool inherit = false)
            where T: Attribute
        {
            return (T)Attribute.GetCustomAttribute(type, typeof(T), inherit);
        }

        /// <summary>
        /// Get the specified attributs from the type.
        /// </summary>
        /// <typeparam name="T">Attribute type to return.</typeparam>
        /// <param name="type">Type of object to search.</param>
        /// <param name="inherit">Whether to search ancestors of the object for the attribute.</param>
        /// <returns>An array of attributes if they exists.</returns>
        public static T[] GetCustomAttributes<T>(this Type type, bool inherit = false)
            where T : Attribute
        {
            return Attribute.GetCustomAttributes(type, typeof(T), inherit).Select(a => (T)a).ToArray();
        }

        /// <summary>
        /// Determine if the type is a static class.
        /// </summary>
        /// <param name="type">Type of object to test.</param>
        /// <returns>True if the type is static.</returns>
        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// Get the element type of the specified type.
        /// The specified type must inherit from IEnumerable.
        /// </summary>
        /// <exception cref="System.ArgumentException">Argument "type" must inherit from IEnumerable.</exception>
        /// <param name="type">Type to get the element type from.</param>
        /// <returns>The element type of the specified type.</returns>
        public static Type GetElementType(this Type type)
        {
            var is_enum = type._GetElementType();
            if (is_enum == null)
                throw new ArgumentException(String.Format(Resources.Multilingual.Extensions_Types_TypeExtensions_GetElementType, nameof(type)), nameof(type));

            return is_enum.GetGenericArguments()[0];
        }

        /// <summary>
        /// Determine if the type inherits from IEnumerable and what the element type is.
        /// </summary>
        /// <param name="type">Type to get the element type from.</param>
        /// <returns>The element type of the specified type.</returns>
        private static Type _GetElementType(this Type type)
        {
            if (type == null || type == typeof(string))
                return null;

            if (type.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(type.GetElementType());

            if (type.IsGenericType)
            {
                foreach (var arg in type.GetGenericArguments())
                {
                    var is_enum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (is_enum.IsAssignableFrom(type))
                        return is_enum;
                }
            }

            var ifaces = type.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (var iface in ifaces)
                {
                    var is_enum = iface._GetElementType();
                    if (is_enum != null)
                        return is_enum;
                }
            }

            if (type.BaseType != null && type.BaseType != typeof(object))
                return type.BaseType._GetElementType();

            return null;
        }
    }
}
