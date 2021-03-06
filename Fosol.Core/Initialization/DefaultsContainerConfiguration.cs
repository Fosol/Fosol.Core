﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Core.Initialization
{
    /// <summary>
    /// DefaultsContainerConfiguration sealed class, provides a way to configure and initialize a DefaultsContainer object.
    /// </summary>
    public sealed class DefaultsContainerConfiguration
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - Reference to parent DefaultsContainer object that this class will initialize.
        /// </summary>
        private DefaultsContainer Container { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DefaultsContainerConfiguration class.
        /// </summary>
        /// <param name="parent">DefaultsContainer that this class will initialize.</param>
        internal DefaultsContainerConfiguration(DefaultsContainer parent)
        {
            this.Container = parent;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the DefaultsContainer.
        /// </summary>
        /// <param name="actions">Action method that will configure the default values and instances.</param>
        internal void Initiailize(List<Action<DefaultsContainerConfiguration>> actions)
        {
            foreach (var action in actions)
            {
                action.Invoke(this);
            }
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance.
        /// </summary>
        /// <param name="type">Type of object to resolve.</param>
        /// <returns>Default value or instance for the specified type.</returns>
        public object Resolve(Type type)
        {
            return this.Container.Resolve(type);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified name.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>Default value or instance for the specified type and name.</returns>
        public object Resolve(Type type, string name)
        {
            return this.Container.Resolve(type, name);
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <returns>Default value or instance for the specified type.</returns>
        public T Resolve<T>()
        {
            return this.Container.Resolve<T>();
        }

        /// <summary>
        /// Resolves the type and returns the default value or instance for the specified name.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="name">Unique name to identify the value or instance.</param>
        /// <returns>Default value or instance for the specified type and name.</returns>
        public T Resolve<T>(string name)
        {
            return this.Container.Resolve<T>(name);
        }

        /// <summary>
        /// Set the default value for the specified type.
        /// This will add the default value if one doesn't already exist for the specified type.
        /// It will overwrite the default value if one has already been set for the specified type.
        /// </summary>
        /// <param name="type">Type of value or instance.</param>
        /// <param name="value">Default value to initialize with.</param>
        public void Set(Type type, object value)
        {
            this.Set(type, null, value);
        }

        /// <summary>
        /// Set the default value for the specified type and name.
        /// This will add the default value if on doesn't already exist for the specified type and name.
        /// It will overwrite the default value if one has already been set for the specified type and name.
        /// </summary>
        /// <param name="type">Type of value or instance.</param>
        /// <param name="name">Uniue name to identify the value or instance.</param>
        /// <param name="value">Default value to initialize with.</param>
        public void Set(Type type, string name, object value)
        {
            this.Container.Items[new MemberKey(type, name)] = value;
        }

        /// <summary>
        /// Set the default value for the specified type and name.
        /// This will add the default value if on doesn't already exist for the specified type and name.
        /// It will overwrite the default value if one has already been set for the specified type and name.
        /// </summary>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <param name="value">Default value to initialize with.</param>
        public void Set<T>(T value)
        {
            this.Set(typeof(T), value);
        }

        /// <summary>
        /// Set the default value for the specified type and name.
        /// This will add the default value if on doesn't already exist for the specified type and name.
        /// It will overwrite the default value if one has already been set for the specified type and name.
        /// </summary>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <param name="name">Uniue name to identify the value or instance.</param>
        /// <param name="value">Default value to initialize with.</param>
        public void Set<T>(string name, T value)
        {
            this.Set(typeof(T), name, value);
        }

        /// <summary>
        /// Add a default value to the container.
        /// </summary>
        /// <exception cref="System.ArgumentException">An element with the same key already exists.</exception>
        /// <param name="type">Type of value or instance.</param>
        /// <param name="value">Default value to initialize with.</param>
        public void Add(Type type, object value)
        {
            this.Add(type, null, value);
        }

        /// <summary>
        /// Add a default value to the container.
        /// </summary>
        /// <exception cref="System.ArgumentException">An element with the same key already exists.</exception>
        /// <param name="type">Type of value or instance.</param>
        /// <param name="name">Uniue name to identify the value or instance.</param>
        /// <param name="value">Default value to initialize with.</param>
        public void Add(Type type, string name, object value)
        {
            this.Container.Items.Add(new MemberKey(type, name), value);
        }

        /// <summary>
        /// Add a default value to the container.
        /// </summary>
        /// <exception cref="System.ArgumentException">An element with the same key already exists.</exception>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <param name="value">Default value to initialize with.</param>
        public void Add<T>(T value)
        {
            this.Add(typeof(T), value);
        }

        /// <summary>
        /// Add a default value to the container.
        /// </summary>
        /// <exception cref="System.ArgumentException">An element with the same key already exists.</exception>
        /// <typeparam name="T">Type of value or instance.</typeparam>
        /// <param name="name">Uniue name to identify the value or instance.</param>
        /// <param name="value">Default value to initialize with.</param>
        public void Add<T>(string name, T value)
        {
            this.Add(typeof(T), name, value);
        }
        #endregion
    }
}
