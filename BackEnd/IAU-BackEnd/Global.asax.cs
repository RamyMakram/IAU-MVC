﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IAU_BackEnd
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			MvcHandler.DisableMvcResponseHeader = true;
			//Mapper.Initialize(c =>
			//{
			//    c.AddProfile<ApplicationProfile>();
			//});
			//FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			//RouteConfig.RegisterRoutes(RouteTable.Routes);
			//BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
		protected void Application_BeginRequest(object Sender, EventArgs eventE)
		{
			var cridantl = HttpContext.Current.Request.Headers["crd"];
			if (cridantl == null || cridantl == "" || cridantl != "dkvkk45523g25dedks44w7ee7@k309m$.f,dkks")
				HttpContext.Current.Response.StatusCode = 401;
		}
	}
}
