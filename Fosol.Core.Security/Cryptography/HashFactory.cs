using System;
using System.Security.Cryptography;

namespace Fosol.Core.Security.Cryptography
{
    /// <summary>
    /// HashFactory sealed class, provides a generic way to hash data.
    /// You can go right to the source and use the HashAlgorithms directly instead of using this class.
    /// </summary>
    public sealed class HashFactory
    {
        #region Variables
        private readonly HashAlgorithm _Algorithm;
        private readonly Type _GeneratorType;
        private int _Iterations;
        #endregion

        #region Properties
        /// <summary>
        /// get - The HashAlgorithm used for hashing data.
        /// </summary>
        public HashAlgorithm Algorithm
        {
            get { return _Algorithm; }
        }

        /// <summary>
        /// get/set - The number of iterations for the operation.
        /// Default = 1000.
        /// </summary>
        public int Iterations
        {
            get { return _Iterations; }
            set { _Iterations = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a HashFactory class.  This will use Rfc2898DeriveBytes to generate a key.
        /// 
        /// Prebuilt HashAlgorithms;
        /// HMACMD5
        /// HMACRIPEMD160
        /// HMACSHA1
        /// HMACSHA256
        /// HMACSHA384
        /// HMACSHA512
        /// MD5CryptoServiceProvide
        /// RIPEMD160Managed
        /// SHA1CryptoServiceProvider
        /// SHA1Managed
        /// SHA256Managed
        /// SHA384Managed
        /// SHA512Managed
        /// </summary>
        /// <param name="hash">HashAlgorithm object to use for hashing.</param>
        public HashFactory(HashAlgorithm hash)
            : this(hash, typeof(Rfc2898DeriveBytes))
        {

        }

        /// <summary>
        /// Creates a new instance of a HashFactory class.
        /// 
        /// Prebuilt HashAlgorithms;
        /// HMACMD5
        /// HMACRIPEMD160
        /// HMACSHA1
        /// HMACSHA256
        /// HMACSHA384
        /// HMACSHA512
        /// MD5CryptoServiceProvide
        /// RIPEMD160Managed
        /// SHA1CryptoServiceProvider
        /// SHA1Managed
        /// SHA256Managed
        /// SHA384Managed
        /// SHA512Managed
        /// 
        /// Prebuild DerivedBytes types;
        /// PasswordDerviedBytes
        /// Rfc2898DeriveBytes
        /// </summary>
        /// <param name="algorithm">HashAlgorithm object to use for hashing.</param>
        /// <param name="derivedBytes">Type of DerivedBytes object to use to generate a key.</param>
        public HashFactory(HashAlgorithm algorithm, Type derivedBytes)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(algorithm, nameof(algorithm));
            Fosol.Core.Validation.Argument.Assert.IsAssignable(derivedBytes, typeof(DeriveBytes), nameof(derivedBytes));
            _Algorithm = algorithm;
            _GeneratorType = derivedBytes;
            _Iterations = 1000;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a hash for the data provided.
        /// Appends a salt to the data to create a better hash.
        /// </summary>
        /// <param name="data">Byte array containing data to hash.</param>
        /// <param name="salt">Byte array containg salt.</param>
        /// <returns>Hashed byte array.</returns>
        public byte[] ComputeHash(byte[] data, byte[] salt)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(data, nameof(data));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(salt, nameof(salt));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(salt.Length, 16, nameof(salt), "Parameter \"{0}.Length\" must be greater than or equal to 16.");

            var generator = (DeriveBytes)Fosol.Core.Reflection.ReflectionHelper.ConstructObject(_GeneratorType, new object[] { data, salt, this.Iterations });
            var key = generator.GetBytes(data.Length + salt.Length);
            return _Algorithm.ComputeHash(key);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
