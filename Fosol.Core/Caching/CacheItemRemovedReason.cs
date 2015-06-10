using System;

namespace Fosol.Core.Caching
{
    /// <summary>
    /// CacheItemRemovedReason enum, provides a number of options for removing cached items.
    /// </summary>
    enum CacheItemRemovedReason
    {
        Removed = 1,
        Expired,
        Underused,
        DependancyChanged
    }
}
