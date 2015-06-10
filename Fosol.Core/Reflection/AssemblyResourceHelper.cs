using System;
using System.IO;
using System.Reflection;

namespace Fosol.Core.Reflection
{
    /// <summary>
    /// AssemblyResourceHelper static class, provides methods to help with loading assembly resources.
    /// </summary>
    public static class AssemblyResourceHelper
    {
        /// <summary>
        /// Gets a Stream from the currently executing assembly.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "resourceName" must not be null.</exception>
        /// <exception cref="System.ArgumentException">Parameter "resourceName" must not be empty or whitespace.</exception>
        /// <exception cref="System.IO.FileLoadException"></exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        /// <exception cref="System.BadImageFormatException"></exception>
        /// <exception cref="System.MemberAccessException"></exception>
        /// <param name="resourceName">Assembly resource full name.</param>
        /// <returns>Stream to assembly resource.</returns>
        public static Stream GetStream(string resourceName)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(resourceName, nameof(resourceName));
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Gets a Stream to the specified Assembly Resource.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters "assemblyName" and "resourceName" must not be null.</exception>
        /// <exception cref="System.ArgumentException">Parameters "assemblyName" and "resourceName" must not be empty or whitespace.</exception>
        /// <exception cref="System.IO.FileLoadException"></exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        /// <exception cref="System.BadImageFormatException"></exception>
        /// <exception cref="System.MemberAccessException"></exception>
        /// <param name="assemblyName">Assembly full name.</param>
        /// <param name="resourceName">Assembly resource full name.</param>
        /// <returns>Stream to assembly resource.</returns>
        public static Stream GetStream(string assemblyName, string resourceName)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(assemblyName, nameof(assemblyName));
            Validation.Argument.Assert.IsNotNullOrWhitespace(resourceName, nameof(resourceName));
            Assembly assembly = Assembly.Load(assemblyName);
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
