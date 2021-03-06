﻿using Fosol.Core.Validation.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fosol.Core.Validation.Property
{
    /// <summary>
    /// Assert static class provides methods to validate property values.
    /// </summary>
    public static class Assert
    {
        #region IsValid
        /// <summary>
        /// Assert the value is true.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Parameter "value" must be true.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValid(bool value, string paramName, string message, Exception innerException = null)
        {
            if (!value)
                throw new PropertyException(String.Format(message, paramName), paramName, innerException);
        }

        /// <summary>
        /// Assert the function returns true.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Parameter "function" must return true.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValid(Func<bool> function, string paramName, string message, Exception innerException = null)
        {
            if (!function())
                throw new PropertyException(String.Format(message, paramName), paramName, innerException);
        }
        #endregion

        #region IsTrue
        /// <summary>
        /// Assert the value is true.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must be true.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsTrue(bool value, string propertyName, string message = null, Exception innerException = null)
        {
            if (!value)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsTrue, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returns true.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must return true.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsTrue(Func<bool> function, string propertyName, string message = null, Exception innerException = null)
        {
            if (!function())
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsTrue, propertyName), propertyName, innerException);
        }
        #endregion

        #region IsFalse
        /// <summary>
        /// Assert the value is false.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must be false.</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsFalse(bool value, string propertyName, string message = null, Exception innerException = null)
        {
            if (value)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsFalse, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returns false.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must return false.</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsFalse(Func<bool> function, string propertyName, string message = null, Exception innerException = null)
        {
            if (function())
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsFalse, propertyName), propertyName, innerException);
        }
        #endregion

        #region AreEqual
        /// <summary>
        /// Assert the value is equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must be equal to "validValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreEqual<T>(T value, T validValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (!Object.Equals(value, validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreEqual, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must return a value equal to "validValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreEqual<T>(Func<T> function, T validValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (!Object.Equals(function(), validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreEqual, propertyName), propertyName, innerException);
        }
        #endregion

        #region AreReferenceEqual
        /// <summary>
        /// Assert the value is reference equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must be equal to "validValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreReferenceEqual<T>(T value, T validValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (!Object.ReferenceEquals(value, validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreReferenceEqual, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is reference equal to the valid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must return a value refereence equal to "validValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="validValue">Valid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreReferenceEqual<T>(Func<T> function, T validValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (!Object.ReferenceEquals(function(), validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreReferenceEqual, propertyName), propertyName, innerException);
        }
        #endregion

        #region AreNotEqual
        /// <summary>
        /// Assert the value is not equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must not be equal to "invalidValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotEqual<T>(T value, T invalidValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (Object.Equals(value, invalidValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreNotEqual, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is not equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must return a value not equal to "invalidValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotEqual<T>(Func<T> function, T invalidValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (Object.Equals(function(), invalidValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreNotEqual, propertyName), propertyName, innerException);
        }
        #endregion

        #region AreNotReferenceEqual
        /// <summary>
        /// Assert the value is not reference equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must not be equal to "invalidValue".</exception>
        /// <param name="value">Value to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotReferenceEqual<T>(T value, T invalidValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (Object.ReferenceEquals(value, invalidValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreNotReferenceEqual, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returned value is not reference equal to the invalid value.
        /// </summary>
        /// <typeparam name="T">Type of object to compare values.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must return a value not refereence equal to "invalidValue".</exception>
        /// <param name="function">Function to test for equality.</param>
        /// <param name="invalidValue">Invalid value to compare against.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void AreNotReferenceEqual<T>(Func<T> function, T invalidValue, string propertyName, string message = null, Exception innerException = null)
        {
            if (Object.ReferenceEquals(function(), invalidValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_AreNotReferenceEqual, propertyName), propertyName, innerException);
        }
        #endregion

        #region IsNull
        /// <summary>
        /// Assert value is null.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must be null.</exception>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNull<T>(T value, string propertyName, string message = null, Exception innerException = null)
        {
            if (value != null)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNull, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert function is null or returns a value that is null.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must be null or return a value that is null.</exception>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <param name="function">Function to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNull<T>(Func<T> function, string propertyName, string message = null, Exception innerException = null)
        {
            if ((function != null && function() != null) 
                || function() != null)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNull, propertyName), propertyName, innerException);
        }
        #endregion

        #region IsNotNull
        /// <summary>
        /// Assert value is not null.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNull<T>(T value, string propertyName, string message = null, Exception innerException = null)
        {
            if (value == null)
                throw new PropertyNullException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNotNull, propertyName), innerException);
        }

        /// <summary>
        /// Assert function returned value is not null.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not return null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNull<T>(Func<T> function, string propertyName, string message = null, Exception innerException = null)
        {
            if (function() == null)
                throw new PropertyNullException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNotNull, propertyName), innerException);
        }
        #endregion

        #region IsNotNullOrEmpty
        /// <summary>
        /// Assert value is not null or empty.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must not be empty.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrEmpty(string value, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, propertyName, message, innerException);
            if (value == String.Empty)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNotNullOrEmpty, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert function returned value is not null or empty.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must not return empty.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not return null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrEmpty(Func<string> function, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(function, propertyName, message, innerException);
            if (function() == String.Empty)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNotNullOrEmpty, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the collection is not null or empty.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "obj" must not be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "obj" must not be null.</exception>
        /// <param name="obj">Collection object to test.</param>
        /// <param name="propertyName">Parameter name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrEmpty(ICollection obj, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(obj, propertyName, message, innerException);
            if (obj.Count == 0)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNotNullOrEmpty_Collection, propertyName), propertyName, innerException);
        }
        #endregion

        #region IsNotNullOrWhitespace
        /// <summary>
        /// Assert value is not null, empty or whitespace.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must not be empty or whitespace.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrWhitespace(string value, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNullOrEmpty(value, propertyName, message, innerException);
            if (value.Trim() == String.Empty)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNotNullOrWhitespace, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert function returned value is not null, empty or whitespace.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" must not return empty or whitespace.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not return null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsNotNullOrWhitespace(Func<string> function, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNullOrEmpty(function, propertyName, message, innerException);
            if (function().Trim() == String.Empty)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsNotNullOrWhitespace, propertyName), propertyName, innerException);
        }
        #endregion

        #region IsValidIndexPosition
        /// <summary>
        /// Assert the indexPosition is a valid position within the specified count.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="indexPosition">Index position to test.</param>
        /// <param name="count">Number of items within collection.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(int indexPosition, int count, string propertyName, string message = null, Exception innerException = null)
        {
            if (indexPosition < 0 || indexPosition >= count)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsValidIndexPosition, propertyName), innerException);
        }

        /// <summary>
        /// Assert the function returns a valid position within the specified count.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="function">Function that returns an index position to test.</param>
        /// <param name="count">Number of items within collection.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(Func<int> function, int count, string propertyName, string message = null, Exception innerException = null)
        {
            var index_position = function();
            if (index_position < 0 || index_position >= count)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsValidIndexPosition, propertyName), innerException);
        }

        /// <summary>
        /// Assert the indexPosition is a valid position within the specified collection.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="indexPosition">Index position to test.</param>
        /// <param name="collection">Collection the index position will be compared with.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(int indexPosition, ICollection collection, string propertyName, string message = null, Exception innerException = null)
        {
            if (indexPosition < 0 || indexPosition >= collection.Count)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsValidIndexPosition, propertyName), innerException);
        }

        /// <summary>
        /// Assert the function returns a valid position within the specified collection.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "indexPosition" must be a valid index position within the collection.</exception>
        /// <param name="function">Function that returns an index position to test.</param>
        /// <param name="collection">Collection the index position will be compared with.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsValidIndexPosition(Func<int> function, ICollection collection, string propertyName, string message = null, Exception innerException = null)
        {
            var index_position = function();
            if (index_position < 0 || index_position >= collection.Count)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsValidIndexPosition, propertyName), innerException);
        }
        #endregion

        #region StartsWith
        /// <summary>
        /// Assert the value starts with the specified valid value.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must start with the specified value.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void StartsWith(string value, string validValue, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, propertyName, message, innerException);
            if (!value.StartsWith(validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_StartsWith, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returns a value that starts with the specified valid value.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" return a value that starts with the specified value.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not be null or return a null value.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void StartsWith(Func<string> function, string validValue, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(function, propertyName, message, innerException);
            if (!function().StartsWith(validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_StartsWith, propertyName), propertyName, innerException);
        }
        #endregion

        #region EndsWith
        /// <summary>
        /// Assert the value ends with the specified valid value.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must end with the specified value.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void EndsWith(string value, string validValue, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, propertyName, message, innerException);
            if (!value.EndsWith(validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_EndsWith, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the function returns a value that ends with the specified valid value.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "function" return a value that ends with the specified value.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not be null or return a null value.</exception>
        /// <param name="function">Function to test.</param>
        /// <param name="validValue">Valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void EndsWith(Func<string> function, string validValue, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(function, propertyName, message, innerException);
            if (!function().EndsWith(validValue))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_EndsWith, propertyName), propertyName, innerException);
        }
        #endregion

        #region IsMinimum
        /// <summary>
        /// Assert the value is greater than or equal to the minimum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "value" must be a value greater than or equal to the minimum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMinimum<T>(T value, T minimum, string propertyName, string message = null, Exception innerException = null)
            where T: IComparable
        {
            IsNotNull(value, propertyName, message, innerException);
            if (((IComparable)value).CompareTo(minimum) == -1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsMinimum, propertyName), innerException);
        }

        /// <summary>
        /// Assert the value is greater than or equal to the minimum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "value" must be a value greater than or equal to the minimum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMinimum<T>(Nullable<T> value, T minimum, string propertyName, string message = null, Exception innerException = null)
            where T : struct, IComparable
        {
            IsNotNull(value, propertyName, message, innerException);
            IsTrue(value.HasValue, nameof(value));
            if (((IComparable)value.Value).CompareTo(minimum) == -1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsMinimum, propertyName), innerException);
        }

        /// <summary>
        /// Assert the function returns a value greater than or equal to the minimum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not be null or return null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "function" must return a value greater than or equal to the minimum value.</exception>
        /// <param name="function">Function to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMinimum<T>(Func<T> function, T minimum, string propertyName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(function, propertyName, message, innerException);
            if (function().CompareTo(minimum) == -1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsMinimum, propertyName), innerException);
        }
        #endregion

        #region IsMaximum
        /// <summary>
        /// Assert the value is less than or equal to the maximum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "value" must be a value less than or equal to the maximum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMaximum<T>(T value, T maximum, string propertyName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(value, propertyName, message, innerException);
            if (((IComparable)value).CompareTo(maximum) == 1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsMaximum, propertyName), innerException);
        }

        /// <summary>
        /// Assert the value is less than or equal to the maximum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "value" must be a value less than or equal to the maximum value.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMaximum<T>(Nullable<T> value, T maximum, string propertyName, string message = null, Exception innerException = null)
            where T : struct, IComparable
        {
            IsNotNull(value, propertyName, message, innerException);
            IsTrue(value.HasValue, nameof(value));
            if (((IComparable)value.Value).CompareTo(maximum) == 1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsMaximum, propertyName), innerException);
        }

        /// <summary>
        /// Assert the function returns a value less than or equal to the maximum value.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not be null or return null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "function" must return a value less than or equal to the maximum value.</exception>
        /// <param name="function">Function to compare.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsMaximum<T>(Func<T> function, T maximum, string propertyName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(function, propertyName, message, innerException);
            if (function().CompareTo(maximum) == 1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsMaximum, propertyName), innerException);
        }
        #endregion

        #region IsInRange
        /// <summary>
        /// Assert the value is within the specified range of minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "value" must be a value within the specified range of minimum and maximum values.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsInRange<T>(T value, T minimum, T maximum, string propertyName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(value, propertyName, message, innerException);
            if (((IComparable)value).CompareTo(minimum) == -1 && ((IComparable)value).CompareTo(maximum) == 1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsInRange, propertyName), innerException);
        }

        /// <summary>
        /// Assert the value is within the specified range of minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "value" must be a value within the specified range of minimum and maximum values.</exception>
        /// <param name="value">Value to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsInRange<T>(Nullable<T> value, T minimum, T maximum, string propertyName, string message = null, Exception innerException = null)
            where T : struct, IComparable
        {
            IsNotNull(value, propertyName, message, innerException);
            IsTrue(value.HasValue, nameof(value));
            if (((IComparable)value.Value).CompareTo(minimum) == -1 && ((IComparable)value.Value).CompareTo(maximum) == 1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsInRange, propertyName), innerException);
        }

        /// <summary>
        /// Assert the function returns a value which is within the specified range of minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "function" must not be null or return null.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyOutOfRangeException">Property "function" must return a value within the specified range of minimum and maximum values.</exception>
        /// <param name="function">Function to compare.</param>
        /// <param name="minimum">Miminum valid value.</param>
        /// <param name="maximum">Maximum valid value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsInRange<T>(Func<T> function, T minimum, T maximum, string propertyName, string message = null, Exception innerException = null)
            where T : IComparable
        {
            IsNotNull(function, propertyName, message, innerException);
            var value = function();
            if (value.CompareTo(minimum) == -1 && value.CompareTo(maximum) == 1)
                throw new PropertyOutOfRangeException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsInRange, propertyName), innerException);
        }
        #endregion

        #region IsType
        /// <summary>
        /// Assert the value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must be of the specified type.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validType">Valid type.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsType<T>(T value, Type validType, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, propertyName, message, innerException);
            if (value.GetType() != validType)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsType, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the type is of the specified type.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "type" must be of the specified type.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "type" must not be null.</exception>
        /// <param name="type">Type to test.</param>
        /// <param name="validType">Valid type.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsType(Type type, Type validType, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(type, propertyName, message, innerException);
            if (type != validType)
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsType, propertyName), propertyName, innerException);
        }
        #endregion

#if !WINDOWS_APP && !WINDOWS_PHONE_APP
        #region IsAssignable
        /// <summary>
        /// Assert the value is of an assignable type.
        /// </summary>
        /// <typeparam name="T">Type of value to compare.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "value" must be an assignable type.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "value" must not be null.</exception>
        /// <param name="value">Value to test.</param>
        /// <param name="validType">Valid assignable type.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsAssignable<T>(T value, Type validType, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(value, propertyName, message, innerException);
            if (validType.IsAssignableFrom(value.GetType()))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsAssignable, propertyName), propertyName, innerException);
        }

        /// <summary>
        /// Assert the type specified is of an assignable type.
        /// </summary>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "type" must be an assignable type.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "type" must not be null.</exception>
        /// <param name="type">Type to test.</param>
        /// <param name="validType">Valid assignable type.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void IsAssignable(Type type, Type validType, string propertyName, string message = null, Exception innerException = null)
        {
            IsNotNull(type, propertyName, message, innerException);
            if (validType.IsAssignableFrom(type))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_IsAssignable, propertyName), propertyName, innerException);
        }
        #endregion

        #region HasAttribute
        /// <summary>
        /// Assert the object has the specified attribute defined.
        /// </summary>
        /// <typeparam name="T">Type of object to test.</typeparam>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyException">Property "obj" must contain the specified attribute type.</exception>
        /// <exception cref="Fosol.Core.Validation.Exceptions.PropertyNullException">Property "obj" must not be null.</exception>
        /// <param name="obj">Object to test.</param>
        /// <param name="attributeType">Attribute type to look for.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="inherit">Whether to look for the attribute type within the ancestory of the object.</param>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="innerException">The exception that originally was thrown.</param>
        public static void HasAttribute<T>(T obj, Type attributeType, string propertyName, bool inherit = false, string message = null, Exception innerException = null)
        {
            IsNotNull(obj, propertyName, message, innerException);
            if (!Fosol.Core.Extensions.Generics.GenericExtensions.HasAttribute<T>(obj, attributeType, inherit))
                throw new PropertyException(String.Format(message ?? Resources.Multilingual.Validation_Property_Assert_HasAttribute, propertyName), propertyName, innerException);
        }
        #endregion
#endif
    }
}
