using System;
using System.Collections.Specialized;

namespace Fosol.Core.Text
{
    /// <summary>
    /// DynamicElement abstract class, provides a way to dynamically generate every time it is called.
    /// </summary>
    public abstract class DynamicElement
        : Element
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DynamicElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary of attributes to include with this keyword.</param>
        public DynamicElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the dynamic value of the keyword.
        /// </summary>
        /// <param name="data">Object containing data for dynamic keywords.</param>
        /// <returns>The dynamic value of the keyword.</returns>
        public abstract string Render(object data);
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
