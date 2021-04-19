using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
	public class HomeController : BaseController
	{
		// GET: Home
		public ActionResult Home()
		{
			if (Request.Cookies["lang"] == null)
				Response.Cookies.Add(new HttpCookie("lang", "ar"));
			return View();
		}
		public ActionResult ChangeLanguage(string lang)
		{
			Response.Cookies.Add(new HttpCookie("lang", lang ?? "ar"));
			return RedirectToAction("Home");
		}
	}
}