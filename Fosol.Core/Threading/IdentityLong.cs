using System;
using System.Threading;

namespace Fosol.Core.Threading
{
    /// <summary>
    /// IdentityLong seale class, provides an atomic threadsafe way to have a global identity of type long.
    /// </summary>
    public sealed class IdentityLong
    {
        #region Variables
        private static long _Id;
        #endregion

        #region Properties
        /// <summary>
        /// get - The current identity value.
        /// </summary>
        public long Id
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
        /// <returns>The new identity Id value.</returns>
        public long Increment()
        {
            return Interlocked.Increment(ref _Id);
        }

        /// <summary>
        /// Decrement the Id.
        /// </summary>
        /// <returns>The new identity Id value.</returns>
        public long Decrement()
        {
            return Interlocked.Decrement(ref _Id);
        }

        /// <summary>
        /// Returns the Id value of the object.
        /// </summary>
        /// <param name="value">IdentityLong object.</param>
        /// <returns>Id property value.</returns>
        public static implicit operator long (IdentityLong value)
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

            if (obj.GetType() != typeof(IdentityLong))
                return false;

            return this.Id == ((IdentityLong)obj).Id;
        }

        /// <summary>
        /// Determines if the Identities are equal.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if the Id property values are equal.</returns>
        public bool Equals(IdentityLong obj)
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
        /// <returns>HashCode of the Id.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        #endregion

        #region Events
        #endregion
    }
}
