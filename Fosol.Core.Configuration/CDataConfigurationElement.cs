using System;
using System.Configuration;
using System.Reflection;

namespace Fosol.Core.Configuration
{
    /// <summary>
    /// CDataConfigurationElement class provides a way to access text data between ConfigurationElement nodes.
    /// </summary>
    public class CDataConfigurationElement
        : ConfigurationElement
    {
        #region Variables
        private readonly string _PropertyName;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a CDataConfigurationElement class.
        /// </summary>
        public CDataConfigurationElement()
        {
            var properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                var attributes = GetAttributes<ConfigurationPropertyAttribute>(property);
                var cdata_attributes = GetAttributes<Attributes.CDataConfigurationPropertyAttribute>(property);

                if (attributes == null || attributes.Length == 0)
                    throw new ConfigurationErrorsException(String.Format(Resources.Multilingual.CDataConfigurationElement_Attribute_Missing, nameof(ConfigurationPropertyAttribute)));
                else if (cdata_attributes == null || cdata_attributes.Length == 0)
                    throw new ConfigurationErrorsException(String.Format(Resources.Multilingual.CDataConfigurationElement_Attribute_Missing, nameof(Attributes.CDataConfigurationPropertyAttribute)));
                else if (cdata_attributes.Length > 1)
                    throw new ConfigurationErrorsException(String.Format(Resources.Multilingual.CDataConfigurationElement_Attribute_Multiple, nameof(Attributes.CDataConfigurationPropertyAttribute)));
                else if (!(property.PropertyType == typeof(string)))
                    throw new ConfigurationErrorsException(String.Format(Resources.Multilingual.CDataConfigurationElement_InvalidType, nameof(CDataConfigurationElement)));
                else
                {
                    _PropertyName = attributes[0].Name;
                    break;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the value of the requested attribute.
        /// </summary>
        /// <typeparam name="T">Type of attribute.</typeparam>
        /// <param name="propertyInfo">PropertyInfo object.</param>
        /// <returns>Object of the specified type.</returns>
        private T[] GetAttributes<T>(PropertyInfo propertyInfo)
            where T : Attribute
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(T), true);
            return Array.ConvertAll<object, T>(attributes, (o) => { return o as T; });
        }

        /// <summary>
        /// Serialize the element.
        /// </summary>
        /// <param name="writer">XmlWriter object.</param>
        /// <param name="serializeCollectionKey">If true it will serialize the collection keys.</param>
        /// <returns>True if the object was serialized.</returns>
        protected override bool SerializeElement(System.Xml.XmlWriter writer, bool serializeCollectionKey)
        {
            if (string.IsNullOrEmpty(_PropertyName))
                return base.SerializeElement(writer, serializeCollectionKey);

            foreach (ConfigurationProperty property in Properties)
            {
                var name = property.Name;
                var converter = property.Converter;
                var value = converter.ConvertToString(base[name]);

                if (name == _PropertyName)
                    writer.WriteCData(value);
                else
                    writer.WriteAttributeString(name, value);
            }

            return true;
        }

        /// <summary>
        /// Deserialize the element.
        /// </summary>
        /// <param name="reader">XmlReader object.</param>
        /// <param name="serializeCollectionKey">Itrue it will also serialize the collection keys.</param>
        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            if (string.IsNullOrEmpty(_PropertyName))
                base.DeserializeElement(reader, serializeCollectionKey);
            else
            {
                foreach (ConfigurationProperty property in Properties)
                {
                    var name = property.Name;
                    if (name == _PropertyName)
                        base[name] = reader.ReadString().Trim();
                    else
                        base[name] = reader.GetAttribute(name);
                }
                reader.ReadEndElement();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
