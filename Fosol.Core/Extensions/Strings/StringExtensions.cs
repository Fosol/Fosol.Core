using Fosol.Core.Extensions.Bytes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace Fosol.Core.Extensions.Strings
{
    /// <summary>
    /// StringExtensions static class, provides extension methods for String objects.
    /// </summary>
    public static class StringExtensions
    {

        #region Streams
        /// <summary>
        /// Writes the string to a stream.
        /// Returns the stream filled with the string data.
        /// Remember that the stream index position will be at the end of the string.
        /// Remember to close the stream.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "data" cannot be null.</exception>
        /// <param name="value">String data to write to stream.</param>
        /// <param name="resetPosition">Whether the stream position should be reset to 0 after being written to.</param>
        /// <returns>MemoryStream object containing string value.</returns>
        public static Stream ToMemoryStream(this string value, bool resetPosition = false)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(value, nameof(value));

            var stream = new MemoryStream();
            ToStream(value, stream);

            if (resetPosition)
                stream.Position = 0;

            return stream;
        }

        /// <summary>
        /// Writes the string to the specified stream.
        /// Remember that the stream index position will be at the end of the string.
        /// Remember to close the stream.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "data" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "data" cannot be null.</exception>
        /// <param name="value">String data to write to stream.</param>
        /// <param name="stream">Stream object to receive string value.</param>
        public static void ToStream(this string value, System.IO.Stream stream)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(value, nameof(value));
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), Resources.Multilingual.Extensions_Strings_StringExtensions_ToStream_CanWrite);

            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
        }

        /// <summary>
        /// Writes the data into the specified stream with the specified encoding.
        /// </summary>
        /// <param name="data">String data to write to stream.</param>
        /// <param name="stream">Stream object to receive the string data.</param>
        /// <param name="encoding">Encoding to use when streaming the data.</param>
        /// <param name="bufferSize">Byte size of the buffer being copied at one time.  By default global DefaultBufferSize.</param>
        /// <param name="statusCallback">Event fires every time a buffer has been written to the destination stream.</param>
        /// <param name="args">Data to pass to the statusCallback event.</param>
        public static void ToStream(this string value, System.IO.Stream stream, Encoding encoding, int bufferSize = IO.StreamHelper.DefaultBufferSize, IO.StreamHelper.StreamWriteProgressCallback statusCallback = null, params Object[] args)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsNotNull(stream, nameof(stream));
            Validation.Argument.Assert.IsInRange(bufferSize, -1, value.Length, nameof(bufferSize));
            Validation.Argument.Assert.AreNotEqual(bufferSize, 0, nameof(bufferSize));
            Validation.Argument.Assert.IsValid(stream.CanWrite, nameof(stream), Resources.Multilingual.Extensions_Strings_StringExtensions_ToStream_CanWrite);

            if (bufferSize > value.Length)
                bufferSize = value.Length;

            var bytes = encoding.GetBytes(value);
            bytes.ToStream(stream, bufferSize, statusCallback);
        }
        #endregion

        #region Transform
        /// <summary>
        /// Applies the String.Format() method.
        /// </summary>
        /// <param name="format">Format text to insert the arguments.</param>
        /// <param name="arg">Argument to insert into the format text.</param>
        /// <returns>A new string with the supplied format and value.</returns>
        public static string Format(this string format, object arg)
        {
            return String.Format(format, arg);
        }

        /// <summary>
        /// Applies the String.Format() method.
        /// </summary>
        /// <param name="format">Format text to insert the arguments.</param>
        /// <param name="args">An array of arguments to insert into the format text.</param>
        /// <returns>A new string with the supplied format and values.</returns>
        public static string Format(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        /// <summary>
        /// Creates a new string an uppercases every single word based on the provided 'text' value.
        /// This method will uppercase ever word after a space.
        /// </summary>
        /// <param name="text">String value to use to create the new result.</param>
        /// <returns>A new string using title case.</returns>
        public static string ToTitleCase(this string text)
        {
            return text.ToTitleCase(new[] { ' ' });
        }

        /// <summary>
        /// Creates a new string an uppercases every single word based on the provided 'text' value.
        /// </summary>
        /// <param name="text">String value to use to create the new result.</param>
        /// <param name="separator">An array of Char which indicate new words.</param>
        /// <returns>A new string using title case.</returns>
        public static string ToTitleCase(this string text, char[] separator)
        {
            var result = new List<char>();
            var new_word = true;
            foreach (var c in text)
            {
                var is_separator = separator.Contains(c);
                if (!is_separator && new_word)
                {
                    result.Add(Char.ToUpper(c));
                    new_word = false;
                }
                else result.Add(Char.ToLower(c));

                if (is_separator) new_word = true;
            }

            return new String(result.ToArray());
        }

        /// <summary>
        /// Indent the text value the specified number of times.
        /// </summary>
        /// <param name="text">Text value to be indented</param>
        /// <param name="quantity">Number of tabs to indent the text.</param>
        /// <param name="tab">Tab value to use when indenting.  Default value is "\t".</param>
        /// <returns>Indented text value.</returns>
        public static string Indent(this string text, int quantity, string tab = "\t")
        {
            if (quantity <= 0)
                return text;

            var indent_array = new string[quantity];

            for (var i = 0; i < quantity; i++) indent_array[i] = tab;

            return indent_array.Aggregate((a, b) => a + b) + text;
        }

        /// <summary> 
        /// Removes control characters and other non-UTF-8 characters 
        /// </summary> 
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">The string to process.</param> 
        /// <returns>A string with no control characters or entities above 0x00FD.</returns> 
        public static string RemoveInvalidUTF8Characters(this string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < value.Length; i++)
            {
                ch = value[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters 
                // except tabs and new lines 
                if ((ch == 0x9)
                    || (ch == 0xA)
                    || (ch == 0xD)
                    || ((ch >= 0x20) && (ch <= 0xD7FF))
                    || ((ch >= 0xE000) && (ch <= 0xFFFD))
                    //|| ((ch >= 0x10000) && (ch <= 0x10FFFF))
                    )
                {
                    newString.Append(ch);
                }
                else
                {
                    // Replace invalid character with a space
                    newString.Append(' ');
                    //throw new Exception("Invalid Character " + ch);
                }
            }
            return newString.ToString();
        }
        #endregion

        #region Convert
        /// <summary>
        /// Converts the string value into an array of byte.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">String value to convert.</param>
        /// <param name="encoding">
        ///     Encoding object to use when converting to an array of byte.
        /// </param>
        /// <returns>Array of byte.</returns>
        public static byte[] ToByteArray(this string value, Encoding encoding = null)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(value, nameof(value));
#if WINDOWS_APP || WINDOWS_PHONE_APP
            var default_encoding = Encoding.UTF8;
#else
            var default_encoding = Encoding.Default;
#endif
            Initialization.Assert.IsNotNull<Encoding>(ref encoding, default_encoding);
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Converts a hex value into a number.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">Hex value to convert.</param>
        /// <returns>The number from the hex.</returns>
        public static byte HexToByte(this string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return byte.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// Converts a hex value into a number.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">Hex value to convert.</param>
        /// <returns>The number from the hex.</returns>
        public static int HexToInt(this string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// Converts a hex value into a number.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">Hex value to convert.</param>
        /// <returns>The number from the hex.</returns>
        public static long HexToLong(this string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return long.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// Converts a hex value into a number.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <param name="value">Hex value to convert.</param>
        /// <returns>The number from the hex.</returns>
        public static short HexToShort(this string value)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            return short.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// Split a string into an array by spliting at the specified separator using the string comparison.
        /// </summary>
        /// <param name="value">String value to split.</param>
        /// <param name="separator">Separator to look for.</param>
        /// <param name="comparison">StringComparison object.</param>
        /// <returns>A new array of string values.</returns>
        public static string[] Split(this string value, string separator, StringComparison comparison = StringComparison.InvariantCulture)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsNotNullOrEmpty(separator, nameof(separator));

            var start = 0;
            var values = new List<string>();

            for (var i = 0; i < value.Length; i++)
            {
                if (String.Compare(value.Substring(i, separator.Length), separator, comparison) == 0)
                {
                    values.Add(value.Substring(start, i - start));
                    start = i + 1;
                    i += separator.Length;
                }
            }

            if (start == 0)
                // The string did not contain any separators.
                return new string[] { value };
            else if (start < value.Length)
                // Capture the last part of the string and place it in the values.
                values.Add(value.Substring(start, value.Length - start));

            return values.ToArray();
        }

        /// <summary>
        /// Splits a string value into a list of KeyValuePair objects.
        /// </summary>
        /// <param name="value">String to split.</param>
        /// <param name="keySeparator">Key separator.</param>
        /// <param name="valueSeparator">Value separator.</param>
        /// <param name="comparison">StringComparison object.</param>
        /// <returns>New instance of a List of KeyValuePairs of type String, String.</returns>
        public static List<KeyValuePair<string, string>> SplitToKeyValuePairs(this string value, string keySeparator, string valueSeparator, StringComparison comparison = StringComparison.InvariantCulture)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsNotNullOrEmpty(keySeparator, nameof(keySeparator));
            Validation.Argument.Assert.IsNotNullOrEmpty(valueSeparator, nameof(valueSeparator));

            var values = new List<KeyValuePair<string, string>>();
            // Separate all keys.
            var key_and_values = value.Split(keySeparator, comparison);
            foreach (var kav in key_and_values)
            {
                // Separate all values.
                values.Add(kav.SplitToKeyValuePair(valueSeparator, comparison));
            }

            return values;
        }

        /// <summary>
        /// Splits a string value into a KeyValuePair object.
        /// </summary>
        /// <param name="value">String to split.</param>
        /// <param name="separator">Separator value.</param>
        /// <param name="comparison">StringComparison object.</param>
        /// <returns>New instance of a KeyValuePair object of type String, String.</returns>
        public static KeyValuePair<string, string> SplitToKeyValuePair(this string value, string separator, StringComparison comparison = StringComparison.InvariantCulture)
        {
            Validation.Argument.Assert.IsNotNull(value, nameof(value));
            Validation.Argument.Assert.IsNotNullOrEmpty(separator, nameof(separator));

            // Separate all values.
            var key_value = value.Split(separator, comparison);
            switch (key_value.Length)
            {
                case 0:
                    // Something fishy occured, return the original string as a key without any value.
                    return new KeyValuePair<string, string>(value, String.Empty);
                case 1:
                    // The original string had not separator, thus no value.
                    return new KeyValuePair<string, string>(key_value[0], string.Empty);
                case 2:
                    // This is what we want to occur.
                    return new KeyValuePair<string, string>(key_value[0], key_value[1]);
                default:
                    // For some reason there were more than two values.  Aggregate the remaining values together.
                    return new KeyValuePair<string, string>(key_value[0], key_value.Skip(1).Aggregate((v1, v2) => v1 + separator + v2));

            }
        }

        /// <summary>
        /// Splits a string value up based on the separators specified and returns a NameValueCollection object.
        /// </summary>
        /// <param name="value">String to split.</param>
        /// <param name="keySeparator">Key separator.</param>
        /// <param name="valueSeparator">Value separator.</param>
        /// <param name="comparison">StringComparison object.</param>
        /// <returns>New instance of a NameValueCollection object.</returns>
        public static NameValueCollection SplitToNameValueCollection(this string value, string keySeparator, string valueSeparator, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var kv = value.SplitToKeyValuePairs(keySeparator, valueSeparator, comparison);
            var nvc = new NameValueCollection(kv.Count);

            foreach (var v in kv)
            {
                nvc.Add(v.Key, v.Value);
            }

            return nvc;
        }
        #endregion

        #region Information
        /// <summary>
        /// Counts the number of words in the specified text.
        /// </summary>
        /// <param name="text">Text value to count.</param>
        /// <returns>Number of words in the specified text.</returns>
        public static int WordCount(this string text)
        {
            return text.WordCount(new char[] { '.', '?', '!', ' ', ';', ':', ',', '(', ')' });
        }

        /// <summary>
        /// Counts the number of words in the specified text.
        /// </summary>
        /// <param name="text">Text value to count.</param>
        /// <param name="separators">An array of char that signify a new word.</param>
        /// <param name="splitOptions">StringSplitOptions value.</param>
        /// <returns>Number of words in the specified text.</returns>
        public static int WordCount(this string text, char[] separators, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            var words = text.Split(separators, splitOptions);

            return words.Length;
        }
        #endregion
    }
}
