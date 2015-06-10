using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fosol.Core.Extensions.Collections
{
    /// <summary>
    /// CollectionExtensions static class, provides extension methods for Collection objects.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of values onto the end of the collection.
        /// </summary>
        /// <typeparam name="T">Type of objects in the collection.</typeparam>
        /// <param name="collection">Collection object to add to.</param>
        /// <param name="values">Values to add to the collection.</param>
        public static void AddRange<T>(this ICollection<T> collection, params T[] values)
        {
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }
    }
}
