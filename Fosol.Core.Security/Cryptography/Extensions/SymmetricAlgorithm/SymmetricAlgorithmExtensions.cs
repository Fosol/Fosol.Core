using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Fosol.Core.Security.Cryptography.Extensions.SymmetricAlgorithm
{
    /// <summary>
    /// SymmetricAlgorithmExtensions static class, provides helpful methods for SymmetricAlgorithm objects.
    /// </summary>
    public static class SymmetricAlgorithmExtensions
    {
        #region Methods
        /// <summary>
        /// Gets all the valid IV bit sizes for the specified SymmetricAlgorithm.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <returns>A new instance of an array of type int containing all valid IV bit sizes.</returns>
        public static int[] GetValidIVSizes(this System.Security.Cryptography.SymmetricAlgorithm algorithm)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));

            var sizes = new List<int>();

            var legal_block_sizes = algorithm.LegalBlockSizes;
            foreach (var bs in legal_block_sizes)
            {
                for (var size = bs.MinSize; size <= bs.MaxSize; size += bs.SkipSize)
                {
                    sizes.Add(size);
                }
            }

            return sizes.ToArray();
        }

        /// <summary>
        /// Validate the IV size for the specified SymmetricAlgorithm.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="bitLength">Length of the key in bits (essentially length * 8).</param>
        /// <returns>True if the IV size is a valid size.</returns>
        public static bool ValidIVSize(this System.Security.Cryptography.SymmetricAlgorithm algorithm, int bitLength)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));

            var legal_block_sizes = algorithm.LegalBlockSizes;
            foreach (var bs in legal_block_sizes)
            {
                // Apparently a legal IV size is the same as a BlockSize divided by 8.
                if (bitLength >= bs.MinSize && bitLength <= bs.MaxSize)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Validate the IV size for the specified SymmetricAlgorithm.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameters 'algorithm' and 'iv' cannot be null.</exception>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="iv">Initialization vector value.</param>
        /// <returns>True if the IV is a valid size.</returns>
        public static bool ValidIVSize(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] iv)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(iv, nameof(iv));

            return algorithm.ValidIVSize(iv.Length * 8);
        }

        /// <summary>
        /// Gets all the valid key bit sizes for the specified SymmetricAlgorithm.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <returns>A new instance of an array of type int containing all valid key bit sizes.</returns>
        public static int[] GetValidKeySizes(this System.Security.Cryptography.SymmetricAlgorithm algorithm)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));

            var sizes = new List<int>();

            var legal_key_sizes = algorithm.LegalKeySizes;
            foreach (var ks in legal_key_sizes)
            {
                for (var size = ks.MinSize; size <= ks.MaxSize; size += ks.SkipSize)
                {
                    sizes.Add(size);
                }
            }

            return sizes.ToArray();
        }

        /// <summary>
        /// Validate the key size for the SymmetricAlgorithm.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="key">Key value.</param>
        /// <returns>True if the key is a valid size.</returns>
        public static bool ValidKeySize(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] key)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsNotNull(key, nameof(key));
            return algorithm.ValidKeySize(key.Length * 8);
        }
        #endregion

        #region Encrypt
        /// <summary>
        /// Encrypt data with the specified SymmetricAlgorithm.
        /// This method uses the Rfc2898DeriveBytes class to generate the algorithm Key and IV.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="data">Data to be encrypted.</param>
        /// <param name="password">Password used to encrypt data.</param>
        /// <param name="salt">Salt to be used to encrypt data.</param>
        /// <param name="iterations">The number of iterations for the operation.</param>
        /// <returns>Encrypted data.</returns>
        public static byte[] Encrypt(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] data, string password, byte[] salt, int iterations = 1000)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(salt, nameof(salt));

            return algorithm.Encrypt(data, new Rfc2898DeriveBytes(password, salt, iterations));
        }

        /// <summary>
        /// Encrypt data with the specified SymmetricAlgorithm.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="data">Data to be encrypted.</param>
        /// <param name="derive">DeriveBytes object.</param>
        /// <returns>Encrypted data.</returns>
        public static byte[] Encrypt(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] data, DeriveBytes derive)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));
            Fosol.Core.Validation.Argument.Assert.IsNotNull(derive, nameof(derive));

            algorithm.Key = derive.GetBytes(algorithm.KeySize);
            algorithm.IV = derive.GetBytes(algorithm.BlockSize / 8);

            return algorithm.Encrypt(data);
        }

        /// <summary>
        /// Encrypt data with the specified SymmetricAlgorithm.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="data">Data to be encrypted.</param>
        /// <param name="key">Algorithm key.</param>
        /// <param name="iv">Algorithm initialization vector.</param>
        /// <returns>Encrypted data.</returns>
        public static byte[] Encrypt(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] data, byte[] key = null, byte[] iv = null)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));

            // Ensure key size if valid.
            if (key != null && key.Length > 0)
            {
                Fosol.Core.Validation.Argument.Assert.IsTrue(algorithm.ValidKeySize(key), "key", "Parameter 'key' size is invalid.");
                algorithm.Key = key;
            }

            // Ensure iv size is valid.
            if (iv != null && iv.Length > 0)
            {
                Fosol.Core.Validation.Argument.Assert.IsTrue(algorithm.ValidIVSize(iv), "iv.Length", "Parameter 'iv' length is not a legal initialization vector size.");
                algorithm.IV = iv;
            }

            var encryptor = algorithm.CreateEncryptor();

            using (var memory_stream = new System.IO.MemoryStream())
            {
                using (var crypto_stream = new CryptoStream(memory_stream, encryptor, CryptoStreamMode.Write))
                {
                    crypto_stream.Write(data, 0, data.Length);
                }
                return memory_stream.ToArray();
            }
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Decrypt data with the specified SymmetricAlgorithm.
        /// This method uses the Rfc2898DeriveBytes class to generate the algorithm Key and IV.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="data">Data to be decrypted.</param>
        /// <param name="password">Password used to decrypt data.</param>
        /// <param name="salt">Salt to be used to decrypt data.</param>
        /// <param name="iterations">The number of iterations for the operation.</param>
        /// <returns>Decrypted data.</returns>
        public static byte[] Decrypt(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] data, string password, byte[] salt, int iterations = 1000)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(salt, nameof(salt));

            return algorithm.Decrypt(data, new Rfc2898DeriveBytes(password, salt, iterations));
        }

        /// <summary>
        /// Decrypt the data with the specified SymmetricAlgorithm.
        /// </summary>
        /// <param name="algorithm">SymmetricAlgorithm object.</param>
        /// <param name="data">Data to be decrypted.</param>
        /// <param name="derive">DeriveBytes object.</param>
        /// <returns>Decrypted data.</returns>
        public static byte[] Decrypt(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] data, DeriveBytes derive)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));
            Fosol.Core.Validation.Argument.Assert.IsNotNull(derive, nameof(derive));

            algorithm.Key = derive.GetBytes(algorithm.KeySize);
            algorithm.IV = derive.GetBytes(algorithm.BlockSize / 8);

            return algorithm.Decrypt(data);
        }

        /// <summary>
        /// Decrypt the data with the specified SymmetricAlgorithm.
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.iv(v=vs.110).aspx"/>
        /// <param name="data">Data to be decrypted.</param>
        /// <param name="key">Algorithm key.</param>
        /// <param name="iv">Algorithm initialization vector.</param>
        /// <returns>Decrypted data.</returns>
        public static byte[] Decrypt(this System.Security.Cryptography.SymmetricAlgorithm algorithm, byte[] data, byte[] key = null, byte[] iv = null)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));

            // Ensure key size if valid.
            if (key != null && key.Length > 0)
            {
                Fosol.Core.Validation.Argument.Assert.IsTrue(algorithm.ValidKeySize(key), "key", "Parameter 'key' size is invalid.");
                algorithm.Key = key;
            }

            // Ensure iv size is valid.
            if (iv != null && iv.Length > 0)
            {
                Fosol.Core.Validation.Argument.Assert.IsTrue(algorithm.ValidIVSize(iv), "iv.Length", "Parameter 'iv' length is not a legal initialization vector size.");
                algorithm.IV = iv;
            }

            var decryptor = algorithm.CreateDecryptor();

            using (var stream = new System.IO.MemoryStream())
            {
                using (var crypto_stream = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
                {
                    crypto_stream.Write(data, 0, data.Length);
                }

                return stream.ToArray();
            }
        }
        #endregion
    }
}
