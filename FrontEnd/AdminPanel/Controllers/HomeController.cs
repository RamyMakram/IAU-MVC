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
			var Data = APIHandeling.getData("Service_Type/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
			ViewBag.ServiceType = Services;
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
	}
}