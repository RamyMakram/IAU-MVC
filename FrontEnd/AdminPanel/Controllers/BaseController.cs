using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
	public class BaseController : Controller
	{
		string RedirectUrl = ConfigurationManager.AppSettings["RedirectUrl"].ToString();

		// GET: Base
		protected override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			if (Request.Cookies["u"] != null && Request.Cookies["token"] != null)
			{
				var res = APIHandeling.getData($"User/VerfiyUser?id={Request.Cookies["u"].Value}&token={Request.Cookies["token"].Value}");
				var Res = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<ResponseClass>(Res.Result);
				if (!lst.success)
				{
					Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-9) });
					Response.Cookies.Set(new HttpCookie("token") { Expires = DateTime.Now.AddYears(-9) });
					Response.Cookies.Set(new HttpCookie("lang") { Expires = DateTime.Now.AddYears(-9) });
					context.Result = Redirect(RedirectUrl);
					return;
				}
				else
				{
					var data = JObject.Parse(lst.result.ToString());
					var Permissions = data["perm"].Values<string>().ToArray<string>();
					TempData["Permissions"] = Permissions;
					TempData["IsMostafid"] = data["IS_Mostafid"].Value<bool>();
					var RequestPage = Request.Url.AbsolutePath.Split('/')[1];
					if (RequestPage != "" && RequestPage != "Home" && RequestPage != "Email" && RequestPage != "SendedRequests")
					{
						var PerName = Models.Privilges.PagesPriviliges.FirstOrDefault(q => q.path == RequestPage);
						if (PerName == null || !Permissions.Contains(PerName.Name))
							context.Result = RedirectToAction("NotPermited", "Error");
					}
				}
			}
			else
			{
				Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-9) });
				Response.Cookies.Set(new HttpCookie("token") { Expires = DateTime.Now.AddYears(-9) });
				Response.Cookies.Set(new HttpCookie("lang") { Expires = DateTime.Now.AddYears(-9) });
				context.Result = Redirect(RedirectUrl);
			}
		}
		public ActionResult Logout()
		{
			Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-9) });
			Response.Cookies.Set(new HttpCookie("token") { Expires = DateTime.Now.AddYears(-9) });
			Response.Cookies.Set(new HttpCookie("lang") { Expires = DateTime.Now.AddYears(-9) });
			return Redirect(RedirectUrl);
		}
	}
}