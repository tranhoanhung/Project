using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookstoreMVC5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            Session["cart"] = "";
            Session["favoriteProduct"] = "";
            Session["Admin_id"] = "";
            Session["Admin_user"] = "";
            Session["Admin_fullname"] = "";
            Session["Message"] = "";
            Session["id"] = "";
            Session["user"] = "";
            Session["OrderId"] = "";
        }
    }
}
