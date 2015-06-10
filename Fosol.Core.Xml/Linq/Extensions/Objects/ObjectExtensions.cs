using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Fosol.Core.Xml.Linq.Extensions.Objects
{
    /// <summary>
    /// ObjectExtensions static class, provides extension methods for Objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns the properties of the given object as XElements.
        /// Properties with null values are still returned, but as empty
        /// elements. Underscores in property names are replaces with hyphens.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Enumerable collection of XElement objects that represent the properties of the object.</returns>
        public static IEnumerable<XElement> AsXElements(this object source)
        {
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                object value = prop.GetValue(source, null);
                yield return new XElement(prop.Name.Replace("_", "-"), value);
            }
        }

        /// <summary>
        /// Returns the properties of the given object as XAttribute.
        /// Properties with null values are returned as empty attributes.
        /// Underscores in property names are replaces with hyphens.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Enumerable collection of XAttribute objects that represent the properties of the object.</returns>
        public static IEnumerable<XAttribute> AsXAttributes(this object source)
        {
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                object value = prop.GetValue(source, null);
                yield return new XAttribute(prop.Name.Replace("_", "-"), value ?? "");
            }
        }
    }
}
