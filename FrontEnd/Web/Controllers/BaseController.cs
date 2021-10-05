using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
	public class BaseController : Controller
	{
		protected override void OnActionExecuting(ActionExecutingContext Context)
		{
			base.OnActionExecuting(Context);
			if (Request.Cookies["lang"] == null)
				Response.Cookies.Set(new HttpCookie("lang", "en"));
		}
	}
}