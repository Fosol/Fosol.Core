using System;
using System.Runtime.Serialization;

namespace Fosol.Core.ServiceModel.Faults
{
    [DataContract(Name = "ParameterNullFault", Namespace = "http://team.fosol.ca")]
    public sealed class ParameterNullFault
        : ParameterFault
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ParameterNullFault(string parameterName, string message)
            : base(parameterName, message)
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
