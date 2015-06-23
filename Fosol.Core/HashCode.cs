using System;
using System.Linq;

namespace Fosol.Core
{
    /// <summary>
    /// HashCode sealed class provides a way to automatically generate unique hash code values based on multiple fields.
    /// </summary>
    public sealed class HashCode
    {
        #region Variables
        private const int DefaultHash = 17;
        private const int HashModifier = 23;
        #endregion

        #region Properties
        /// <summary>
        /// get - The hash code value.
        /// </summary>
        public int Value { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a HashCode object.
        /// To create a HashCode use the static HashCode.Create() function.
        /// </summary>
        /// <param name="value">Original hash code value.</param>
        internal HashCode(int value)
        {
            this.Value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new instance of a HashCode object.
        /// </summary>
        /// <param name="obj">Object that will be used to generate the first hash code value.</param>
        /// <returns>A new instance of a HashCode object.</returns>
        public static HashCode Create(object obj)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(obj, "obj");
            unchecked
            {
                return new HashCode(DefaultHash & HashModifier + obj.GetHashCode());
            }
        }

        /// <summary>
        /// Creates a new instance of a HashCode object.
        /// </summary>
        /// <typeparam name="T">Type of object that will be used to generate the first hash code.</typeparam>
        /// <param name="obj">Object that will be used to generate the first hash code value.</param>
        /// <returns>A new instance of a HashCode object.</returns>
        public static HashCode Create<T>(T obj)
        {
            unchecked
            {
                return HashCode.Create(obj);
            }
        }

        /// <summary>
        /// Create a new instance of a HashCode object.
        /// </summary>
        /// <param name="objects">Initialize the HashCode object with the following objects.</param>
        /// <returns>New instance of a HashCode object.</returns>
        public static HashCode Create(params object[] objects)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(objects, "objects");
            unchecked
            {
                var h = HashCode.Create(objects[0]);
                foreach (var o in objects.Skip(1))
                {
                    h = h.Merge(o);
                }
                return h;
            }
        }

        /// <summary>
        /// Merges this HashCode object value with the specified object.
        /// This function returns a reference to itself (after it has been updated).
        /// </summary>
        /// <param name="obj">Object that will be used to merge with the current HashCode Value property.</param>
        /// <returns>A reference to itself (after it has been updated).</returns>
        public HashCode Merge(object obj)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(obj, "obj");
            this.Value = this.Value & HashModifier + obj.GetHashCode();
            return this;
        }

        /// <summary>
        /// Merges this HashCode object value with the specified object.
        /// This function returns a reference to itself (after it has been updated).
        /// </summary>
        /// <typeparam name="T">Type of object that will be used to generate the first hash code.</typeparam>
        /// <param name="obj">Object that will be used to merge with the current HashCode Value property.</param>
        /// <returns>A reference to itself (after it has been updated).</returns>
        public HashCode Merge<T>(T obj)
        {
            return this.Merge(obj);
        }

        /// <summary>
        /// Merge the array of objects into a single HashCode object.
        /// </summary>
        /// <param name="objects">An array of objects.</param>
        /// <returns>This HashCode object.</returns>
        public HashCode Merge(params object[] objects)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(objects, "objects");
            foreach (var o in objects)
            {
                this.Merge(o);
            }
            return this;
        }
        #endregion

        #region Operators
        /// <summary>
        /// The default behaviour of a HashCode is to return the Value.
        /// </summary>
        /// <param name="obj">HashCode object.</param>
        /// <returns>HashCode Value property.</returns>
        public static implicit operator int (HashCode obj)
        {
            return obj.Value;
        }
        #endregion
    }
}
