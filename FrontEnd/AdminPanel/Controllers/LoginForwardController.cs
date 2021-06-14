using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class LoginForwardController : Controller
    {
		// GET: LoginForward
		public ActionResult Login(string t)
		{
			if (t == null)
				return RedirectToAction("Home", "Home");
			var res = APIHandeling.getData("User/VerfiyToken?token=" + t);
			var Res = res.Content.ReadAsStringAsync();
			var lst = JsonConvert.DeserializeObject<ResponseClass>(Res.Result);
			if (lst.success)
			{
				Response.Cookies.Add(new HttpCookie("u", lst.result.ToString()));
				Response.Cookies.Add(new HttpCookie("token", t));
				return RedirectToAction("Home", "Home");
			}
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}