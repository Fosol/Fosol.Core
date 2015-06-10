using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Configuration
{
    /// <summary>
    /// TypeConfigurationElement generic class of type T, provides a way to include an object type as a configurable option.
    /// </summary>
    public class TypeConfigurationElement<T>
        : TypeConfigurationElement
        where T : class
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TypeElement object of type T.
        /// </summary>
        public TypeConfigurationElement()
            : base(typeof(T))
        {
        }
        #endregion

        #region Methods
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
