using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fosol.Core
{
    /// <summary>
    /// SpecialEnum abstract class provides a way to handle complex Enum operations.
    /// 
    /// Example usage below;
    /// public sealed class MyEnum : SpecialEnum<int, MyEnum>
    /// {
    ///     [Description("one")]
    ///     public static readonly MyEnum One = new MyEnum(1);
    ///     
    ///     [Description("two")]
    ///     public static readonly MyEnum Two = new MyEnum(2);
    ///     
    ///     [Description("three")]
    ///     public static readonly MyEnum Three = new MyEnum(3);
    ///     
    ///     private MyEnum(int value)
    ///         : base(value) {}
    ///         
    ///     public static implicit operator MyEnum(int value)
    ///     {
    ///         return Convert(value); }
    ///     }
    /// }
    /// </summary>
    /// <typeparam name="TValue">Value type of each enum defined.</typeparam>
    /// <typeparam name="TDerived">Self reference to the class which inherits from SpecialEnum.</typeparam>
    public abstract class SpecialEnum<TValue, TDerived>
        : IEquatable<TDerived>, IComparable<TDerived>, IComparable, IComparer<TDerived>
        where TValue : struct, IComparable<TValue>, IEquatable<TValue>
        where TDerived : SpecialEnum<TValue, TDerived>
    {
        #region Variables
        private readonly TValue _Value;
        private static readonly SortedList<TValue, TDerived> _Values = new SortedList<TValue, TDerived>();
        private string _Name;
        private DescriptionAttribute _DescriptionAttribute;
        #endregion

        #region Properties
        /// <summary>
        /// get - The value of the enum field.
        /// </summary>
        public TValue Value
        {
            get { return _Value; }
        }

        /// <summary>
        /// get - Enumerable collection of values for this enum field.
        /// </summary>
        public static IEnumerable<TDerived> Values
        {
            get { return _Values.Values; }
        }

        /// <summary>
        /// get - The given name of the enum field value.
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// get - The description accompanying the enum field value.
        /// </summary>
        public string Description
        {
            get
            {
                if (_DescriptionAttribute != null)
                    return _DescriptionAttribute.Description;

                return _Name;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new static instance of SpecialEnum.
        /// </summary>
        static SpecialEnum()
        {
            var fields = typeof(TDerived)
                .GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                .Where(t => t.FieldType == typeof(TDerived));

            foreach (var field in fields)
            {
                field.GetValue(null);

                var instance = (TDerived)field.GetValue(null);
                instance._Name = field.Name;
                instance._DescriptionAttribute = field.GetCustomAttributes(true).OfType<DescriptionAttribute>().FirstOrDefault();
            }
        }

        /// <summary>
        /// Creates a new instance of SpecialEnum.
        /// </summary>
        /// <param name="value">Adds the specified value to the enum.</param>
        protected SpecialEnum(TValue value)
        {
            _Value = value;
            _Values.Add(value, (TDerived)this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Convert the value into an special enum value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TDerived Convert(TValue value)
        {
            return _Values[value];
        }

        /// <summary>
        /// Try to convert the value into an special enum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryConvert(TValue value, out TDerived result)
        {
            return _Values.TryGetValue(value, out result);
        }

        /// <summary>
        /// Converts a SpecialEnum into a value.
        /// </summary>
        /// <param name="value">SpecialEnum object.</param>
        public static implicit operator TValue(SpecialEnum<TValue, TDerived> value)
        {
            return value.Value;
        }

        /// <summary>
        /// Converts a value into a SpecialEnum.
        /// </summary>
        /// <param name="value">Value object.</param>
        public static implicit operator SpecialEnum<TValue, TDerived>(TValue value)
        {
            return _Values[value];
        }

        /// <summary>
        /// Converts a SpecialEnum into the derived tye.
        /// </summary>
        /// <param name="value">SpecialEnum object.</param>
        public static implicit operator TDerived(SpecialEnum<TValue, TDerived> value)
        {
            return value;
        }

        /// <summary>
        /// Returns the name of the special enum field value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _Name;
        }

        /// <summary>
        /// Determine if the object is equal to the current SpecialEnum.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>Returns true if object is equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                if (obj is TValue)
                    return Value.Equals((TValue)obj);

                if (obj is TDerived)
                    return _Values.Equals(((TDerived)obj).Value);
            }
            return false;
        }

        /// <summary>
        /// Determine if the other derived object is equal to the current SpecialEnum derived type.
        /// </summary>
        /// <param name="other">Derived object.</param>
        /// <returns>True if the other derived type is equal.</returns>
        bool IEquatable<TDerived>.Equals(TDerived other)
        {
            return Value.Equals(other.Value);
        }

        /// <summary>
        /// Get the hash code for this SpecialEnum.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Compare this SpecialEnum iwth the other derived object.
        /// </summary>
        /// <param name="other">Derived object.</param>
        /// <returns>-1 if the SpecialEnum is less than the other.  0 if the SpecialEnum is equal to the other.  1 if the SpecialEnum is greater than the other.</returns>
        int IComparable<TDerived>.CompareTo(TDerived other)
        {
            return this.Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Compare this SpecialEnum with the specified object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>-1 if the SpecialEnum is less than the object.  0 if the SpecialEnum is equal to the object.  1 if the SpecialEnum is greater than the object.</returns>
        int IComparable.CompareTo(object obj)
        {
            if (obj != null)
            {
                if (obj is TValue)
                    return this.Value.CompareTo((TValue)obj);

                if (obj is TDerived)
                    return this.Value.CompareTo(((TDerived)obj).Value);
            }

            return -1;
        }

        /// <summary>
        /// Compare this SpecialEnum derived values.
        /// </summary>
        /// <param name="x">First derived value.</param>
        /// <param name="y">Second derived value.</param>
        /// <returns>-1 if 'x' is less than 'y'.  0 if 'x' is equal to 'y'.  1 if 'x' is greater than 'y'.</returns>
        int IComparer<TDerived>.Compare(TDerived x, TDerived y)
        {
            return (x == null) ? -1 : (y == null) ? 1 : x.Value.CompareTo(y.Value);
        }

        /// <summary>
        /// Get the derived value for the specified enum field name.
        /// </summary>
        /// <param name="name">Enum field name.</param>
        /// <returns>Derived value of the specified enum field name.</returns>
        public static TDerived Parse(string name)
        {
            foreach (TDerived value in Values)
            {
                if (0 == String.Compare(value.Name, name, true))
                    return value;
            }

            return null;
        }
        #endregion
    }
}
