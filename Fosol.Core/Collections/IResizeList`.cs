using System;
using System.Collections;
using System.Collections.Generic;

namespace Fosol.Core.Collections
{
    /// <summary>
    /// IResizeList of type T interface provides a generic list which can be resized to control how much internal memory is being allocated for the internal array.
    /// </summary>
    /// <typeparam name="T">Type of elements within the list.</typeparam>
    public interface IResizeList<T>
        : IList<T>, ICollection<T>, IEnumerable<T>, IResizeList
    {
        #region Methods
        /// <summary>
        /// Add the collection of items to this list.
        /// </summary>
        /// <param name="items">Items to add to the list.</param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Insert the collection into this list.
        /// </summary>
        /// <param name="index">Index position where the new items will begin to be inserted.</param>
        /// <param name="items">Collection of items to be inserted.</param>
        void InsertRange(int index, IEnumerable<T> items);
        #endregion
    }
}
