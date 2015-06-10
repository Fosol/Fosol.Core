using System;
using System.Runtime.Serialization;

namespace Fosol.Core.ServiceModel.Faults
{
    [DataContract(Name = "ParameterOverflowFault", Namespace = "http://team.fosol.ca")]
    public sealed class ParameterOverflowFault
        : ParameterFault
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ParameterOverflowFault(string paramName, string message)
            : base(paramName, message)
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
