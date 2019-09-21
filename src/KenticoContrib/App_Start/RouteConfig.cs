using System.Web.Mvc;
using System.Web.Routing;

using Kentico.Web.Mvc;

namespace KenticoContrib
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Maps routes to Kentico HTTP handlers and features enabled in ApplicationConfig.cs
            // Always map the Kentico routes before adding other routes. Issues may occur if Kentico URLs are matched by a general route, for example images might not be displayed on pages
            routes.Kentico().MapRoutes();

            routes.MapRoute(
                "Home",
                "",
                new { controller = "Home", action = "Index" }
            );
        }
    }
}
