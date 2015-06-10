using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Core.Caching
{
    /// <summary>
    /// A SimpleCache object provides a dictionary to manage a collection of CacheItem objects.
    /// </summary>
    /// <typeparam name="T">Type of item being place in cache.</typeparam>
    public sealed class SimpleCache<T>
        : IEnumerable<CacheItem<T>>, IDisposable
    {
        #region Variables
        private readonly System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private readonly Dictionary<string, CacheItem<T>> _Items;
        #endregion

        #region Properties
        /// <summary>
        /// get - Number of items in cache.
        /// </summary>
        public int Count
        {
            get { return _Items.Count; }
        }

        /// <summary>
        /// get - Fetch the item in cache with the specified key.
        /// This property is not thread safe because it does not use a read lock.
        /// Use the Get() method if you need a read lock.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument "key" cannot be null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Argument "key" does not point to an existing item within cache.</exception>
        /// <param name="key">Cache key to look for.</param>
        /// <returns>Item if found.</returns>
        public CacheItem<T> this[string key]
        {
            get
            {
                return _Items[key];
            }
        }

        /// <summary>
        /// get - Collection of all the keys within the cache.
        /// </summary>
        public Dictionary<string, CacheItem<T>>.KeyCollection Keys
        {
            get { return _Items.Keys; }
        }

        /// <summary>
        /// get - Collection of all the values within the cache.
        /// </summary>
        internal Dictionary<string, CacheItem<T>>.ValueCollection Values
        {
            get { return _Items.Values; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Cache object.
        /// </summary>
        public SimpleCache()
        {
            _Items = new Dictionary<string, CacheItem<T>>(StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Creates a new instance of a Cache object.
        /// </summary>
        /// <param name="comparer">Controls how cache keys are compared.</param>
        public SimpleCache(IEqualityComparer<string> comparer)
        {
            _Items = new Dictionary<string, CacheItem<T>>(comparer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the item from cache with the specified key.
        /// Uses a read lock while fetching.
        /// </summary>
        /// <param name="key">Cache key to identify the item.</param>
        /// <returns>Item from cache.</returns>
        public CacheItem<T> Get(string key)
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items[key];
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Try to get the value from cache, if it is not in cache add it to cache.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot add a null value to the cache.</exception>
        /// <param name="key">Unique cache key to identify the item.</param>
        /// <param name="add">Function to retrieve the value that will be added to cache.</param>
        /// <returns>Item from cache if it exists.</returns>
        public CacheItem<T> LazyGet(string key, Func<T> add)
        {
            _Lock.EnterUpgradeableReadLock();
            try
            {
                if (_Items.ContainsKey(key))
                    return _Items[key];

                _Lock.EnterWriteLock();
                try
                {
                    // Try again before attempting to add the value.
                    if (_Items.ContainsKey(key))
                        return _Items[key];

                    var value = new CacheItem<T>(key, add());
                    if (value == null)
                        throw new InvalidOperationException();

                    _Items.Add(key, value);
                    return value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
            finally
            {
                _Lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Clears cache of all items.
        /// </summary>
        public void Clear()
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Clear();
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Add the item to cache with the specified cache key.
        /// </summary>
        /// <param name="key">Cache key to uniquely identify the item.</param>
        /// <param name="value">Item to add to cache.</param>
        public void Add(string key, T value)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Add(key, new CacheItem<T>(key, value));
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the item in cache with the specified key.
        /// </summary>
        /// <param name="key">Cache key to look for.</param>
        /// <returns>True if the item was found and removed.</returns>
        public bool Remove(string key)
        {
            _Lock.EnterWriteLock();
            try
            {
                return _Items.Remove(key);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Checks the cache if it contains the specified key.
        /// </summary>
        /// <param name="key">Cache key to look for.</param>
        /// <returns>True if the key exists in cache.</returns>
        public bool ContainsKey(string key)
        {
            return _Items.ContainsKey(key);
        }

        /// <summary>
        /// Returns the Enumerator.
        /// </summary>
        /// <returns>A new instance of the Enumerator.</returns>
        public IEnumerator<CacheItem<T>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns the Enumerator.
        /// </summary>
        /// <returns>A new instance of the Enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Dispose and clear the cache.
        /// </summary>
        public void Dispose()
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Clear();
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion

        /// <summary>
        /// Cache Enumerator provides a way to enumerate through the dictionary.
        /// </summary>
        public struct Enumerator
            : IEnumerator<CacheItem<T>>, System.Collections.IEnumerator
        {
            #region Variables
            private CacheItem<T>[] _Items;
            private int _Position;
            private CacheItem<T> _CurrentItem;
            #endregion

            #region Properties
            #endregion

            #region Constructors
            /// <summary>
            /// Creates a new instance of an Enumerator.
            /// </summary>
            /// <param name="cache"></param>
            public Enumerator(SimpleCache<T> cache)
            {
                _Items = cache._Items.Values.ToArray();
                _Position = -1;
                _CurrentItem = default(CacheItem<T>);
            }
            #endregion

            #region Methods
            /// <summary>
            /// get - The current item being viewed.
            /// </summary>
            public CacheItem<T> Current
            {
                get { return _CurrentItem; }
            }

            /// <summary>
            /// Dispose this Enumerator.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Returns the current item being viewed.
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            /// <summary>
            /// Move to the next item in the enumeration.
            /// </summary>
            /// <returns>True if there is a value.</returns>
            public bool MoveNext()
            {
                if (++_Position >= _Items.Count())
                    return false;
                else
                    _CurrentItem = _Items[_Position];
                return true;
            }

            /// <summary>
            /// Reset the enumerator to start over.
            /// </summary>
            public void Reset()
            {
                _CurrentItem = default(CacheItem<T>);
                _Position = -1;
            }
            #endregion

            #region Operators
            #endregion

            #region Events
            #endregion
        }
    }
}
