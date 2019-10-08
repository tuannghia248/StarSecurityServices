﻿using System;
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
                url: "dang-nhap",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Service",
               url: "dich-vu",
               defaults: new { controller = "Home", action = "Service", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
