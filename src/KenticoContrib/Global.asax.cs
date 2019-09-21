using System.Web;
using System.Web.Routing;

using Kentico.Web.Mvc;

namespace KenticoContrib
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
