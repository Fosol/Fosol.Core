using Fosol.Core.Extensions.Streams;
using Fosol.Core.Extensions.Strings;
using Fosol.Core.Xml.Extensions;
using Fosol.Core.Xml.Extensions.XmlSerializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Fosol.Core.Xml.Serialization
{
    /// <summary>
    /// XmlHelper static class, provides methods for serializing XML data.
    /// </summary>
    public static class XmlHelper
    {
        #region Variables
        private static IDictionary<Type, XmlSerializer> _CachedSerializers = new Dictionary<Type, XmlSerializer>();
        #endregion

        #region Methods
        /// <summary>
        /// From MSDN:
        /// To increase performance, the XML serialization infrastructure dynamically generates assemblies to serialize and 
        /// deserialize specified types. The infrastructure finds and reuses those assemblies. This behavior occurs only when 
        /// using the following constructors:
        /// 
        /// XmlSerializer.XmlSerializer(Type)
        /// XmlSerializer.XmlSerializer(Type, String)
        /// 
        /// If you use any of the other constructors, multiple versions of the same assembly are generated and never unloaded, 
        /// which results in a memory leak and poor performance. The easiest solution is to use one of the previously mentioned 
        /// two constructors. Otherwise, you must cache the assemblies.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "classType" cannot be null.</exception>
        /// <param name="classType">Type of class being serialized.</param>
        /// <returns>DataContractSerializer object.</returns>
        public static XmlSerializer GetSerializer(Type classType)
        {
            if (!_CachedSerializers.ContainsKey(classType))
            {
                _CachedSerializers.Add(classType, new XmlSerializer(classType));
            }
            return _CachedSerializers[classType];
        }

        #region Serialize
        /// <summary>
        /// Serialize the object into a string using the XmlSerializer.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" must be defined with a SerializableAttribute.</exception>
        /// <exception cref="System.ArgumentNullException">Paramter "data" cannot be null.</exception>
        /// <param name="data">Object to serialize.</param>
        /// <returns>Serialized object as a string.</returns>
        public static string Serialize(object data)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.HasAttribute(data, typeof(SerializableAttribute), nameof(data));

            using (var stream = new MemoryStream())
            {
                GetSerializer(data.GetType()).ToStream(data, stream, true);
                return stream.WriteToString();
            }
        }

        /// <summary>
        /// Serializes the object and saves it to the specified path as a file.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "path" must not be empty or whitespace, and must be marked with a SerializableAttribute.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data" and "path" cannot be null.</exception>"
        /// <param name="data">Object to serialize.</param>
        /// <param name="path">Path and filename of the loccation to save the data.</param>
        /// <param name="fileMode">Control how the operating system opens the file.</param>
        /// <param name="fileShare">Control how other file streams can access the file.</param>
        public static void Serialize(object data, string path, FileMode fileMode = FileMode.CreateNew, FileShare fileShare = FileShare.None)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.IsNotNullOrWhitespace(path, nameof(path));
            Validation.Argument.Assert.HasAttribute(data, typeof(SerializableAttribute), nameof(data));

            using (var stream = File.Open(path, fileMode, FileAccess.Write, fileShare))
            {
                GetSerializer(data.GetType()).ToStream(data, stream);
            }
        }

        /// <summary>
        /// Serialize the object into the specified IsolatedStorgeFileStream.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream" must be writable.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data" and "stream" must not be null.</exception>"
        /// <typeparam name="T">The type of the object being serializes.</typeparam>
        /// <param name="data">Object to serialize and save.</param>
        /// <param name="stream">IsolatedStorageFileStream object.</param>
        /// <returns>A new instance of an object of type T.</returns>
        public static void Serialize(object data, System.IO.IsolatedStorage.IsolatedStorageFileStream stream)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), Resources.Multilingual.Serialization_XmlHelper_Serialize_CanWrite);

            using (var source = new MemoryStream())
            {
                GetSerializer(data.GetType()).ToStream(data, source, true);
                source.CopyTo(stream);
            }
        }
        #endregion

        #region Deserialize
        /// <summary>
        /// Deserialize the stream into the specified object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "stream" cannot be null.</exception>
        /// <typeparam name="T">Type of object to create from the serialized stream.</typeparam>
        /// <param name="stream">Stream object containing the serialized data.</param>
        /// <returns>Object of type T.</returns>
        public static T Deserialize<T>(Stream stream)
        {
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));

            return (T)GetSerializer(typeof(T)).Deserialize(stream);
        }

        /// <summary>
        /// Deserialize the Xml string value into an object of the specified type.
        /// Uses the XmlSerializer object to deserialize.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "data" cannot be null.</exception>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <param name="data">String data to deserialize.</param>
        /// <returns>Object of type T.</returns>
        public static T Deserialize<T>(string data)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));

            using (var stream = data.ToMemoryStream(true))
            {
                return (T)GetSerializer(typeof(T)).Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserialize the file into the specified object type.
        /// </summary>
        /// <typeparam name="T">The type of the object being deserialized.</typeparam>
        /// <param name="path">Path and filename of the file being read.</param>
        /// <param name="fileShare">Control how other file streams can access this file.</param>
        /// <returns>A new instance of an object of type T.</returns>
        public static T Deserialize<T>(string path, FileShare fileShare = FileShare.Read)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(path, nameof(path));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, fileShare))
            {
                return (T)GetSerializer(typeof(T)).Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserialize the IsolatedStorgeFileStream object into the specified type.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream" must be readable.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "stream" must not be null.</exception>"
        /// <typeparam name="T">The type of the object being deserialized.</typeparam>
        /// <param name="stream">IsolatedStorageFileStream object.</param>
        /// <returns>A new instance of an object of type T.</returns>
        public static T Deserialize<T>(System.IO.IsolatedStorage.IsolatedStorageFileStream stream)
        {
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanRead, nameof(stream), Resources.Multilingual.Serialization_XmlHelper_Deserialize_CanRead);

            using (var reader = new StreamReader(stream))
            {
                return Deserialize<T>(reader.ReadToEnd());
            }
        }
        #endregion
        #endregion
    }
}
