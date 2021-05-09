using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
	public class SubServicesController : BaseController
	{
		public ActionResult Home()
		{
			HttpResponseMessage Data = null;
			if (Request.QueryString["MainService"] == null)
				Data = APIHandeling.getData("Sub_Services/GetSub_Services");
			else
				Data = APIHandeling.getData("Sub_Services/GetSub_ServicesByMain?id=" + Request.QueryString["MainService"]);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<SubServicesDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Detials(int Id)
		{
			var Data = APIHandeling.getData("Sub_Services/GetSub_Services?id=" + Id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<SubServicesDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Edit(int Id)
		{
			var Data = APIHandeling.getData("Sub_Services/GetSub_Services?id=" + Id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
			{
				LoadEditOrCreate('e');
				var data = JsonConvert.DeserializeObject<SubServicesDTO>(res.result.ToString());
				return View(data);
			}
			else
				return RedirectToAction("NotFound", "Error");
		}
		public void LoadEditOrCreate(char c)
		{
			if (Request.QueryString["MainService"] == null || c == 'e')
			{
				var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";
				var Data = APIHandeling.getData("Main_Services/GetActive");
				var resJson = Data.Content.ReadAsStringAsync();
				var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
				if (res.success)
				{
					var data = JsonConvert.DeserializeObject<ICollection<MainServiceDTO>>(res.result.ToString());
					if (isar)
						ViewBag.MainServices = new SelectList(data, "Main_Services_ID", "Main_Services_Name_AR");
					else
						ViewBag.MainServices = new SelectList(data, "Main_Services_ID", "Main_Services_Name_EN");
				}
			}
		}
		public ActionResult Create()
		{
			LoadEditOrCreate('c');
			return View();
		}
		public ActionResult Deactive(int id)
		{
			return View(new SubServicesDTO() { Sub_Services_ID = id });
		}
		public ActionResult Active(int id)
		{
			return View(new SubServicesDTO() { Sub_Services_ID = id });
		}

		[HttpPost]
		public ActionResult Edit(int Id, SubServicesDTO subServicesDTO)
		{
			var data = JsonConvert.DeserializeObject<ICollection<RequiredDocsDTO>>(subServicesDTO.Required);
			subServicesDTO.Sub_Services_ID = Id;
			subServicesDTO.Required_Documents = data;
			var Req = APIHandeling.Post("Sub_Services/Update", subServicesDTO);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}

		[HttpPost]
		public ActionResult Create(SubServicesDTO subServicesDTO, int? MainService)
		{
			var data = JsonConvert.DeserializeObject<ICollection<RequiredDocsDTO>>(subServicesDTO.Required);
			subServicesDTO.Required_Documents = data;
			if (MainService != null)
				subServicesDTO.Main_Services_ID = MainService;
			var Req = APIHandeling.Post("Sub_Services/Create", subServicesDTO);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return MainService == null ? RedirectToAction("Home") : RedirectToAction("Home", new { MainService = MainService });
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Deactive(int id, string x)
		{
			var Req = APIHandeling.getData("Sub_Services/Deactive?id=" + id);
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
			var Req = APIHandeling.getData("Sub_Services/Active?id=" + id);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}