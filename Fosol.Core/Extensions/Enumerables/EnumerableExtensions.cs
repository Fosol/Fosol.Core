using System;
using System.Collections;
using System.Collections.Generic;

namespace Fosol.Core.Extensions.Enumerables
{
    /// <summary>
    /// EnumerableExtensions static class, provides extension methods for Enumerable objects.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Get the generic <see cref="Type"/> of the <see cref="IEnumerable{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The generic IEnumerable <see cref="Type"/>.</typeparam>
        /// <param name="enumerable">The enumerable object.</param>
        /// <returns>The generic item <see cref="Type"/>.</returns>
        public static Type GetGenericType<T>(this IEnumerable<T> enumerable)
        {
            return typeof(T);
        }

        /// <summary>
        /// Shortcut to perform a foreach on the items and call the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// Shortcut to add all the items into the destination collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="destination"></param>
        public static void AddTo<T>(this IEnumerable<T> items, ICollection<T> destination)
        {
            items.ForEach(i => destination.Add(i));
        }
    }
}
