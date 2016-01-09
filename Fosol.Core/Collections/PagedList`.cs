using Fosol.Core.Extensions.Enumerables;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Collections
{
    [JsonConverter(typeof(JsonConverters.PagedListConverter))]
    public sealed class PagedList<T> : IList<T>
    {
        #region Variables
        #endregion

        #region Properties
        public int Total { get; set; }

        public IList<T> Items { get; } = new List<T>();

        public T this[int index]
        {
            get
            {
                return this.Items[index];
            }

            set
            {
                this.Items[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Constructors
        public PagedList()
        {

        }

        public PagedList(IEnumerable<T> items, int total)
        {
            items.AddTo(this.Items);
            this.Total = total;
        }
        #endregion

        #region Methods
        #endregion

        public void Add(T item)
        {
            this.Items.Add(item);
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.Items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return this.Items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.Items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
    }
}
