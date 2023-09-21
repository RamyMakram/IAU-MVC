using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			MvcHandler.DisableMvcResponseHeader = true;
		}
		protected void Application_EndRequest()
		{   //here breakpoint
			if (HttpContext.Current.Response.StatusCode >= 400 && HttpContext.Current.Response.StatusCode <= 599)
				HttpContext.Current.Response.Redirect(Request.Url.ToString().Replace(Request.Url.PathAndQuery, "") + "/" + "Error");
		}
	}
}
