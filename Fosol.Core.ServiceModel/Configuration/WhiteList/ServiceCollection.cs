using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.WhiteList
{
    /// <summary>
    /// Collection of Services
    /// </summary>
    [ConfigurationCollection(typeof(ServiceCollection),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "service")]
    public sealed class ServiceCollection 
        : Fosol.Core.Configuration.ConfigurationElementCollection<ServiceElement>
    {
        #region Properties
        /// <summary>
        /// get/set - Return the ServiceElement at the specified position
        /// </summary>
        /// <param name="index">Position in collection</param>
        /// <returns>ServiceElement</returns>
        public new ServiceElement this[int index]
        {
            get { return (ServiceElement)BaseGet(index); }
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
        /// Returns the key of the ServiceElement
        /// </summary>
        /// <param name="element">ServiceElement object</param>
        /// <returns>Name value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Name;
        }

        /// <summary>
        /// Adds a new ServiceElement to the collection
        /// </summary>
        /// <param name="element">ServiceElement to add</param>
        public new void Add(ServiceElement element)
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
        /// Removes a ServiceElement from the collection
        /// </summary>
        /// <param name="element">The ServiceElement to remove</param>
        public new void Remove(ServiceElement element)
        {
            BaseRemove(element.Name);
        }

        /// <summary>
        /// Removes a ServiceElement from the collection with the key name
        /// </summary>
        /// <param name="host">Key name to remove</param>
        public new void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Removes a ServiceElement from the collection at the specified index
        /// </summary>
        /// <param name="index">Position of ServiceElement to remove</param>
        public new void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        #endregion
    }
}
