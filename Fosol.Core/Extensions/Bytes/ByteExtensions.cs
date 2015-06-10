using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Core.Extensions.Bytes
{
    /// <summary>
    /// ByteExtensions static class, provides extension methods for Byte objects.
    /// </summary>
    public static class ByteExtensions
    {
        #region Byte Extensions
        /// <summary>
        /// Converts a number into a hex value.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">Number to convert.</param>
        /// <returns>Hex value that represents the number.</returns>
        public static string ToHex(this byte value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return Convert.ToString(value, 16);
        }
        #endregion

        #region Byte[] Extensions
        /// <summary>
        /// Returns a hex value for the specified byte array.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">Byte array to convert to a hex value.</param>
        /// <returns>Hex value represents the byte array.</returns>
        public static string ToHex(this byte[] value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));

            return BitConverter.ToString(value).Replace("-", "");
        }

        /// <summary>
        /// Writes the data into the specified stream.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "stream" cannot be readonly.</exception>
        /// <exception cref="System.ArgumentException">Parameter "bufferSize" cannot be 0.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "stream" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "bufferSize" cannot be less than 1 or greater than the size of the data.</exception>
        /// <param name="value">Array of byte to write into stream.</param>
        /// <param name="stream">Stream object that will receive the data.</param>
        /// <param name="bufferSize">Size of buffer to write into the stream at one time.  Default size is the global DefaultBufferSize.</param>
        /// <param name="statusCallback">Event fires every time a buffer has been written to the destination stream.</param>
        /// <param name="args">Data to pass to the statusCallback event.</param>
        public static void ToStream(this byte[] value, System.IO.Stream stream, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamWriteProgressCallback statusCallback = null, params Object[] args)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsMinimum(bufferSize, 1, nameof(bufferSize));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), Resources.Multilingual.Extensions_Bytes_ByteExtensions_ToStream_CanWrite);

            if (bufferSize > value.Length)
                bufferSize = value.Length;

            var buffer = new byte[bufferSize];
            int index = 0;
            int total = 0;
            while (true)
            {
                // Calculate what remains to be streamed into the buffer.
                if (index + bufferSize > value.Length)
                    bufferSize = value.Length - index;

                stream.Write(buffer, index, bufferSize);
                total += index;

                // Fire the status callback event.
                if (statusCallback != null)
                {
                    if (stream.CanSeek)
                        statusCallback(total, stream.Length, args);
                    else
                        statusCallback(total, -1, args);
                }

                index += bufferSize;

                // The data has been fully written into the stream.
                if (index == value.Length)
                    break;
            }
        }

        /// <summary>
        /// Converts a byte array into a string value.
        /// By default it uses the Encoding.Default to convert the byte array into a string.
        /// </summary>
        /// <param name="value">Byte array to convert into a string.</param>
        /// <param name="encoding">Encoding to use when creating the string.</param>
        /// <returns>String value.</returns>
        public static string ToStringValue(this byte[] value, Encoding encoding = null)
        {
#if WINDOWS_APP || WINDOWS_PHONE_APP
            if (encoding == null)
                encoding = Encoding.UTF8;
            return encoding.GetString(value, 0, value.Length);
#else
            if (encoding == null)
                encoding = Encoding.Default;
            return encoding.GetString(value);
#endif
        }

        /// <summary>
        /// Converts a byte array into a string value.
        /// Use this method when the source byte array was encoded with a different encoding, or you want to convert to a different encoding.
        /// </summary>
        /// <param name="value">Byte array to convert into a string.</param>
        /// <param name="sourceEncoding">Encoding the byte array was created with.</param>
        /// <param name="destEncoding">Encoding to use when creating the string.</param>
        /// <returns>String value.</returns>
        public static string ToStringValue(this byte[] value, Encoding sourceEncoding, Encoding destEncoding)
        {
            Validation.Argument.Assert.IsNotNull(sourceEncoding, nameof(sourceEncoding));
            Validation.Argument.Assert.IsNotNull(destEncoding, nameof(destEncoding));
            var buffer = Encoding.Convert(sourceEncoding, destEncoding, value);
#if WINDOWS_APP || WINDOWS_PHONE_APP
            return destEncoding.GetString(buffer, 0, value.Length);
#else
            return destEncoding.GetString(buffer);
#endif
        }

        /// <summary>
        /// Inserts the data into the destination array starting at the destIndex position.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "data" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "destIndex" must be a valid index position within the destination.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "length" must be greater than or equal to '0' and less than or equal to the length of the value array.</exception>
        /// <param name="destination">Destination byte array object.</param>
        /// <param name="data">Byte array to be copied into destination.</param>
        /// <param name="destIndex">Index position to start copying into destination array.</param>
        /// <param name="length">Length of data to append into the destination array.</param>
        /// <returns>Position within the destination array after the data has been copied.</returns>
        public static int Insert(this byte[] destination, byte[] data, int destIndex = 0, int length = 0)
        {
            return Insert(destination, data, destIndex, 0, length);
        }

        /// <summary>
        /// Inserts the data into the destination array starting at the destIndex position.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "data" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "destIndex" must be a valid index position within the destination.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "length" must be greater than or equal to '0' and less than or equal to the length of the value array.</exception>
        /// <param name="destination">Destination byte array object.</param>
        /// <param name="data">Byte array to be copied into destination.</param>
        /// <param name="destIndex">Index position to start copying into destination array.</param>
        /// <param name="dataIndex">Index position to start copying from the data array.</param>
        /// <param name="length">Length of data to append into the destination array.</param>
        /// <returns>Position within the destination array after the data has been copied.</returns>
        public static int Insert(this byte[] destination, byte[] data, int destIndex, int dataIndex, int length)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));

            // Default to the value.Length.
            if (length <= 0)
                length = data.Length - dataIndex;

            Validation.Argument.Assert.IsInRange(destIndex, 0, destination.Length - length, nameof(destIndex));
            Validation.Argument.Assert.IsInRange(length, 0, data.Length, nameof(length));
            Validation.Argument.Assert.IsValid(destination.Length < length + destIndex, nameof(data), "Parameter \"{0}\" is too large and cannot be inserted into the destination.");

            int i;
            for (i = 0; i < length; i++)
                destination[destIndex + i] = data[dataIndex + i];
            return i + destIndex;
        }

        /// <summary>
        /// Look for the first index position of the search value.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "value" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "startIndex" must be a valid index position within the data.</exception>
        /// <param name="data">Byte array to search through.</param>
        /// <param name="value">Byte array value to search for.</param>
        /// <param name="startIndex">Index position to start searching at within the data.</param>
        /// <returns>Index position of value within data, or -1 if not found.</returns>
        public static int IndexOf(this byte[] data, byte[] value, int startIndex = 0)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsInRange(startIndex, 0, data.Length, nameof(startIndex));

            if (data.Length == 0 || value.Length == 0)
                return -1;

            for (int i = startIndex; i < data.Length; i++)
                if (IsMatch(data, value, i))
                    return i;

            return -1;
        }

        /// <summary>
        /// Search for the value within the data and return all the index positions it was found.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "value" cannot be null.</exception>
        /// <param name="data">Data to search through for value.</param>
        /// <param name="value">Pattern to look for.</param>
        /// <returns>An array of index positions that the value was found.</returns>
        public static int[] IndexOfAll(this byte[] data, byte[] value)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.IsNotNull(value, nameof(value));

            if (data.Length == 0 || value.Length == 0)
                return new int[0];

            var result = new List<int>();

            for (int i = 0; i < data.Length; i++)
            {
                if (!IsMatch(data, value, ref i))
                    continue;
                result.Add(i);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Confirm whether the value is found within the data at the index position.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "value" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "index" must be a valid index position within the data.</exception>
        /// <param name="data">Data to confirm against.</param>
        /// <param name="value">Value to confirm that it is a match.</param>
        /// <param name="startIndex">Index position within the data that the value should be found at.</param>
        /// <returns>True if the value is at the index position within the data.</returns>
        public static bool IsMatch(this byte[] data, byte[] value, int startIndex = 0)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsInRange(startIndex, 0, data.Length, nameof(startIndex));

            if (value.Length > (data.Length - startIndex))
                return false;

            for (int i = 0; i < value.Length; i++)
                if (data[startIndex + i] != value[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Confirm whether the value is found within the data at the index position.
        /// Also provides a reference to the index position if the value is a match.
        /// This provides a way to avoid retracing the same positions within the data.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters "data", and "value" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "index" must be a valid index position within the data.</exception>
        /// <param name="data">Data to confirm against.</param>
        /// <param name="value">Value to confirm that it is a match.</param>
        /// <param name="startIndex">Index position within the data that the value should be found at.</param>
        /// <returns>True if the value is at the index position within the data.</returns>
        public static bool IsMatch(this byte[] data, byte[] value, ref int startIndex)
        {
            Validation.Argument.Assert.IsNotNull(data, nameof(data));
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsInRange(startIndex, 0, data.Length, nameof(startIndex));

            if (value.Length > (data.Length - startIndex))
                return false;

            for (int i = 0; i < value.Length; i++)
                if (data[startIndex + i] != value[i])
                    return false;

            // Update the index value to the end of the found search value.
            startIndex += value.Length;
            return true;
        }
        #endregion
    }
}
