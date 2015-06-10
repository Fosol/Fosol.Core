using System;
using System.Threading;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// ThreadNameElement sealed class, provides a way to render the current thread name.
    /// </summary>
    [Element("threadName")]
    public sealed class ThreadNameElement
        : DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ThreadNameElement object.
        /// </summary>
        public ThreadNameElement()
            : base()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the thread name.
        /// </summary>
        /// <param name="data">Information object containing data for the keyword.</param>
        /// <returns>Thread name.</returns>
        public override string Render(object data)
        {
            return Thread.CurrentThread.Name;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
