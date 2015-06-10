using Fosol.Core.Net.Extensions.Strings;
using System;
using System.Web;

namespace Fosol.Core.Net
{
    /// <summary>
    /// UrlHelper static class, provides methods to help with URL values.
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Determines whether the specified URL is relative.
        /// </summary>
        /// <param name="url">URL value to test.</param>
        /// <returns>True if the URL is relative.</returns>
        public static bool IsRelative(string url)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(url, nameof(url));
            return url.IsRelative();
        }

        /// <summary>
        /// Determines whether the specified URL has a schema defined.
        /// </summary>
        /// <param name="url">URL value to test.</param>
        /// <returns>True if the URL has a schema.</returns>
        public static bool HasSchema(string url)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(url, nameof(url));
            return url.HasSchema();
        }

        /// <summary>
        /// Determines whether the specified URL is rooted.
        /// </summary>
        /// <param name="url">URL value to test.</param>
        /// <returns>True if the URL is rooted.</returns>
        public static bool IsRooted(string url)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(url, nameof(url));
            return url.IsRooted();
        }

        /// <summary>
        /// Converts a relative URL value into an absolute URL value using the current HttpContext schema, host and port.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'url' cannot be empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'url' cannot be null.</exception>
        /// <param name="relative">Relative URL to convert to an absolute URL.</param>
        /// <returns>Absolute URL.</returns>
        public static string ToAbsolute(string relative)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(relative, nameof(relative));
            return relative.ToAbsolute();
        }

        /// <summary>
        /// Converts a relative URL value into an absolute URL value using the specified HttpContext schema, host and port.
        /// </summary>
        /// <param name="relative">Relative URL to convert to an absolute URL.</param>
        /// <param name="context">HttpContext object.</param>
        /// <returns>Absolute URL.</returns>
        public static string ToAbsolute(string relative, HttpContext context)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(relative, nameof(relative));
            Validation.Argument.Assert.IsNotNull(context, nameof(context));

            return relative.ToAbsolute(context);
        }

        /// <summary>
        /// Converts the relative URL value into an absolute URL using the current application domain virtual path.
        /// If the URL passed to this method is not relative it will return the URL passed to it.
        /// </summary>
        /// <param name="relative">Relative URL to convert to an absolute URL.</param>
        /// <returns>Absolute URL.</returns>
        public static string ToAbsoluteForAppDomain(string relative)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(relative, nameof(relative));
            return relative.ToAbsoluteForAppDomain();
        }

        /// <summary>
        /// Get the current requests route URL value.
        /// </summary>
        /// <param name="context">HttpContextBase of the current request.</param>
        /// <returns>Route URL.</returns>
        public static string RequestRouteUrl(HttpContextBase context)
        {
            Validation.Argument.Assert.IsNotNull(context, nameof(context));
            return context.RequestRouteUrl();
        }

        /// <summary>
        /// Try to convert a relative URL value into an absolute URL value using the current HttpContext schema, host and port.
        /// </summary>
        /// <param name="relative">Relative URL value to convert.</param>
        /// <param name="absolute">Absolute URL value.</param>
        /// <returns>True if the relative URL can be converted into an absolute URL.</returns>
        public static bool TryToConvertToAbsolute(string relative, out string absolute)
        {
            return relative.TryToConvertToAbsolute(out absolute);
        }

        /// <summary>
        /// Replaces all relative URLs within the HTML with absolute URLs based on the current request HttpContext.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceRelativeWithAbsolute(string html)
        {
            Validation.Argument.Assert.IsNotNull(html, nameof(html));
            return html.ReplaceRelativeWithAbsolute();
        }
    }
}
