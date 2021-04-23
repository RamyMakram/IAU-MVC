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
	public class UnitsController : BaseController
	{
		// GET: Units
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Units/GetUnits");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<UnitsDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Detials(int Id)
		{
			var Data = APIHandeling.getData("Units/GetUnits?id=" + Id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<UnitsDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Edit(int Id)
		{
			LoadEditOrCreate();
			var Data = APIHandeling.getData("Units/GetUnits?id=" + Id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
			{
				var data = JsonConvert.DeserializeObject<UnitsDTO>(res.result.ToString());
				int length = data.Units_Request_Type.Count;
				data.Units_ReqType = new int[length];
				for (int i = 0; i < length; i++)
					data.Units_ReqType[i] = data.Units_Request_Type.ElementAt(i).Request_Type_ID.Value;
				return View(data);
			}
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Create()
		{
			LoadEditOrCreate();
			return View();
		}
		public void LoadEditOrCreate()
		{
			var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";
			var Data = APIHandeling.getData("Locations/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Locations = JsonConvert.DeserializeObject<ICollection<LocationsDTO>>(res.result.ToString());
			if (isar)
				ViewBag.Locations = new SelectList(Locations, "Location_ID", "Location_Name_AR");
			else
				ViewBag.Locations = new SelectList(Locations, "Location_ID", "Location_Name_EN");
			Data = APIHandeling.getData("Service_Type/GetActive");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
			if (isar)
				ViewBag.Services = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_AR");
			else
				ViewBag.Services = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_EN");
			Data = APIHandeling.getData("UnitTypes/GetActive");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Types = JsonConvert.DeserializeObject<ICollection<UnitTypeDTO>>(res.result.ToString());
			if (isar)
				ViewBag.Types = new SelectList(Types, "Units_Type_ID", "Units_Type_Name_AR");
			else
				ViewBag.Types = new SelectList(Types, "Units_Type_ID", "Units_Type_Name_EN");
			Data = APIHandeling.getData("Request_Type/GetActive");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var ReqTypes = JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString());
			if (isar)
				ViewBag.ReqTypes = new SelectList(ReqTypes, "Request_Type_ID", "Request_Type_Name_AR");
			else
				ViewBag.ReqTypes = new SelectList(ReqTypes, "Request_Type_ID", "Request_Type_Name_EN");

			Data = APIHandeling.getData("Units/ThereIsNoMostafid");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			ViewBag.ViewMostafid = (bool)res.result;
		}
		public ActionResult Deactive(int id)
		{
			return View(new UnitsDTO() { Units_ID = id });
		}
		public ActionResult Active(int id)
		{
			return View(new UnitsDTO() { Units_ID = id });
		}
		[HttpPost]
		public ActionResult Create(UnitsDTO unitsDTO)
		{
			int length = unitsDTO.Units_ReqType.Length;
			unitsDTO.Units_Request_Type = new List<Units_Request_TypeDTO>();
			for (int i = 0; i < length; i++)
				unitsDTO.Units_Request_Type.Add(new Units_Request_TypeDTO() { Request_Type_ID = unitsDTO.Units_ReqType[i] });

			var Req = APIHandeling.Post("Units/Create", unitsDTO);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Edit(int Id, UnitsDTO unitsDTO)
		{
			var data = unitsDTO.Units_ReqType;
			unitsDTO.Units_ID = Id;
			int length = unitsDTO.Units_ReqType.Length;
			unitsDTO.Units_Request_Type = new List<Units_Request_TypeDTO>();
			for (int i = 0; i < length; i++)
				unitsDTO.Units_Request_Type.Add(new Units_Request_TypeDTO() { Request_Type_ID = unitsDTO.Units_ReqType[i] });
			var Req = APIHandeling.Post("Units/Update", unitsDTO);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}

		[HttpPost]
		public ActionResult Deactive(int id, string x)
		{
			var Req = APIHandeling.getData("Units/Deactive?id=" + id);
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
			var Req = APIHandeling.getData("Units/Active?id=" + id);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}