using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace Fosol.Core.ServiceModel.Extensions.WebOperationContexts
{
    /// <summary>
    /// Helpful extension methods for WebOperationContext object.
    /// </summary>
    public static class WebOperationContextExtensions
    {
        #region Methods
        /// <summary>
        /// Sets the WebOperationContext.Current.OutgoingResponse.Format value.
        /// WebMessageFormatException - If format is not a valid WebMessageFormat.
        /// </summary>
        /// <exception cref="System.ServiceModel.Web.WebFaultException<WebFault>">The format must be a value WebMessageFormat value.</exception>
        /// <param name="context">WebOperationContext object.</param>
        /// <param name="format">Requested format [XML|JSON]</param>
        public static void SetResponseFormat(this WebOperationContext context, string format)
        {
            try
            {
                context.OutgoingResponse.Format = ((WebMessageFormat)Enum.Parse(typeof(WebMessageFormat), format, true));
            }
            catch (Exception ex)
            {
                WebFaultContract.RaiseFault("Invalid format. XML or JSON[JSONP] are only supported.", ex, HttpStatusCode.BadRequest);
            }
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
        /// <param name="context">WebOperationContext object.</param>
        /// <param name="defaultFormat">The default WebMessageFormat value if one is not supplied in the query parameters.</param>
        /// <param name="queryParamName">The query string parameter name used to specify the WebMessageFormat (default is 'format').</param>
        /// <returns>The WebMessageFormat that the OutgoingResponse will use.</returns>
        public static WebMessageFormat AutoResponseFormat(this WebOperationContext context, WebMessageFormat defaultFormat = WebMessageFormat.Xml, string queryParamName = "format")
        {
            var format = defaultFormat;

            // An Accept header can contain multiple content types.
            var accept_header = context.IncomingRequest.Headers["Accept"];
            if (!string.IsNullOrEmpty(accept_header))
            {
                var accepts = accept_header.Split(',');

                // A valid type is [json|xml].
                foreach (var accept in accepts)
                {
                    var type = accept.Trim();
                    if (type.EndsWith("json", StringComparison.InvariantCulture) && type.Length >= 4 && "/+".Contains(type.Substring(type.Length - 5, 1)))
                    {
                        format = WebMessageFormat.Json;
                        break;
                    }
                    else if (type.EndsWith("xml", StringComparison.InvariantCulture) && type.Length >= 3 && "/+".Contains(type.Substring(type.Length - 4, 1)))
                    {
                        format = WebMessageFormat.Xml;
                        break;
                    }
                }
            }

            // Check if the parameter exists in the query, if it does, use it.
            // Valid query format values [Xml|Json].
            if (context.IncomingRequest.UriTemplateMatch != null)
            {
                var query_format = context.IncomingRequest.UriTemplateMatch.QueryParameters[queryParamName];
                // If the format is invalid throw exception.
                if (!string.IsNullOrEmpty(query_format))
                    Fosol.Core.ServiceModel.Validation.Assert.IsValue(Enum.TryParse<WebMessageFormat>(query_format, true, out format), true, "format");
            }

            context.OutgoingResponse.Format = format;
            return format;
        }

        /// <summary>
        /// Sets the WebOperationContext.Current.OutgoingResponse.SetETag() value.
        /// Throws WebFaultException with HttpWebStatusCode of 304 if the entityTag is equal to the IncomingRequest ETag.
        /// </summary>
        /// <param name="context">WebOperationContext object.</param>
        /// <param name="entityTag">Unique key that provides a way to determine if the data has changed since the last request</param>
        public static void SetETag(this WebOperationContext context, string entityTag)
        {
            context.OutgoingResponse.SetETag(entityTag);

            // Need to manually check the Conditional GET so that the StatusCode is correctly returned for JSONP requests.
            if (context.IncomingRequest.Headers["If-None-Match"] == string.Format("\"{0}\"", entityTag))
                WebFaultContract.RaiseFault("Content not modified.", HttpStatusCode.NotModified);
            //context.IncomingRequest.CheckConditionalRetrieve(entityTag);
        }

        /// <summary>
        /// Set the outgoing response HTTP status code.
        /// </summary>
        /// <param name="context">WebOperationContext object.</param>
        /// <param name="statusCode">HttpStatusCode you want to return.</param>
        public static void SetOutgoingResponseStatusCode(this WebOperationContext context, HttpStatusCode statusCode)
        {
            context.OutgoingResponse.StatusCode = statusCode;
        }
        #endregion
    }
}
