using System;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// TextElement sealed class, provdies a basic text value that may include parameters.
    /// </summary>
    [Element("text")]
    public sealed class TextElement
        : StaticElement
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TextElement object.
        /// </summary>
        /// <param name="text">Original string value that created this keyword.</param>
        public TextElement(string text)
            : base(text)
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
