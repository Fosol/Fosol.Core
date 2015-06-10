using System;
using System.Collections;
using System.Collections.Generic;

namespace Fosol.Core.Validation.Argument
{
    /// <summary>
    /// Assert static class provides methods to validate parameter values.
    /// </summary>
    public static class Assert
    {
        #region IsValid
        /// <summary>
        /// Assert the value is true.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be true.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValid(bool value, string paramName, string message, Exception innerException = null)
        {
            if (!value)
                throw new ArgumentException(String.Format(message, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returns true.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" must return true.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValid(Func<bool> function, string paramName, string message, Exception innerException = null)
        {
            if (!function())
                throw new ArgumentException(String.Format(message, paramName), paramName, innerException);
        }
        #endregion

        #region IsTrue
        /// <summary>
        /// Assert the value is true.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be true.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsTrue(bool value, string paramName, string message = null, Exception innerException = null)
        {
            if (!value)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsTrue, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returns true.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" must return true.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsTrue(Func<bool> function, string paramName, string message = null, Exception innerException = null)
        {
            if (!function())
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsTrue, paramName), paramName, innerException);
        }
        #endregion

        #region IsFalse
        /// <summary>
        /// Assert the value is false.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be false.</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsFalse(bool value, string paramName, string message = null, Exception innerException = null)
        {
            if (value)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsFalse, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returns false.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" must return false.</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsFalse(Func<bool> function, string paramName, string message = null, Exception innerException = null)
        {
            if (function())
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsFalse, paramName), paramName, innerException);
        }
        #endregion

        #region AreEqual
        /// <summary>
        /// Assert the value is equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "value" must be equal to "validValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreEqual<T>(T value, T validValue, string paramName, string message = null, Exception innerException = null)
        {
            if (!Object.Equals(value, validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreEqual, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "function" must return a value equal to "validValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreEqual<T>(Func<T> function, T validValue, string paramName, string message = null, Exception innerException = null)
        {
            if (!Object.Equals(function(), validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreEqual, paramName), paramName, innerException);
        }
        #endregion

        #region AreReferenceEqual
        /// <summary>
        /// Assert the value is reference equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "value" must be equal to "validValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreReferenceEqual<T>(T value, T validValue, string paramName, string message = null, Exception innerException = null)
        {
            if (!Object.ReferenceEquals(value, validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreReferenceEqual, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is reference equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "function" must return a value refereence equal to "validValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreReferenceEqual<T>(Func<T> function, T validValue, string paramName, string message = null, Exception innerException = null)
        {
            if (!Object.ReferenceEquals(function(), validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreReferenceEqual, paramName), paramName, innerException);
        }
        #endregion

        #region AreNotEqual
        /// <summary>
        /// Assert the value is not equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "value" must not be equal to "invalidValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotEqual<T>(T value, T invalidValue, string paramName, string message = null, Exception innerException = null)
        {
            if (Object.Equals(value, invalidValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreNotEqual, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is not equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "function" must return a value not equal to "invalidValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotEqual<T>(Func<T> function, T invalidValue, string paramName, string message = null, Exception innerException = null)
        {
            if (Object.Equals(function(), invalidValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreNotEqual, paramName), paramName, innerException);
        }
        #endregion

        #region AreNotReferenceEqual
        /// <summary>
        /// Assert the value is not reference equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "value" must not be equal to "invalidValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotReferenceEqual<T>(T value, T invalidValue, string paramName, string message = null, Exception innerException = null)
        {
            if (Object.ReferenceEquals(value, invalidValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreNotReferenceEqual, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is not reference equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "function" must return a value not refereence equal to "invalidValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotReferenceEqual<T>(Func<T> function, T invalidValue, string paramName, string message = null, Exception innerException = null)
        {
            if (Object.ReferenceEquals(function(), invalidValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_AreNotReferenceEqual, paramName), paramName, innerException);
        }
        #endregion

        #region IsNull
        /// <summary>
        /// Assert value is null.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must be null.</exception>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNull<T>(T value, string paramName, string message = null, Exception innerException = null)
        {
            if (value != null)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNull, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert function is null or returns a value that is null.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" must be null or return a value that is null.</exception>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <param name="function">Function to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNull<T>(Func<T> function, string paramName, string message = null, Exception innerException = null)
        {
            if ((function != null && function() != null) 
                || function() != null)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNull, paramName), paramName, innerException);
        }
        #endregion

        #region IsNotNull
        /// <summary>
        /// Assert value is not null.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNull<T>(T value, string paramName, string message = null, Exception innerException = null)
        {
            if (value == null)
                throw new ArgumentNullException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNotNull, paramName), innerException);
        }

        /// <summary>
        /// Assert function returned value is not null.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not return null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNull<T>(Func<T> function, string paramName, string message = null, Exception innerException = null)
        {
            if (function() == null)
                throw new ArgumentNullException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNotNull, paramName), innerException);
        }
        #endregion

        #region IsNotNullOrEmpty
        /// <summary>
        /// Assert value is not null or empty.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must not be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrEmpty(string value, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, paramName, message, innerException);
            if (value == String.Empty)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNotNullOrEmpty, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert function returned value is not null or empty.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" must not return empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not return null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrEmpty(Func<string> function, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(function, paramName, message, innerException);
            if (function() == String.Empty)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNotNullOrEmpty, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the collection is not null or empty.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "obj" must not be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "obj" must not be null.</exception>
        /// <param name="obj">Collection object to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrEmpty(ICollection obj, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(obj, paramName, message, innerException);
            if (obj.Count == 0)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNotNullOrEmpty_Collection, paramName), paramName, innerException);
        }
        #endregion

        #region IsNotNullOrWhitespace
        /// <summary>
        /// Assert value is not null, empty or whitespace.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must not be empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrWhitespace(string value, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNullOrEmpty(value, paramName, message, innerException);
            if (value.Trim() == String.Empty)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNotNullOrWhitespace, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert function returned value is not null, empty or whitespace.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" must not return empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not return null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrWhitespace(Func<string> function, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNullOrEmpty(function, paramName, message, innerException);
            if (function().Trim() == String.Empty)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsNotNullOrWhitespace, paramName), paramName, innerException);
        }
        #endregion

        #region IsValidIndexPosition
        /// <summary>
        /// Assert the indexPosition is a valid position within the specified count.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="indexPosition">Index position to test.</param>
        /// <param name="count">Number of items within collection.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(int indexPosition, int count, string paramName, string message = null, Exception innerException = null)
        {
            if (indexPosition < 0 || indexPosition >= count)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsValidIndexPosition, paramName), innerException);
        }

        /// <summary>
        /// Assert the function returns a valid position within the specified count.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="function">Function that returns an index position to test.</param>
        /// <param name="count">Number of items within collection.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(Func<int> function, int count, string paramName, string message = null, Exception innerException = null)
        {
            var index_position = function();
            if (index_position < 0 || index_position >= count)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsValidIndexPosition, paramName), innerException);
        }

        /// <summary>
        /// Assert the indexPosition is a valid position within the specified collection.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="indexPosition">Index position to test.</param>
        /// <param name="collection">Collection the index position will be compared with.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(int indexPosition, ICollection collection, string paramName, string message = null, Exception innerException = null)
        {
            if (indexPosition < 0 || indexPosition >= collection.Count)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsValidIndexPosition, paramName), innerException);
        }

        /// <summary>
        /// Assert the function returns a valid position within the specified collection.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="function">Function that returns an index position to test.</param>
        /// <param name="collection">Collection the index position will be compared with.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(Func<int> function, ICollection collection, string paramName, string message = null, Exception innerException = null)
        {
            var index_position = function();
            if (index_position < 0 || index_position >= collection.Count)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsValidIndexPosition, paramName), innerException);
        }
        #endregion

        #region StartsWith
        /// <summary>
        /// Assert the value starts with the specified valid value.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must start with the specified value.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void StartsWith(string value, string validValue, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, paramName, message, innerException);
            if (!value.StartsWith(validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_StartsWith, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returns a value that starts with the specified valid value.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" return a value that starts with the specified value.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not be null or return a null value.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void StartsWith(Func<string> function, string validValue, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(function, paramName, message, innerException);
            if (!function().StartsWith(validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_StartsWith, paramName), paramName, innerException);
        }
        #endregion

        #region EndsWith
        /// <summary>
        /// Assert the value ends with the specified valid value.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" must end with the specified value.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void EndsWith(string value, string validValue, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, paramName, message, innerException);
            if (!value.EndsWith(validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_EndsWith, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returns a value that ends with the specified valid value.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "function" return a value that ends with the specified value.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not be null or return a null value.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void EndsWith(Func<string> function, string validValue, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(function, paramName, message, innerException);
            if (!function().EndsWith(validValue))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_EndsWith, paramName), paramName, innerException);
        }
        #endregion

        #region IsMinimum
        /// <summary>
        /// Assert the value is greater than or equal to the minimum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "value" must be a value greater than or equal to the minimum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMinimum<T>(T value, T minimum, string paramName, string message = null, Exception innerException = null)
            where T: IComparable
        {
            IsNotNull(value, paramName, message, innerException);
            if (value.CompareTo(minimum) >= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsMinimum, paramName), innerException);
        }

        /// <summary>
        /// Assert the value is greater than or equal to the minimum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "value" must be a value greater than or equal to the minimum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMinimum<T>(Nullable<T> value, T minimum, string paramName, string message = null, Exception innerException = null)
            where T : struct, IComparable
        {
            IsNotNull(value, paramName, message, innerException);
            IsTrue(value.HasValue, nameof(value));
            if (value.Value.CompareTo(minimum) >= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsMinimum, paramName), innerException);
        }

        /// <summary>
        /// Assert the function returns a value greater than or equal to the minimum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not be null or return null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "function" must return a value greater than or equal to the minimum value.</exception>
        /// <param name="function">Function to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMinimum<T>(Func<T> function, T minimum, string paramName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(function, paramName, message, innerException);
            if (function().CompareTo(minimum) >= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsMinimum, paramName), innerException);
        }
        #endregion

        #region IsMaximum
        /// <summary>
        /// Assert the value is less than or equal to the maximum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "value" must be a value less than or equal to the maximum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMaximum<T>(T value, T maximum, string paramName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(value, paramName, message, innerException);
            if (value.CompareTo(maximum) <= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsMaximum, paramName), innerException);
        }

        /// <summary>
        /// Assert the value is less than or equal to the maximum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "value" must be a value less than or equal to the maximum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMaximum<T>(Nullable<T> value, T maximum, string paramName, string message = null, Exception innerException = null)
            where T : struct, IComparable
        {
            IsNotNull(value, paramName, message, innerException);
            IsTrue(value.HasValue, nameof(value));
            if (value.Value.CompareTo(maximum) <= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsMaximum, paramName), innerException);
        }

        /// <summary>
        /// Assert the function returns a value less than or equal to the maximum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not be null or return null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "function" must return a value less than or equal to the maximum value.</exception>
        /// <param name="function">Function to compare.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMaximum<T>(Func<T> function, T maximum, string paramName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(function, paramName, message, innerException);
            if (function().CompareTo(maximum) <= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsMaximum, paramName), innerException);
        }
        #endregion

        #region IsInRange
        /// <summary>
        /// Assert the value is within the specified range of minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "value" must be a value within the specified range of minimum and maximum values.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsInRange<T>(T value, T minimum, T maximum, string paramName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(value, paramName, message, innerException);
            if (value.CompareTo(minimum) >= 0 && value.CompareTo(maximum) <= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsInRange, paramName), innerException);
        }

        /// <summary>
        /// Assert the value is within the specified range of minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "value" must be a value within the specified range of minimum and maximum values.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsInRange<T>(Nullable<T> value, T minimum, T maximum, string paramName, string message = null, Exception innerException = null)
            where T : struct, IComparable
        {
            IsNotNull(value, paramName, message, innerException);
            IsTrue(value.HasValue, nameof(value));
            if (value.Value.CompareTo(minimum) >= 0 && value.Value.CompareTo(maximum) <= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsInRange, paramName), innerException);
        }

        /// <summary>
        /// Assert the function returns a value which is within the specified range of minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentNullException">Parameter "function" must not be null or return null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "function" must return a value within the specified range of minimum and maximum values.</exception>
        /// <param name="function">Function to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsInRange<T>(Func<T> function, T minimum, T maximum, string paramName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(function, paramName, message, innerException);
            var value = function();
            if (value.CompareTo(minimum) >= 0 && value.CompareTo(maximum) <= 0)
                throw new ArgumentOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsInRange, paramName), innerException);
        }
        #endregion

        #region IsType
        /// <summary>
        /// Assert the value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "value" must be of the specified type.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validType">Valid type.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsType<T>(T value, Type validType, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, paramName, message, innerException);
            if (value.GetType() != validType)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsType, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the type is of the specified type.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "type" must be of the specified type.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "type" must not be null.</exception>
        /// <param name="type">Type to test.</param>
        /// <param name="validType">Valid type.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsType(Type type, Type validType, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(type, paramName, message, innerException);
            if (type != validType)
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsType, paramName), paramName, innerException);
        }
        #endregion

#if !WINDOWS_APP && !WINDOWS_PHONE_APP
        #region IsAssignable
        /// <summary>
        /// Assert the value is of an assignable type.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "value" must be an assignable type.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validType">Valid assignable type.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsAssignable<T>(T value, Type validType, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, paramName, message, innerException);
            if (validType.IsAssignableFrom(value.GetType()))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsAssignable, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the type specified is of an assignable type.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "type" must be an assignable type.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "type" must not be null.</exception>
        /// <param name="type">Type to test.</param>
        /// <param name="validType">Valid assignable type.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsAssignable(Type type, Type validType, string paramName, string message = null, Exception innerException = null)
        {
            IsNotNull(type, paramName, message, innerException);
            if (validType.IsAssignableFrom(type))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_IsAssignable, paramName), paramName, innerException);
        }
        #endregion

        #region HasAttribute
        /// <summary>
        /// Assert the object has the specified attribute defined.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <exception cref="System.ArgumentException">Parameter "obj" must contain the specified attribute type.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "obj" must not be null.</exception>
        /// <param name="obj">Object to test.</param>
        /// <param name="attributeType">Attribute type to look for.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="inherit">Whether to look for the attribute type within the ancestory of the object.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void HasAttribute<T>(T obj, Type attributeType, string paramName, bool inherit = false, string message = null, Exception innerException = null)
        {
            IsNotNull(obj, paramName, message, innerException);
            if (!Fosol.Core.Extensions.Generics.GenericExtensions.HasAttribute<T>(obj, attributeType, inherit))
                throw new ArgumentException(String.Format(message ?? Resources.Multilingual.Validation_Argument_Assert_HasAttribute, paramName), paramName, innerException);
        }
        #endregion
#endif
    }
}
