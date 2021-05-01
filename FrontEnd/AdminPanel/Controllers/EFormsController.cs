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
	public class EFormsController : BaseController
	{
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("E_Forms/GetE_Forms");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<E_FormsDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Detials(int id)
		{
			var Data = APIHandeling.getData("E_Forms/GetE_Forms?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<E_FormsDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public void LoadCreateOrEdit()
		{
			var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";
			var Data = APIHandeling.getData("Sub_Services/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var SubSrevice = JsonConvert.DeserializeObject<ICollection<SubServicesDTO>>(res.result.ToString());
			if (isar)
				ViewBag.SubSrevice = new SelectList(SubSrevice, "Sub_Services_ID", "Sub_Services_Name_AR");
			else
				ViewBag.SubSrevice = new SelectList(SubSrevice, "Sub_Services_ID", "Sub_Services_Name_EN");
		}
		public ActionResult Create()
		{
			LoadCreateOrEdit();
			return View();
		}
		public ActionResult Edit(int id)
		{
			LoadCreateOrEdit();
			var Data = APIHandeling.getData("E_Forms/GetE_Forms?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<E_FormsDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Create(E_FormsDTO loc)
		{
			HttpPostedFileBase file = loc.Files[0];
			loc.FileName = file.FileName;
			byte[] Bytes = new byte[file.InputStream.Length + 1];
			file.InputStream.Read(Bytes, 0, Bytes.Length);
			loc.Base64 = Convert.ToBase64String(Bytes);
			loc.Files = null;
			var Req = APIHandeling.Post("E_Forms/Create", loc);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Edit(int Id, E_FormsDTO loc)
		{
			loc.ID = Id;
			var Req = APIHandeling.Post("E_Forms/Update", loc);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Deactive(int id)
		{
			return View(new E_FormsDTO() { ID = id });
		}
		public ActionResult Active(int id)
		{
			return View(new E_FormsDTO() { ID = id });
		}

		[HttpPost]
		public ActionResult Deactive(int id, string x)
		{
			var Req = APIHandeling.getData("E_Forms/Deactive?id=" + id);
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
			var Req = APIHandeling.getData("E_Forms/Active?id=" + id);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}