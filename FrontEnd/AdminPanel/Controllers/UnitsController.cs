using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				int length = 0;
				if (data.Units_Request_Type != null)
				{
					length = data.Units_Request_Type.Count;
					data.Units_ReqType = new int[length];
					for (int i = 0; i < length; i++)
						data.Units_ReqType[i] = data.Units_Request_Type.ElementAt(i).Request_Type_ID.Value;
				}
				if (data.ServiceTypes != null)
				{
					length = data.ServiceTypes.Count;
					data.Units_ServiceType = new int[length];
					for (int i = 0; i < length; i++)
						data.Units_ServiceType[i] = data.ServiceTypes.ElementAt(i).Service_Type_ID;
				}
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
			var Data = APIHandeling.getData("UnitsLocation/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Locations = JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString());
			if (isar)
				ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_AR");
			else
				ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_EN");
			Data = APIHandeling.getData("Service_Type/GetActive");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
			if (isar)
				ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_AR");
			else
				ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_EN");
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
			Data = APIHandeling.getData("UnitLevels/GetUnitLevelForUnit");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Levels = JsonConvert.DeserializeObject<ICollection<UnitLevelDTO>>(res.result.ToString());
			ViewBag.Levels = Levels.ToList().ConvertAll(q => { return new SelectListItem() { Value = q.ID.ToString(), Text = (isar ? q.Name_AR : q.Name_EN) + " - " + q.Code, Selected = false }; });
			Data = APIHandeling.getData("Service_Type/GetActiveService_TypeCharList");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Service_TypeCharList = JsonConvert.DeserializeObject<List<string>>(res.result.ToString());
			ViewBag.Service_TypeCharList = Service_TypeCharList.ConvertAll(a => { return new SelectListItem() { Value = a.ToString(), Text = a.ToString(), Selected = false }; });
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
			length = unitsDTO.Units_ServiceType.Length;
			unitsDTO.UnitServiceTypes = new List<UnitServiceTypesDTO>();
			for (int i = 0; i < length; i++)
				unitsDTO.UnitServiceTypes.Add(new UnitServiceTypesDTO() { ServiceTypeID = unitsDTO.Units_ServiceType[i] });

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
			unitsDTO.Units_ID = Id;
			int length = 0;
			if (unitsDTO.Units_ReqType != null)
			{
				length = unitsDTO.Units_ReqType.Length;
				unitsDTO.Units_Request_Type = new List<Units_Request_TypeDTO>();
				for (int i = 0; i < length; i++)
					unitsDTO.Units_Request_Type.Add(new Units_Request_TypeDTO() { Request_Type_ID = unitsDTO.Units_ReqType[i] });
			}
			if (unitsDTO.Units_ServiceType != null)
			{
				length = unitsDTO.Units_ServiceType.Length;
				unitsDTO.UnitServiceTypes = new List<UnitServiceTypesDTO>();
				for (int i = 0; i < length; i++)
				{
					if (unitsDTO.ServiceTypeID != unitsDTO.Units_ServiceType[i])
						unitsDTO.UnitServiceTypes.Add(new UnitServiceTypesDTO() { ServiceTypeID = unitsDTO.Units_ServiceType[i] });
				}
			}
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

		[HttpPost]
		public JsonResult GetMainServices(int id, string MainService)
		{
			var Main = JsonConvert.DeserializeObject<List<int>>(MainService);
			var Data = APIHandeling.Post("Main_Services/GetActiveWithServiceTypeAndUnit?id=" + id, Main);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(JsonConvert.DeserializeObject<ICollection<MainServiceDTO>>(res.result.ToString()), JsonRequestBehavior.AllowGet);
			else
				return Json(new List<Object>(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult AddMainService(int id, string mainServicesDTO)
		{
			var Main = JsonConvert.DeserializeObject<Unit_MainServiceEditDTO>(mainServicesDTO);
			var Data = APIHandeling.Post("Units/UpdateMainService?id=" + id, Main);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult getUnits(int id, int? uID)
		{
			var Data = APIHandeling.getData($"Units/GetActiveUnits_byLevel?id={id}&uintId={(uID != null ? uID.Value.ToString() : "null")}");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(JsonConvert.DeserializeObject<ICollection<UnitsDTO>>(res.result.ToString()), JsonRequestBehavior.AllowGet);
			else
				return Json(new List<Object>(), JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult getEforms(int id)
		{
			var Data = APIHandeling.getData("E_Forms/GetE_FormsWithSubService?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(JsonConvert.DeserializeObject<ICollection<E_FormsDTO>>(res.result.ToString()), JsonRequestBehavior.AllowGet);
			else
				return Json(new List<Object>(), JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult getSub(int id)
		{
			var Data = APIHandeling.getData("Sub_Services/GetSub_ServicesByMain?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(JsonConvert.DeserializeObject<ICollection<SubServicesDTO>>(res.result.ToString()), JsonRequestBehavior.AllowGet);
			else
				return Json(new List<Object>(), JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult getUnitTypes(int id)
		{
			var Data = APIHandeling.getData("UnitTypes/GetUnits_TypeOFLevel?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(JsonConvert.DeserializeObject<ICollection<UnitTypeDTO>>(res.result.ToString()), JsonRequestBehavior.AllowGet);
			else
				return Json(new List<Object>(), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult getTypeOFUnit(int id)
		{
			var Data = APIHandeling.getData("UnitTypes/GetUnits_TypeOFUnit?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(JsonConvert.DeserializeObject<UnitTypeDTO>(res.result.ToString()), JsonRequestBehavior.AllowGet);
			else
				return Json(new List<Object>(), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetCode(string code, int id)
		{
			Console.WriteLine(code + "  " + id);
			var Data = APIHandeling.getData("Units/GenrateCode?Ref_Number=" + code + "&SubID=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			Console.WriteLine(res.result + "  ");
			return Json(res.result.ToString(), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUsers(int id)
		{
			var Data = APIHandeling.getData("User/GetAllByUnit?UID=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(JsonConvert.DeserializeObject<ICollection<UserDTO>>(res.result.ToString()), JsonRequestBehavior.AllowGet);
			else
				return Json(new List<Object>(), JsonRequestBehavior.AllowGet);
		}
	}
}