using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Fosol.Core.Net.Extensions.Strings
{
    /// <summary>
    /// StringExtensions static class, provides extension methods for NET related strings.
    /// </summary>
    public static class StringExtensions
    {
        #region URIs
        /// <summary>
        /// Converts a relative URL value into an absolute URL value using the current HttpContext schema, host and port.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'url' cannot be empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'url' cannot be null.</exception>
        /// <param name="relative">Relative URL to convert to an absolute URL.</param>
        /// <returns>Absolute URL.</returns>
        public static string ToAbsolute(this string relative)
        {
            return relative.ToAbsolute(HttpContext.Current);
        }

        /// <summary>
        /// Converts a relative URL value into an absolute URL value using the specified HttpContext schema, host and port.
        /// </summary>
        /// <param name="context">HttpContext object.</param>
        /// <param name="relative">Relative URL to convert to an absolute URL.</param>
        /// <returns>Absolute URL.</returns>
        public static string ToAbsolute(this string relative, HttpContext context)
        {
            Validation.Argument.Assert.IsNotNull(context, nameof(context));

            Uri uri;
            if (Uri.TryCreate(relative, UriKind.RelativeOrAbsolute, out uri))
            {
                if (uri.IsAbsoluteUri)
                    return relative;
            }

            if (relative.StartsWith("/"))
                relative = relative.Insert(0, "~");

            if (!relative.StartsWith("~/"))
                relative = relative.Insert(0, "~/");

            var request = context.Request.Url;
            var port = request.Port != 80 ? (":" + request.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}", request.Scheme, request.Host, port, VirtualPathUtility.ToAbsolute(relative));
        }

        /// <summary>
        /// Converts the relative URL value into an absolute URL using the current application domain virtual path.
        /// If the URL passed to this method is not relative it will return the URL passed to it.
        /// </summary>
        /// <param name="relative">Relative URL to convert to an absolute URL.</param>
        /// <returns>Absolute URL.</returns>
        public static string ToAbsoluteForAppDomain(this string relative)
        {
            if (!relative.IsRelative())
                return relative;

            var virtual_domain_path = HttpRuntime.AppDomainAppVirtualPath;

            if (relative[0] == '~' && relative.Length == 1)
                return virtual_domain_path;

            // Remove last slash.
            if (relative[relative.Length - 1] == '/')
                relative = relative.Substring(0, relative.Length - 1);

            return virtual_domain_path + (relative[0] != '/' ? "/" : string.Empty) + relative;
        }

        /// <summary>
        /// Get the current requests route URL value.
        /// </summary>
        /// <param name="context">HttpContextBase of the current request.</param>
        /// <returns>Route URL.</returns>
        public static string RequestRouteUrl(this HttpContextBase context)
        {
            var route_data = RouteTable.Routes.GetRouteData(context);
            var url_helper = new System.Web.Mvc.UrlHelper(context.Request.RequestContext);
            var route_url = url_helper.RouteUrl(route_data.Values);

            return route_url;
        }

        /// <summary>
        /// Try to convert a relative URL value into an absolute URL value using the current HttpContext schema, host and port.
        /// </summary>
        /// <param name="relative">Relative URL value to convert.</param>
        /// <param name="absolute">Absolute URL value.</param>
        /// <returns>True if the relative URL can be converted into an absolute URL.</returns>
        public static bool TryToConvertToAbsolute(this string relative, out string absolute)
        {
            try
            {
                absolute = relative.ToAbsolute();
                return true;
            }
            catch
            {
                absolute = null;
                return false;
            }
        }

        /// <summary>
        /// Replaces all relative URLs within the HTML with absolute URLs based on the current request HttpContext.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceRelativeWithAbsolute(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            const string htmlPattern = "(?<attrib>\\shref|\\ssrc|\\sbackground)\\s*?=\\s*?"
                + "(?<delim1>[\"'\\\\]{0,2})(?!#|http|ftp|mailto|javascript)"
                + "/(?<url>[^\"'>\\\\]+)(?<delim2>[\"'\\\\]{0,2})";

            var htmlRegex = new Regex(htmlPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            html = htmlRegex.Replace(html, m =>
                htmlRegex.Replace(m.Value, "${attrib}=${delim1}" + ("~/" + m.Groups["url"].Value)) + "${delim2}").InternalReplaceRelativeWithAbsolute();

            const string cssPattern = "@import\\s+?(url)*['\"(]{1,2}"
                + "(?!http)\\s*/(?<url>[^\"')]+)['\")]{1,2}";

            var cssRegex = new Regex(cssPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            html = cssRegex.Replace(html, m =>
                cssRegex.Replace(m.Value, "@import url(" + ("~/" + m.Groups["url"].Value)) + ")").InternalReplaceRelativeWithAbsolute();

            return html;
        }

        /// <summary>
        /// Attempts to replace the relative URL with an absolute URL value.
        /// If it cannot convert the relative URL value into an absolute URL it will return the original relative URL.
        /// </summary>
        /// <param name="relative">Url value to convert into an absolute URL.</param>
        /// <returns>Absolute URL value or the original url value if it cannot be converted.</returns>
        private static string InternalReplaceRelativeWithAbsolute(this string relative)
        {
            string absolute_url;
            if (relative.TryToConvertToAbsolute(out absolute_url))
                return absolute_url;

            return relative;
        }
        #endregion
    }
}
