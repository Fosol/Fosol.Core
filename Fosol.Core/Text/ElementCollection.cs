using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Core.Text
{
    /// <summary>
    /// ElementCollection sealed class, provides a collection of Element objects that are used to create a dynamically rendered text.
    /// </summary>
    public class ElementCollection
        : IElementCollection<Element>
    {
        #region Variables
        private readonly List<Element> _Elements;
        #endregion

        #region Properties
        /// <summary>
        /// get - Collection of ElementCollection objects.
        /// </summary>
        public List<Element> Elements { get { return _Elements; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ElementCollection object.
        /// </summary>
        public ElementCollection()
        {
            _Elements = new List<Element>();
        }

        /// <summary>
        /// Creates a new instance of a ElementCollection object.
        /// </summary>
        internal ElementCollection(List<Element> elements)
        {
            Validation.Argument.Assert.IsNotNull(elements, nameof(elements));
            _Elements = elements;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Renders the collection of Elements into a dynamically generated string.
        /// </summary>
        /// <param name="data">Information to be used when rendering the output string.</param>
        /// <returns>The dynamically generated output string value.</returns>
        public virtual string Render(object data)
        {
            var builder = new StringBuilder();

            foreach (var key in this.Elements)
            {
                var static_key = key as StaticElement;
                var dynamic_key = key as DynamicElement;

                if (static_key != null)
                    builder.Append(static_key.Text);
                else if (dynamic_key != null)
                    builder.Append(dynamic_key.Render(data));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns the Enumerator for the elements within this collection.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        public IEnumerator<Element> GetEnumerator()
        {
            return _Elements.GetEnumerator();
        }

        /// <summary>
        /// Returns the Enumerator for the elements within this collection.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Elements.GetEnumerator();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
