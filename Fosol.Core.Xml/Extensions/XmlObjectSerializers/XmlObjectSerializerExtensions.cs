using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Fosol.Core.Xml.Extensions.XmlObjectSerializers
{
    public static class XmlObjectSerializerExtensions
    {
        /// <summary>
        /// Converts a DataContract object into a stream.
        /// The object must be defined with the DataContractAttribute.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" must be defined with a DataContractAttribute.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "stream" cannot be null.</exception>
        /// <param name="data">DataContract object to serialize to stream.</param>
        /// <param name="stream">Stream to write object to.</param>
        /// <param name="resetPosition">Whether the position in the stream should be reset to where it began before writing.</param>
        public static void ToStream(this DataContractSerializer serializer, object data, Stream stream, bool resetPosition = true)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
#if !WINDOWS_APP && !WINDOWS_PHONE_APP
            Validation.Argument.Assert.HasAttribute(data, typeof(System.Runtime.Serialization.DataContractAttribute), nameof(data));
#endif
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), Resources.Multilingual.Extensions_XmlObjectSerializerExtensions_ToStream_CanWrite);

            var position = stream.Position;
            serializer.WriteObject(stream, data);

            // Set the position to the beginning of the stream after writing to it.
            if (resetPosition)
                stream.Position = position;
        }

        /// <summary>
        /// Converts a DataContract object into a stream.
        /// The object must be defined with the DataContractAttribute.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" must be defined with a DataContractAttribute.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "stream" cannot be null.</exception>
        /// <param name="data">DataContract object to serialize to stream.</param>
        /// <param name="stream">Stream to write object to.</param>
        /// <param name="resetPosition">Whether the position in the stream should be reset to where it began before writing.</param>
        public static void ToStream(this DataContractJsonSerializer serializer, object data, Stream stream, bool resetPosition = true)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
#if !WINDOWS_APP && !WINDOWS_PHONE_APP
            Validation.Argument.Assert.HasAttribute(data, typeof(System.Runtime.Serialization.DataContractAttribute), nameof(data));
#endif
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), Resources.Multilingual.Extensions_XmlObjectSerializerExtensions_ToStream_CanWrite);

            var position = stream.Position;
            serializer.WriteObject(stream, data);

            // Set the position to the beginning of the stream after writing to it.
            if (resetPosition)
                stream.Position = position;
        }
    }
}
