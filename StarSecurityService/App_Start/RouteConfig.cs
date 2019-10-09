using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StarSecurityService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Service",
               url: "Service",
               defaults: new { controller = "Home", action = "Service", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Recruitment",
              url: "Recruitment",
              defaults: new { controller = "Home", action = "Recruitment", id = UrlParameter.Optional }
          );


            routes.MapRoute(
              name: "Network",
              url: "Network",
              defaults: new { controller = "Home", action = "Network", id = UrlParameter.Optional }
          );


            routes.MapRoute(
              name: "Testimonial",
              url: "Testimonial",
              defaults: new { controller = "Home", action = "Testimonial", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

