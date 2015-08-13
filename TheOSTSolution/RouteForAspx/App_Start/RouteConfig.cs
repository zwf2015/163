using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RouteForAspx
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //url: "{controller}.aspx?Vercode=123456&operation={action}/{id}",
            //itf.taoshi.com/Chat.aspx?Vercode=123456&operation=getlist&customerid=115
            routes.MapRoute(
                name: "As",
                url: "{controller}.as/{action}"
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
