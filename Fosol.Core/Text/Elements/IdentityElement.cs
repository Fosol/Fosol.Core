using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// IdentityElement sealed class, provides a thread identity information (name and authentication information).
    /// </summary>
    [Element("identity")]
    public sealed class IdentityElement
        : DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Value to separate identity values.
        /// </summary>
        [DefaultValue(":")]
        [ElementProperty("delimiter", new[] { "del", "d" })]
        public string Delimiter { get; set; }

        /// <summary>
        /// get/set - Whether to include the name.
        /// </summary>
        [DefaultValue(true)]
        [ElementProperty("name", new[] { "showname", "n" })]
        public bool ShowName { get; set; }

        /// <summary>
        /// get/set - Whether to include the authentication type.
        /// </summary>
        [DefaultValue(true)]
        [ElementProperty("type", new[] { "showauthtype", "t" })]
        public bool ShowAuthType { get; set; }

        /// <summary>
        /// get/set - Whether to include the is authenticated value.
        /// </summary>
        [DefaultValue(true)]
        [ElementProperty("auth", new[] { "showisauth", "a" })]
        public bool ShowIsAuthenticated { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a IdentityElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public IdentityElement(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns Thread identity information (name and authentication information).
        /// </summary>
        /// <param name="data">Information object containing data for the keyword.</param>
        /// <returns>Thread identity information (name and authentication information).</returns>
        public override string Render(object data)
        {
            var principal = System.Threading.Thread.CurrentPrincipal;

            if (principal != null)
            {
                var identity = principal.Identity;
                if (identity != null)
                {
                    var builder = new StringBuilder(string.Empty);

                    if (this.ShowIsAuthenticated)
                    {
                        if (identity.IsAuthenticated)
                            builder.Append("auth");
                        else
                            builder.Append("notauth");
                    }

                    if (this.ShowAuthType)
                    {
                        if (builder.Length > 0)
                            builder.Append(this.Delimiter);
                        builder.Append(identity.AuthenticationType);
                    }

                    if (this.ShowName)
                    {
                        if (builder.Length > 0)
                            builder.Append(this.Delimiter);
                        builder.Append(identity.Name);
                    }

                    return builder.ToString();
                }
            }

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
