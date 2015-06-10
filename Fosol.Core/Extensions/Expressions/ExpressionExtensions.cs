using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fosol.Core.Extensions.Expressions
{
    /// <summary>
    /// ExpressionExtensions static class, provides extension methods to modify expression trees.
    /// </summary>
    public static class ExpressionExtensions
    {
        #region ReplaceParameter
        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">Tree to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static Expression ReplaceParameter(this Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            Expression exp = null;
            Type expressionType = expression.GetType();
            if (expressionType == typeof(ParameterExpression) || expressionType.IsSubclassOf(typeof(ParameterExpression)))
            {
                exp = ((ParameterExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(MemberExpression) || expressionType.IsSubclassOf(typeof(MemberExpression)))
            {
                exp = ((MemberExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(MethodCallExpression) || expressionType.IsSubclassOf(typeof(MethodCallExpression)))
            {
                exp = ((MethodCallExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(NewExpression) || expressionType.IsSubclassOf(typeof(NewExpression)))
            {
                exp = ((NewExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(UnaryExpression) || expressionType.IsSubclassOf(typeof(UnaryExpression)))
            {
                exp = ((UnaryExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(ConstantExpression) || expressionType.IsSubclassOf(typeof(ConstantExpression)))
            {
                exp = ((ConstantExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(ConditionalExpression) || expressionType.IsSubclassOf(typeof(ConditionalExpression)))
            {
                exp = ((ConditionalExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(LambdaExpression) || expressionType.IsSubclassOf(typeof(LambdaExpression)))
            {
                exp = ((LambdaExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(MemberInitExpression) || expressionType.IsSubclassOf(typeof(MemberInitExpression)))
            {
                exp = ((MemberInitExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else if (expressionType == typeof(BinaryExpression) || expressionType.IsSubclassOf(typeof(BinaryExpression)))
            {
                exp = ((BinaryExpression)expression).ReplaceParameter(oldParameter, newParameter);
            }
            else
            {
                //did I forget some expression type? probably. this will take care of that... :)	
                throw new NotImplementedException("Expression type " + expression.GetType().FullName + " not supported by this expression tree parser.");
            }
            return exp;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">LambdaExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static LambdaExpression ReplaceParameter(this LambdaExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            LambdaExpression lambdaExpression = null;

            // Only change the Lambda expression type if the parameters are changed.
            var body = expression.Body.ReplaceParameter(oldParameter, newParameter);
            var parameters = (expression.Parameters != null) ? expression.Parameters.ReplaceParameter(oldParameter, newParameter) : null;
            var type = expression.Parameters.Any(p => p.Type == oldParameter.Type) ? typeof(Func<,>).MakeGenericType(newParameter.Type, typeof(bool)) : expression.Type;

            lambdaExpression = Expression.Lambda(
                type,
                expression.Body.ReplaceParameter(oldParameter, newParameter),
                (expression.Parameters != null) ? expression.Parameters.ReplaceParameter(oldParameter, newParameter) : null
            );
            return lambdaExpression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">BinaryExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static BinaryExpression ReplaceParameter(this BinaryExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            BinaryExpression binaryExp = null;
            binaryExp = Expression.MakeBinary(
                expression.NodeType,
                (expression.Left != null) ? expression.Left.ReplaceParameter(oldParameter, newParameter) : null,
                (expression.Right != null) ? expression.Right.ReplaceParameter(oldParameter, newParameter) : null,
                expression.IsLiftedToNull,
                expression.Method,
                (expression.Conversion != null) ? expression.Conversion.ReplaceParameter(oldParameter, newParameter) : null
            );
            return binaryExp;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">ParameterExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static ParameterExpression ReplaceParameter(this ParameterExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            ParameterExpression paramExpression = null;
            if (expression.Equals(oldParameter))
            {
                paramExpression = newParameter;
            }
            else
            {
                paramExpression = expression;
            }
            return paramExpression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">MemberExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static MemberExpression ReplaceParameter(this MemberExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            var new_expression = (expression.Expression != null) ? expression.Expression.ReplaceParameter(oldParameter, newParameter) : null;

            var m = new_expression.Type;

            if (new_expression.Type == newParameter.Type)
            {
                // If the new expression is of the new parameter type we need to use the method information for the new expression.
                var mi = newParameter.Type.GetMember(expression.Member.Name).First();
                return Expression.MakeMemberAccess(new_expression, mi);
            }
            else if (new_expression.Type != expression.Member.DeclaringType)
            {
                // If the new expression has a different type than the old expression we need to update the method info with the new expression type.
                var mi = new_expression.Type.GetMember(expression.Member.Name).First();
                return Expression.MakeMemberAccess(new_expression, mi);
            }

            return Expression.MakeMemberAccess(new_expression, expression.Member);
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">MemberInitExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static MemberInitExpression ReplaceParameter(this MemberInitExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return Expression.MemberInit(
                (expression.NewExpression != null) ? expression.NewExpression.ReplaceParameter(oldParameter, newParameter) : null,
                (expression.Bindings != null) ? expression.Bindings.ReplaceParameter(oldParameter, newParameter) : null
            );
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">MethodCallExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static MethodCallExpression ReplaceParameter(this MethodCallExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            MethodCallExpression callExpression = null;
            callExpression = Expression.Call(
                (expression.Object != null) ? expression.Object.ReplaceParameter(oldParameter, newParameter) : null,
                expression.Method,
                (expression.Arguments != null) ? expression.Arguments.ReplaceParameter(oldParameter, newParameter) : null
            );
            return callExpression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">NewExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static NewExpression ReplaceParameter(this NewExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return Expression.New(
                expression.Constructor,
                (expression.Arguments != null) ? expression.Arguments.ReplaceParameter(oldParameter, newParameter) : null,
                expression.Members);
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of ParameterExpressions with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;ParameterExpression&gt; to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<ParameterExpression> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> expressionArguments, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            if (expressionArguments != null)
            {
                foreach (ParameterExpression argument in expressionArguments)
                {
                    if (argument != null)
                    {
                        yield return argument.ReplaceParameter(oldParameter, newParameter);
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of Expressions with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;Expression&gt; to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A IEnumerable returning the passed in set of Expressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<Expression> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<Expression> expressionArguments, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            if (expressionArguments != null)
            {
                foreach (Expression argument in expressionArguments)
                {
                    if (argument != null)
                    {
                        yield return argument.ReplaceParameter(oldParameter, newParameter);
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of ElementInits with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;ElementInit&gt; to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<ElementInit> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<ElementInit> elementInits, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            if (elementInits != null)
            {
                foreach (ElementInit elementInit in elementInits)
                {
                    if (elementInit != null)
                    {
                        yield return Expression.ElementInit(elementInit.AddMethod, elementInit.Arguments.ReplaceParameter(oldParameter, newParameter));
                    }
                    else
                    {
                        yield return null;
                    }

                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of MemberBindings with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;MemberBinding&gt; to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<MemberBinding> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<MemberBinding> memberBindings, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            if (memberBindings != null)
            {
                foreach (MemberBinding binding in memberBindings)
                {
                    if (binding != null)
                    {
                        switch (binding.BindingType)
                        {
                            case MemberBindingType.Assignment:
                                MemberAssignment memberAssignment = (MemberAssignment)binding;
                                yield return Expression.Bind(binding.Member, memberAssignment.Expression.ReplaceParameter(oldParameter, newParameter));
                                break;
                            case MemberBindingType.ListBinding:
                                MemberListBinding listBinding = (MemberListBinding)binding;
                                yield return Expression.ListBind(binding.Member, listBinding.Initializers.ReplaceParameter(oldParameter, newParameter));
                                break;
                            case MemberBindingType.MemberBinding:
                                MemberMemberBinding memberMemberBinding = (MemberMemberBinding)binding;
                                yield return Expression.MemberBind(binding.Member, memberMemberBinding.Bindings.ReplaceParameter(oldParameter, newParameter));
                                break;
                        }
                    }
                    else
                    {
                        yield return null;
                    }

                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">UnaryExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static UnaryExpression ReplaceParameter(this UnaryExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return Expression.MakeUnary(
                expression.NodeType,
                (expression.Operand != null) ? expression.Operand.ReplaceParameter(oldParameter, newParameter) : null,
                expression.Type,
                expression.Method);
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree. Note: this version of ReplaceParameter exists just for conformity - there can't be a parameter expression hiding under a constant expression so this could really be skipped.	
        /// </summary>	
        /// <param name="expression">ConstantExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static ConstantExpression ReplaceParameter(this ConstantExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            //return Expression.Constant(expression.Value, expression.Type);	
            return expression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">ConditionalExpression to replace parameters in</param>	
        /// <param name="oldParameter">Parameter to replace</param>	
        /// <param name="newParameter">Parameter to use as replacement</param>	
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static ConditionalExpression ReplaceParameter(this ConditionalExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return Expression.Condition(
                (expression.Test != null) ? expression.Test.ReplaceParameter(oldParameter, newParameter) : null,
                (expression.IfTrue != null) ? expression.IfTrue.ReplaceParameter(oldParameter, newParameter) : null,
                (expression.IfFalse != null) ? expression.IfFalse.ReplaceParameter(oldParameter, newParameter) : null);
        }
        #endregion

        #region ReplaceParameters
        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">Tree to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static Expression ReplaceParameters(this Expression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            Expression exp = null;
            Type expressionType = expression.GetType();
            if (expressionType == typeof(ParameterExpression) || expressionType.IsSubclassOf(typeof(ParameterExpression)))
            {
                exp = ((ParameterExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(MemberExpression) || expressionType.IsSubclassOf(typeof(MemberExpression)))
            {
                exp = ((MemberExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(MethodCallExpression) || expressionType.IsSubclassOf(typeof(MethodCallExpression)))
            {
                exp = ((MethodCallExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(NewExpression) || expressionType.IsSubclassOf(typeof(NewExpression)))
            {
                exp = ((NewExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(UnaryExpression) || expressionType.IsSubclassOf(typeof(UnaryExpression)))
            {
                exp = ((UnaryExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(ConstantExpression) || expressionType.IsSubclassOf(typeof(ConstantExpression)))
            {
                exp = ((ConstantExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(ConditionalExpression) || expressionType.IsSubclassOf(typeof(ConditionalExpression)))
            {
                exp = ((ConditionalExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(LambdaExpression) || expressionType.IsSubclassOf(typeof(LambdaExpression)))
            {
                exp = ((LambdaExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(MemberInitExpression) || expressionType.IsSubclassOf(typeof(MemberInitExpression)))
            {
                exp = ((MemberInitExpression)expression).ReplaceParameters(paramMap);
            }
            else if (expressionType == typeof(BinaryExpression) || expressionType.IsSubclassOf(typeof(BinaryExpression)))
            {
                exp = ((BinaryExpression)expression).ReplaceParameters(paramMap);
            }
            else
            {
                //did I forget some expression type? probably. this will take care of that... :)	
                throw new NotImplementedException("Expression type " + expression.GetType().FullName + " not supported by this expression tree parser.");
            }
            return exp;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">LambdaExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static LambdaExpression ReplaceParameters(this LambdaExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            LambdaExpression lambdaExpression = null;

            // Only change the Lambda expression type if the parameters are changed.
            var body = expression.Body.ReplaceParameters(paramMap);
            var parameters = (expression.Parameters != null) ? expression.Parameters.ReplaceParameters(paramMap) : null;
            var delegate_param_type = expression.Type.GetMethod("Invoke").GetParameters()[0].ParameterType;
            var type = paramMap.ContainsKey(delegate_param_type) ? typeof(Func<,>).MakeGenericType(paramMap[delegate_param_type].Type, typeof(bool)) : expression.Type;

            lambdaExpression = Expression.Lambda(
                type,
                expression.Body.ReplaceParameters(paramMap),
                (expression.Parameters != null) ? expression.Parameters.ReplaceParameters(paramMap) : null
            );
            return lambdaExpression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">BinaryExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static BinaryExpression ReplaceParameters(this BinaryExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            BinaryExpression binaryExp = null;
            binaryExp = Expression.MakeBinary(
                expression.NodeType,
                (expression.Left != null) ? expression.Left.ReplaceParameters(paramMap) : null,
                (expression.Right != null) ? expression.Right.ReplaceParameters(paramMap) : null,
                expression.IsLiftedToNull,
                expression.Method,
                (expression.Conversion != null) ? expression.Conversion.ReplaceParameters(paramMap) : null
            );
            return binaryExp;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">ParameterExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static ParameterExpression ReplaceParameters(this ParameterExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            ParameterExpression paramExpression = null;
            if (paramMap.ContainsKey(expression.Type))
            {
                paramExpression = paramMap[expression.Type];
            }
            else
            {
                paramExpression = expression;
            }
            return paramExpression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">MemberExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static MemberExpression ReplaceParameters(this MemberExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            var new_expression = (expression.Expression != null) ? expression.Expression.ReplaceParameters(paramMap) : null;

            if (paramMap.ContainsKey(expression.Type))
            {
                // If the new expression is of the new parameter type we need to use the method information for the new expression.
                var mi = paramMap[expression.Type].Type.GetMember(expression.Member.Name).First();
                return Expression.MakeMemberAccess(new_expression, mi);
            }
            else if (new_expression.Type != expression.Member.DeclaringType)
            {
                // If the new expression has a different type than the old expression we need to update the method info with the new expression type.
                var mi = new_expression.Type.GetMember(expression.Member.Name).First();
                return Expression.MakeMemberAccess(new_expression, mi);
            }

            return Expression.MakeMemberAccess(new_expression, expression.Member);
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">MemberInitExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static MemberInitExpression ReplaceParameters(this MemberInitExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            return Expression.MemberInit(
                (expression.NewExpression != null) ? expression.NewExpression.ReplaceParameters(paramMap) : null,
                (expression.Bindings != null) ? expression.Bindings.ReplaceParameters(paramMap) : null
            );
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">MethodCallExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static MethodCallExpression ReplaceParameters(this MethodCallExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            MethodCallExpression callExpression = null;
            callExpression = Expression.Call(
                (expression.Object != null) ? expression.Object.ReplaceParameters(paramMap) : null,
                expression.Method,
                (expression.Arguments != null) ? expression.Arguments.ReplaceParameters(paramMap) : null
            );
            return callExpression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">NewExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static NewExpression ReplaceParameters(this NewExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            return Expression.New(
                expression.Constructor,
                (expression.Arguments != null) ? expression.Arguments.ReplaceParameters(paramMap) : null,
                expression.Members);
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of ParameterExpressions with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;ParameterExpression&gt; to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<ParameterExpression> ReplaceParameters(this System.Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> expressionArguments, Dictionary<Type, ParameterExpression> paramMap)
        {
            if (expressionArguments != null)
            {
                foreach (ParameterExpression argument in expressionArguments)
                {
                    if (argument != null)
                    {
                        yield return argument.ReplaceParameters(paramMap);
                    }
                    else
                    {
                        yield return null;

                    }

                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of Expressions with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;Expression&gt; to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A IEnumerable returning the passed in set of Expressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<Expression> ReplaceParameters(this System.Collections.ObjectModel.ReadOnlyCollection<Expression> expressionArguments, Dictionary<Type, ParameterExpression> paramMap)
        {
            if (expressionArguments != null)
            {
                foreach (Expression argument in expressionArguments)
                {
                    if (argument != null)
                    {
                        yield return argument.ReplaceParameters(paramMap);
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of ElementInits with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;ElementInit&gt; to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<ElementInit> ReplaceParameters(this System.Collections.ObjectModel.ReadOnlyCollection<ElementInit> elementInits, Dictionary<Type, ParameterExpression> paramMap)
        {
            if (elementInits != null)
            {
                foreach (ElementInit elementInit in elementInits)
                {
                    if (elementInit != null)
                    {
                        yield return Expression.ElementInit(elementInit.AddMethod, elementInit.Arguments.ReplaceParameters(paramMap));
                    }
                    else
                    {
                        yield return null;
                    }

                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of MemberBindings with another ParameterExpression, and return as an IEnumerable	
        /// </summary>	
        /// <param name="expression">ReadOnlyCollection&lt;MemberBinding&gt; to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>	
        public static IEnumerable<MemberBinding> ReplaceParameters(this System.Collections.ObjectModel.ReadOnlyCollection<MemberBinding> memberBindings, Dictionary<Type, ParameterExpression> paramMap)
        {
            if (memberBindings != null)
            {
                foreach (MemberBinding binding in memberBindings)
                {
                    if (binding != null)
                    {
                        switch (binding.BindingType)
                        {
                            case MemberBindingType.Assignment:
                                MemberAssignment memberAssignment = (MemberAssignment)binding;
                                yield return Expression.Bind(binding.Member, memberAssignment.Expression.ReplaceParameters(paramMap));
                                break;
                            case MemberBindingType.ListBinding:
                                MemberListBinding listBinding = (MemberListBinding)binding;
                                yield return Expression.ListBind(binding.Member, listBinding.Initializers.ReplaceParameters(paramMap));
                                break;
                            case MemberBindingType.MemberBinding:
                                MemberMemberBinding memberMemberBinding = (MemberMemberBinding)binding;
                                yield return Expression.MemberBind(binding.Member, memberMemberBinding.Bindings.ReplaceParameters(paramMap));
                                break;
                        }
                    }
                    else
                    {
                        yield return null;
                    }

                }
            }
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">UnaryExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static UnaryExpression ReplaceParameters(this UnaryExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            return Expression.MakeUnary(
                expression.NodeType,
                (expression.Operand != null) ? expression.Operand.ReplaceParameters(paramMap) : null,
                expression.Type,
                expression.Method);
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree. Note: this version of ReplaceParameters exists just for conformity - there can't be a parameter expression hiding under a constant expression so this could really be skipped.	
        /// </summary>	
        /// <param name="expression">ConstantExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static ConstantExpression ReplaceParameters(this ConstantExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            //return Expression.Constant(expression.Value, expression.Type);	
            return expression;
        }

        /// <summary>	
        /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree	
        /// </summary>	
        /// <param name="expression">ConditionalExpression to replace parameters in</param>	
        /// <param name="paramMap">Parameters to replace</param> 
        /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>	
        public static ConditionalExpression ReplaceParameters(this ConditionalExpression expression, Dictionary<Type, ParameterExpression> paramMap)
        {
            return Expression.Condition(
                (expression.Test != null) ? expression.Test.ReplaceParameters(paramMap) : null,
                (expression.IfTrue != null) ? expression.IfTrue.ReplaceParameters(paramMap) : null,
                (expression.IfFalse != null) ? expression.IfFalse.ReplaceParameters(paramMap) : null);
        }
        #endregion
    }
}
