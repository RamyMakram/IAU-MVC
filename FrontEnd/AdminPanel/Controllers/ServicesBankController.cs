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
	public class ServicesBankController : BaseController
	{
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Main_Services/GetMain_Services");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<MainServiceDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Detials(int Id)
		{
			var Data = APIHandeling.getData("Main_Services/GetMain_Services?id=" + Id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<MainServiceDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Edit(int Id)
		{
			var Data = APIHandeling.getData("Main_Services/GetMain_Services?id=" + Id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
			{
				LoadEditOrCreate();
				var data = JsonConvert.DeserializeObject<MainServiceDTO>(res.result.ToString());
				int length = data.ValidTo.Count;
				data.Applicant_Types = new int[length];
				for (int i = 0; i < length; i++)
					data.Applicant_Types[i] = data.ValidTo.ElementAt(i).ApplicantTypeID;
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
			var Data = APIHandeling.getData("Applicant_Type/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Applicanttype = JsonConvert.DeserializeObject<ICollection<ApplicantTypeDTO>>(res.result.ToString());
			if (isar)
				ViewBag.ApplicantType = new SelectList(Applicanttype, "Applicant_Type_ID", "Applicant_Type_Name_AR");
			else
				ViewBag.ApplicantType = new SelectList(Applicanttype, "Applicant_Type_ID", "Applicant_Type_Name_EN");
			Data = APIHandeling.getData("Service_Type/GetActive");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
			if (isar)
				ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_AR");
			else
				ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_EN");
		}
		public ActionResult Deactive(int id)
		{
			return View(new MainServiceDTO() { Main_Services_ID = id });
		}
		public ActionResult Active(int id)
		{
			return View(new MainServiceDTO() { Main_Services_ID = id });
		}
		[HttpPost]
		public ActionResult Create(MainServiceDTO MainServiceDTO)
		{
			int length = MainServiceDTO.Applicant_Types.Length;
			MainServiceDTO.ValidTo = new List<ValidToDTO>();
			for (int i = 0; i < length; i++)
				MainServiceDTO.ValidTo.Add(new ValidToDTO() { ApplicantTypeID = MainServiceDTO.Applicant_Types[i] });

			var Req = APIHandeling.Post("Main_Services/Create", MainServiceDTO);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Edit(int Id, MainServiceDTO MainServiceDTO)
		{
			var data = MainServiceDTO.Applicant_Types;
			MainServiceDTO.Main_Services_ID = Id;
			int length = MainServiceDTO.Applicant_Types.Length;
			MainServiceDTO.ValidTo = new List<ValidToDTO>();
			for (int i = 0; i < length; i++)
				MainServiceDTO.ValidTo.Add(new ValidToDTO() { ApplicantTypeID = MainServiceDTO.Applicant_Types[i] });
			var Req = APIHandeling.Post("Main_Services/Update", MainServiceDTO);
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
			var Req = APIHandeling.getData("Main_Services/Deactive?id=" + id);
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
			var Req = APIHandeling.getData("Main_Services/Active?id=" + id);
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
			var Data = APIHandeling.Post("Main_Services/_Delete?id=" + id, new { });
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(res);
		}
	}
}