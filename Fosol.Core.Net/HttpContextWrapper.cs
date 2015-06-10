using System;
using System.Web;

namespace Fosol.Core.Net
{
    /// <summary>
    /// HttpContextWrapper class, provides a way to unit test the HttpContext.Current value.
    /// </summary>
    public class HttpContextWrapper
        : System.Web.HttpContextWrapper
    {
        #region Variables
        private static HttpContextBase _Current;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The current HttpContext value.
        /// </summary>
        public static HttpContextBase Current
        {
            get
            {
                if (_Current != null)
                    return _Current;

                _Current = new HttpContextWrapper(HttpContext.Current);
                return _Current;
            }
            set
            {
                _Current = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a HttpContextWrapper class.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        public HttpContextWrapper(HttpContext httpContext)
            : base(httpContext)
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
