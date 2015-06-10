using Fosol.Core.Extensions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fosol.Core.Text
{
    /// <summary>
    /// ElementLibrary static class, provides a dictionary of currently configured Element objects.
    /// </summary>
    public static class ElementLibrary
    {
        #region Variables
        private static readonly Caching.SimpleCache<Type> _Cache = new Caching.SimpleCache<Type>();
        #endregion

        #region Properties
        /// <summary>
        /// get - An array of key names within the library.
        /// </summary>
        public static string[] Keys
        {
            get
            {
                return _Cache.Keys.ToArray();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the library with 
        /// The Logger cannot use a keyword unless it's been registered.
        /// </summary>
        static ElementLibrary()
        {
            Initialize();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize what Element types are in the library.
        /// </summary>
        private static void Initialize()
        {
            foreach (var type in GetKeywordTypes(Assembly.GetCallingAssembly(), typeof(Elements.TextElement).Namespace))
            {
                Add(type);
            }
        }

        /// <summary>
        /// Refresh the library so that it only contains the default Element types.
        /// </summary>
        public static void Refresh()
        {
            _Cache.Clear();
            Initialize();
        }

        /// <summary>
        /// Add the Element Type to the library.
        /// </summary>
        /// <param name="type">Element Type.</param>
        /// <returns>Number of items in library.</returns>
        public static int Add(Type type)
        {
            Validation.Argument.Assert.IsNotNull(type, nameof(type));
            Validation.Argument.Assert.IsAssignable(type, typeof(Element), nameof(type));
            Validation.Argument.Assert.HasAttribute(type, typeof(ElementAttribute), nameof(type));

            var attr = type.GetCustomAttribute(typeof(ElementAttribute)) as ElementAttribute;
            if (_Cache.ContainsKey(attr.Name))
            {
                if (attr.Override)
                    _Cache.Remove(attr.Name);
                else
                    throw new InvalidOperationException(string.Format("Element \"{0}\" already exists.", attr.Name));
            }

            _Cache.Add(attr.Name, type);
            return _Cache.Count;
        }

        /// <summary>
        /// Add all the Element Types within the specified Assembly and Namespace.
        /// </summary>
        /// <param name="assemblyString">Fully qualified name of the assembly.</param>
        /// <param name="nameOrNamespace">Namespace or fully qualified name to the Element(s).</param>
        /// <returns>Number of items in library.</returns>
        public static int Add(string assemblyString, string nameOrNamespace)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(assemblyString, nameof(nameOrNamespace));
            Validation.Argument.Assert.IsNotNullOrEmpty(nameOrNamespace, nameof(nameOrNamespace));

            var assembly = Assembly.Load(assemblyString);
            if (assembly == null)
                throw new InvalidOperationException(string.Format("Assembly name \"{0}\" is invalid.", assemblyString));

            return Add(assembly, nameOrNamespace);
        }

        /// <summary>
        /// Add all the Element Types within the specified Assembly and Namespace.
        /// </summary>
        /// <param name="assembly">Assembly containing Element(s).</param>
        /// <param name="nameOrNamespace">Namespace or fully qualified name to the Element(s).</param>
        /// <returns>Number of items in library.</returns>
        public static int Add(Assembly assembly, string nameOrNamespace)
        {
            // Fetch every Element in the specified namespacePath.
            foreach (var type in GetKeywordTypes(assembly, nameOrNamespace))
            {
                Add(type);
            }

            return _Cache.Count;
        }

        /// <summary>
        /// Fetch all the Element Type objects in the specified Assembly and Namespace.
        /// </summary>
        /// <param name="assembly">Assembly containing Element objects.</param>
        /// <param name="nameOrNamespace">Namespace or fully qualified name to the Element(s).</param>
        /// <returns>Collection of Element Types.</returns>
        static IEnumerable<Type> GetKeywordTypes(Assembly assembly, string nameOrNamespace)
        {
            var type = GetKeywordType(assembly, nameOrNamespace);
            if (type != null)
                return new List<Type>() { type };

            // The keywordNamespace is only a path to possibly numerous Element objects.
            return (
                from t in assembly.GetTypes()
                where String.Equals(t.Namespace, nameOrNamespace, StringComparison.Ordinal)
                    && typeof(Element).IsAssignableFrom(t)
                    && t.HasAttribute(typeof(ElementAttribute))
                select t);
        }

        /// <summary>
        /// Checks to see if the fullyQualifiedTypeName is of Type Element.
        /// </summary>
        /// <param name="assembly">Assembly containing Element objects.</param>
        /// <param name="fullyQualifiedTypeName">Fully qualified name of the Element.</param>
        /// <returns>Element Type, or null if the fullyQualifiedTypeName was only a namespace.</returns>
        static Type GetKeywordType(Assembly assembly, string fullyQualifiedTypeName)
        {
            var type = assembly.GetType(fullyQualifiedTypeName, false);

            // The keywordNamespace pointed directly to a Type.
            // The keywordNamespace is a valid type.
            // And it has been marked with the KeywordAttribute.
            if (type != null)
            {
                if (typeof(Element).IsAssignableFrom(type)
                    && type.HasAttribute(typeof(ElementAttribute)))
                    return type;

                throw new InvalidOperationException(string.Format("Element \"{0}\" does not exist.", fullyQualifiedTypeName));
            }

            return null;
        }

        /// <summary>
        /// Get the Keyword Type for the specified key name.
        /// First it will check the library for an existing Element.
        /// Then it will check if the executing assembly contains a Element with the specified name.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "typeName" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "typeName" cannot be null.</exception>
        /// <exception cref="System.InvalidOperation">Type must exist.</exception>
        /// <param name="name">Unique name to identify the Target keyword.</param>
        /// <returns>Type of Target.</returns>
        public static Type Get(string name)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(name, nameof(name));

            Type type = null;
            // The cache contains the Element so return it.
            if (_Cache.ContainsKey(name))
                type = _Cache[name].Value;

            // Check if the name is a fully qualified type name in the executing assembly.
            if (type == null)
                type = GetKeywordType(Assembly.GetEntryAssembly(), name);
            if (type == null)
                type = GetKeywordType(Assembly.GetCallingAssembly(), name);
            if (type == null)
                type = GetKeywordType(Assembly.GetExecutingAssembly(), name);
            if (type == null)
                throw new InvalidOperationException(string.Format("Element \"{0}\" does not exist.", name));
            return type;
        }

        /// <summary>
        /// Checks to see if the library contains the Element with the specified name.
        /// </summary>
        /// <param name="name">Name of Element.</param>
        /// <returns>True if exists.</returns>
        public static bool ContainsKey(string name)
        {
            return _Cache.ContainsKey(name);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
