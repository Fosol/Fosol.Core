using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fosol.Core.Extensions.Objects
{
    /// <summary>
    /// ObjectExtensions static class, provides extension methods for Objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Confirms whether the element inherits from the specified type.
        /// </summary>
        /// <param name="element">Object to verify against.</param>
        /// <param name="type">Type to look for.</param>
        /// <returns>True if the element inherits from the specified type.</returns>
        public static bool InheritsFrom(this object element, Type type)
        {
            Validation.Argument.Assert.IsNotNull(type, nameof(type));

            var element_type = element.GetType();

            // Loop through the inheritance of the element until you find the type or get to the end.
            while (element_type != typeof(object))
            {
                var current = element_type.IsGenericType ? element_type.GetGenericTypeDefinition() : element_type;

                if (element_type == type)
                    return true;

                element_type = element_type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Places the object within the specified stream.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream" must be writable.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "stream" cannot be null.</exception>
        /// <param name="data">Object to copy to stream.</param>
        /// <param name="stream">Stream object to receive object.</param>
        /// <param name="keepOpen">Determines whether the stream is closed on exist.</param>
        public static void ToStream(this object data, Stream stream, bool keepOpen = true)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), "Parameter \"{0}.CanWrite\" must be true.");

            if (keepOpen)
            {
                var writer = new StreamWriter(stream);
                writer.Write(data);
                writer.Flush();
            }
            else
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(data);
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Converts an object with the BinaryFormatter into an array of bytes.
        /// </summary>
        /// <param name="obj">Object to convert to a byte array.</param>
        /// <returns>A new byte array representing the object.</returns>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;

            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }

        /// <summary>
        /// Deep clones an object into another copy of that object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'source' must be serializable.</exception>
        /// <typeparam name="T">Type of the object to clone.</typeparam>
        /// <param name="source">Source object that will be cloned.</param>
        /// <returns>A copy of the original 'source' object.</returns>
        public static T DeepClone<T>(this T source)
        {
            Validation.Argument.Assert.IsValid(typeof(T).IsSerializable, nameof(source), "Parameter \"{0}\" must be a serializable type.");

            if (Object.ReferenceEquals(source, null))
                return default(T);

            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
