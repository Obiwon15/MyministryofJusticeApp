using System.Web.Mvc;
using System.Web.Routing;

namespace ministryofjusticeWebUi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );

            routes.MapRoute(
                "Case",
                "Case",
                new
                {
                    controller = "Case",
                    action = "Index",
                    status = (string)null,
                }
            );
        }
    }
}