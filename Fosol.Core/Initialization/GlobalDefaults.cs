using System;

namespace Fosol.Core.Initialization
{
    /// <summary>
    /// GlobalDefaults static class, provides a static reference to a singleton version of a DefaultsContainer class.
    /// Essentially it provides a default wrapper so you don't have to implement one yourself.
    /// </summary>
    public static class GlobalDefaults
    {
        #region Variables
        /// <summary>
        /// Singleton reference of a DefaultsContainer class.
        /// </summary>
        private static readonly DefaultsContainer _Container;
        #endregion

        #region Properties
        /// <summary>
        /// get - DefaultsContainer object.
        /// </summary>
        public static DefaultsContainer Container
        {
            get
            {
                return _Container;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the GlobalDefaults class.
        /// Creates a new singleton instance of a DefaultsContainer class.
        /// </summary>
        static GlobalDefaults()
        {
            _Container = new DefaultsContainer();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a configuration action to the DefaultsContainer class.
        /// </summary>
        /// <param name="action">Configuration action to perform when Initialize method is called.</param>
        public static void Configure(Action<DefaultsContainerConfiguration> action)
        {
            GlobalDefaults.Container.Configure(action);
        }

        /// <summary>
        /// Initializes the DefaultsContainer class.
        /// </summary>
        public static void Initialize()
        {
            GlobalDefaults.Container.Initialize();
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance.
        /// </summary>
        /// <param name="type">Type of object to resolve.</param>
        /// <returns>Default value or instance for the specified type.</returns>
        public static object Resolve(Type type)
        {
            return GlobalDefaults.Container.Resolve(type);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified name.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>Default value or instance for the specified type and name.</returns>
        public static object Resolve(Type type, string name)
        {
            return GlobalDefaults.Container.Resolve(type, name);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <returns>Default value or instance for the specified type.</returns>
        public static T Resolve<T>()
        {
            return GlobalDefaults.Container.Resolve<T>();
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified name.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>Default value or instance for the specified type and name.</returns>
        public static T Resolve<T>(string name)
        {
            return GlobalDefaults.Container.Resolve<T>(name);
        }
        #endregion
    }
}
