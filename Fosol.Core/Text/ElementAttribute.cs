using System;

namespace Fosol.Core.Text
{

    /// <summary>
    /// ElementAttribute sealed class, provides the Element name value within the special syntax {[name]}
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ElementAttribute
        : Attribute
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The name of the keyword.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - Controls whether this keyword will override a prior keyword with the same name.
        /// </summary>
        public bool Override { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a FormatElementAttribute.
        /// </summary>
        /// <param name="name">Unique name to identify the FormatElement.</param>
        /// <param name="overrideKeyword">Whether this keyword should override another with the same name.</param>
        public ElementAttribute(string name, bool overrideKeyword = false)
        {
            this.Name = name;
            this.Override = overrideKeyword;
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion
    }
}
