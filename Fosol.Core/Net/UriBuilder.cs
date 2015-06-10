using System;
using System.Text.RegularExpressions;

namespace Fosol.Core.Net
{
    /// <summary>
    /// UriBuilder sealed class, provides a way to parse URI values into logical parts.
    /// - The reason for this class is to provide a clean and simple way to deal with URI values.
    /// - The current .NET classes and methods do not provide a decent way to manage query string parameters.
    /// 
    /// Contains a number of regex string values to validate the different parts of URI value.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc3986"/>
    public sealed class UriBuilder
    {
        #region Variables
        public const string ReservedGenDelimsRegex = @"[:/\?#\[@\]]";
        public const string ReservedSubDelimsRegex = @"[!\$&'\(\)\*\+,;=]";
        public const string UnreservedCharactersRegex = @"(?i)[a-z0-9-\._~]";
        public const string PercentEncodedRegex = "%(?i)[0-9a-z]{2}";
        public const string SchemeRegex = @"(?i)([a-z]([a-z0-9\+\-\.])*)";
        public const string UserInfoRegex = "(" + UnreservedCharactersRegex + "|" + PercentEncodedRegex + "|" + ReservedSubDelimsRegex + ")+";
        public const string IPv4SegmentRegex = "(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])";
        public const string IPv4AddressRegex = "(" + IPv4SegmentRegex + @"\.){3,3}" + IPv4SegmentRegex;
        public const string IPv6SegmentRegex = "(?i)[0-9a-f]{1,4}";
        public const string IPv6AddressRegex = "("
                                            + "(" + IPv6SegmentRegex + ":){7,7}" + IPv6SegmentRegex + "|"
                                            + "(" + IPv6SegmentRegex + ":){1,7}:|"
                                            + "(" + IPv6SegmentRegex + ":){1,6}:" + IPv6SegmentRegex + "|"
                                            + "(" + IPv6SegmentRegex + ":){1,5}(:" + IPv6SegmentRegex + "){1,2}|"
                                            + "(" + IPv6SegmentRegex + ":){1,4}(:" + IPv6SegmentRegex + "){1,3}|"
                                            + "(" + IPv6SegmentRegex + ":){1,3}(:" + IPv6SegmentRegex + "){1,4}|"
                                            + "(" + IPv6SegmentRegex + ":){1,2}(:" + IPv6SegmentRegex + "){1,5}|"
                                            + IPv6SegmentRegex + ":((:" + IPv6SegmentRegex + "){1,6})|"
                                            + ":((:" + IPv6SegmentRegex + "){1,7}|:)|"
                                            + ":((:" + IPv6SegmentRegex + "){1,7}|:)|"
                                            + "fe80:(:" + IPv6SegmentRegex + "){0,4}%(?i)[0-9a-z]{1,}|"
                                            + "::(ffff(:0{1,4}){0,1}:){0,1}" + IPv4SegmentRegex + "|"
                                            + "(" + IPv6SegmentRegex + ":){1,4}:" + IPv4SegmentRegex + ""
                                            + ")";
        public const string HostRegex = "(" + UnreservedCharactersRegex + "|" + PercentEncodedRegex + "|" + ReservedSubDelimsRegex + ")+";
        public const string PortRegex = "[0-9]+";
        public const string PathSegmentRegex = "(" + UnreservedCharactersRegex + "|" + PercentEncodedRegex + "|" + ReservedSubDelimsRegex + "|[:@])+";
        public const string QueryRegex = "(" + UnreservedCharactersRegex + "|" + PercentEncodedRegex + "|" + ReservedSubDelimsRegex + @"|[:@/\?])+";
        public const string FragmentRegex = "(" + UnreservedCharactersRegex + "|" + PercentEncodedRegex + "|" + ReservedSubDelimsRegex + @"|[:@/\?])+";
        public const int MaximumURILength = 2048;
        private string _Scheme;
        private string _Username;
        private string _Host;
        private int _Port = 80;
        private UriPath _Path;
        private UriQuery _Query;
        private string _Fragment;
        private Uri _Uri;
        private bool _IsDirty = true;

        private const string _WhitespaceEncoding = "%20";
        private const string _FormatBoundary = @"\A({0})\Z";
#if WINDOWS_APP || WINDOWS_PHONE_APP
        private static readonly Regex _SchemeRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.SchemeRegex), RegexOptions.None);
        private static readonly Regex _UserInfoRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.UserInfoRegex), RegexOptions.None);
        private static readonly Regex _IPv4AddressRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.IPv4AddressRegex), RegexOptions.None);
        private static readonly Regex _IPv6AddressRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.IPv6AddressRegex), RegexOptions.None);
        private static readonly Regex _HostRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.HostRegex), RegexOptions.None);
        private static readonly Regex _PortRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.PortRegex), RegexOptions.None);
        private static readonly Regex _FragmentRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.FragmentRegex), RegexOptions.None);
        private static readonly Regex _EncodeSpaces = new Regex(@"\s", RegexOptions.None);
#else
        private static readonly Regex _SchemeRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.SchemeRegex), RegexOptions.Compiled);
        private static readonly Regex _UserInfoRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.UserInfoRegex), RegexOptions.Compiled);
        private static readonly Regex _IPv4AddressRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.IPv4AddressRegex), RegexOptions.Compiled);
        private static readonly Regex _IPv6AddressRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.IPv6AddressRegex), RegexOptions.Compiled);
        private static readonly Regex _HostRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.HostRegex), RegexOptions.Compiled);
        private static readonly Regex _PortRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.PortRegex), RegexOptions.Compiled);
        private static readonly Regex _FragmentRegex = new Regex(String.Format(_FormatBoundary, UriBuilder.FragmentRegex), RegexOptions.Compiled);
        private static readonly Regex _EncodeSpaces = new Regex(@"\s", RegexOptions.Compiled);
#endif
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Uri scheme value. 
        /// </summary>
        /// <format>ALPHA *( ALPHA / DIGIT / "+" / "-" / "." )</format>
#if WINDOWS_APP
        /// <exception cref="System.FormatException">Property cannot contain invalid characters.</exception>
#else
        /// <exception cref="System.UriFormatException">Property cannot contain invalid characters.</exception>
#endif
        public string Scheme
        {
            get { return _Scheme; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
#if WINDOWS_APP || WINDOWS_PHONE_APP
                    throw new FormatException("Scheme value has invalid characters.");
#else
                    throw new UriFormatException("Scheme value has invalid characters.");
#endif

                var match = _SchemeRegex.Match(value);

                if (!match.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                    throw new FormatException("Scheme value has invalid characters.");
#else
                    throw new UriFormatException("Scheme value has invalid characters.");
#endif

                _Scheme = value;
                _IsDirty = true;
            }
        }

        /// <summary>
        /// get/set - Uri authority value [userinfo@]host[:port]
        /// </summary>
        /// <format>[ userinfo "@" ] host [ ":" port ]</format>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        /// <exception cref="System.FormatException">Property cannot contain invalid characters.</exception>
#else
        /// <exception cref="System.UriFormatException">Property cannot contain invalid characters.</exception>
#endif
        public string Authority
        {
            get { return String.Format("{0}{1}", _Host, (_Port != 80 ? ":" + _Port : String.Empty)); }
            set
            {
                if (String.IsNullOrEmpty(value))
                    this.Host = value;

                // Extract the user information.
                var index_of_at = value.IndexOf('@');
                if (index_of_at != -1)
                {
                    if (index_of_at + 1 >= value.Length)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                        throw new FormatException("Host value is invalid.");
#else
                        throw new UriFormatException("Host value is invalid.");
#endif

                    this.Username = value.Substring(0, index_of_at);

                    // Remove the user information from the authority.
                    value = value.Substring(index_of_at + 1);
                }

                // This must be an IP address with a port #.
                if (value.StartsWith("["))
                {
                    var index_of_bracket = value.LastIndexOf(']');
                    if (index_of_bracket != -1)
                    {
                        this.Host = value.Substring(1, index_of_bracket - 1);

                        // Check for a port.
                        var index_of_colon = value.Substring(index_of_bracket + 1).IndexOf(':');
                        if (index_of_colon != -1)
                        {
                            if (index_of_colon + 1 >= value.Length)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                                throw new FormatException("Port value is invalid.");
#else
                                throw new UriFormatException("Port value is invalid.");
#endif

                            var port = value.Substring(index_of_colon + 1);

                            // Validate port.
                            var port_match = _PortRegex.Match(port);
                            if (!port_match.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                                throw new FormatException("Port value has invalid characters.");
#else
                                throw new UriFormatException("Port value has invalid characters.");
#endif

                            this.Port = Convert.ToInt32(port);
                            this.Host = value.Substring(0, index_of_colon);
                        }
                    }
                    else
#if WINDOWS_APP || WINDOWS_PHONE_APP
                        throw new FormatException("Host value has invalid characters.");
#else
                        throw new UriFormatException("Host value has invalid characters.");
#endif
                }
                else
                {
                    // Separate the host from the port.
                    var index_of_colon = value.LastIndexOf(':');
                    if (index_of_colon != -1)
                    {
                        if (index_of_colon + 1 >= value.Length)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                            throw new FormatException("Port value is invalid.");
#else
                            throw new UriFormatException("Port value is invalid.");
#endif

                        var port = value.Substring(index_of_colon + 1);

                        // Validate port.
                        var port_match = _PortRegex.Match(port);
                        if (!port_match.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                            throw new FormatException("Port value has invalid characters.");
#else
                            throw new UriFormatException("Port value has invalid characters.");
#endif

                        this.Port = Convert.ToInt32(port);
                        this.Host = value.Substring(0, index_of_colon);
                    }
                    else
                    {
                        this.Host = value;
                    }
                }
            }
        }

        /// <summary>
        /// get/set - Uri username value.
        /// </summary>
        /// <format>*( unreserved / pct-encoded / sub-delims / ":" )</format>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        /// <exception cref="System.FormatException">Property cannot contain invalid characters.</exception>
#else
        /// <exception cref="System.UriFormatException">Property cannot contain invalid characters.</exception>
#endif
        public string Username
        {
            get { return _Username; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _Username = value;
                    _IsDirty = true;
                    return;
                }

                var index_of_colon = value.IndexOf(':');
                if (index_of_colon != -1)
                {
                    value = value.Substring(0, index_of_colon);
                }

                // Validate the characters.
                var match_userinfo = _UserInfoRegex.Match(value);
                if (!match_userinfo.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                    throw new FormatException("User information has invalid characters.");
#else
                    throw new UriFormatException("User information has invalid characters.");
#endif

                _Username = value;
                _IsDirty = true;
            }
        }

        /// <summary>
        /// get/set - Uri host value.
        /// </summary>
        /// <format>IP-literal / IPv4address / reg-name</format>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        /// <exception cref="System.FormatException">Property cannot contain invalid characters.</exception>
#else
        /// <exception cref="System.UriFormatException">Property cannot contain invalid characters.</exception>
#endif
        public string Host
        {
            get { return _Host; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _Host = value;
                    _IsDirty = true;
                    return;
                }

                var is_ip4 = false;
                var is_ip6 = false;

                // Check if it's an IPv4 address.
                var ip4_match = _IPv4AddressRegex.Match(value);
                if (ip4_match.Success)
                    is_ip4 = true;

                // Check if it's an IPv6 address.
                if (!is_ip4)
                {
                    var ip6_match = _IPv6AddressRegex.Match(value);
                    if (ip6_match.Success)
                        is_ip6 = true;
                }

                if (!is_ip4 && !is_ip6)
                {
                    // Validate host.
                    var host_match = _HostRegex.Match(value);
                    if (!host_match.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                        throw new FormatException("Host value has invalid characters.");
#else
                        throw new UriFormatException("Host value has invalid characters.");
#endif
                }

                _Host = value;
                _IsDirty = true;
            }
        }

        /// <summary>
        /// get/set - Uri port value.
        /// </summary>
        /// <format>*DIGIT</format>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        /// <exception cref="System.FormatException">Property cannot contain invalid characters.</exception>
#else
        /// <exception cref="System.UriFormatException">Property cannot contain invalid characters.</exception>
#endif
        public int Port
        {
            get { return _Port; }
            set
            {
                var port_match = _PortRegex.Match(value.ToString());
                if (!port_match.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                    throw new FormatException("Port value has invalid characters.");
#else
                    throw new UriFormatException("Port value has invalid characters.");
#endif

                _Port = value;
                _IsDirty = true;
            }
        }

        /// <summary>
        /// get/set - Uri path value.
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _Path = new UriPath();
                    _IsDirty = true;
                    return;
                }

                _Path = new UriPath(value);
                _IsDirty = true;
            }
        }

        /// <summary>
        /// get/set - Uri query value.
        /// </summary>
        /// <format>*( pchar / "/" / "?" )</format>
        public string Query
        {
            get { return _Query.ToString(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _Query = new UriQuery();
                    _IsDirty = true;
                    return;
                }

                // Extract the fragment.
                var index_of_pound = value.IndexOf('#');
                if (index_of_pound != -1)
                {
                    this.Fragment = value.Substring(index_of_pound);
                    value = value.Substring(0, index_of_pound);
                }

                _Query = new UriQuery(value);
                _IsDirty = true;
            }
        }

        /// <summary>
        /// get/set - Uri fragment value.
        /// </summary>
        /// <format>*( pchar / "/" / "?" )</format>
#if WINDOWS_APP || WINDOWS_PHONE_APP
        /// <exception cref="System.FormatException">Property cannot contain invalid characters.</exception>
#else
        /// <exception cref="System.UriFormatException">Property cannot contain invalid characters.</exception>
#endif
        public string Fragment
        {
            get { return _Fragment; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _Fragment = null;
                    _IsDirty = true;
                    return;
                }

                // Replace Whitespace.
                UriBuilder.ReplaceWhitespaces(ref value);

                var index_of_pound = value.IndexOf('#');
                if (index_of_pound != -1)
                {
                    if (index_of_pound + 1 >= value.Length)
                    {
                        _Fragment = null;
                        _IsDirty = true;
                        return;
                    }
                    value = value.Substring(index_of_pound + 1);
                }

                var match = _FragmentRegex.Match(value);

                if (!match.Success)
#if WINDOWS_APP || WINDOWS_PHONE_APP
                    throw new FormatException("Fragment value has invalid characters.");
#else
                    throw new UriFormatException("Fragment value has invalid characters.");
#endif

                _Fragment = value;
                _IsDirty = true;
            }
        }

        /// <summary>
        /// get/set - The Uri value created by this UriBuilder.
        /// </summary>
        public Uri Uri
        {
            get
            {
                if (_IsDirty)
                {
                    _Uri = new Uri(this.ToString());
                    _IsDirty = false;
                    return _Uri;
                }

                return _Uri;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UriBuilder class.
        /// </summary>
        public UriBuilder()
        {
            _Query = new UriQuery();
        }

        /// <summary>
        /// Creates a new instance of a UriBuilder class.
        /// </summary>
        /// <param name="uri">Initial URI value.</param>
        public UriBuilder(string uri)
        {
            Validation.Argument.Assert.IsNotNullOrWhitespace(uri, nameof(uri));
            var temp_uri = new Uri(uri, UriKind.RelativeOrAbsolute);
            if (temp_uri.IsAbsoluteUri)
            {
                this.Initialize(temp_uri);
                return;
            }
            this.Initialize(new Uri("http://" + uri));
        }

        /// <summary>
        /// Creates a new instance of a UriBuilder class.
        /// </summary>
        /// <param name="uri">Initial URI value.</param>
        public UriBuilder(Uri uri)
        {
            this.Initialize(uri);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the UriBuilder with the URI specified.
        /// </summary>
        /// <param name="uri">Initial URI value.</param>
        private void Initialize(Uri uri)
        {
            _Scheme = uri.Scheme;
            _Host = uri.Host;
            _Port = uri.Port;
            _Path = new UriPath(uri.AbsolutePath);

            if (uri.Query != null)
                _Query = new UriQuery(uri.Query);
            else
                _Query = new UriQuery();

            _Fragment = uri.Fragment;
        }

        /// <summary>
        /// Returns the URI as a string.
        /// </summary>
        /// <returns>A new URI value.</returns>
        public override string ToString()
        {
            return string.Concat(new string[]
            {
                _Scheme,
                "://",
                _Host,
                _Port != 80 && _Port != -1 ? ":" + _Port : String.Empty,
                (_Host.Length == 0 && _Path.Count == 0) ? String.Empty : _Path,
                _Query.Count > 0 ? "?" + _Query.ToString() : String.Empty,
                !String.IsNullOrEmpty(_Fragment) ? "#" + _Fragment : String.Empty
            });
        }

        /// <summary>
        /// Get the QueryParameters object.
        /// </summary>
        /// <returns>QueryParameters object for this UriBuilder.</returns>
        public UriQuery GetQueryParameters()
        {
            return _Query;
        }

        /// <summary>
        /// Get a reference to the QueryPath object.
        /// </summary>
        /// <returns>QueryPath object for this UriBuilder.</returns>
        public UriPath GetPath()
        {
            return _Path;
        }

        /// <summary>
        /// Replaces all spaces in uri with the URL encoded value of '%20' (without single-quotes).
        /// </summary>
        /// <param name="uri">Uri value.</param>
        /// <returns>Updated uri value with replaced spaces.</returns>
        public static string ReplaceWhitespaces(string uri)
        {
            if (String.IsNullOrEmpty(uri))
                return uri;

            return _EncodeSpaces.Replace(uri, _WhitespaceEncoding);
        }

        /// <summary>
        /// Replaces all spaces in uri with the URL encoded value of '%20' (without single-quotes).
        /// </summary>
        /// <param name="uri">Uri value.</param>
        internal static void ReplaceWhitespaces(ref string uri)
        {
            if (!String.IsNullOrEmpty(uri))
            {
                uri = UriBuilder.ReplaceWhitespaces(uri);
            }
        }

        /// <summary>
        /// Encodes the URL.
        /// </summary>
        /// <param name="url">URL to encode.</param>
        /// <returns>Encoded URL.</returns>
        public static string UrlEncode(string url)
        {
            return System.Net.WebUtility.UrlEncode(url);
        }

        /// <summary>
        /// Decodes the URL.
        /// </summary>
        /// <param name="url">URL to decode.</param>
        /// <returns>Decode the URL.</returns>
        public static string UrlDecode(string url)
        {
            return System.Net.WebUtility.UrlDecode(url);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
