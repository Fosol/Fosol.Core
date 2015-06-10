using System;

namespace Fosol.Core.Text
{
    /// <summary>
    /// ElementBoundary class, provides a way to control the boundary syntax for keywords.
    /// </summary>
    public class ElementBoundary
    {
        #region Variables
        private readonly string _Value;
        private readonly StringComparison _StringComparison;
        #endregion

        #region Properties
        /// <summary>
        /// get - The string boundary.
        /// </summary>
        public string Value { get { return _Value; } }

        /// <summary>
        /// get - The length of the boundary.
        /// </summary>
        public int Length { get { return _Value.Length; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a FormatElementBoundary object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stringComparison"></param>
        public ElementBoundary(string value, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(value, nameof(value));
            _Value = value;
            _StringComparison = stringComparison;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Find the first index position of the boundary.
        /// </summary>
        /// <param name="text">Text value to look for the start boundary in.</param>
        /// <param name="startIndex">Index position to begin searching within the string.</param>
        /// <param name="checkForEscape">When 'true' it will check for to see if the boundary is escaped, if escaped it will continue looking for the next boundary.</param>
        /// <returns>Index position of boundary if found, or -1 if not found.</returns>
        public int IndexOfBoundaryIn(string text, int startIndex, bool checkForEscape = true)
        {
            if (startIndex >= text.Length)
                return -1;

            var pos = text.IndexOf(_Value, startIndex);

            // Check if the boundary has been escaped.
            // If it's escaped continue looking or return -1.
            if (checkForEscape
                && pos != -1
                && IsEscaped(text, pos))
            {
                pos = ShiftRight(text, pos, true);
                if (pos != -1)
                    return IndexOfBoundaryIn(text, pos);
            }

            return pos;
        }

        /// <summary>
        /// Find the first index position of the boundary.
        /// </summary>
        /// <param name="text">Text value to look for the start boundary in.</param>
        /// <param name="startIndex">Index position to begin searching within the string.</param>
        /// <param name="count">Number of character positions to examine.</param>
        /// <param name="checkForEscape">When 'true' it will check for to see if the boundary is escaped, if escaped it will continue looking for the next boundary.</param>
        /// <returns>Index position of boundary if found, or -1 if not found.</returns>
        public int IndexOfBoundaryIn(string text, int startIndex, int count, bool checkForEscape = true)
        {
            if (startIndex >= text.Length)
                return -1;

            // Count is too large, change it to the end of the string.
            if (count >= text.Length)
                count = text.Length - startIndex;

            var pos = text.IndexOf(_Value, startIndex, count);

            // Check if the boundary has been escaped.
            // If it's escaped continue looking or return -1.
            if (checkForEscape
                && pos != -1
                && IsEscaped(text, pos))
            {
                pos = ShiftRight(text, pos, true);
                if (pos != -1)
                    return IndexOfBoundaryIn(text, pos, count - (pos - startIndex));
            }

            return pos;
        }

        /// <summary>
        /// Move the index past the boundary position.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="boundaryIndex"></param>
        /// <param name="isEscaped"></param>
        /// <returns></returns>
        internal int ShiftRight(string text, int boundaryIndex, bool isEscaped)
        {
            if (isEscaped)
            {
                var pos = boundaryIndex + (_Value.Length * 2);

                if (pos < text.Length)
                    return pos;

                return -1;
            }

            return ShiftRight(text, boundaryIndex); ;
        }

        /// <summary>
        /// Move the index past the boundary position.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="boundaryIndex"></param>
        /// <returns></returns>
        public int ShiftRight(string text, int boundaryIndex)
        {
            var pos = boundaryIndex + _Value.Length;

            if (pos < text.Length)
                return pos;

            return -1;
        }

        /// <summary>
        /// Move the index position to before the boundary position.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="boundaryIndex"></param>
        /// <returns></returns>
        public int ShiftLeft(string text, int boundaryIndex)
        {
            var pos = boundaryIndex - 1;

            if (pos >= 0)
                return pos;

            return -1;
        }

        /// <summary>
        /// Check if the current boundary has been escaped.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="boundaryIndex"></param>
        /// <returns></returns>
        public bool IsEscaped(string text, int boundaryIndex)
        {
            if (boundaryIndex >= text.Length)
                return false;

            // If we've moved past the end of the string return false.
            if (text.Length <= boundaryIndex + _Value.Length)
                return false;
            else if (text.Substring(boundaryIndex + _Value.Length, _Value.Length).Equals(_Value, _StringComparison))
                return true;

            return false;
        }

        /// <summary>
        /// Returns the boundary value.
        /// </summary>
        /// <returns>Boundary value.</returns>
        public override string ToString()
        {
            return this.Value;
        }
        #endregion

        #region Operators
        public static string operator +(ElementBoundary val1, ElementBoundary val2)
        {
            return val1.Value + val2.Value;
        }

        public static implicit operator String(ElementBoundary boundary)
        {
            return boundary.Value;
        }
        #endregion

        #region Events
        #endregion
    }
}
