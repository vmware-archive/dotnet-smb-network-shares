using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NET_framework4_6_1_win_network_smb_share
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
                routeTemplate: "api/{controller}/{action}",
                defaults: new {  }
            );
        }
    }
}
