using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// CounterElement sealed class, provides a counter value (increases on each execution).
    /// </summary>
    [Element("counter")]
    public sealed class CounterElement
        : DynamicElement
    {
        #region Variables
        private static Dictionary<string, int> _Counters = new Dictionary<string, int>();
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A sequence name provides a way to have multiple counters.
        /// </summary>
        [DefaultValue("default")]
        [ElementProperty("counter", new string[] { "c", "count", "name" })]
        public string CounterName { get; set; }

        /// <summary>
        /// get/set - The starting value of the sequence.
        /// </summary>
        [ElementProperty("value", new string[] { "v", "val", "start" })]
        public int Value { get; set; }

        /// <summary>
        /// get/set - The value to increment each time.
        /// </summary>
        [DefaultValue(1)]
        [ElementProperty("increment", new string[] { "i", "inc" })]
        public int Increment { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CounterElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public CounterElement(StringDictionary attributes)
            : base(attributes)
        {
            lock (_Counters)
            {
                // Subtract the increment to ensure the first render results in the start value.
                // Any other instance of a counter without a unique CounterName will use the static value.
                if (!_Counters.ContainsKey(this.CounterName))
                    _Counters.Add(this.CounterName, this.Value - this.Increment);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// A counter value (increases on each execution).
        /// </summary>
        /// <param name="data">Information object containing data for the keyword.</param>
        /// <returns>A counter value (increases on each execution).</returns>
        public override string Render(object data)
        {
            return GetAndSetNextSequenceValue(this.CounterName, this.Increment).ToString();
        }

        /// <summary>
        /// Gets the named sequence and increments it.
        /// </summary>
        /// <param name="name">Name of the sequence.</param>
        /// <param name="increment">Value to increment each time.</param>
        /// <returns>Next sequence counter value.</returns>
        private static int GetAndSetNextSequenceValue(string name, int increment)
        {
            lock (_Counters)
            {
                return (_Counters[name] = _Counters[name] + increment);
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
