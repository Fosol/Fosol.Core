using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Fosol.Core.Text.Elements
{
    /// <summary>
    /// NewlineElement sealed class, provides a a newline character.
    /// </summary>
    [Element("newline")]
    public sealed class NewlineElement
        : StaticElement
    {
        #region Variables
        public enum LineEnding
        {
            /// <summary>
            /// \r
            /// </summary>
            CR,
            /// <summary>
            /// \n
            /// </summary>
            LF,
            /// <summary>
            /// \r\n
            /// </summary>
            CRLF,
            /// <summary>
            /// Enivronment.Newline
            /// </summary>
            Default,
            /// <summary>
            /// No line ending.
            /// </summary>
            None
        }
        private LineEnding _Mode;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Line ending mode syntax.
        /// </summary>
        [DefaultValue(LineEnding.Default)]
        [ElementProperty("mode", new string[] { "m" }, typeof(EnumConverter), typeof(LineEnding))]
        public LineEnding Mode
        {
            get { return _Mode; }
            set
            {
                // Update the Text property.
                if (value != _Mode)
                    this.Text = NewlineElement.GetLineEnding(value);
                _Mode = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a NewlineElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public NewlineElement(StringDictionary attributes)
            : base(System.Environment.NewLine, attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the line ending string based on the specified mode.
        /// </summary>
        /// <param name="mode">LineEnding mode option.</param>
        /// <returns>Newline string value based on the specified mode.</returns>
        private static string GetLineEnding(LineEnding mode)
        {
            switch (mode)
            {
                case (LineEnding.CR):
                    return "\r";
                case (LineEnding.LF):
                    return "\n";
                case (LineEnding.CRLF):
                    return "\r\n";
                case (LineEnding.None):
                    return string.Empty;
                default:
                    return System.Environment.NewLine;
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
