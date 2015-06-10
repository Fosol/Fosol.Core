using System;
using System.Diagnostics;

namespace Fosol.Core.Diagnostics
{
    /// <summary>
    /// DiagnosticHelper static class, provides methods for diagnostics.
    /// </summary>
    public static class DiagnosticHelper
    {
        #region Methods
        /// <summary>
        /// Watch how long it takes to execute the specified action.
        /// </summary>
        /// <param name="action">Action to watch execute.</param>
        /// <returns>Length of time to execute the specified action.</returns>
        public static TimeSpan Watch(Action action)
        {
            var stop_watch = Stopwatch.StartNew();
            action();
            stop_watch.Stop();
            return stop_watch.Elapsed;
        }
        #endregion
    }
}
