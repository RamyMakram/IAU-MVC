using IAU.DTO.Helper;
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
	public class CoordinatorController : BaseController
	{
		// GET: Coordinator
		public ActionResult Index()
		{
			ViewBag.CookieLang = Request.Cookies["lang"].Value;
			return View();
		}

		[HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> Index(string email, string pass)
		{
			var res =await APIHandeling.LoginAdminAsync($"User/Login?email={email}&pass={pass}");
			var resJson = res.Content.ReadAsStringAsync();
			var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (lst.success)
				return Redirect(ConfigurationManager.AppSettings["AdminPanel"].ToString() + "/LoginForward/Login?t=" + JObject.Parse(lst.result.ToString())["Token"]);
			return View();
		}
	}
}