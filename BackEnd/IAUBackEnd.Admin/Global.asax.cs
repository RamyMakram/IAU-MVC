using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IAUBackEnd.Admin
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		public static bool Setting_UseMessage;
		protected void Application_Start()
		{
			var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			if (config.AppSettings.Settings["Use_Message"] != null)
				Setting_UseMessage = bool.Parse(config.AppSettings.Settings["Use_Message"].Value);
			else
			{
				config.AppSettings.Settings.Add("Use_Message", "false");
				Setting_UseMessage = false;
			}
			config.Save();

			HttpConfiguration config2 = GlobalConfiguration.Configuration;
			config2.Formatters.JsonFormatter
						.SerializerSettings
						.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			MvcHandler.DisableMvcResponseHeader = true;
		}
		protected void Application_BeginRequest(object Sender, EventArgs eventE)
		{
			//string[] WS_AllowedSites = { "https://localhost:44346", "https://adminpanel.iau-bsc.com", "https://dashb-mustafid.iau.edu.sa" };
			//var contexURl = HttpContext.Current.Request.Headers.Get("Origin");
			//var cridantl = HttpContext.Current.Request.Headers["crd"];
			//if (WS_AllowedSites.Contains(contexURl) && HttpContext.Current.Request.Path == "/WSHandler.ashx")
			//{

			//}
			//else if ((cridantl == null || cridantl == "" || cridantl != "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks"))
			//	HttpContext.Current.Response.StatusCode = 401;
		}
	}
}
