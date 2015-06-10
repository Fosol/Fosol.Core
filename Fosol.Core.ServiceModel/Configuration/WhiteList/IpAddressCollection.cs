using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.WhiteList
{
    /// <summary>
    /// Collection of IP Addresses 
    /// </summary>
    [ConfigurationCollection(typeof(IpAddressCollection),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "ipAddress")]
    public sealed class IpAddressCollection 
        : Fosol.Core.Configuration.ConfigurationElementCollection<IpAddressElement>
    {
        #region Properties
        /// <summary>
        /// get/set - Return the IpAddressElement at the specified position
        /// </summary>
        /// <param name="index">Position in collection</param>
        /// <returns>IpAddressElement</returns>
        public new IpAddressElement this[int index]
        {
            get { return (IpAddressElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// get - The collection type
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the key of the IpAddressElement
        /// </summary>
        /// <param name="element">IpAddressElement object</param>
        /// <returns>Name value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IpAddressElement)element).Value;
        }

        /// <summary>
        /// Adds a new IpAddressElement to the collection
        /// </summary>
        /// <param name="element">IpAddressElement to add</param>
        public new void Add(IpAddressElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public new void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Removes a IpAddressElement from the collection
        /// </summary>
        /// <param name="element">The IpAddressElement to remove</param>
        public new void Remove(IpAddressElement element)
        {
            BaseRemove(element.Value);
        }

        /// <summary>
        /// Removes a IpAddressElement from the collection with the key name
        /// </summary>
        /// <param name="host">Key name to remove</param>
        public new void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Removes a IpAddressElement from the collection at the specified index
        /// </summary>
        /// <param name="index">Position of IpAddressElement to remove</param>
        public new void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        #endregion
    }
}
