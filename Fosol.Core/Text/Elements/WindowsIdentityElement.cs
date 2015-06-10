using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Principal;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// WindowsIdentityElement sealed class, provides a thread Windows identity information (username).
    /// </summary>
    [Element("windowsIdentity")]
    public sealed class WindowsIdentityElement
        : StaticElement
    {
        #region Variables
        /// <summary>
        /// Valid parameter options.
        /// </summary>
        public enum Parameter
        {
            /// <summary>
            /// bool - Whether to include the username.
            /// </summary>
            Username = 0,
            /// <summary>
            /// bool - Whether to include the domain.
            /// </summary>
            Domain = 1
        }
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Whether to show the username.
        /// </summary>
        [DefaultValue(true)]
        [ElementProperty("username")]
        public bool ShowUsername { get; set; }

        /// <summary>
        /// get/set - Whether to show the domain.
        /// </summary>
        [DefaultValue(true)]
        [ElementProperty("domain")]
        public bool ShowDomain { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a WindowsIndentityElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public WindowsIdentityElement(StringDictionary attributes)
            : base(attributes)
        {
            var current_id = WindowsIdentity.GetCurrent();
            if (current_id != null)
            {
                if (this.ShowUsername)
                {
                    // Include username and domain information.
                    if (this.ShowDomain)
                        this.Text = current_id.Name;
                    // Only include username.
                    else
                    {
                        var i = current_id.Name.LastIndexOf('\\');
                        if (i >= 0)
                            this.Text = current_id.Name.Substring(i + 1);
                        else
                            this.Text = current_id.Name;
                    }
                }
                else
                {
                    // Only include the domain.
                    if (this.ShowDomain)
                    {
                        var i = current_id.Name.IndexOf('\\');
                        if (i >= 0)
                            this.Text = current_id.Name.Substring(0, i);
                        else
                            this.Text = current_id.Name;
                    }
                }
            }
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
