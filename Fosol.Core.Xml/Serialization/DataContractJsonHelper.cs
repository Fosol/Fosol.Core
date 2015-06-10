using Fosol.Core.Extensions.Streams;
using Fosol.Core.Extensions.Strings;
using Fosol.Core.Xml.Extensions;
using Fosol.Core.Xml.Extensions.XmlObjectSerializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Fosol.Core.Xml.Serialization
{
    /// <summary>
    /// Utility methods to serialize DataContract Json data.
    /// </summary>
    public static class DataContractJsonHelper
    {
        #region Variables
        private static IDictionary<Type, DataContractJsonSerializer> _CachedJsonSerializers = new Dictionary<Type, DataContractJsonSerializer>();
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
        /// <returns>DataContractJsonSerializer object.</returns>
        public static DataContractJsonSerializer GetSerializer(Type classType)
        {
            if (!_CachedJsonSerializers.ContainsKey(classType))
            {
                _CachedJsonSerializers.Add(classType, new DataContractJsonSerializer(classType));
            }
            return _CachedJsonSerializers[classType];
        }

        #region Serialize
        /// <summary>
        /// Converts a DataContract object into a stream.
        /// The object must be defined with the DataContractJsonSerializer.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" must be defined with a DataContractAttribute.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "stream" cannot be null.</exception>
        /// <param name="data">DataContract object to serialize to stream.</param>
        /// <param name="stream">Stream to write object to.</param>
        /// <param name="resetPosition">Whether the position in the stream should be reset to where it began before writing.</param>
        public static void Serialize(object data, Stream stream, bool resetPosition = true)
        {
            GetSerializer(data.GetType()).ToStream(data, stream, resetPosition);
        }

        /// <summary>
        /// Serialize the DataContract object into a string using the DataContractSerializer.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" must be defined with a DataContractAttribute.</exception>
        /// <exception cref="System.ArgumentNullException">Paramter "data" cannot be null.</exception>
        /// <param name="data">Object to serialize.</param>
        /// <returns>Serialized object as a string.</returns>
        public static string Serialize(object data)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
#if !WINDOWS_APP && !WINDOWS_PHONE_APP
            Validation.Argument.Assert.HasAttribute(data, typeof(System.Runtime.Serialization.DataContractAttribute), nameof(data));
#endif

            using (var stream = new MemoryStream())
            {
                Serialize(data, stream);
                return stream.WriteToString();
            }
        }

        /// <summary>
        /// Serialize the object and save it to the path specified as a file.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" must be defined with a DataContractAttribute.  Parameter "path" must not be empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Paramters "data" and "path" cannot be null.</exception>
        /// <param name="data">Object to serialize.</param>
        /// <param name="path">Path and filename of the location to save the file.</param>
        /// <param name="fileMode">Control how the operating system will open the file.</param>
        /// <param name="fileShare">Control how other file streams can access the file.</param>
        public static void Serialize(object data, string path, FileMode fileMode = FileMode.CreateNew, FileShare fileShare = FileShare.None)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.HasAttribute(data, typeof(System.Runtime.Serialization.DataContractAttribute), nameof(data));
            Validation.Argument.Assert.IsNotNullOrWhitespace(path, nameof(path));

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
        /// Uses the DataContractJsonSerializer object to deserialize.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "stream" cannot be null.</exception>
        /// <typeparam name="T">Type of object to create from the serialized stream.</typeparam>
        /// <param name="stream">Stream object containing the serialized data.</param>
        /// <returns>Object of type T.</returns>
        public static T Deserialize<T>(Stream stream)
        {
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            return (T)GetSerializer(typeof(T)).ReadObject(stream);
        }

        /// <summary>
        /// Deserialize the string value into an object of the specified type.
        /// Uses the DataContractJsonSerializer object to deserialize.
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
                return (T)GetSerializer(typeof(T)).ReadObject(stream);
            }
        }

        /// <summary>
        /// Deserialize a file at the specified path into an object of type T.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "path" must not be empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Paramter "path" cannot be null.</exception>
        /// <param name="data">Object to deserialize.</param>
        /// <param name="path">Path and filename of the location to save the file.</param>
        /// <param name="fileShare">Control how other file streams can access the file.</param>
        /// <returns>A new instance of an object of type T.</returns>
        public static T Deserialize<T>(string path, FileShare fileShare = FileShare.Read)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(path, nameof(path));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, fileShare))
            {
                return Deserialize<T>(stream);
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
