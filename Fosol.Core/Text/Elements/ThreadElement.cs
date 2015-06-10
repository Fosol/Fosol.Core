using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// ThreadElement sealed class, provides a way to output thread information within the TraceEvent object.
    /// </summary>
    [Element("thread")]
    public sealed class ThreadElement
        : DynamicElement
    {
        #region Variables
        private string _Key;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The key value for this ThreadElement.
        /// </summary>
        [DefaultValue("Name")]
        [ElementProperty("Key", new[] { "k" })]
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ThreadElement object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public ThreadElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the thread Id from the TraceEvent.
        /// </summary>
        /// <param name="data">Information object containing data for the keyword.</param>
        /// <returns>A message.</returns>
        public override string Render(object data)
        {
            var thread = Thread.CurrentThread;
            if (thread != null)
            {
                var prop = (
                    from p in thread.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    where p.Name.Equals(this.Key, StringComparison.InvariantCulture)
                    select p).FirstOrDefault();

                if (prop != null)
                    return string.Format("{0}", prop.GetValue(thread));
            }

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
