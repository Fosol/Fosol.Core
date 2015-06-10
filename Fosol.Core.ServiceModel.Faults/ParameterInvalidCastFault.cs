using System;
using System.Runtime.Serialization;

namespace Fosol.Core.ServiceModel.Faults
{
    [DataContract(Name = "ParameterInvalidCastFault", Namespace = "http://team.fosol.ca")]
    public sealed class ParameterInvalidCastFault
        : ParameterFault
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ParameterInvalidCastFault(string paramName, string message)
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
