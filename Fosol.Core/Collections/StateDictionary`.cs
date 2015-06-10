using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Collections
{
    /// <summary>
    /// StateDictionary generic class of type T, provides a dictionary for storing state.
    /// </summary>
    /// <typeparam name="T">Type of values to be stored within this StateDictionary.</typeparam>
    public class StateDictionary<T>
        : Dictionary<string, T>
    {
        #region Proerties
        /// <summary>
        /// get/set - The value for the specified key.
        /// </summary>
        /// <param name="key">Unique name to identify the state infor
        public new T this[string key]
        {
            get
            {
                if (this.ContainsKey(key))
                    return base[key];

                return default(T);
            }
            set
            {
                base[key] = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Copies the items in this StateDictionary into the destination dictionary.
        /// </summary>
        /// <param name="destination">Destination dictionary.</param>
        public void CopyTo(IDictionary<string, T> destination)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(destination, nameof(destination));
            foreach (string key in this.Keys)
            {
                destination[key] = this[key];
            }
        }
        #endregion
    }
}
