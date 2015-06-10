using Fosol.Core.Configuration;
using System.Collections.Specialized;
using System.Configuration;

namespace Fosol.Core.ServiceModel.Configuration.HttpHeader
{
    /// <summary>
    /// Collection of HTTP headers to add to the response.
    /// </summary>
    [ConfigurationCollection(typeof(HeaderCollection),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "header")]
    public sealed class HeaderCollection : ConfigurationElementCollection<HeaderElement>
    {
        #region Properties
        /// <summary>
        /// get/set - Return the HeaderElement at the specified position
        /// </summary>
        /// <param name="index">Position in collection</param>
        /// <returns>HeaderElement</returns>
        public new HeaderElement this[int index]
        {
            get { return (HeaderElement)BaseGet(index); }
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
        /// Returns the key of the HeaderElement
        /// </summary>
        /// <param name="element">HeaderElement object</param>
        /// <returns>Name value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HeaderElement)element).Name;
        }

        /// <summary>
        /// Adds a new HeaderElement to the collection
        /// </summary>
        /// <param name="element">HeaderElement to add</param>
        public new void Add(HeaderElement element)
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
        /// Removes a HeaderElement from the collection
        /// </summary>
        /// <param name="element">The HeaderElement to remove</param>
        public new void Remove(HeaderElement element)
        {
            BaseRemove(element.Name);
        }

        /// <summary>
        /// Removes a HeaderElement from the collection with the key name
        /// </summary>
        /// <param name="host">Key name to remove</param>
        public new void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Removes a HeaderElement from the collection at the specified index
        /// </summary>
        /// <param name="index">Position of HeaderElement to remove</param>
        public new void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Casts the HeaderCollection into a NameValueCollection.
        /// </summary>
        /// <param name="element">HeaderCollection object to cast.</param>
        /// <returns>NameValueCollection object.</returns>
        public static implicit operator NameValueCollection(HeaderCollection element)
        {
            NameValueCollection headers = new NameValueCollection();

            foreach (HeaderElement header in element)
                headers.Add(header.Name, header.Value);

            return headers;
        }
        #endregion
    }
}
