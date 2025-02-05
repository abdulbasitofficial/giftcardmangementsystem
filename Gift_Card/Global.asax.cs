using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Gift_Card.Models;
namespace Gift_Card
{
    public class MvcApplication : System.Web.HttpApplication
    {
        GiftCardEntities ctx = new GiftCardEntities();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
