using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Core.Initialization
{
    /// <summary>
    /// Defaults Container sealed class, provides a dictionary of default values or instances for the configured types.
    /// Populate the container with values/instances with either the constructor or method Configure.
    /// All values will be initialized the first time you call the Resolve function.
    /// If you want to force early initialization call the Initialize method.
    /// Thread safe - DefaultsContainer is readonly after initialization has occured.  This does not guarantee that the configuration actions
    /// you provide are thread safe.
    /// </summary>
    public sealed class DefaultsContainer
    {
        #region Variables
        private readonly System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private readonly List<Action<DefaultsContainerConfiguration>> _Configure;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Whether the default values and instances have been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// get - Dictionary of default values/instances.
        /// </summary>
        internal Dictionary<MemberKey, object> Items { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DefaultContainer class.
        /// </summary>
        public DefaultsContainer()
        {
            this.Items = new Dictionary<MemberKey, object>();
            _Configure = new List<Action<DefaultsContainerConfiguration>>();
        }

        /// <summary>
        /// Creates a new instance of a DefaultContainer class.
        /// </summary>
        /// <param name="action">Action method to call during initialization.</param>
        public DefaultsContainer(Action<DefaultsContainerConfiguration> action)
            : base()
        {
            this.Configure(action);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Each action added to the configuration will be called by the Initialize method.
        /// You can call the Configure method multiple times.
        /// </summary>
        /// <param name="action">Action to perform when initializing.</param>
        public void Configure(Action<DefaultsContainerConfiguration> action)
        {
            this._Configure.Add(action);
        }

        /// <summary>
        /// Configure this DefaultsContainer by adding the default value or instance for the specified type and name.
        /// </summary>
        /// <param name="type">Type of default value or instance.</param>
        /// <param name="name">Unique name to identify the default value or instance.</param>
        /// <param name="value">The default value or instance.</param>
        public void Configure(Type type, string name, object value)
        {
            this._Configure.Add(c => c.Add(type, name, value));
        }

        /// <summary>
        /// Configure this DefaultsContainer by adding the default value or instance for the specified type.
        /// </summary>
        /// <param name="type">Type of default value or instance.</param>
        /// <param name="value">The default value or instance.</param>
        public void Configure(Type type, object value)
        {
            this._Configure.Add(c => c.Add(type, value));
        }

        /// <summary>
        /// Configure this DefaultsContainer by adding the default value or instance for the specified type and name.
        /// </summary>
        /// <typeparam name="T">Type of default value or instance.</typeparam>
        /// <param name="name">Unique name to identify the default value or instance.</param>
        /// <param name="value">The default value or instance.</param>
        public void Configure<T>(string name, T value)
        {
            this._Configure.Add(c => c.Add<T>(name, value));
        }

        /// <summary>
        /// Configure this DefaultsContainer by adding the default value or instance for the specified type.
        /// </summary>
        /// <typeparam name="T">Type of default value or instance.</typeparam>
        /// <param name="value">The default value or instance.</param>
        public void Configure<T>(T value)
        {
            this._Configure.Add(c => c.Add<T>(value));
        }

        /// <summary>
        /// Initialize the container with the configured values and instances.
        /// This method itself is thread safe, however the actions it performs may not be.
        /// This method will only be executed once.
        /// </summary>
        public void Initialize()
        {
            _Lock.EnterUpgradeableReadLock();
            try
            {
                if (!this.IsInitialized)
                {
                    _Lock.EnterWriteLock();
                    try
                    {
                        var config = new DefaultsContainerConfiguration(this);
                        config.Initiailize(this._Configure);
                        this.IsInitialized = true;
                    }
                    finally
                    {
                        _Lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _Lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Find the MemberKey for the specified type and name.
        /// </summary>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>MemberKey in the dictionary if it exists.</returns>
        internal MemberKey FindKey(Type type, string name)
        {
            return this.Items.Keys.FirstOrDefault(k => k.Type == type && k.Name == name);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance.
        /// </summary>
        /// <param name="type">Type of object to resolve.</param>
        /// <returns>Default value or instance for the specified type.</returns>
        public object Resolve(Type type)
        {
            return this.Resolve(type, null);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified name.
        /// </summary>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>Default value or instance for the specified type and name.</returns>
        public object Resolve(Type type, string name)
        {
            if (!this.IsInitialized)
                this.Initialize();

            var key = this.FindKey(type, name);

            if (key == null)
                throw new InvalidOperationException("The specified type and/or name could not be found.  Before attempting to resolve a default value or instance configure the value and add it to the DefaultsContainer.");

            return this.Items[key];
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance.
        /// </summary>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <returns>Default value or instance for the specified type.</returns>
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified name.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>Default value or instance for the specified type and name.</returns>
        public T Resolve<T>(string name)
        {
            return (T)this.Resolve(typeof(T), name);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified name.
        /// </summary>
        /// <typeparam name="T">Type of value or instance to return.</typeparam>
        /// <param name="type">Type of value or instance to resolve.</param>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>Default value or instance for the specified type and name.</returns>
        public T Resolve<T>(Type type, string name)
        {
            return (T)this.Resolve(type, name);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified type.
        /// </summary>
        /// <typeparam name="T">Type of value or instance to return.</typeparam>
        /// <param name="type">Type of value or instance to resolve.</param>
        /// <returns>Default value or instance for the specified type.</returns>
        public T Resolve<T>(Type type)
        {
            return (T)this.Resolve(type);
        }

        /// <summary>
        /// Determines if the container contains a key for the specified type and name.
        /// </summary>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>True if the key exists.</returns>
        public bool ContainsKey(Type type, string name)
        {
            var key = this.FindKey(type, name);
            if (key == null)
                return false;
            return true;
        }
        #endregion
    }
}
