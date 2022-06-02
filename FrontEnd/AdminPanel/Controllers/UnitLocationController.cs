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
	public class UnitLocationController : BaseController
	{
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("UnitsLocation/GetUnits_Location");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult AllDel()
		{
			var Data = APIHandeling.getData("UnitsLocation/GetDeleted");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return PartialView("DeletedModelsView", JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString()));
			else
				return PartialView("DeletedModelsView", null);
		}
		[HttpPost]
		public JsonResult RestoreItem(int id)
		{
			var Data = APIHandeling.Post("UnitsLocation/_Restore?id=" + id, new { });
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(res);
		}
		public ActionResult Detials(int id)
		{
			var Data = APIHandeling.getData("UnitsLocation/GetLocationWithLang?id=" + id + "&lang=" + Request.Cookies["lang"].Value);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<UnitsLocDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Create()
		{
			var Data = APIHandeling.getData("Locations/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
			{
				var Locations = JsonConvert.DeserializeObject<ICollection<LocationsDTO>>(res.result.ToString());
				ViewBag.Locations = new SelectList(Locations, "Location_ID", Request.Cookies["lang"].Value == "ar" ? "Location_Name_AR" : "Location_Name_EN");
				return View();
			}
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Edit(int id)
		{
			var Data = APIHandeling.getData("Locations/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
			{
				var Locations = JsonConvert.DeserializeObject<ICollection<LocationsDTO>>(res.result.ToString());
				ViewBag.Locations = new SelectList(Locations, "Location_ID", Request.Cookies["lang"].Value == "ar" ? "Location_Name_AR" : "Location_Name_EN");
			}
			Data = APIHandeling.getData("UnitsLocation/GetUnits_Location?id=" + id);
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<UnitsLocDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Edit(int Id, UnitsLocDTO loc)
		{
			loc.Units_Location_ID = Id;
			var Req = APIHandeling.Post("UnitsLocation/EditUnits_Location", loc);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Deactive(int Id)
		{
			return View(new UnitsLocDTO() { Units_Location_ID = Id });
		}
		public ActionResult Active(int Id)
		{
			return View(new UnitsLocDTO() { Units_Location_ID = Id });
		}

		[HttpPost]
		public ActionResult Deactive(int id, string x)
		{
			var Req = APIHandeling.getData("UnitsLocation/Deactive?id=" + id);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}

		[HttpPost]
		public ActionResult Active(int id, string x)
		{
			var Req = APIHandeling.getData("UnitsLocation/Active?id=" + id);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}

		[HttpPost]
		public ActionResult Create(UnitsLocDTO user)
		{
			var Req = APIHandeling.Post("UnitsLocation/Create", user);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public JsonResult Delete(int id)
		{
			var Data = APIHandeling.Post("UnitsLocation/_Delete?id=" + id, new { });
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(res);
		}
	}
}