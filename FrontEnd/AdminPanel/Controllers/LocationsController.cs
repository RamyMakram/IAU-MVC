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
	public class LocationsController : BaseController
	{
		// GET: Locations
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Locations/GetLocations");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<LocationsDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Detials(int id)
		{
			var Data = APIHandeling.getData("Locations/GetLocation?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<LocationsDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Create()
		{
			return View();
		}
		public ActionResult Edit(int id)
		{
			var Data = APIHandeling.getData("Locations/GetLocation?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<LocationsDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Edit(int Id, LocationsDTO loc)
		{
			loc.Location_ID = Id;
			var Req = APIHandeling.Post("Locations/UpdateLocation", loc);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Deactive(int id)
		{
			return View(new LocationsDTO() { Location_ID = id });
		}
		public ActionResult Active(int id)
		{
			return View(new LocationsDTO() { Location_ID = id });
		}

		[HttpPost]
		public ActionResult Deactive(int id, string x)
		{
			var Req = APIHandeling.getData("Locations/DeactiveLocation?id=" + id);
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
			var Req = APIHandeling.getData("Locations/ActiveLocation?id=" + id);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}

		[HttpPost]
		public ActionResult Create(LocationsDTO user)
		{
			var Req = APIHandeling.Post("Locations/Create", user);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}