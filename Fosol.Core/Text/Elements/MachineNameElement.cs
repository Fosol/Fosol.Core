using System;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// MachineNameElement sealed class, provides a way to output the machine name.
    /// </summary>
    [Element("machineName")]
    public sealed class MachineNameElement
        : StaticElement
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new MachineNameElement object.
        /// Initialize with MachineName.
        /// </summary>
        public MachineNameElement()
            : base(Environment.MachineName)
        {
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
