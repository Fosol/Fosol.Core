using System;
using System.IO;
using System.Xml.Serialization;

namespace Fosol.Core.Xml.Extensions.XmlSerializers
{
    /// <summary>
    /// XmlSerializerExtensions static class, provides extension methods for XmlSerializer objects.
    /// </summary>
    public static class XmlSerializerExtensions
    {
        /// <summary>
        /// Converts a DataContract object into a stream.
        /// The object must be defined with the SerializableAttribute.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" must be defined with a SerializableAttribute.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "stream" cannot be null and must be writable.</exception>
        /// <param name="serializer">XmlSerializer object.</param>
        /// <param name="data">Serializable object to serialize to stream.</param>
        /// <param name="stream">Stream to write object to.</param>
        /// <param name="resetPosition">Whether to reset the stream position to 0 after writing to it.</param>
        public static void ToStream(this XmlSerializer serializer, object data, Stream stream, bool resetPosition = false)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.HasAttribute(data, typeof(SerializableAttribute), nameof(data));
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), Resources.Multilingual.Extensions_XmlSerializerExtensions_ToStream_CanWrite);

            serializer.Serialize(stream, data);

            if (resetPosition)
                stream.Position = 0;
        }
    }
}
