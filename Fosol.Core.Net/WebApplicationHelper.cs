using System;
using System.Diagnostics;
using System.Web.Hosting;

namespace Fosol.Core.Net
{
    /// <summary>
    /// WebApplicationHelper static class, provides methods to assist with generic application features.
    /// </summary>
    public static class WebApplicationHelper
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Get the application name for the current process.
        /// </summary>
        /// <returns>Name of the current application running.</returns>
        public static string GetApplicationName()
        {
            try
            {
                var path = HostingEnvironment.ApplicationVirtualPath;

                if (string.IsNullOrEmpty(path))
                {
                    path = Process.GetCurrentProcess().MainModule.ModuleName;
                    var num = path.IndexOf('.');
                    if (num != -1)
                        path = path.Remove(num);
                }
                if (string.IsNullOrEmpty(path))
                {
                    return "/";
                }
                else
                    return path;
            }
            catch
            {
                return "/";
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
