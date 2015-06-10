using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Converters
{
    /// <summary>
    /// EnumConverter sealed class, provides a way to convert string values into enum values.
    /// Provides a way to define an EnumConverter in an Attribute.
    /// </summary>
    /// <typeparam name="T">Type of enum.</typeparam>
    public sealed class EnumConverter<T>
        : System.ComponentModel.EnumConverter
        where T : struct, IConvertible
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of an EnumConverter object.
        /// </summary>
        public EnumConverter()
            : base(typeof(T))
        {

        }
        #endregion
    }
}
