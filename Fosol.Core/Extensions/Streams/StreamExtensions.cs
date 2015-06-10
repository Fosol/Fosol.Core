using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fosol.Core.Extensions.Streams
{
    /// <summary>
    /// StreamExtensions static class, provides extension methods for Stream objects.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Copy one stream into another.
        /// It will start to read from the current position in the source stream.
        /// It will start to write at the current position in the destination stream.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream.CanRead" cannot be false.</exception>
        /// <exception cref="System.ArgumentException">Parameter "destination.CanWrite" cannot be false.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "destination" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "bufferSize" cannot be less than 1.</exception>
        /// <param name="source">Source stream to read from.</param>
        /// <param name="destination">Destination stream to write to.</param>
        /// <param name="bufferSize">Byte size of the buffer being copied at one time.  By default global DefaultBufferSize.</param>
        /// <param name="statusCallback">Event fires every time a buffer has been written to the destination stream.</param>
        /// <param name="args">Data to pass to the statusCallback event.</param>
        public static void CopyTo(this Stream source, Stream destination, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamWriteProgressCallback statusCallback = null, params Object[] args)
        {
            Validation.Argument.Assert.IsValid(source.CanWrite, nameof(source), Resources.Multilingual.Extensions_Streams_StreamExtensions_CopyTo_CanRead);
            Validation.Argument.Assert.IsNotNull(destination, nameof(destination));
            Validation.Argument.Assert.IsValid(destination.CanWrite, nameof(destination), Resources.Multilingual.Extensions_Streams_StreamExtensions_CopyTo_CanWrite);
            Validation.Argument.Assert.IsMinimum(bufferSize, 1, nameof(bufferSize));

            var buffer = new byte[bufferSize];
            int read = 0;
            int total = 0;
            do
            {
                // Read into buffer.
                read = source.Read(buffer, 0, bufferSize);
                total += read;

                if (read > 0)
                {
                    // Write buffer to destination.
                    destination.Write(buffer, 0, read);

                    // Fire the status callback event.
                    if (statusCallback != null)
                    {
                        if (source.CanSeek)
                            statusCallback(total, source.Length, args);
                        else
                            statusCallback(total, -1, args);
                    }
                }
            } while (read > 0);
        }

        /// <summary>
        /// Writes the stream into a string.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream.CanRead" cannot be false.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "stream" cannot be null.</exception>
        /// <param name="stream">Stream to convert into a string.</param>
        /// <returns>The contents of the string as a string value.</returns>
        public static string WriteToString(this Stream stream)
        {
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanRead, nameof(stream), Resources.Multilingual.Extensions_Streams_StreamExtensions_WriteToString_CanRead);

            using (var reader = new StreamReader(stream))
            {
                stream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Writes the stream to a string with the specified encoding.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters "stream", and "encoding" cannot be null.</exception>
        /// <param name="stream">Stream object to read from.</param>
        /// <param name="encoding">Encoding to use when creating the string.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <returns>String value.</returns>
        public static string WriteToString(this Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks = false)
        {
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsNotNull(encoding, nameof(encoding));

            using (var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Writes the stream to a string with the specified encoding.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream" must be readable.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "stream", and "encoding" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "bufferSize" must be greater than 0.</exception>
        /// <param name="stream">Stream object to read from.</param>
        /// <param name="encoding">Encoding to use when creating the string.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="bufferSize">Buffer size to read at one time from the stream.</param>
        /// <param name="statusCallback">Callback event fires each time the buffer is read from.</param>
        /// <param name="args">Parameters to pass to the statusCallback event.</param>
        /// <returns>String value.</returns>
        public static string WriteToString(this Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks = false, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamReadProgressCallback statusCallback = null, params Object[] args)
        {
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanRead, nameof(stream), Resources.Multilingual.Extensions_Streams_StreamExtensions_WriteToString_CanRead);
            Validation.Argument.Assert.IsNotNull(encoding, nameof(encoding));
            Validation.Argument.Assert.IsMinimum(bufferSize, 1, nameof(bufferSize));

            using (var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize))
            {
                var result = new List<char>();
                var buffer = new char[bufferSize];
                var read = 0;

                while (true)
                {
                    read += reader.Read(buffer, read, bufferSize);

                    if (read == 0)
                        break;

                    // Fire the status callback event.
                    if (statusCallback != null)
                    {
                        if (stream.CanSeek)
                            statusCallback(read, stream.Length, args);
                        else
                            statusCallback(read, -1, args);
                    }

                    result.AddRange(buffer);
                }

                return new string(result.ToArray());
            }
        }

        /// <summary>
        /// Reads the stream into a byte array.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "stream" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "bufferSize" must be greater than 0.</exception>
        /// <param name="stream">Source stream.</param>
        /// <param name="bufferSize">Byte size of the buffer to read.</param>
        /// <param name="statusCallback">Callback event fires each time the buffer is read from.</param>
        /// <param name="args">Parameters to pass to the statusCallback event.</param>
        /// <returns>Array of byte.</returns>
        public static byte[] WriteToByteArray(this Stream stream, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamReadProgressCallback statusCallback = null, params Object[] args)
        {
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsMinimum(bufferSize, 1, nameof(bufferSize));

            var buffer = new byte[bufferSize];
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    var read = stream.Read(buffer, 0, bufferSize);
                    if (read <= 0)
                        return ms.ToArray();

                    ms.Write(buffer, 0, read);

                    // Fire the status callback event.
                    if (statusCallback != null)
                    {
                        if (stream.CanSeek)
                            statusCallback(read, stream.Length, args);
                        else
                            statusCallback(read, -1, args);
                    }
                }
            }
        }
    }
}
