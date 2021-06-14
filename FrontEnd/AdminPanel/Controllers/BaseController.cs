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
	public class BaseController : Controller
	{
		// GET: Base
		protected override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			if (Request.Cookies["u"] != null&& Request.Cookies["token"] != null)
			{
				var res = APIHandeling.getData($"User/VerfiyUser?id={Request.Cookies["u"].Value}&token={Request.Cookies["token"].Value}");
				var Res = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<ResponseClass>(Res.Result);
				if (!lst.success)
				{
					Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-9) });
					Response.Cookies.Set(new HttpCookie("token") { Expires = DateTime.Now.AddYears(-9) });
					Response.Cookies.Set(new HttpCookie("lang") { Expires = DateTime.Now.AddYears(-9) });
					context.Result = Redirect("https://iau-bsc.com/Coordinator");
					return;
				}
				else
				{
					var data = JsonConvert.DeserializeObject<string[]>(lst.result.ToString());
					//ViewData["Permissions"] = data;
					TempData["Permissions"] = data;
				}
			}
			else
			{
				Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-9) });
				Response.Cookies.Set(new HttpCookie("token") { Expires = DateTime.Now.AddYears(-9) });
				Response.Cookies.Set(new HttpCookie("lang") { Expires = DateTime.Now.AddYears(-9) });
				context.Result = Redirect("https://iau-bsc.com/Coordinator");
			}
		}
		public ActionResult Logout()
		{
			Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-9) });
			Response.Cookies.Set(new HttpCookie("token") { Expires = DateTime.Now.AddYears(-9) });
			Response.Cookies.Set(new HttpCookie("lang") { Expires = DateTime.Now.AddYears(-9) });
			return Redirect("https://iau-bsc.com/Coordinator/index");
		}
	}
}