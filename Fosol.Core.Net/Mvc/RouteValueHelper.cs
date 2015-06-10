using System;
using System.Web.Routing;

namespace Fosol.Core.Net.Mvc
{
    /// <summary>
    /// RouteValueHelper static class, provides helper methods when using a RouteValueDictionary object.
    /// </summary>
    public static class RouteValueHelper
    {
        /// <summary>
        /// Merge the two RouteValueDictionary objects.
        /// The newRouteValues will override any existing values in the defaultRouteValues.
        /// </summary>
        /// <param name="defaultRouteValues">Default route values.</param>
        /// <param name="newRouteValues">new values that will override the default.</param>
        /// <returns>A new instance of a RouteValueDictionary object.</returns>
        public static RouteValueDictionary MergeRouteValues(RouteValueDictionary defaultRouteValues, RouteValueDictionary newRouteValues)
        {
            RouteValueDictionary routes;

            if (defaultRouteValues != null)
            {
                routes = new RouteValueDictionary(defaultRouteValues);

                if (newRouteValues != null)
                {
                    foreach (var current in newRouteValues)
                    {
                        routes[current.Key] = current.Value;
                    }
                }

                return routes;
            }

            if (newRouteValues != null)
                return new RouteValueDictionary(newRouteValues);

            return new RouteValueDictionary();
        }

        /// <summary>
        /// Merges the route values into a new instance of a RouteValueDictionary object.
        /// </summary>
        /// <param name="actionName">Action name.</param>
        /// <param name="controllerName">Controller name.</param>
        /// <param name="routeValues">Route values to include in the new RouteValueDictionary.</param>
        /// <returns>A new instance of a RouteValueDictionary object.</returns>
        public static RouteValueDictionary MergeRouteValues(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var routes = new RouteValueDictionary();
            if (routeValues != null)
            {
                Initialization.Assert.IsNotNull(ref routeValues, new RouteValueDictionary());
                foreach (var current in routeValues)
                {
                    routes[current.Key] = current.Value;
                }
            }
            if (actionName != null)
                routes["action"] = actionName;
            if (controllerName != null)
                routes["controller"] = controllerName;

            return routes;
        }
    }
}
