using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;

namespace Web.Controllers
{
	public class CoordinatorController : Controller
	{
		// GET: Coordinator
		public ActionResult Index()
		{
			ViewBag.CookieLang = Request.Cookies["lang"].Value;
			return View();
		}

		[HttpPost]
		public ActionResult Index(string email, string pass)
		{
			var res = APIHandeling.LoginAdmin($"User/Login?email={email}&pass={pass}");
			var resJson = res.Content.ReadAsStringAsync();
			var lst = JsonConvert.DeserializeObject<GeniricReciever>(resJson.Result);
			if (lst.success)
				return Redirect(ConfigurationManager.AppSettings["AdminPanel"].ToString() + "/LoginForward/Login?t=" + JObject.Parse(lst.result)["Token"]);
			return View();
		}
	}
}