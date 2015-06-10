using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Fosol.Core.ServiceModel
{
    /// <summary>
    /// CorrelationState can be used as a simple dictionary storage class.
    /// CorrelationState provides a way to capture information that can be automatically stored within the specified OperationContext.
    /// CorrelationState provides a way to mimic HttpContext.Current.Items collection.
    /// Helps to save object information for the lifetime of a request.
    /// </summary>
    public sealed class CorrelationState
        : IExtension<OperationContext>, IDictionary<string, object>
    {
        #region Variables
        System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        OperationContext _Owner;
        IDictionary<string, object> _State;
        bool _IsAttached;
        #endregion

        #region Properties
        /// <summary>
        /// get - The current OperationContext owner of this CorrelationState object.
        /// </summary>
        public OperationContext Owner
        {
            get { return _Owner; }
        }

        /// <summary>
        /// get - Whether this CorrelationState object is attached to its owner OperationContext.
        /// </summary>
        public bool IsAttached
        {
            get 
            {
                _Lock.EnterReadLock();
                try
                {
                    return _IsAttached;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// get/set - A value within the state collection.
        /// </summary>
        /// <param name="key">Key name to identify the value in the state collection.</param>
        /// <returns>The value for the specified key name.</returns>
        public object this[string key]
        {
            get { return _State[key]; }
            set { _State[key] = value; }
        }

        /// <summary>
        /// get - A collection of key values stored within this dictionary.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return _State.Keys; }
        }

        /// <summary>
        /// get - A collection of values stored witin this dictionary.
        /// </summary>
        public ICollection<object> Values
        {
            get { return _State.Values; }
        }

        /// <summary>
        /// get - The number of items stored in this dictionary.
        /// </summary>
        public int Count
        {
            get { return _State.Count; }
        }

        /// <summary>
        /// get - Whether this dictionary is readonly only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _State.IsReadOnly; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a CorrelationState object.
        /// </summary>
        public CorrelationState()
        {
            _State = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a new instance of a CorrelationState object.
        /// Adds it to the OperationContext specified as an Extension.
        /// </summary>
        /// <param name="owner">OperationContext object that owns this CorrelationState.</param>
        public CorrelationState(OperationContext owner)
            : this()
        {
            var extension = owner.Extensions.Find<CorrelationState>();

            if (extension == null)
            {
                owner.Extensions.Add(this);
            }

            _Owner = owner;
            _IsAttached = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Attach this CorrelationState to the OperationContext.
        /// </summary>
        /// <param name="owner">OperationContext object that owns this CorrelationState.</param>
        public void Attach(OperationContext owner)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Owner = owner;
                _IsAttached = true;
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Detach this CorrelationState from the OperationContext.
        /// </summary>
        /// <param name="owner">OperationContext object that owns this CorrelationState.</param>
        public void Detach(OperationContext owner)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Owner = owner;
                _IsAttached = false;
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns the value for the specified key as the specified type.
        /// </summary>
        /// <exception cref="System.InvalidCastException">Value for the specified key is not assignable from the specified type.</exception>
        /// <typeparam name="T">Type of value for the specified key.</typeparam>
        /// <param name="key">Key name to identify the value stored in state.</param>
        /// <returns>The value for the specified key as the specified type.</returns>
        public T GetValue<T>(string key)
        {
            var value = _State[key];

            if (typeof(T).IsAssignableFrom(value.GetType()))
                return (T)_State[key];

            throw new InvalidCastException(string.Format("Value for key '{0}' is not of assignable from the specified type '{1}'", key, typeof(T).Name));
        }

        /// <summary>
        /// Returns the current CorrelationState object for the current OperationContext.
        /// If the OperationContext does not yet contain a CorrelationState object extension it will automatically add it.
        /// </summary>
        /// <returns>CorrelationState saved to the OperationContext extensions.</returns>
        public static CorrelationState Current()
        {
            return CorrelationState.Current(OperationContext.Current);
        }

        /// <summary>
        /// Returns the current CorrelationState object for the specified OperationContext.
        /// If the OperationContext does not yet contain a CorrelationState object extension it will automatically add it.
        /// </summary>
        /// <param name="context">OperationContext object.</param>
        /// <returns>CorrelationState saved to the OperationContext extensions.</returns>
        public static CorrelationState Current(OperationContext context)
        {
            return new CorrelationState(context);
        }

        /// <summary>
        /// Add new value to dictionary.
        /// </summary>
        /// <param name="key">Unique key to identify the value.</param>
        /// <param name="value">Value to be stored in dictionary.</param>
        public void Add(string key, object value)
        {
            _State.Add(key, value);
        }

        /// <summary>
        /// Determines if the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">Key value to look for.</param>
        /// <returns>True if the dictionary contains the key.</returns>
        public bool ContainsKey(string key)
        {
            return _State.ContainsKey(key);
        }

        /// <summary>
        /// Remove the value for the specified key.
        /// </summary>
        /// <param name="key">Key value to look for.</param>
        /// <returns>True if the value was removed.</returns>
        public bool Remove(string key)
        {
            return _State.Remove(key);
        }

        /// <summary>
        /// Try to get the value for the specified key.
        /// </summary>
        /// <param name="key">Key value to look for.</param>
        /// <param name="value">Variable to hold value for the specified key.</param>
        /// <returns>True if the value was found in the dictionary.</returns>
        public bool TryGetValue(string key, out object value)
        {
            return _State.TryGetValue(key, out value);
        }

        /// <summary>
        /// Add the specified KeyValuePair object into the dictionary.
        /// </summary>
        /// <param name="item">KeyValuePair object.</param>
        public void Add(KeyValuePair<string, object> item)
        {
            _State.Add(item);
        }

        /// <summary>
        /// Clear the dictionary of all values.
        /// </summary>
        public void Clear()
        {
            _State.Clear();
        }

        /// <summary>
        /// Determine if the dictionary contains the specified KeyValuePair object.
        /// </summary>
        /// <param name="item">KeyValuePair object to look for.</param>
        /// <returns>True if the dictionary contains the object.</returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return _State.Contains(item);
        }

        /// <summary>
        /// Copies the values within the dictionary into the specified array, starting at the specified index within the array.
        /// </summary>
        /// <param name="array">Array to copy values into.</param>
        /// <param name="arrayIndex">The starting position within the array to begin copying</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _State.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Remove the specified KeyValuePair object from the dictionary.
        /// </summary>
        /// <param name="item">KeyValuePair object to remove.</param>
        /// <returns>True if the object was removed.</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            return _State.Remove(item);
        }

        /// <summary>
        /// Returns the enumerator for this dictionary.
        /// </summary>
        /// <returns>The enumerator for this dictionary.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _State.GetEnumerator();
        }

        /// <summary>
        /// Returns the enumerator for this dictionary.
        /// </summary>
        /// <returns>The enumerator for this dictionary.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _State.GetEnumerator();
        }
        #endregion

        #region Events
        #endregion
    }
}
