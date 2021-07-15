using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using System.Web.Http;
using ministryofjusticeWebUi.Infrastructures;
using System;
using System.Collections.Generic;

namespace ministryofjusticeWebUi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Session_End(Object sender, EventArgs eventAgr)
        {
            List<string> userList = null;
            if (null != Session["Users"] && null != Session.SessionID)
            {
                userList = Session["Users"] as List<string>;
                userList.Remove(Session.SessionID);
                Session["Users"] = userList;
            }
        }
    }

    }