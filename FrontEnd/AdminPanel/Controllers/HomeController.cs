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
			var Data = APIHandeling.getData("Statistics/Get");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var data = JObject.Parse(res.result.ToString());
			ViewBag.ServiceType = JsonConvert.DeserializeObject<ICollection<GlobalCount>>(data["MostafidCount"].ToString());
			ViewBag.Stat = data["Stat"].ToString();
			ViewBag.Locations = JsonConvert.DeserializeObject<ICollection<GlobalCount>>(data["Locations"].ToString());
			return View();
		}
		public ActionResult ChangeLanguage(string lang)
		{
			Response.Cookies.Add(new HttpCookie("lang", lang ?? "ar"));
			return RedirectToAction("Home");
		}
		public JsonResult GetOrderCount(int ID)
		{
			var Data = APIHandeling.getData("Request/GetRequestsCount?UserID=" + ID);
			var resJson = Data.Content.ReadAsStringAsync();
			return Json(resJson.Result, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult FollowRequest(string requestCode)
		{
			var res = APIHandeling.Post("/Request/FollowRequest", requestCode);
			var lst = res.Content.ReadAsStringAsync().Result;
			return Json(lst, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetDelayedRequests()
		{
			var Data = APIHandeling.getData("/DelayedRequest/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(res.success ? res.result.ToString() : "[]");
		}
	}
}