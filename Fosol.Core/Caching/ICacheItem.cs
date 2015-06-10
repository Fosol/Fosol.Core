using System;

namespace Fosol.Core.Caching
{
    /// <summary>
    /// ICacheItem interface, provides a base interface for CacheItem objects.
    /// Only used internally.
    /// </summary>
    internal interface ICacheItem
    {
        DateTime CreatedDate { get; }
        void Dispose();
        DateTime? ExpiresOn { get; set; }
        string Key { get; }
        void Renew();
        void Renew(DateTime expiresOn);
        void Renew(TimeSpan timeToLive);
        bool RenewOnRequest { get; set; }
        TimeSpan? TimeToLive { get; set; }
    }
}
