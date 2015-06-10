using Fosol.Core.Extensions.NameValueCollections;
using Fosol.Core.Extensions.PropertyInfos;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Fosol.Core.Text
{
    /// <summary>
    /// Element abstract class, provides a way to identify an individual part of text.
    /// </summary>
    public abstract class Element
    {
        #region Variables
        private string _Name;
        private readonly StringDictionary _Attributes = new StringDictionary();
        #endregion

        #region Properties
        /// <summary>
        /// get - The name value in the Element.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        /// <summary>
        /// get - A dictionary of attributes for this keyword.
        /// </summary>
        public StringDictionary Attributes
        {
            get { return _Attributes; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Element object.
        /// Initializes the Name property with the ElementAttribute.Name property.
        /// </summary>
        /// <exception cref="Fosol.Common.Exceptions.AttributeRequiredException">The ElementAttributeAttribute is required.</exception>
        internal Element()
        {
            var attr = (ElementAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(ElementAttribute));
            if (attr != null)
                this.Name = attr.Name;
            else
                throw new Core.Exceptions.AttributeException(typeof(ElementAttribute));
        }

        /// <summary>
        /// Creates a new instance of a Element object.
        /// </summary>
        /// <param name="attributes">Dictionary of attributes to include with this keyword.</param>
        internal Element(StringDictionary attributes)
            : this()
        {
            InitAttributes(attributes);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new instance of the Element that uses the specified name.
        /// </summary>
        /// <param name="name">Unique name to identify the Element.</param>
        /// <param name="attributes">Attributes for the element.</param>
        /// <returns>New Instance of a Element.</returns>
        public static Element CreateNew(string name, NameValueCollection attributes)
        {
            return CreateNew(name, attributes.ToStringDictionary());
        }

        /// <summary>
        /// Creates a new instance of the Element that uses the specified name.
        /// </summary>
        /// <param name="name">Unique name to identify the Element.</param>
        /// <param name="attributes">Attributes for the Element.</param>
        /// <returns>New Instance of a Element.</returns>
        public static Element CreateNew(string name, StringDictionary attributes)
        {
            // There was no Element with the specified name, simply generate a static TextElement.
            if (!ElementLibrary.ContainsKey(name))
                return new Elements.TextElement(name);

            var type = ElementLibrary.Get(name);
            var is_static = typeof(StaticElement).IsAssignableFrom(type);
            var is_dynamic = typeof(DynamicElement).IsAssignableFrom(type);

            // Return a new instance of the Element.
            if (type != null)
            {
                if (is_static)
                {
                    if (type.GetConstructor(new Type[] { typeof(StringDictionary) }) != null)
                        return (StaticElement)Activator.CreateInstance(type, attributes);
                    else if (type.GetConstructor(new Type[0]) != null)
                        return (StaticElement)Activator.CreateInstance(type);
                }
                else if (is_dynamic)
                {
                    if (type.GetConstructor(new Type[] { typeof(StringDictionary) }) != null)
                        return (DynamicElement)Activator.CreateInstance(type, attributes);
                    else if (type.GetConstructor(new Type[0]) != null)
                        return (DynamicElement)Activator.CreateInstance(type);
                }
            }

            // There was no Element with the specified name, simply generate a static TextElement.
            return new Elements.TextElement(name);
        }

        /// <summary>
        /// Initializes the Attributes collection.
        /// </summary>
        /// <param name="attributes">Dictionary of attributes to include with this keyword.</param>
        protected void InitAttributes(StringDictionary attributes)
        {
            // Get all the valid attributes.
            var properties = (
                from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(ElementPropertyAttribute), false).FirstOrDefault() != null
                select p);

            // Loop through the valid properties (this will stop incorrect parameters from being provided).
            foreach (var prop in properties)
            {
                string value = null;
                var attr = prop.GetCustomAttributes(typeof(ElementPropertyAttribute), false).FirstOrDefault() as ElementPropertyAttribute;

                // Check if the parameters have a match.
                if (attributes != null)
                {
                    value = attributes[attr.Name];

                    // Go through abbreviations.
                    if (value == null && attr.Abbreviations != null)
                    {
                        foreach (var abbr in attr.Abbreviations)
                        {
                            // Found a valid abbreviation.
                            if (attributes[abbr] != null)
                            {
                                value = attributes[abbr];
                                break;
                            }
                        }
                    }
                }

                // Parameter was found so use it.
                if (value != null)
                {
                    if (attr.Converter == null)
                        prop.SetValue2(this, value);
                    else
                        prop.SetValue2(this, value, attr.Converter);

                    this.Attributes.Add(attr.Name, value);
                    continue;
                }

                var default_attr = prop.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;

                // Use the default value.
                if (default_attr != null)
                {
                    prop.SetValue(this, default_attr.Value);
                    continue;
                }

                var required_attr = prop.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

                // This parameter is required and has not been set; throw exception.
                if (required_attr != null)
                {
                    throw new Exceptions.FormatAttributeException(this.Name, attr.Name);
                }
            }
        }

        /// <summary>
        /// Returns a formatted string value to create this keyword.
        /// </summary>
        /// <example>
        /// {datetime?format=u}
        /// </example>
        /// <returns>Special formatted string value.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Returns the HashCode for this keyword.
        /// This HashCode is composed of the Name and the Parameters.
        /// </summary>
        /// <returns>HashCode for this keyword.</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode()
                + this.Attributes.GetHashCode();
        }

        /// <summary>
        /// Determine if the object is equal to this Element.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if they are equal.</returns>
        public override bool Equals(object obj)
        {
            var element = obj as Element;

            if (ReferenceEquals(element, null))
                return false;

            if (ReferenceEquals(element, this))
                return true;

            if (this.Name == element.Name
                && this.Attributes.Count == element.Attributes.Count)
                return this.Attributes.Equals(element.Attributes);

            return false;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
