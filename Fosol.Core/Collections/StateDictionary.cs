using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Collections
{
    /// <summary>
    /// StateDictionary class provides a dictionary for storing state.
    /// </summary>
    public class StateDictionary
        : Dictionary<string, object>
    {
        #region Proerties
        /// <summary>
        /// get/set - The value for the specified key.
        /// </summary>
        /// <param name="key">Unique name to identify the state information.</param>
        /// <returns>The value for the specified key.</returns>
        public new object this[string key]
        {
            get
            {
                if (this.ContainsKey(key))
                    return base[key];

                return default(object);
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
        public void CopyTo(IDictionary<string, object> destination)
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
