using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Initialization
{
    /// <summary>
    /// Assert static class, provides methods to initialize variables with a default value.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// If the value is null it will initialize to the default value.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Reference to original variable.</param>
        /// <param name="initValue">Default value to initialize with.</param>
        public static void IsNotNull<T>(ref T value, ref T initValue)
        {
            if (value == null)
                value = initValue;
        }

        /// <summary>
        /// If the value is null it will initialize to the default value.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Reference to original variable.</param>
        /// <param name="initValue">Default value to initialize with.</param>
        public static void IsNotNull<T>(ref T value, T initValue)
        {
            if (value == null)
                value = initValue;
        }

        /// <summary>
        /// If the value is null or empty it will initialize to the default value.
        /// </summary>
        /// <param name="value">Reference to original variable.</param>
        /// <param name="initValue">Default value to initialize with.</param>
        public static void IsNotNullOrEmpty(ref string value, string initValue)
        {
            if (String.IsNullOrEmpty(value))
                value = initValue;
        }

        /// <summary>
        /// If the value is null, empty or whitespace it will initialize to the default value.
        /// </summary>
        /// <param name="value">Reference to original variable.</param>
        /// <param name="initValue">Default value to initialize with.</param>
        public static void IsNotNullOrWhitespace(ref string value, string initValue)
        {
            if (String.IsNullOrWhiteSpace(value))
                value = initValue;
        }

        /// <summary>
        /// If the value is the default value (i.e. null or 0) it will initialize the value.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Reference to original variable.</param>
        /// <param name="initValue">Default value to initialize with.</param>
        public static void IsNotDefault<T>(ref T value, ref T initValue)
            where T: IComparable
        {
            if (value.CompareTo(default(T)) == 0)
                value = initValue;
        }

        /// <summary>
        /// If the value is the default value (i.e. null or 0) it will initialize the value.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Reference to original variable.</param>
        /// <param name="initValue">Default value to initialize with.</param>
        public static void IsNotDefault<T>(ref T value, T initValue)
            where T : IComparable
        {
            if (value.CompareTo(default(T)) == 0)
                value = initValue;
        }
    }
}
