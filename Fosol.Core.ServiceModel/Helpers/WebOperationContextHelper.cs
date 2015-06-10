using Fosol.Core.ServiceModel.Extensions.OperationContexts;
using Fosol.Core.ServiceModel.Extensions.WebOperationContexts;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Web;

namespace Fosol.Core.ServiceModel.Helpers
{
    /// <summary>
    /// Useful methods for web services.
    /// </summary>
    public static class WebOperationContextHelper
    {
        #region Methods
        /// <summary>
        /// Sets the WebOperationContext.Current.OutgoingResponse.Format value.
        /// WebMessageFormatException - If format is not a valid WebMessageFormat.
        /// </summary>
        /// <param name="format">Requested format [XML|JSON]</param>
        public static void SetResponseFormat(string format)
        {
            WebOperationContext.Current.SetResponseFormat(format);
        }

        /// <summary>
        /// Sets the WebOperationContext.Current.OutgoingResponse.Format to the specified WebMessageFormat value.
        /// First it will check if the WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters contains a 'format' parameter.
        /// If the query parameter contains a 'format' value it will use it.
        /// If the query parameter does not contain a 'format' value it will use the 'defaultFormat' value.
        /// If the format value specified is invalid it will throw an exception.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Paramter "defaultFormat" cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter "defaultFormat" is an invalid WebMessageFormat value.</exception>
        /// <param name="defaultFormat">The default WebMessageFormat value if one is not supplied in the query parameters.</param>
        /// <param name="queryParamName">The query string parameter name used to specify the WebMessageFormat (default is 'format').</param>
        /// <returns>The WebMessageFormat that the OutgoingResponse will use.</returns>
        public static WebMessageFormat AutoResponseFormat(WebMessageFormat defaultFormat = WebMessageFormat.Xml, string queryParamName = "format")
        {
            return WebOperationContext.Current.AutoResponseFormat(defaultFormat, queryParamName);
        }

        /// <summary>
        /// Sets the WebOperationContext.Current.OutgoingResponse.SetETag() value.
        /// Throws WebFaultException with HttpWebStatusCode of 304 if the entityTag is equal to the IncomingRequest ETag.
        /// </summary>
        /// <param name="entityTag">Unique key that provides a way to determine if the data has changed since the last request</param>
        public static void SetETag(string entityTag)
        {
            WebOperationContext.Current.SetETag(entityTag);
        }

        /// <summary>
        /// Returns client ip
        /// Look up is in following sequence
        /// 1. X-Postmedia-True-Client-IP
        /// 2. True-Client-IP
        /// 3. X-Forwarded-For
        /// 4. Request Address (Dns resoloved)
        /// </summary>
        /// <returns>list of ip addresses for the client</returns>
        public static List<string> GetClientIP()
        {
            return System.ServiceModel.OperationContext.Current.GetClientIP();
        }

        /// <summary>
        /// Returns client ip with source
        /// Look up is in following sequence
        /// 1. X-Postmedia-True-Client-IP
        /// 2. True-Client-IP
        /// 3. X-Forwarded-For
        /// 4. Request Address (Dns resoloved)
        /// </summary>
        /// <returns>dictionary list of ip addresses for the client with source. Key = source, Value = ip</returns>
        public static Dictionary<string, string> GetClientIPWithSource()
        {
            return System.ServiceModel.OperationContext.Current.GetClientIPWithSource();
        }

        /// <summary>
        /// Set the outgoing response HTTP status code.
        /// </summary>
        /// <param name="context">WebOperationContext object.</param>
        /// <param name="statusCode">HttpStatusCode you want to return.</param>
        public static void SetOutgoingResponseStatusCode(HttpStatusCode statusCode)
        {
            WebOperationContext.Current.SetOutgoingResponseStatusCode(statusCode);
        }
        #endregion
    }
}
