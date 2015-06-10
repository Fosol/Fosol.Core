using System;
using System.Collections.Generic;

namespace Fosol.Core.Text
{

    /// <summary>
    /// IElementCollection interface, provides an interface for maintaining a collection of Element objects used for parsing and rending text.
    /// </summary>
    public interface IElementCollection<T>
        : IEnumerable<T>
        where T : Element
    {
        /// <summary>
        /// get - Collection of FormatElement objects.
        /// </summary>
        global::System.Collections.Generic.List<T> Elements { get; }

        /// <summary>
        /// Generate the dynamic output of this format.
        /// </summary>
        /// <param name="data">Information that can be used when rendering the dynamic elements.</param>
        /// <returns>A dynamic string value.</returns>
        string Render(object data);
    }
}
