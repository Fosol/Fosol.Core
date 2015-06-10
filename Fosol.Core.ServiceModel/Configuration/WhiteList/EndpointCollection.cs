using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.WhiteList
{
    /// <summary>
    /// Collection of endpoints for a specific service.
    /// </summary>
    [ConfigurationCollection(typeof(EndpointCollection),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "endpoint")]
    public sealed class EndpointCollection 
        : Fosol.Core.Configuration.ConfigurationElementCollection<EndpointElement>
    {
        #region Properties
        /// <summary>
        /// get/set - Return the EndpointElement at the specified position
        /// </summary>
        /// <param name="index">Position in collection</param>
        /// <returns>EndpointElement</returns>
        public new EndpointElement this[int index]
        {
            get { return (EndpointElement)BaseGet(index); }
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
        /// Returns the key of the EndpointElement
        /// </summary>
        /// <param name="element">EndpointElement object</param>
        /// <returns>Name value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EndpointElement)element).Name;
        }

        /// <summary>
        /// Adds a new EndpointElement to the collection
        /// </summary>
        /// <param name="element">EndpointElement to add</param>
        public new void Add(EndpointElement element)
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
        /// Removes a EndpointElement from the collection
        /// </summary>
        /// <param name="element">The EndpointElement to remove</param>
        public new void Remove(EndpointElement element)
        {
            BaseRemove(element.Name);
        }

        /// <summary>
        /// Removes a EndpointElement from the collection with the key name
        /// </summary>
        /// <param name="host">Key name to remove</param>
        public new void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Removes a EndpointElement from the collection at the specified index
        /// </summary>
        /// <param name="index">Position of EndpointElement to remove</param>
        public new void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        #endregion
    }
}

