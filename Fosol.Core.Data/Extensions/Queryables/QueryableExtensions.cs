using System;
using System.Data.Objects;
using System.Linq;

namespace Fosol.Core.Data.Extensions.Queryables
{
    /// <summary>
    /// QueryableExtensions static class, provides extension methods for Queryable objects.
    /// </summary>
    public static class QueryableExtensions
    {
        #region Methods
        /// <summary>
        /// Outputs the LINQ query to a string.
        /// </summary>
        /// <param name="obj">IQueryable object.</param>
        /// <returns>LINQ statement as a string.</returns>
        public static string ToTraceString(this IQueryable obj)
        {
            return ((ObjectQuery)obj).ToTraceString();
        }
        #endregion
    }
}
