using System;

namespace Fosol.Core.Initialization
{
    /// <summary>
    /// MemberKey private sealed class, provides a way to identify a GlobalDefaults value or instance.
    /// </summary>
    sealed class MemberKey
    {
        #region Properties
        /// <summary>
        /// get - The type of the default value or instance.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// get - A unique name to identify the default value or instance.
        /// </summary>
        public string Name { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a MemberKey class.
        /// </summary>
        /// <param name="type">Type of the default value or instance.</param>
        public MemberKey(Type type)
            : this(type, null)
        {
        }

        /// <summary>
        /// Creates a new instance of a MemberKey class.
        /// </summary>
        /// <param name="type">Type of the default value or instance.</param>
        /// <param name="name">Unique name to identify the default value or instance.</param>
        public MemberKey(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a unqiue hashcode to identify this MemberKey.
        /// </summary>
        /// <returns>A unique hashcode to identify this MemberKey.</returns>
        public override int GetHashCode()
        {
            return HashCode.Create(this.Type, this.Name).Value;
        }
        #endregion
    }
}
