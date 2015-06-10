using System;
using System.Linq;
using System.Xml;

namespace Fosol.Core.Xml
{
    /// <summary>
    /// XmlHelper static class, provides methods to help with Xml related content.
    /// </summary>
    public static class XmlHelper
    {
        #region Methods
        /// <summary>
        /// Validates whether the string value contains valid Xml characters.
        /// </summary>
        /// <param name="value">String value to test.</param>
        /// <returns>True if the string value is valid Xml.</returns>
        public static bool IsValidXmlString(string value)
        {
            try
            {
                XmlConvert.VerifyXmlChars(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Strips out all the invalid characters from a string value.
        /// </summary>
        /// <param name="value">String value to clean.</param>
        /// <returns>New string value.</returns>
        public static string RemoveInvalidXmlChars(string value)
        {
            var valid_chars = value.Where(c => XmlConvert.IsXmlChar(c)).ToArray();
            return new string(valid_chars);
        }

        /// <summary>
        /// Encodes the string value.
        /// </summary>
        /// <param name="value">String value to encode.</param>
        /// <returns>Encoded string value.</returns>
        public static string EncodeValue(string value)
        {
            return XmlConvert.EncodeName(value);
        }

        /// <summary>
        /// Decode the string value.
        /// </summary>
        /// <param name="value">String value to decode.</param>
        /// <returns>Decoded string value.</returns>
        public static string DecodeValue(string value)
        {
            return XmlConvert.DecodeName(value);
        }
        #endregion
    }
}
