using System;
using System.Collections.Generic;
using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.HttpHeader
{
    /// <summary>
    /// An HTTP header to add to the response.
    /// </summary>
    public sealed class HeaderElement : ConfigurationElement
    {
        #region Properties
        /// <summary>
        /// get/set - The HTTP header name.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - The HTTP header value.
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Provides a unique HashCode for this HeaderElement.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Provides a way to compare HeaderElement objects by their Name value.
        /// </summary>
        public class HeaderComparer : IEqualityComparer<HeaderElement>
        {
            #region Methods
            public bool Equals(HeaderElement a, HeaderElement b)
            {
                if (object.ReferenceEquals(a, b)) return true;

                if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;

                return a.Name == b.Name;
            }

            public int GetHashCode(HeaderElement obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }
        #endregion
    }
}
