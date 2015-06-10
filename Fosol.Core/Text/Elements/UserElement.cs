using System;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// UserElement sealed class, providees a way to render the current username.
    /// </summary>
    [Element("user")]
    public sealed class UserElement
        : DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Outputs the username and domain.
        /// </summary>
        /// <param name="data">Information object containing data for the keyword.</param>
        /// <returns>The username currently logged in.</returns>
        public override string Render(object data)
        {
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
