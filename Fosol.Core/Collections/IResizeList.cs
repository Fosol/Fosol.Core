using System;
using System.Collections;
using System.Collections.Generic;

namespace Fosol.Core.Collections
{
    /// <summary>
    /// ResizeList interface provides a generic list which can be resized to control how much internal memory is being allocated for the internal array.
    /// </summary>
    /// <typeparam name="T">Type of elements within the list.</typeparam>
    public interface IResizeList
        : IList, ICollection, IEnumerable
    {
        #region Properties
        /// <summary>
        /// get/set - The current capacity of this list.  If you are going to add a large number of items you may want to change the capacity first.
        /// </summary>
        int Capacity { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add the collection of items to this list.
        /// </summary>
        /// <param name="items">Items to add to the list.</param>
        void AddRange(IEnumerable items);

        /// <summary>
        /// Insert the collection into this list.
        /// </summary>
        /// <param name="index">Index position where the new items will begin to be inserted.</param>
        /// <param name="items">Collection of items to be inserted.</param>
        void InsertRange(int index, IEnumerable items);

        /// <summary>
        /// Remove a range of items from this list.
        /// </summary>
        /// <param name="index">Index position to begin removing items.</param>
        /// <param name="count">Number of items to remove.</param>
        void RemoveRange(int index, int count);
        #endregion
    }
}
