using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fosol.Core.Net.Mvc.Extensions.Controllers
{
    /// <summary>
    /// ControllerExtensions static class, provides extension methods for the Controller class.
    /// </summary>
    public static class ControllerExtensions
    {
        #region Methods
        /// <summary>
        /// If the returnUrl is a local URL it will redirect to that action.
        /// Otherwise it will redirect to the default route action.
        /// </summary>
        /// <param name="controller">Controller object.</param>
        /// <param name="returnUrl">Return URL value.</param>
        /// <param name="defaultRouteName">Name of the default route.</param>
        /// <returns>A new instance of a RedirectResult or a RedirectToRouteResult object.</returns>
        public static ActionResult RedirectToLocal(this Controller controller, string returnUrl, string defaultRouteName = "Default")
        {
            // Only local URLs are allowed to be redirected to within the controller.
            if (controller.Url.IsLocalUrl(returnUrl))
            {
                return new RedirectResult(returnUrl);
            }

            // Get the default route and redirect to it.
            var default_route = (Route)RouteTable.Routes[defaultRouteName];
            RouteValueDictionary routeValues = RouteValueHelper.MergeRouteValues(controller.RouteData.Values, default_route.Defaults);
            return new RedirectToRouteResult(routeValues);
        }
        #endregion
    }
}
