using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Core.Net
{
    /// <summary>
    /// UriPath sealed class, provides a way to manage a URI path and all of its segments.
    /// </summary>
    public sealed class UriPath
        : IEnumerable<UriPathSegment>
    {
        #region Variables
        private List<UriPathSegment> _Segments;
        private bool _IsAbsolute;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The path segment value at the specified index position.
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">Parameter 'index' must be a valid position within the collection.</exception>
        /// <param name="index">Index position within the collection.</param>
        /// <returns>The URI path segment at the specified index position.</returns>
        public UriPathSegment this[int index]
        {
            get { return _Segments[index]; }
            set { _Segments[index] = value; }
        }

        /// <summary>
        /// get - The number of path segments there are.
        /// </summary>
        public int Count
        {
            get { return _Segments.Count; }
        }

        /// <summary>
        /// get - Whether this collection is readonly.
        /// </summary>
        public bool Readonly
        {
            get { return false; }
        }

        /// <summary>
        /// get/set - Whether the URI path is absolute or relative.
        /// </summary>
        public bool IsAbsolute
        {
            get { return _IsAbsolute; }
            set { _IsAbsolute = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UriPath class.
        /// </summary>
        public UriPath()
        {
            _Segments = new List<UriPathSegment>();
        }

        /// <summary>
        /// Creates a new instance of a UriPath class.
        /// </summary>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        /// <exception cref="System.FormatException">Parameter 'path' cannot start with "//" (without double quotes).</exception>
#else
        /// <exception cref="System.UriFormatException">Parameter 'path' cannot start with "//" (without double quotes).</exception>
#endif
        /// <param name="path">Initial path value.</param>
        public UriPath(string path)
            : this()
        {
            if (String.IsNullOrEmpty(path))
                return;

            if (path.StartsWith("//"))
#if WINDOWS_APP || WINDOWS_PHONE_APP
                throw new FormatException("Path has invalid characters.");
#else
                throw new UriFormatException("Path has invalid characters.");
#endif

            if (path.StartsWith("/"))
            {
                _IsAbsolute = true;

                if (path.Length == 1)
                    return;

                path = path.Substring(1);
            }

            // Replace Whitespace.
            UriBuilder.ReplaceWhitespaces(ref path);

            var split_path = path.Split('/');

            foreach (var path_segment in split_path)
            {
                _Segments.Add(new UriPathSegment(path_segment));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the enumerator for the UriPath collection..
        /// </summary>
        /// <returns>Enumerator for the UriPath collection..</returns>
        public IEnumerator<UriPathSegment> GetEnumerator()
        {
            return _Segments.GetEnumerator();
        }

        /// <summary>
        /// Get the enumerator for the UriPath collection..
        /// </summary>
        /// <returns>Enumerator for the UriPath collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Segments.GetEnumerator();
        }

        /// <summary>
        /// Add the segment to the current path.
        /// </summary>
        /// <param name="segment">UriPathSegment object.</param>
        public void Add(UriPathSegment segment)
        {
            Validation.Argument.Assert.IsNotNull(segment, nameof(segment));

            _Segments.Add(segment);
        }

        /// <summary>
        /// Add the specified value to the current path as an additional segment.
        /// </summary>
        /// <param name="segment">Segment value.</param>
        public void Add(string segment)
        {
            _Segments.Add(new UriPathSegment(segment));
        }

        /// <summary>
        /// Removes the segment at the specified index position.
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">Parameter 'segmentIndex' must be a valid position.</exception>
        /// <param name="segmentIndex">Index position of the segment to remove.</param>
        public void RemoveAt(int segmentIndex)
        {
            _Segments.RemoveAt(segmentIndex);
        }

        /// <summary>
        /// Clear the URI path of all segments.
        /// </summary>
        public void Clear()
        {
            _Segments.Clear();
        }

        /// <summary>
        /// Returns a string value representing the full path.
        /// </summary>
        /// <returns>The full path as a single string.</returns>
        public override string ToString()
        {
            if (_Segments.Count == 0)
                return (this.IsAbsolute ? "/" : String.Empty);

            return (this.IsAbsolute ? "/" : String.Empty) + _Segments.Select(s => s.Value).Aggregate((a, b) => a + "/" + b);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Convert the UriPath object into a string.
        /// </summary>
        /// <param name="obj">UriPath object.</param>
        /// <returns>String value representing the full URI path.</returns>
        public static implicit operator string (UriPath obj)
        {
            return obj.ToString();
        }

        /// <summary>
        /// Convert the string value into a new UriPath object.
        /// </summary>
        /// <param name="value">String path value.</param>
        /// <returns>A new instance of a UriPath object.</returns>
        public static explicit operator UriPath(string value)
        {
            return new UriPath(value);
        }
        #endregion

        #region Events
        #endregion
    }
}
