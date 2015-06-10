using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Caching
{
    /// <summary>
    /// CacheItemPriority enum, provides a number of options for CacheItem priority.
    /// </summary>
    enum CacheItemPriority
    {
        Low = 1,
        BelowNormal,
        Normal,
        AboveNormal,
        High,
        NotRemovable,
        Default = Normal
    }
}
