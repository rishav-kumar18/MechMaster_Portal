using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MYMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UserDetails",
                url: "UserMaster/Details/{*id}",
                defaults: new { controller = "UserMaster", action = "Details" }
                );

            routes.MapRoute(
                name: "UserEdit",
                url: "UserMaster/Edit/{*id}",
                defaults: new { controller = "UserMaster", action = "Edit" }
);

            routes.MapRoute(
                name: "UserDelete",
                url: "UserMaster/Delete/{*id}",
                defaults: new { controller = "UserMaster", action = "Delete" }
);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
