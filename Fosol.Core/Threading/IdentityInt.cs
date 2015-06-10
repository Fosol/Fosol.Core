using System;
using System.Threading;

namespace Fosol.Core.Threading
{
    /// <summary>
    /// IdentityInt sealed class, provides an atomic threadsafe way to have a global identity of type int.
    /// </summary>
    public sealed class IdentityInt
    {
        #region Variables
        private static int _Id;
        #endregion

        #region Properties
        /// <summary>
        /// get - The current identity value.
        /// </summary>
        public int Id
        {
            get { return _Id; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Increment the Id.
        /// </summary>
        /// <returns>The new identity.</returns>
        public int Increment()
        {
            return Interlocked.Increment(ref _Id);
        }

        /// <summary>
        /// Decrement the Id.
        /// </summary>
        /// <returns></returns>
        public int Decrement()
        {
            return Interlocked.Decrement(ref _Id);
        }

        /// <summary>
        /// Returns the Id value of the object.
        /// </summary>
        /// <param name="value">IdentityInt object.</param>
        /// <returns>Id property value.</returns>
        public static implicit operator int (IdentityInt value)
        {
            return value.Id;
        }

        /// <summary>
        /// Determines if the Identities are equal.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if the Id property values are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != typeof(IdentityInt))
                return false;

            return this.Id == ((IdentityInt)obj).Id;
        }

        /// <summary>
        /// Determines if the Identities are equal.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if the Id property values are equal.</returns>
        public bool Equals(IdentityInt obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            return this.Id == obj.Id;
        }

        /// <summary>
        /// Returns the hash code for the Id.
        /// </summary>
        /// <returns>Unique hashcode for the Id.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        #endregion

        #region Events
        #endregion
    }
}
