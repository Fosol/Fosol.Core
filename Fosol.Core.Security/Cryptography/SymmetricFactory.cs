using Fosol.Core.Security.Cryptography.Extensions.SymmetricAlgorithm;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Fosol.Core.Security.Cryptography
{
    /// <summary>
    /// SymmetricFactory sealed class, provides a simple interface to encrypt and decrypt data with a SymmetricAlgorithm.
    /// </summary>
    public sealed class SymmetricFactory
        : ICryptography
    {
        #region Variables
        private SymmetricAlgorithm _Algorithm;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SymmetricFactory class.
        /// 
        /// Standard SymmetricAlgorithm Types
        /// ---------------------------------
        /// Aes
        /// AesManaged
        /// AesCryptoServiceProvider
        /// DES
        /// DESCryptoServiceProvider
        /// RC2
        /// RC2CryptoServiceProvider
        /// Rijndael
        /// RijndaelManaged
        /// TripleDES
        /// TripleDESCryptoServiceProvider
        /// </summary>
        /// <param name=nameof(symmetricAlgorithmType)>SymmetricAlgorithm type name.</param>
        /// <param name=nameof(key)>SymmetricAlgorithm key value.</param>
        /// <param name=nameof(iv)>SymmetricAlgorithm IV value.</param>
        public SymmetricFactory(Type symmetricAlgorithmType, byte[] key, byte[] iv)
        {
            Fosol.Core.Validation.Argument.Assert.IsAssignable(symmetricAlgorithmType, typeof(SymmetricAlgorithm), nameof(symmetricAlgorithmType));
            _Algorithm = Fosol.Core.Reflection.ReflectionHelper.ConstructObject(symmetricAlgorithmType) as SymmetricAlgorithm;

            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidKeySize(key), nameof(key), "Parameter \"{0}\" must be a valid key size.");
            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidIVSize(iv), nameof(iv), "Parameter \"{0}\" must be a valid IV size.");
            _Algorithm.Key = key;
            _Algorithm.IV = iv;
        }

        /// <summary>
        /// Creates a new instance of a SymmetricFactory class.
        /// 
        /// Standard SymmetricAlgorithm Types
        /// ---------------------------------
        /// Aes
        /// AesManaged
        /// AesCryptoServiceProvider
        /// DES
        /// DESCryptoServiceProvider
        /// RC2
        /// RC2CryptoServiceProvider
        /// Rijndael
        /// RijndaelManaged
        /// TripleDES
        /// TripleDESCryptoServiceProvider
        /// </summary>
        /// <param name=nameof(symmetricAlgorithmType)>SymmetricAlgorithm type name.</param>
        /// <param name=nameof(password)>Password used to encrypt/decrypt data.</param>
        /// <param name=nameof(salt)>Salt used to encrypt/decrypt data.</param>
        /// <param name=nameof(iterations)>Number of types the process should be executed.</param>
        public SymmetricFactory(Type symmetricAlgorithmType, string password, byte[] salt, int iterations = 1000)
        {
            Fosol.Core.Validation.Argument.Assert.IsAssignable(symmetricAlgorithmType, typeof(SymmetricAlgorithm), nameof(symmetricAlgorithmType));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(salt, nameof(salt));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(iterations, 1, nameof(iterations));
            _Algorithm = Fosol.Core.Reflection.ReflectionHelper.ConstructObject(symmetricAlgorithmType) as SymmetricAlgorithm;

            var derive = new Rfc2898DeriveBytes(password, salt, iterations);
            _Algorithm.Key = derive.GetBytes(_Algorithm.KeySize / 8);
            _Algorithm.IV = derive.GetBytes(_Algorithm.BlockSize / 8);
        }

        /// <summary>
        /// Creates a new instance of a SymmetricFactory class.
        /// 
        /// Standard SymmetricAlgorithm Types
        /// ---------------------------------
        /// Aes
        /// AesManaged
        /// AesCryptoServiceProvider
        /// DES
        /// DESCryptoServiceProvider
        /// RC2
        /// RC2CryptoServiceProvider
        /// Rijndael
        /// RijndaelManaged
        /// TripleDES
        /// TripleDESCryptoServiceProvider
        /// </summary>
        /// <param name=nameof(symmetricAlgorithmType)></param>
        /// <param name=nameof(password)>Password used to encrypt/decrypt data.</param>
        /// <param name=nameof(salt)>Salt used to encrypt/decrypt data.</param>
        /// <param name=nameof(keyBitLength)>Length in bits of the key.</param>
        /// <param name=nameof(ivBitLength)>Length in bits of the initialization vector.</param>
        /// <param name=nameof(iterations)>Number of types the process should be executed.</param>
        public SymmetricFactory(Type symmetricAlgorithmType, string password, byte[] salt, int keyBitLength, int ivBitLength, int iterations = 1000)
        {
            Fosol.Core.Validation.Argument.Assert.IsAssignable(symmetricAlgorithmType, typeof(SymmetricAlgorithm), nameof(symmetricAlgorithmType));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(salt, nameof(salt));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(iterations, 1, nameof(iterations));
            _Algorithm = Fosol.Core.Reflection.ReflectionHelper.ConstructObject(symmetricAlgorithmType) as SymmetricAlgorithm;

            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidKeySize(keyBitLength), nameof(keyBitLength), "Parameter \"{0}\" must be a valid key size.");
            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidIVSize(ivBitLength), nameof(ivBitLength), "Parameter \"{0}\" must be a valid IV size.");
            _Algorithm.KeySize = keyBitLength;
            _Algorithm.BlockSize = ivBitLength * 8;

            var derive = new Rfc2898DeriveBytes(password, salt, iterations);
            _Algorithm.Key = derive.GetBytes(_Algorithm.KeySize / 8);
            _Algorithm.IV = derive.GetBytes(_Algorithm.BlockSize / 8);
        }

        /// <summary>
        /// Creates a new instance of a SymmetricFactory class.
        /// 
        /// Standard SymmetricAlgorithm Types
        /// ---------------------------------
        /// Aes
        /// AesManaged
        /// AesCryptoServiceProvider
        /// DES
        /// DESCryptoServiceProvider
        /// RC2
        /// RC2CryptoServiceProvider
        /// Rijndael
        /// RijndaelManaged
        /// TripleDES
        /// TripleDESCryptoServiceProvider
        /// </summary>
        /// <param name=nameof(symmetricAlgorithmType)>SymmetricAlgorithm type name.</param>
        /// <param name=nameof(key)>SymmetricAlgorithm key value.</param>
        /// <param name=nameof(iv)>SymmetricAlgorithm IV value.</param>
        public SymmetricFactory(string symmetricAlgorithmType, byte[] key, byte[] iv)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(symmetricAlgorithmType, nameof(symmetricAlgorithmType));
            _Algorithm = SymmetricAlgorithm.Create(symmetricAlgorithmType);
            Fosol.Core.Validation.Argument.Assert.IsAssignable(_Algorithm, typeof(SymmetricAlgorithm), nameof(symmetricAlgorithmType));

            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidKeySize(key), nameof(key), "Parameter \"{0}\" must be a valid key size.");
            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidIVSize(iv), nameof(iv), "Parameter \"{0}\" must be a valid IV size.");
            _Algorithm.Key = key;
            _Algorithm.IV = iv;
        }

        /// <summary>
        /// Creates a new instance of a SymmetricFactory class.
        /// 
        /// Standard SymmetricAlgorithm Types
        /// ---------------------------------
        /// Aes
        /// AesManaged
        /// AesCryptoServiceProvider
        /// DES
        /// DESCryptoServiceProvider
        /// RC2
        /// RC2CryptoServiceProvider
        /// Rijndael
        /// RijndaelManaged
        /// TripleDES
        /// TripleDESCryptoServiceProvider
        /// </summary>
        /// <param name=nameof(symmetricAlgorithmType)>SymmetricAlgorithm type name.</param>
        /// <param name=nameof(password)>Password used to encrypt/decrypt data.</param>
        /// <param name=nameof(salt)>Salt used to encrypt/decrypt data.</param>
        /// <param name=nameof(iterations)>Number of types the process should be executed.</param>
        public SymmetricFactory(string symmetricAlgorithmType, string password, byte[] salt, int iterations = 1000)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(symmetricAlgorithmType, nameof(symmetricAlgorithmType));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(salt, nameof(salt));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(iterations, 1, nameof(iterations));
            _Algorithm = SymmetricAlgorithm.Create(symmetricAlgorithmType);
            Fosol.Core.Validation.Argument.Assert.IsAssignable(_Algorithm, typeof(SymmetricAlgorithm), nameof(symmetricAlgorithmType));

            var derive = new Rfc2898DeriveBytes(password, salt, iterations);
            _Algorithm.Key = derive.GetBytes(_Algorithm.KeySize / 8);
            _Algorithm.IV = derive.GetBytes(_Algorithm.BlockSize / 8);
        }

        /// <summary>
        /// Creates a new instance of a SymmetricFactory class.
        /// 
        /// Standard SymmetricAlgorithm Types
        /// ---------------------------------
        /// Aes
        /// AesManaged
        /// AesCryptoServiceProvider
        /// DES
        /// DESCryptoServiceProvider
        /// RC2
        /// RC2CryptoServiceProvider
        /// Rijndael
        /// RijndaelManaged
        /// TripleDES
        /// TripleDESCryptoServiceProvider
        /// </summary>
        /// <param name=nameof(symmetricAlgorithmType)>SymmetricAlgorithm type name.</param>
        /// <param name=nameof(password)>Password used to encrypt/decrypt data.</param>
        /// <param name=nameof(salt)>Salt used to encrypt/decrypt data.</param>
        /// <param name=nameof(keyBitLength)>Length in bits of the key.</param>
        /// <param name=nameof(ivBitLength)>Length in bits of the initialization vector.</param>
        /// <param name=nameof(iterations)>Number of types the process should be executed.</param>
        public SymmetricFactory(string symmetricAlgorithmType, string password, byte[] salt, int keyBitLength, int ivBitLength, int iterations = 1000)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(symmetricAlgorithmType, nameof(symmetricAlgorithmType));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(salt, nameof(salt));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(iterations, 1, nameof(iterations));
            _Algorithm = SymmetricAlgorithm.Create(symmetricAlgorithmType);
            Fosol.Core.Validation.Argument.Assert.IsAssignable(_Algorithm, typeof(SymmetricAlgorithm), nameof(symmetricAlgorithmType));

            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidKeySize(keyBitLength), nameof(keyBitLength), "Parameter \"{0}\" must be a valid key size.");
            Fosol.Core.Validation.Argument.Assert.IsValid(_Algorithm.ValidIVSize(ivBitLength), nameof(ivBitLength), "Parameter \"{0}\" must be a valid IV size.");
            _Algorithm.KeySize = keyBitLength;
            _Algorithm.BlockSize = ivBitLength * 8;

            var derive = new Rfc2898DeriveBytes(password, salt, iterations);
            _Algorithm.Key = derive.GetBytes(_Algorithm.KeySize);
            _Algorithm.IV = derive.GetBytes(_Algorithm.BlockSize / 8);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Encrypt the data.
        /// </summary>
        /// <param name=nameof(data)>Data to encrypt</param>
        /// <returns>Encrypted data.</returns>
        public byte[] Encrypt(string data)
        {
            return this.Encrypt(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Encrypt the data.
        /// </summary>
        /// <param name=nameof(data)>Data to encrypt</param>
        /// <param name="encoding">The encoding to use on the data.</param>
        /// <returns>Encrypted data.</returns>
        public byte[] Encrypt(string data, Encoding encoding)
        {
            return this.Encrypt(encoding.GetBytes(data));
        }

        /// <summary>
        /// Encrypt the data.
        /// </summary>
        /// <param name=nameof(data)>Data to encrypt</param>
        /// <returns>Encrypted data.</returns>
        public byte[] Encrypt(byte[] data)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));

            return _Algorithm.Encrypt(data);
        }

        /// <summary>
        /// Decrypt the data.
        /// </summary>
        /// <param name=nameof(data)>Data to decrypt.</param>
        /// <returns>Decrypted data.</returns>
        public byte[] Decrypt(string data)
        {
            return this.Decrypt(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Decrypt the data.
        /// </summary>
        /// <param name=nameof(data)>Data to decrypt.</param>
        /// <param name="encoding">The encoding to use on the data.</param>
        /// <returns>Decrypted data.</returns>
        public byte[] Decrypt(string data, Encoding encoding)
        {
            return this.Decrypt(encoding.GetBytes(data));
        }

        /// <summary>
        /// Decrypt the data.
        /// </summary>
        /// <param name=nameof(data)>Data to decrypt.</param>
        /// <returns>Decrypted data.</returns>
        public byte[] Decrypt(byte[] data)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));

            return _Algorithm.Decrypt(data);
        }

        /// <summary>
        /// Dispose the SymmetricAlgorithm and release all resources used by it.
        /// </summary>
        public void Dispose()
        {
            _Algorithm.Clear();
            _Algorithm.Dispose();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
