using System;

namespace Fosol.Core.Caching
{
    /// <summary>
    /// CacheItem sealed class, provides a way to contain a reference to a value.
    /// </summary>
    /// <typeparam name="T">Type of object being cached.</typeparam>
    public sealed class CacheItem<T>
        : IDisposable, ICacheItem<T>
    {
        #region Variables
        private readonly string _Key;
        private readonly T _Value;
        private readonly DateTime _CreatedDate;
        private TimeSpan? _TimeToLive;
        private DateTime? _ExpiresOn;
        private bool _RenewOnRequest;
        #endregion

        #region Properties
        /// <summary>
        /// get - Unique key to identify this item in cache.
        /// </summary>
        public string Key
        {
            get { return _Key; }
        }

        /// <summary>
        /// get - Value being cached.
        /// </summary>
        public T Value
        {
            get { return _Value; }
        }

        /// <summary>
        /// get - When this CacheItem was created.
        /// </summary>
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
        }

        /// <summary>
        /// get/set - How long this CacheItem should exist in cache before being removed.
        /// </summary>
        public TimeSpan? TimeToLive
        {
            get { return _TimeToLive; }
            set
            {
                _TimeToLive = value;

                if (value.HasValue)
                    _ExpiresOn = Optimization.FastDateTime.UtcNow.Add(_TimeToLive.Value);
            }
        }

        /// <summary>
        /// get/set - When this CacheItem will expire and be removed from cache.
        /// </summary>
        public DateTime? ExpiresOn
        {
            get { return _ExpiresOn; }
            set { _ExpiresOn = value; }
        }

        /// <summary>
        /// get/set - Whether this CacheItem ExpiresOn should be refreshed after this item has received a request.
        /// </summary>
        public bool RenewOnRequest
        {
            get { return _RenewOnRequest; }
            set { _RenewOnRequest = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CacheItem object.
        /// Provides a way to initialize a new CacheItem from an old one and apply a new value.
        /// Useful when Type T is a WeakReference.
        /// </summary>
        /// <param name="item">CacheItem object.</param>
        /// <param name="value">Value to cache.</param>
        internal CacheItem(ICacheItem item, T value)
        {
            _Key = item.Key;
            _CreatedDate = item.CreatedDate;
            _TimeToLive = item.TimeToLive;
            _ExpiresOn = item.ExpiresOn;
            _RenewOnRequest = item.RenewOnRequest;

            _Value = value;
        }

        /// <summary>
        /// Creates a new instance of a CacheItem object.
        /// </summary>
        /// <param name="key">Unique key to identify this item.</param>
        /// <param name="value">Value to cache.</param>
        public CacheItem(string key, T value)
        {
            _Key = key;
            _Value = value;
            _CreatedDate = DateTime.UtcNow;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Renew the ExpiresOn value if the TimeToLive is set.
        /// </summary>
        public void Renew()
        {
            if (_TimeToLive.HasValue)
                _ExpiresOn = Optimization.FastDateTime.UtcNow.Add(_TimeToLive.Value);
        }

        /// <summary>
        /// Renew the ExpiresOn value with the specified TimeToLive.
        /// </summary>
        /// <param name="timeToLive"></param>
        public void Renew(TimeSpan timeToLive)
        {
            Validation.Argument.Assert.IsNotNull(timeToLive, nameof(timeToLive));
            _TimeToLive = timeToLive;
            _ExpiresOn = Optimization.FastDateTime.UtcNow.Add(_TimeToLive.Value);
        }

        /// <summary>
        /// Renew the ExpiresOn value with the specified value.
        /// </summary>
        /// <param name="expiresOn"></param>
        public void Renew(DateTime expiresOn)
        {
            Validation.Argument.Assert.IsNotNull(expiresOn, nameof(expiresOn));
            _ExpiresOn = expiresOn;
        }

        /// <summary>
        /// Dipose the CacheItem.
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
