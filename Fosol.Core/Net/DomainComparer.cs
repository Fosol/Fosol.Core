using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fosol.Core.Net
{
    /// <summary>
    /// DomainComparer class, providates a way to compare domain name values.
    /// Allows the use of a wildcard character when comparing.
    /// </summary>
    public class DomainComparer : IEqualityComparer<string>
    {
        #region Methods
        /// <summary>
        /// Confirm whether the compare value matches the domain.
        /// Compare value may contain wildcard characters [*].
        /// </summary>
        /// <example>
        /// domain = "hotmail.com"
        /// compare = "*.hotmail.com"
        /// answer = true
        /// </example>
        /// <param name="domain">Actual domain name value.</param>
        /// <param name="match">Domain name with wildcard characters.</param>
        /// <returns>True if the domain is a match.</returns>
        public static bool IsMatch(string domain, string match)
        {
            // If the compare value contains wildcards.
            if (match.Contains("*"))
            {
                string check = match;

                // Remove the *. and replace it with a * instead.
                if (check.StartsWith("*."))
                    check = "*" + check.Substring(2, check.Length - 2);

                return IsWildcardMatch(domain, match);
            }

            return match.Equals(domain, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Confirm whether the compare value matches the domain.
        /// Compare value may contain wildcard characters [*].
        /// </summary>
        /// <param name="domain">Actual domain name value.</param>
        /// <param name="match">Domain name with wildcard characters.</param>
        /// <returns>True if the domain is a match.</returns>
        private static bool IsWildcardMatch(string domain, string match)
        {
            // If the compare statement does not start with a wildcard.
            if (!match.StartsWith("*"))
            {
                var stop = match.IndexOf("*");

                // If the first part of the domain doesn't match return false.
                if (!domain.StartsWith(match.Substring(0, stop)))
                    return false;
            }

            // If the compare statement does not end with a wildcard.
            if (!match.EndsWith("*"))
            {
                var start = match.LastIndexOf('*') + 1;

                // If the end part of the domain doesn't match return false.
                if (!domain.EndsWith(match.Substring(start, match.Length - start)))
                    return false;
            }

            // Compare the domain with the match.
            var regex = new Regex(match.Replace(@".", @"\.").Replace(@"*", @".*"));
            return regex.IsMatch(domain);
        }

        /// <summary>
        /// Determines whether both domains are equal.
        /// Compare value may contain wildcard characters [*].
        /// </summary>
        /// <param name="domain">Actual domain name value.</param>
        /// <param name="match">Domain name with wildcard characters.</param>
        /// <returns>True if the domain is a match.</returns>
        public bool Equals(string domain, string match)
        {
            return IsMatch(domain, match);
        }

        /// <summary>
        /// Hashcode of object.
        /// </summary>
        /// <param name="obj">Object of type string.</param>
        /// <returns>Hash code for object.</returns>
        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
        #endregion
    }
}
