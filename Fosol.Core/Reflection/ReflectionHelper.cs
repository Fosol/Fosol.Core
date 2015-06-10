using Fosol.Core.Extensions.Types;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fosol.Core.Reflection
{
    /// <summary>
    /// ReflectionHelper static class provides methods for converting and creating objects during runtime.
    /// </summary>
    public static class ReflectionHelper
    {
        #region Methods
#if WINDOWS_APP || WINDOWS_PHONE_APP
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">Type of object to create an instance of.</param>
        /// <param name="args">Constructor arguments to include when creating the object.</param>
        /// <returns>New instance of the type specified.</returns>
        public static object CreateInstance(this Type type, params object[] args)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(type, nameof(type));
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <param name="type">Type of object to create an instance of.</param>
        /// <returns>New instance of the type specified.</returns>
        public static T CreateInstance<T>(this Type type)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(type, nameof(type));
            return Activator.CreateInstance<T>();
        }
#else
        /// <summary>
        /// Try to convert the value to the specified conversionType.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="convertTo">Type to convert to.</param>
        /// <param name="result">Result of conversion.</param>
        /// <returns>True if successful.</returns>
        public static bool TryConvert(object value, Type convertTo, ref object result)
        {
            if (value == null)
            {
                if (convertTo.IsNullableType())
                {
                    result = null;
                    return true;
                }
                return false;
            }

            // Conversion not required.
            if (convertTo == value.GetType())
            {
                result = value;
                return true;
            }

            try
            {
                if (!convertTo.IsEnum)
                    result = Convert.ChangeType(value, convertTo);
                else
                {
                    var converter = new EnumConverter(convertTo);
                    if (converter.CanConvertFrom(value.GetType()))
                        result = converter.ConvertFrom(value);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Try to convert the value to type T.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="result">Result of conversion.</param>
        /// <returns>True if successful.</returns>
        public static bool TryConvert<T>(object value, ref T result)
        {
            var type = typeof(T);
            if (value == null)
            {
                if (type.IsNullableType())
                {
                    result = default(T);
                    return true;
                }
                return false;
            }

            // Conversion not required.
            if (type == value.GetType())
            {
                result = (T)value;
                return true;
            }

            try
            {
                if (!type.IsEnum)
                    result = (T)Convert.ChangeType(value, type);
                else
                {
                    var converter = new EnumConverter(type);
                    if (converter.CanConvertFrom(value.GetType()))
                        result = (T)converter.ConvertFrom(value);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a new instance of the specified Type.
        /// If the args contain string values that can be converted to enums it will try to convert them.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "type" cannot be null.</exception>
        /// <exception cref="System.InvalidOperationException">Unable to create a new instance of the specified type.</exception>
        /// <param name="type">The specified type to create.</param>
        /// <param name="args">Arguments to pass to the constructor.</param>
        /// <returns>New instance of the specified type.</returns>
        public static object ConstructObject(Type type, params object[] args)
        {
            Validation.Argument.Assert.IsNotNull(type, nameof(type));

            object result = null;
            Exception exception = null;
            try
            {
                if (args != null || args.Length > 0)
                {
                    // Attempt to construct the object with the supplied arguments.
                    var constructor = type.GetConstructor(args.Select(a => a.GetType()).ToArray());
                    if (constructor != null)
                        result = constructor.Invoke(args);
                    else
                    {
                        // Check for constructors that have enumerators.
                        // Looking for a constructor that matches the args.
                        var constructors = (
                            from c in type.GetConstructors()
                            from p in c.GetParameters()
                            where p.ParameterType.IsEnum
                                && c.GetParameters().Count() == args.Count()
                            select c);

                        // Try each constructor to find a match.
                        foreach (var ctor in constructors)
                        {
                            var pars = ctor.GetParameters();
                            var c_args = new object[args.Length];
                            var use_ctor = true;

                            // Use the constructor that works with the supplied args.
                            // Attempt to convert the specific arg when the parameter is an enum.
                            for (var i = 0; i < pars.Count(); i++)
                            {
                                if (pars[i].ParameterType == args[i].GetType())
                                    c_args[i] = args[i];
                                else if (pars[i].ParameterType.IsEnum && args[i] is string)
                                {
                                    try
                                    {
                                        // Convert to enum.
                                        c_args[i] = Enum.Parse(pars[i].ParameterType, (string)args[i], false);
                                    }
                                    catch (TargetInvocationException ex)
                                    {
                                        use_ctor = false;
                                        exception = ex.InnerException;
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        use_ctor = false;
                                        exception = ex;
                                        break;
                                    }
                                }
                                else
                                {
                                    use_ctor = false;
                                    break;
                                }
                            }

                            // Try to use this constructor.
                            if (use_ctor)
                            {
                                result = ctor.Invoke(c_args);
                                break;
                            }
                        }
                    }
                }

                if (result == null)
                {
                    // Attempt to construct the object without arguments.
                    var constructor = type.GetConstructor(new Type[0]);
                    if (constructor == null)
                        throw new InvalidOperationException();
                    result = constructor.Invoke(new object[0]);
                }
            }
            catch (TargetInvocationException ex)
            {
                exception = ex.InnerException;
            }

            if (result != null)
                return result;

            if (exception != null)
                throw new InvalidOperationException("", exception);
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Attempts to create a new instance of the specified Type.
        /// If the args contain string values that can be converted to enums it will try to convert them.
        /// </summary>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <param name="args">Arguments to pass to the constructor.</param>
        /// <returns>New instance of the specified type.</returns>
        public static T ConstructObject<T>(params object[] args)
            where T : class
        {
            var type = typeof(T);
            T result = null;
            Exception exception = null;
            try
            {
                if (args != null || args.Length > 0)
                {
                    // Attempt to construct the object with the supplied arguments.
                    var constructor = type.GetConstructor(args.Select(a => a.GetType()).ToArray());
                    if (constructor != null)
                        result = (T)constructor.Invoke(args);
                    else
                    {
                        // Check for constructors that have enumerators.
                        // Looking for a constructor that matches the args.
                        var constructors = (
                            from c in type.GetConstructors()
                            from p in c.GetParameters()
                            where p.ParameterType.IsEnum
                                && c.GetParameters().Count() == args.Count()
                            select c);

                        // Try each constructor to find a match.
                        foreach (var ctor in constructors)
                        {
                            var pars = ctor.GetParameters();
                            var c_args = new object[args.Length];
                            var use_ctor = true;

                            // Use the constructor that works with the supplied args.
                            // Attempt to convert the specific arg when the parameter is an enum.
                            for (var i = 0; i < pars.Count(); i++)
                            {
                                if (pars[i].ParameterType == args[i].GetType())
                                    c_args[i] = args[i];
                                else if (pars[i].ParameterType.IsEnum && args[i] is string)
                                {
                                    try
                                    {
                                        // Convert to enum.
                                        c_args[i] = Enum.Parse(pars[i].ParameterType, (string)args[i], false);
                                    }
                                    catch (TargetInvocationException ex)
                                    {
                                        use_ctor = false;
                                        exception = ex.InnerException;
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        use_ctor = false;
                                        exception = ex;
                                        break;
                                    }
                                }
                                else
                                {
                                    use_ctor = false;
                                    break;
                                }
                            }

                            // Try to use this constructor.
                            if (use_ctor)
                            {
                                result = (T)ctor.Invoke(c_args);
                                break;
                            }
                        }
                    }
                }

                if (result == null)
                {
                    // Attempt to construct the object without arguments.
                    var constructor = type.GetConstructor(new Type[0]);
                    if (constructor == null)
                        throw new InvalidOperationException();
                    result = (T)constructor.Invoke(new object[0]);
                }
            }
            catch (TargetInvocationException ex)
            {
                exception = ex.InnerException;
            }

            if (result != null)
                return result;

            if (exception != null)
                throw new InvalidOperationException("", exception);
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Attempts to create a new instance of the specified Type.
        /// This method ensures that the specified Type (type) is assignable from type T.
        /// If the args contain string values that can be converted to enums it will try to convert them.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "typeName" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "typeName" cannot be null, or be an invalid type.</exception>
        /// <exception cref="System.InvalidOperationException">Unable to create a new instance of the specified type.</exception>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <param name="type">The specified type to create.</param>
        /// <param name="args">Arguments to pass to the constructor.</param>
        /// <returns>New instance of the specified type.</returns>
        public static T ConstructObject<T>(Type type, params object[] args)
            where T : class
        {
            Validation.Argument.Assert.IsNotNull(type, nameof(type));
            Validation.Argument.Assert.IsAssignable(type, typeof(T), nameof(T));

            T result = null;
            Exception exception = null;
            try
            {
                if (args != null || args.Length > 0)
                {
                    // Attempt to construct the object with the supplied arguments.
                    var constructor = type.GetConstructor(args.Select(a => a.GetType()).ToArray());
                    if (constructor != null)
                        result = (T)constructor.Invoke(args);
                    else
                    {
                        // Check for constructors that have enumerators.
                        // Looking for a constructor that matches the args.
                        var constructors = (
                            from c in type.GetConstructors()
                            from p in c.GetParameters()
                            where p.ParameterType.IsEnum
                                && c.GetParameters().Count() == args.Count()
                            select c);

                        // Try each constructor to find a match.
                        foreach (var ctor in constructors)
                        {
                            var pars = ctor.GetParameters();
                            var c_args = new object[args.Length];
                            var use_ctor = true;

                            // Use the constructor that works with the supplied args.
                            // Attempt to convert the specific arg when the parameter is an enum.
                            for (var i = 0; i < pars.Count(); i++)
                            {
                                if (pars[i].ParameterType == args[i].GetType())
                                    c_args[i] = args[i];
                                else if (pars[i].ParameterType.IsEnum && args[i] is string)
                                {
                                    try
                                    {
                                        // Convert to enum.
                                        c_args[i] = Enum.Parse(pars[i].ParameterType, (string)args[i], false);
                                    }
                                    catch (TargetInvocationException ex)
                                    {
                                        use_ctor = false;
                                        exception = ex.InnerException;
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        use_ctor = false;
                                        exception = ex;
                                        break;
                                    }
                                }
                                else
                                {
                                    use_ctor = false;
                                    break;
                                }
                            }

                            // Try to use this constructor.
                            if (use_ctor)
                            {
                                result = (T)ctor.Invoke(c_args);
                                break;
                            }
                        }
                    }
                }

                if (result == null)
                {
                    // Attempt to construct the object without arguments.
                    var constructor = type.GetConstructor(new Type[0]);
                    if (constructor == null)
                        throw new InvalidOperationException();
                    result = (T)constructor.Invoke(new object[0]);
                }
            }
            catch (TargetInvocationException ex)
            {
                exception = ex.InnerException;
            }

            if (result != null)
                return result;

            if (exception != null)
                throw new InvalidOperationException("", exception);
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Attempts to create a new instance of the specified Type.
        /// This method ensures that the specified Type (typeName) is assignable from type T.
        /// If the args contain string values that can be converted to enums it will try to convert them.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "typeName" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "typeName" cannot be null, or be an invalid type.</exception>
        /// <exception cref="System.InvalidOperationException">Unable to create a new instance of the specified type.</exception>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <param name="typeName">The specified type to create.</param>
        /// <param name="args">Arguments to pass to the constructor.</param>
        /// <returns>New instance of the specified type.</returns>
        public static T ConstructObject<T>(string typeName, params object[] args)
            where T : class
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(typeName, nameof(typeName));
            var type = Type.GetType(typeName);
            Validation.Argument.Assert.IsNotNull(type, nameof(typeName), Resources.Multilingual.Reflection_ReflectionHelper_ConstructObject);
            return ConstructObject<T>(type, args);
        }
#endif
        #endregion
    }
}
