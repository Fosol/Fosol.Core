using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Extensions.Uris
{
    /// <summary>
    /// UriExtensions static class, provides extension methods for Uri objects.
    /// </summary>
    public static class UriExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the absolute Uri without query parameters.
        /// </summary>
        /// <param name="uri">Uri to parse.</param>
        /// <returns>Absolute Uri without query parameters.</returns>
        public static string AbsoluteUriWithoutQuery(this Uri uri)
        {
            // Include the port information.
            if (!((uri.Scheme == "http" && uri.Port == 80) 
                || (uri.Scheme == "https" && uri.Port == 443)))
                return string.Format("{0}://{1}:{2}{3}", uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath);

            return string.Format("{0}://{1}{3}", uri.Scheme, uri.Host, uri.AbsolutePath);
        }
        #endregion
    }
}
