using System;
using System.Security.Cryptography;

namespace Fosol.Core.Security.Cryptography
{
    /// <summary>
    /// RandomFactory static class, provides methods to generate random values.
    /// This class is primarily to remind me how to generate random values.
    /// </summary>
    public static class RandomFactory
    {
        #region Methods
        /// <summary>
        /// Generates a random salt value to use.
        /// Uses RNGCryptoServiceProvider formula to create the salt.
        /// Use Convert.ToBase64String() method to convert the byte array to a string.
        /// </summary>
        /// <param name="saltLength">Length of the byte array.</param>
        /// <returns>A new salt value.</returns>
        public static byte[] GenerateSalt(int saltLength = 16)
        {
            var array = new byte[saltLength];
            RandomFactory.Generate(array);
            return array;
        }

        /// <summary>
        /// Generates a random value and populates the byte array provided.
        /// Uses RNGCryptoServiceProvider formula to create the salt.
        /// Use Convert.ToBase64String() method to convert the byte array to a string.
        /// </summary>
        /// <param name="data">Byte array to populate with random values.</param>
        public static void Generate(byte[] data)
        {
            RandomFactory.Generate(new RNGCryptoServiceProvider(), data);
        }

        /// <summary>
        /// Generates a random value and populates the byte array provided.
        /// Use Convert.ToBase64String() method to convert the byte array to a string.
        /// </summary>
        /// <param name="generator">RandomNumberGenerator object.</param>
        /// <param name="data">Byte array to populate with random values.</param>
        public static void Generate(RandomNumberGenerator generator, byte[] data)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(generator, nameof(generator));
            generator.GetBytes(data);
        }
        #endregion
    }
}
