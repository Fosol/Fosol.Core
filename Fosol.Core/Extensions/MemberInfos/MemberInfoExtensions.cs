using System;
using System.Reflection;

namespace Fosol.Core.Extensions.MemberInfos
{
    /// <summary>
    /// MemberInfoExtensions static class, provides extension methods for MethodInfo class objects.
    /// </summary>
    public static class MemberInfoExtensions
    {
        #region Methods
        /// <summary>
        /// Get the value from the MemberInfo object whether it is a property or a field.
        /// </summary>
        /// <param name="memberInfo">MemberInfo object.</param>
        /// <returns>Value of MemberInfo object.</returns>
        public static object GetValue(this MemberInfo memberInfo)
        {
            var pi = memberInfo as PropertyInfo;
            if (pi == null)
                return ((FieldInfo)memberInfo).GetValue(null);
            return pi.GetValue(null, null);
        }
        #endregion
    }
}
