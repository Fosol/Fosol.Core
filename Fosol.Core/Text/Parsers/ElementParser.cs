using Fosol.Core.Extensions.Dictionaries;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Fosol.Core.Text.Parsers
{
    /// <summary>
    /// ElementParser sealed class, provides a consistant way to generate an ElementCollection which contains the parsed parts of text.
    /// </summary>
    public sealed class ElementParser
        : Parser<ElementCollection, Element>
    {
        #region Variables
        private readonly static ElementBoundary _DefaultStartBoundary = new ElementBoundary("{");
        private readonly static ElementBoundary _DefaultEndBoundary = new ElementBoundary("}");
        private readonly static ElementBoundary _DefaultAttributeBoundary = new ElementBoundary("?");
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new ElementParser object with default boundaries.
        /// </summary>
        /// <example>
        /// {name?param1=value1&param2=value2}
        /// </example>
        public ElementParser()
            : base(_DefaultStartBoundary, _DefaultEndBoundary, _DefaultAttributeBoundary)
        {
        }

        /// <summary>
        /// Creates a new ElementParser object with the specified boundaries.
        /// </summary>
        /// <param name="startBoundary">ElementBoundary object to identify the start of an Element object within text.</param>
        /// <param name="endBoundary">ElementBoundary object to identify the end of an Element object within text.</param>
        /// <param name="attributeBoundary">ElementBoundary object to identify an AttributeElement for an Element object within text.</param>
        public ElementParser(ElementBoundary startBoundary, ElementBoundary endBoundary, ElementBoundary attributeBoundary)
            : base(startBoundary, endBoundary, attributeBoundary)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Parses the specially formatted text and updates the ElementCollection.
        /// </summary>
        /// <param name="text">A format string with special boundary syntax that represents Element objects.</param>
        /// <returns>A new Element object from the format string.</returns>
        public override ElementCollection Parse(string text)
        {
            var elements = new List<Element>();
            var pos = 0;
            do
            {
                elements.Add(NextElement(text, ref pos));
            } while (pos != -1);
            return new ElementCollection(elements);
        }

        /// <summary>
        /// Find the next Element within the text.
        /// </summary>
        /// <param name="text">A formated string with special boundary syntax that represents Element objects.</param>
        /// <param name="indexPosition">Index position to begin searching for elements within the format string.</param>
        /// <returns>A new Element object.</returns>
        private Element NextElement(string text, ref int indexPosition)
        {
            var start = this.StartBoundary.IndexOfBoundaryIn(text, indexPosition);

            // There are no keywords found, return the raw text value.
            if (start == -1)
            {
                var element = CreateElement(text.Substring(indexPosition));
                indexPosition = -1;
                return element;
            }

            // A keyword was found but there is text before it, return the text first.
            if (start > indexPosition)
            {
                var element = CreateElement(text.Substring(indexPosition, start - indexPosition));
                indexPosition = start;
                return element;
            }

            // If we've got to this part it means the indexPosition points to a StartBoundary.
            var after_start = this.StartBoundary.ShiftRight(text, start);

            var end = EndBoundaryIndex(text, after_start);

            // End boundary was not found, return the text value.
            if (end == -1)
            {
                indexPosition = end;
                return CreateElement(text.Substring(start));
            }

            // Update the indexPosition to be after this FormatPart.
            indexPosition = this.EndBoundary.ShiftRight(text, end);

            return CreateElement(text.Substring(after_start, end - after_start));
        }

        /// <summary>
        /// Search for the end boundary.
        /// Check for inner boundaries before returning index position of end boundary.
        /// </summary>
        /// <param name="text">Formatted text containing element boundaries.</param>
        /// <param name="afterStart">Index position after the discovered start boundary.</param>
        /// <returns>Index position of end boundary, or -1 if none is found.</returns>
        private int EndBoundaryIndex(string text, int afterStart)
        {
            var end = this.EndBoundary.IndexOfBoundaryIn(text, afterStart, false);

            // End boundary was not found.
            if (end == -1)
                return end;

            // Check to make sure the current end boundary doesn't belong to an inner boundary.
            // An inner boundary match was discovered.  Continue check for an end boundary.
            if (this.StartBoundary.IndexOfBoundaryIn(text, afterStart, end - afterStart) != -1)
            {
                var after_end = this.EndBoundary.ShiftRight(text, end);
                return EndBoundaryIndex(text, after_end);
            }

            // There was no inner start boundary, now check if this end boundary has been escaped.
            // If it has been escaped continue looking for an end boundary.
            if (this.EndBoundary.IsEscaped(text, end))
            {
                var after_end = this.EndBoundary.ShiftRight(text, end, true);

                if (after_end == -1)
                    return after_end;

                return EndBoundaryIndex(text, after_end);
            }

            return end;
        }

        /// <summary>
        /// Creates a new Element for the specified format.
        /// </summary>
        /// <param name="text">Special syntax string that represents an Element object.</param>
        /// <returns>A new Element object.</returns>
        private Element CreateElement(string text)
        {
            string name;
            NameValueCollection query = new NameValueCollection();
            var attribute_pos = this.AttributeBoundary.IndexOfBoundaryIn(text, this.StartBoundary.Length - 1);
            var is_param = text.StartsWith("@");

            // If there are no attributes, the whole part.Value becomes the name.
            if (attribute_pos == -1)
            {
                // It's using the parameter shortcut syntax.
                if (is_param)
                {
                    name = "parameter";
                    var pos = text.IndexOf("=");

                    // Check for shortcut syntax within (i.e. {@param=value}).
                    if (pos != -1)
                    {
                        query.Add("value", text.Substring(pos + 1));
                    }

                    query.Add("name", text);
                }
                else
                    name = text;
            }
            // If there are attributes, extract them and the name.
            else
            {
                //query = System.Web.HttpUtility.ParseQueryString(format.Substring(attribute_pos + 1));
                var attributes = Extensions.Strings.StringExtensions.SplitToKeyValuePairs(text.Substring(attribute_pos + 1), "&", "=");
                query = new NameValueCollection();

                foreach (var attr in attributes)
                {
                    query.Add(attr.Key, attr.Value);
                }

                // It's using the parameter shortcut syntax.
                if (is_param)
                {
                    name = "parameter";
                    query.Add("name", text.Substring(0, attribute_pos));
                }
                else
                {
                    name = text.Substring(0, attribute_pos);
                }
            }

            // Clean up all the attribute values by replacing 
            foreach (var key in query.AllKeys)
            {
                query[key] = Decode(query[key]);
            }

            return Element.CreateNew(Decode(name), query);
        }

        /// <summary>
        /// Remove escaped boundaries from the text.
        /// </summary>
        /// <param name="text">Text to remove escaped boundaries from.</param>
        /// <returns>Text value without escaped boundaries</returns>
        private string Decode(string text)
        {
            return text.Replace(this.StartBoundary + this.StartBoundary, this.StartBoundary)
                .Replace(this.EndBoundary + this.EndBoundary, this.EndBoundary)
                .Replace(this.AttributeBoundary + this.AttributeBoundary, this.AttributeBoundary);
        }

        /// <summary>
        /// Generate the format string with boundary syntax for the specified Element object.
        /// </summary>
        /// <param name="collection">ElementCollection object to render.</param>
        /// <returns>String value with boundary formats.</returns>
        public override string Render(ElementCollection collection)
        {
            var result = new StringBuilder();
            foreach (var element in collection.Elements)
            {
                // If it has attributes make sure to include them.
                if (element.Attributes.Count > 0)
                    result.Append(this.StartBoundary + element.Name + this.AttributeBoundary + element.Attributes.ToQueryString() + this.EndBoundary);
                else
                {
                    var text = element as StaticElement;
                    // If it's a StaticElement without attributes it can be rendered as a simple string without boundaries.
                    if (text != null)
                        result.Append(text.Text);
                    else
                        result.Append(this.StartBoundary + element.Name + this.EndBoundary);
                }
            }

            return result.ToString();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
