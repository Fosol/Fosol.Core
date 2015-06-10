using Fosol.Core.Extensions.Types;
using System;

namespace Fosol.Core.Xml.Attributes
{
    /// <summary>
    /// XmlSchemaDataTypeAttribute class, provides the XML schema datatype name value in the correct case.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class XmlSchemaDataTypeAttribute
        : Attribute
    {
        #region Variables
        public const string W3OrgXmlSchemaURI = "http://www.w3.org/2001/XMLSchema#";
        #endregion

        #region Properties
        /// <summary>
        /// get - The name of the datatype in the correct case.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// get - Uri to this XML schema datatype.
        /// </summary>
        public Uri Uri { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a XmlSchemaDataTypeAttribute.
        /// </summary>
        /// <param name="name">The name of the datatype in the correct case.</param>
        internal XmlSchemaDataTypeAttribute(string name)
            : this(new Uri(XmlSchemaDataTypeAttribute.W3OrgXmlSchemaURI + name), name)
        {

        }

        /// <summary>
        /// Creates a new instances of a XmlSchemaDataTypeAttribute.
        /// </summary>
        /// <param name="uri">URI to the XML schema datatype.</param>
        /// <param name="name">The name of the datatype in the correct case.</param>
        public XmlSchemaDataTypeAttribute(Uri uri, string name)
        {
            Validation.Argument.Assert.IsNotNull(uri, nameof(uri));
            Validation.Argument.Assert.IsNotNullOrWhitespace(name, nameof(name));
            this.Uri = uri;
            this.Name = name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the DataTypeAttribute for the specified DataType enum value.
        /// </summary>
        /// <param name="dataType">DataType enum value.</param>
        /// <returns>DataTypeAttribute object.</returns>
        public static XmlSchemaDataTypeAttribute Get(XmlSchemaDataType dataType)
        {
            return dataType.GetType().GetCustomAttribute<XmlSchemaDataTypeAttribute>(false);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
