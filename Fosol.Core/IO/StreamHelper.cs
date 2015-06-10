using Fosol.Core.Extensions.Streams;
using System;
using System.IO;
using System.Text;

namespace Fosol.Core.IO
{
    /// <summary>
    /// StreamHelper static class, provides methods to help with manipulating streams.
    /// </summary>
    public static class StreamHelper
    {
        #region Variables
        /// <summary>
        /// It is more efficient to use a buffer that is the same size as the internal buffer of the stream.
        /// Where the internal buffer is set to your desired block size, and to always read less than the block size. 
        /// If the size of the internal buffer was unspecified when the stream was constructed, its default size is 4 kilobytes (4096 bytes)."
        /// </summary>
        public const int DefaultBufferSize = 4096;

        /// <summary>
        /// Event is fired every time the buffer is written to the stream.
        /// </summary>
        /// <param name="readByteTotal">Number of bytes written to stream.</param>
        /// <param name="totalLength">Number of bytes that will be written to the stream.</param>
        /// <param name="data">Parameters passed through to event from caller.</param>
        public delegate void StreamWriteProgressCallback(long readByteTotal, long totalLength, params Object[] data);

        /// <summary>
        /// Event is fired every time the stream is read from.
        /// </summary>
        /// <param name="readByteTotal">Number of bytes read from the stream.</param>
        /// <param name="totalLength">Number of bytes that will be read from the stream.</param>
        /// <param name="data">Parameters passed through to event from caller.</param>
        public delegate void StreamReadProgressCallback(long readByteTotal, long totalLength, params Object[] data);
        #endregion

        #region Methods
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
        public static void CopyTo(Stream source, Stream destination, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamWriteProgressCallback statusCallback = null, params Object[] args)
        {
            source.CopyTo(destination, bufferSize, statusCallback, args);
        }

        /// <summary>
        /// Writes the stream into a string.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream.CanRead" cannot be false.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "stream" cannot be null.</exception>
        /// <param name="stream">Stream to convert into a string.</param>
        /// <returns>The contents of the string as a string value.</returns>
        public static string WriteToString(Stream stream)
        {
            return stream.WriteToString();
        }

        /// <summary>
        /// Writes the stream to a string with the specified encoding.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters "stream", and "encoding" cannot be null.</exception>
        /// <param name="stream">Stream object to read from.</param>
        /// <param name="encoding">Encoding to use when creating the string.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <returns>String value.</returns>
        public static string WriteToString(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks = false)
        {
            return stream.WriteToString(encoding, detectEncodingFromByteOrderMarks);
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
        public static string WriteToString(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks = false, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamReadProgressCallback statusCallback = null, params Object[] args)
        {
            return stream.WriteToString(encoding, detectEncodingFromByteOrderMarks, bufferSize, statusCallback, args);
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
        public static byte[] WriteToByteArray(Stream stream, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamReadProgressCallback statusCallback = null, params Object[] args)
        {
            return stream.WriteToByteArray(bufferSize, statusCallback, args);
        }
        #endregion
    }
}
