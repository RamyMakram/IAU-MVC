using IAU.DTO.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
			try
			{
				//var handler = new HttpClientHandler()
				//{
				//	AllowAutoRedirect = false
				//};
				//HttpClient client = new HttpClient(handler);
				//client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AdminPanel"].ToString());
				////client.BaseAddress = new Uri("https://dashb-mustafid.iau.edu.sa/");
				//System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
				//var res = client.GetAsync("").Result.StatusCode;
				ViewBag.CookieLang = Request.Cookies["lang"].Value;
				return View();
			}
			catch (Exception)
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken()]
		public async System.Threading.Tasks.Task<ActionResult> Index(string email, string pass)
		{
			var res = await APIHandeling.LoginAdminAsync($"User/Login?email={email}&pass={pass}");
			var resJson = res.Content.ReadAsStringAsync();
			var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (lst.success)
				return Redirect(ConfigurationManager.AppSettings["AdminPanel"].ToString() + "/LoginForward/Login?t=" + JObject.Parse(lst.result.ToString())["Token"]);
			ViewBag.Error = true;
			ViewBag.CookieLang = Request.Cookies["lang"].Value;

			return View();
		}
	}
}