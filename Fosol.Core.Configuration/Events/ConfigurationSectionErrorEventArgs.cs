using System;
using System.IO;

namespace Fosol.Core.Configuration.Events
{

    /// <summary>
    /// ConfigurationSectionErrorEventArgs sealed class, provides a way to pass exceptions that occur while loading the configuration.
    /// </summary>
    public sealed class ConfigurationSectionErrorEventArgs
        : EventArgs
    {
        #region Variables
        private Exception _Exception;
        private FileSystemEventArgs _EventArgs;
        #endregion

        #region Properties
        /// <summary>
        /// get - The exception that was thrown.
        /// </summary>
        public Exception Exception
        {
            get { return _Exception; }
            private set { _Exception = value; }
        }

        /// <summary>
        /// get - The FileSystemEventArgs that originally caused the exception.
        /// </summary>
        public FileSystemEventArgs EventArgs
        {
            get { return _EventArgs; }
            private set { _EventArgs = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ConfigurationSectionErrorEventArgs.
        /// </summary>
        /// <param name="exception">Exception that was thrown.</param>
        public ConfigurationSectionErrorEventArgs(Exception exception)
            : this(exception, null)
        {
        }

        /// <summary>
        /// Creates a new instance of a ConfigurationSectionErrorEventArgs.
        /// </summary>
        /// <param name="exception">Exception that was thrown.</param>
        /// <param name="e">FileSystemEventArgs object.</param>
        public ConfigurationSectionErrorEventArgs(Exception exception, FileSystemEventArgs e)
        {
            this.Exception = exception;
            this.EventArgs = e;
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
