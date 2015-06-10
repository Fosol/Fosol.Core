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
        /// Preforms foreach on the enumerable source.
        /// </summary>
        /// <typeparam name="T">Type of object in source.</typeparam>
        /// <param name="source">Source enumerable object to iterate through.</param>
        /// <param name="action">Action to perform on each item within the source.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}
