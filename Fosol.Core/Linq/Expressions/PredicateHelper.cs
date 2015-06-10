using Fosol.Core.Extensions.Expressions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fosol.Core.Linq.Expressions
{
    public static class PredicateHelper
    {
        #region Methods
        /// <summary>
        /// Updates the predicate expression provided by replacing a business model type with the specified DB entity type.
        /// This currently expects that the property names of the TFind type will be the same as the property names of the TReplaceWith type.
        /// </summary>
        /// <typeparam name="TFind">The type to find and replace within the predicate expression.</typeparam>
        /// <typeparam name="TReplaceWith">The type that will be inserted into the predicate expression and replace the original TFind type.</typeparam>
        /// <param name="predicate">Predicate expression.</param>
        /// <returns>Updated predicate expression.</returns>
        public static Expression<Func<TReplaceWith, bool>> ReplaceTypeInExpression<TFind, TReplaceWith>(Expression<Func<TFind, bool>> predicate)
        {
            var p1 = predicate.Parameters.First(p => p.Type == typeof(TFind));
            var p2 = ParameterExpression.Parameter(typeof(TReplaceWith), p1.Name);
            return predicate.ReplaceParameter(p1, p2) as Expression<Func<TReplaceWith, bool>>;
        }
        #endregion
    }
}
