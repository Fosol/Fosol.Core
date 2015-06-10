using System;

namespace Fosol.Core.Caching
{
    /// <summary>
    /// ICacheItem interface, provides base interface for CacheItem objects.
    /// </summary>
    /// <typeparam name="T">Type of object being cached.</typeparam>
    interface ICacheItem<T>
        : ICacheItem
    {
        T Value { get; }
    }
}
