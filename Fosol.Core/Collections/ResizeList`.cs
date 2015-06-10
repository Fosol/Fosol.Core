using System;
using System.Collections;
using System.Collections.Generic;

namespace Fosol.Core.Collections
{
    /// <summary>
    /// ResizeList class of type T provides a basic generic list which can be resized to control how much internal memory is being allocated for the internal array.
    /// </summary>
    /// <typeparam name="T">Type of elements within the list.</typeparam>
    [Serializable]
    public class ResizeList<T> 
        : IResizeList<T>
    {
        #region Variables
        private const int _DefaultCapacity = 4;
        private Object _Lock;
        private T[] _Items;
        private int _Count;
        private static T[] _EmptyArray = new T[0];
        private int _Version;

        /// <summary>
        /// CalculateCapacityDelegate controls how to resize the internal array when the limit is reached.
        /// By default it will double the size of the current internal array.
        /// </summary>
        /// <param name="capacity">The current capacity of the list.</param>
        /// <returns>The new capacity that the list will be updated to.</returns>
        public delegate int CalculateCapacityDelegate(int capacity);
        private CalculateCapacityDelegate _CalculateCapacity = c => { return c * 2; };
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The item at the specified index position.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "index" must be a valid index position within this list.</exception>
        /// <param name="index">Index position within the list.</param>
        /// <returns>The item at the specified index position.</returns>
        public T this[int index]
        {
            get
            {
                Fosol.Core.Validation.Argument.Assert.IsInRange(index, 0, _Count - 1, nameof(index));
                return _Items[index];
            }

            set
            {
                Fosol.Core.Validation.Argument.Assert.IsInRange(index, 0, _Count - 1, nameof(index));
                _Items[index] = value;
                _Version++;
            }
        }

        /// <summary>
        /// get/set - The item at the specified index position.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "this" value must be of type T.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "index" must be a valid index position within this list.</exception>
        /// <param name="index">Index position within the list.</param>
        /// <returns>The item at the specified index position.</returns>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                Fosol.Core.Validation.Property.Assert.IsValid(IsCompatibleObject(value), nameof(value), Resources.Multilingual.Collections_ResizeList_Property_CompatibleType);
                this[index] = (T)value;
            }
        }

        /// <summary>
        /// get - The number of items within this list.
        /// </summary>
        public int Count
        {
            get
            {
                return _Count;
            }
        }

        /// <summary>
        /// get - Whether this list is readonly.
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// get - Whether this list is readonly.
        /// </summary>
        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// get/set - The current capacity of this list.  If you are going to add a large number of items you may want to change the capacity first.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "Capacity" must be greater than or equal to the current size.</exception>
        public int Capacity
        {
            get
            {
                return _Items.Length;
            }
            set
            {
                if (value != _Items.Length)
                {
                    Validation.Property.Assert.IsMinimum(value, _Count, nameof(this.Capacity));
                    if (value > 0)
                    {
                        var new_items = new T[value];
                        if (_Count > 0)
                            Array.Copy(_Items, 0, new_items, 0, _Count);
                        _Items = new_items;
                    }
                    else
                    {
                        _Items = _EmptyArray;
                    }
                }
            }
        }

        /// <summary>
        /// get - Whether the underlining collection is fixed size.
        /// </summary>
        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// get - Synchronization root for this object.
        /// </summary>
        object ICollection.SyncRoot
        {
            get
            {
                if (_Lock == null)
                    System.Threading.Interlocked.CompareExchange(ref _Lock, new Object(), null);

                return _Lock;
            }
        }

        /// <summary>
        /// get - Whether the collection is threadsafe.
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ResizeList of type T.
        /// </summary>
        public ResizeList()
        {
            _Items = _EmptyArray;
        }

        /// <summary>
        /// Creates a new instance of ResizeList of type T.
        /// </summary>
        /// <param name="capacity">Default starting capacity of this list.</param>
        public ResizeList(int capacity)
        {
            Fosol.Core.Validation.Argument.Assert.IsMinimum(capacity, 0, nameof(capacity));
            _Items = new T[capacity];
        }

        /// <summary>
        /// Creates a new instance of ResizeList of type T.
        /// </summary>
        /// <param name="defaultCapacity">Default starting capacity of this list.</param>
        /// <param name="capacity">Function to calculate capacity.</param>
        public ResizeList(int defaultCapacity, CalculateCapacityDelegate capacity)
            : this(defaultCapacity)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(capacity, nameof(capacity));
            _CalculateCapacity = capacity;
        }

        /// <summary>
        /// Creates a new instance of ResizeList of type T.
        /// </summary>
        /// <param name="items">Initialize this list with these items.</param>
        public ResizeList(IEnumerable<T> items)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(items, nameof(items));
            var c = items as ICollection<T>;

            if (c != null)
            {
                var count = c.Count;
                _Items = new T[count];
                c.CopyTo(_Items, 0);
                _Count = count;
            }
            else
            {
                // The items must be lazy loaded into this list.
                _Count = 0;
                _Items = new T[_DefaultCapacity];
                using (var en = items.GetEnumerator())
                {
                    while (en.MoveNext())
                        this.Add(en.Current);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ensures the current capacity can handle the update.
        /// If the current capacity is less than the specified minimum it will resize the internal array.
        /// </summary>
        /// <param name="min">Minimum capacity.</param>
        private void EnsureCapacity(int min)
        {
            if (_Items.Length < min)
            {
                var new_capacity = _Items.Length == 0 ? _DefaultCapacity : _CalculateCapacity(_Items.Length);
                if (new_capacity < min)
                    new_capacity = min;
                this.Capacity = new_capacity;
            }
        }

        /// <summary>
        /// Checks whether the oject is compatible with this collection type.
        /// </summary>
        /// <param name="value">Object to test.</param>
        /// <returns>True if compatible.</returns>
        private static bool IsCompatibleObject(object value)
        {
            if ((value is T) || (value == null && !typeof(T).IsValueType))
                return true;
            return false;
        }

        /// <summary>
        /// Add the item to this list.
        /// </summary>
        /// <param name="item">Item object to add.</param>
        public void Add(T item)
        {
            if (_Count == _Items.Length)
                EnsureCapacity(_Count + 1);
            _Items[_Count++] = item;
            _Version++;
        }

        /// <summary>
        /// Add the item to this list.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a compatible type.</exception>
        /// <param name="value">Item object to add.</param>
        /// <returns>Index position of where it was added, or -1 if the item was not inserted into the collection.</returns>
        int IList.Add(object value)
        {
            Fosol.Core.Validation.Argument.Assert.IsValid(ResizeList<T>.IsCompatibleObject(value), nameof(value), Resources.Multilingual.Collections_ResizeList_Add_CompatibleType);
            this.Add((T)value);
            return this.Count - 1;
        }

        /// <summary>
        /// Add the items in the specified collection to this list.
        /// </summary>
        /// <param name="items">Collection of items to add.</param>
        public void AddRange(IEnumerable<T> items)
        {
            InsertRange(_Count, items);
        }

        /// <summary>
        /// Add the items in the specified collection to this list.
        /// </summary>
        /// <param name="items">Collection of items to add.</param>
        void IResizeList.AddRange(IEnumerable items)
        {
            (this as IResizeList).InsertRange(_Count, items);
        }

        /// <summary>
        /// Clear the items from this list.
        /// </summary>
        public void Clear()
        {
            if (_Count > 0)
            {
                Array.Clear(_Items, 0, _Count);
                _Count = 0;
            }
            _Version++;
        }

        /// <summary>
        /// Determine if this list contains the specified item.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>True if the item exists.</returns>
        public bool Contains(T item)
        {
            return Contains(item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Determine if this list contains the specified item using the specified equality comparer.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <param name="comparer">Equality comparer object.</param>
        /// <returns>True if the item exists.</returns>
        public bool Contains(T item, IEqualityComparer<T> comparer)
        {
            if (item == null)
            {
                for (var i = 0; i < _Count; i++)
                {
                    if (_Items[i] == null)
                        return true;
                }
                return false;
            }

            for (var i = 0; i < _Count; i++)
            {
                if (comparer.Equals(_Items[i], item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determine if this list contains the specified item.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IList.Contains(object value)
        {
            if (!IsCompatibleObject(value))
                return false;
            return this.Contains((T)value);
        }

        /// <summary>
        /// Copy the items in this list to the specified array.
        /// Start copying the items into the destination array at the specified index.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "array" must not be a multi-dimensional array.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "array" must not be null.</exception>
        /// <exception cref="System.ArrayTypeMismatchException">Parameter "array" is not of the correct type.</exception>
        /// <param name="array">Array object to copy items into.</param>
        /// <param name="arrayIndex">Index position within the destination array to begin copying.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(array, nameof(array));
            Fosol.Core.Validation.Argument.Assert.IsValid(array.Rank == 1, nameof(array), Resources.Multilingual.Collections_ResizeList_CopyTo);

            try
            {
                Array.Copy(_Items, 0, array, arrayIndex, _Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw;
            }
        }

        /// <summary>
        /// Copy the items in this list to the specified array.
        /// Start copying items in this list at the specified index position.
        /// Copy the items into the destination array at the specified index position.
        /// Only copy the specified number of items.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "array" must not be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameters "index", "arrayIndex" and "count" must all be valid index positions within their respective arrays.</exception>
        /// <param name="index">Index position within this list to begin copying.</param>
        /// <param name="array">Destination array to copy to.</param>
        /// <param name="arrayIndex">Index position to start copying into the destination array.</param>
        /// <param name="count">Number of items to copy from this list.</param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            Fosol.Core.Validation.Argument.Assert.IsInRange(index, 0, this.Count - 1, nameof(index));
            Fosol.Core.Validation.Argument.Assert.IsNotNull(array, nameof(array));
            Fosol.Core.Validation.Argument.Assert.IsInRange(arrayIndex, 0, array.Length - 1, nameof(arrayIndex));
            Fosol.Core.Validation.Argument.Assert.IsInRange(count, 0, this.Count, nameof(count));
            Array.Copy(_Items, index, array, arrayIndex, count);
        }

        /// <summary>
        /// Copy the items in this list to the specified array.
        /// Start copying the items into the destination array at the specified index.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "array" must not be a multi-dimensional array.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "array" must not be null.</exception>
        /// <exception cref="System.ArrayTypeMismatchException">Parameter "array" is not of the correct type.</exception>
        /// <param name="array">Destination array to copy to.</param>
        /// <param name="index">Index position to start copying into the destination array.</param>
        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(array, nameof(array));
            Fosol.Core.Validation.Argument.Assert.IsValid(array.Rank == 1, nameof(array), Resources.Multilingual.Collections_ResizeList_CopyTo);

            try
            {
                Array.Copy(_Items, 0, array, index, _Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw;
            }
        }

        /// <summary>
        /// Return the enumerator for this list.
        /// </summary>
        /// <returns>New instance of an Enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Return a new instance of Enumerator.
        /// </summary>
        /// <returns>New instance of Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns the index position of the specified item within this list.
        /// </summary>
        /// <param name="item">Item object to look for.</param>
        /// <returns>Index position of the item or -1 if not found.</returns>
        public int IndexOf(T item)
        {
            return Array.IndexOf(_Items, item, 0, _Count);
        }

        /// <summary>
        /// Returns the index position of the specified item within this list.
        /// </summary>
        /// <param name="value">Item object to look for.</param>
        /// <returns>Index position of the item, or -1 if not found.</returns>
        int IList.IndexOf(object value)
        {
            if (!IsCompatibleObject(value))
                return -1;
            return this.IndexOf((T)value);
        }

        /// <summary>
        /// Insert the item at the specified index position within this list.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "index" must be a valid index position within the list.</exception>
        /// <param name="index">Index position within list.</param>
        /// <param name="item">Item to insert.</param>
        public void Insert(int index, T item)
        {
            Fosol.Core.Validation.Argument.Assert.IsInRange(index, 0, _Items.Length, nameof(index));
            if (_Count == _Items.Length)
                EnsureCapacity(_Count + 1);
            if (index < _Count)
            {
                // Move all the current items over one to make room for the new item.
                Array.Copy(_Items, index, _Items, index + 1, _Count - index);
            }
            _Items[index] = item;
            _Count++;
            _Version++;
        }

        /// <summary>
        /// Insert the item at the specified index position within this list.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be a compatible type.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "index" must be a valid index position within the list.</exception>
        /// <param name="index">Index position within list.</param>
        /// <param name="item">Item to insert.</param>
        public void Insert(int index, object value)
        {
            Fosol.Core.Validation.Argument.Assert.IsValid(IsCompatibleObject(value), nameof(value), Resources.Multilingual.Collections_ResizeList_Insert_CompatibleType);
            this.Insert(index, (T)value);
        }

        /// <summary>
        /// Insert the collection into this list.
        /// </summary>
        /// <param name="index">Index position where the new items will begin to be inserted.</param>
        /// <param name="items">Collection of items to be inserted.</param>
        public void InsertRange(int index, IEnumerable<T> items)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(items, nameof(items));
            Fosol.Core.Validation.Argument.Assert.IsInRange(index, 0, _Items.Length, nameof(index));
            var c = items as ICollection<T>;
            if (c != null)
            {
                var count = c.Count;
                if (count > 0)
                {
                    EnsureCapacity(_Count + count);
                    if (index < _Count)
                    {
                        // Move the current items over to make room for the new items being inserted.
                        Array.Copy(_Items, index, _Items, index + count, _Count - index);
                    }

                    // Deal with the situation where it's inserting itself into itself.
                    if (this == c)
                    {
                        // Copy first part of _Items to insert location.
                        Array.Copy(_Items, 0, _Items, index, index);
                        // Copy last part of _Items back to inserted location.
                        Array.Copy(_Items, index + count, _Items, index * 2, _Count - index);
                    }
                    else
                    {
                        var temp = new T[count];
                        c.CopyTo(temp, 0);
                        temp.CopyTo(_Items, index);
                    }
                }
            }
            else
            {
                using (IEnumerator<T> en = items.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
            _Version++;
        }

        /// <summary>
        /// Insert the collection into this list.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "items" must be contain compatible item types.</exception>
        /// <param name="index">Index position where the new items will begin to be inserted.</param>
        /// <param name="items">Collection of items to be inserted.</param>
        void IResizeList.InsertRange(int index, IEnumerable items)
        {
            var en = items.GetEnumerator();

            while (en.MoveNext())
            {
                Insert(index++, en.Current);
            }
        }

        /// <summary>
        /// Remove the item from this list if found.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if the item was removed.</returns>
        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove the item from this list if found.
        /// </summary>
        /// <param name="value">Item to remove.</param>
        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
            {
                this.Remove((T)value);
            }
        }

        /// <summary>
        /// Remove the item at the specified index position.
        /// </summary>
        /// <param name="index">Index position within this list.</param>
        public void RemoveAt(int index)
        {
            Fosol.Core.Validation.Argument.Assert.IsInRange(index, 0, _Items.Length - 1, nameof(index));
            _Count--;
            if (index < _Count)
                Array.Copy(_Items, index + 1, _Items, index, _Count - index);
            _Items[_Count] = default(T);
            _Version++;
        }

        /// <summary>
        /// Remove a range of items from this list.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameters "index" and "count" must be valid index positions or number of items to be removed.</exception>
        /// <param name="index">Index position to begin removing items.</param>
        /// <param name="count">Number of items to remove.</param>
        public void RemoveRange(int index, int count)
        {
            Fosol.Core.Validation.Argument.Assert.IsInRange(index, 0, this.Count - 1, nameof(index));
            Fosol.Core.Validation.Argument.Assert.IsInRange(count, 0, this.Count - index, nameof(count));
            if (count > 0)
            {
                var i = _Count;
                _Count -= count;
                if (index < _Count)
                    Array.Copy(_Items, index + count, _Items, index, _Count - index);
                Array.Clear(_Items, _Count, count);
                _Version++;
            }
        }
        #endregion

        #region Structs
        /// <summary>
        /// Enumerator struct for the ResizeList of type T.
        /// </summary>
        [Serializable]
        public struct Enumerator 
            : IEnumerator<T>
        {
            #region Variables
            private ResizeList<T> _Items;
            private int _Index;
            private int _Version;
            private T _Current;
            #endregion

            #region Properties
            /// <summary>
            /// get - The item at the current location within the enumerator.
            /// </summary>
            public T Current
            {
                get
                {
                    Fosol.Core.Validation.Value.Assert.IsInRange(_Index, 1, _Items.Count, Resources.Multilingual.Collections_ResizeList_Enumerator_Current);
                    return _Current;
                }
            }

            /// <summary>
            /// get - The item at the current lcoation within the enumerator.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
            #endregion

            #region Constructors
            /// <summary>
            /// Creates new instance of Enumerator object.
            /// </summary>
            /// <param name="items">Initializes this with the items.</param>
            internal Enumerator(ResizeList<T> items)
            {
                _Items = items;
                _Index = 0;
                _Version = items._Version;
                _Current = default(T);
            }
            #endregion

            #region Methods
            /// <summary>
            /// Dispose of this Enumerator.
            /// </summary>
            public void Dispose()
            {

            }

            /// <summary>
            /// Move to the next item within this Enumerator.
            /// </summary>
            /// <returns>True if successfully able to move to the next item.</returns>
            public bool MoveNext()
            {
                var temp = _Items;
                Fosol.Core.Validation.Value.Assert.IsTrue(_Version == temp._Version, Resources.Multilingual.Collections_ResizeList_Enumerator_MoveNext);

                if (_Index < temp.Count)
                {
                    _Current = temp._Items[_Index];
                    _Index++;
                    return true;
                }
                else
                {
                    _Current = default(T);
                    _Index++;
                    return false;
                }
            }

            /// <summary>
            /// Reset the Enumerator to before the start.
            /// </summary>
            public void Reset()
            {
                Fosol.Core.Validation.Value.Assert.IsTrue(_Version == _Items._Version, Resources.Multilingual.Collections_ResizeList_Enumerator_Reset);
                _Index = 0;
                _Current = default(T);
            }
            #endregion
        }
        #endregion
    }
}
