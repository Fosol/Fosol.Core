using Fosol.Core.Configuration;
using System.Configuration;
using System.Collections.Generic;
using Fosol.Core.Configuration.Attributes;

namespace Fosol.Core.ServiceModel.Configuration.WhiteList
{
    /// <summary>
    /// An IP address to add to the response.
    /// </summary>
    public sealed class IpAddressElement 
        : CDataConfigurationElement
    {
        #region Properties
        /// <summary>
        /// get/set - The IP address value.
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true, IsKey = true)]
        [CDataConfigurationProperty]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Provides a unique HashCode for this IpAddressElement.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Provides a way to compare IpAddressElement objects by their IP value.
        /// </summary>
        public class IpAddressComparer : IEqualityComparer<IpAddressElement>
        {
            #region Methods
            public bool Equals(IpAddressElement a, IpAddressElement b)
            {
                if (object.ReferenceEquals(a, b)) return true;

                if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;

                return a.Value == b.Value;
            }

            public int GetHashCode(IpAddressElement obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }
        #endregion
    }
}

