﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GoalWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{Id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
