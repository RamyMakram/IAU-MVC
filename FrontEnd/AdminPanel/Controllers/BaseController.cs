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
			if (Request.Cookies["u"] != null)
			{
				var res = APIHandeling.getData("User/VerfiyUser?id=" + Request.Cookies["u"].Value);
				var Res = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<ResponseClass>(Res.Result);
				if (!lst.success)
				{
					Response.Cookies.Remove("u");
					context.Result = RedirectToAction("UNAuthorize", "Error");
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
				context.Result = RedirectToAction("UNAuth", "Error");
		}
	}
}