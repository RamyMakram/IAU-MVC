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
    public class RequestTypeController : BaseController
    {
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Request_Type/GetRequest_Type");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult AllDel()
		{
			var Data = APIHandeling.getData("Request_Type/GetDeleted");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return PartialView("DeletedModelsView", JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString()));
			else
				return PartialView("DeletedModelsView", null);
		}
		[HttpPost]
		public JsonResult RestoreItem(int id)
		{
			var Data = APIHandeling.Post("Request_Type/_Restore?id=" + id, new { });
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(res);
		}
		public ActionResult Detials(int id)
		{
			var Data = APIHandeling.getData("Request_Type/GetRequest_Type?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<RequestTypeDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Create()
		{
			return View();
		}
		public ActionResult Edit(int id)
		{
			var Data = APIHandeling.getData("Request_Type/GetRequest_Type?id=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<RequestTypeDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Create(RequestTypeDTO loc)
		{
			HttpPostedFileBase file = loc.Files[0];
			byte[] Bytes = new byte[file.InputStream.Length + 1];
			file.InputStream.Read(Bytes, 0, Bytes.Length);
			loc.Base64 = Convert.ToBase64String(Bytes);
			loc.Files = null;
			var Req = APIHandeling.Post("Request_Type/Create", loc);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Edit(int Id,RequestTypeDTO loc)
		{
			loc.Request_Type_ID = Id;
			var Req = APIHandeling.Post("Request_Type/Edit", loc);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Deactive(int id)
		{
			return View(new RequestTypeDTO() { Request_Type_ID = id });
		}
		public ActionResult Active(int id)
		{
			return View(new RequestTypeDTO() { Request_Type_ID = id });
		}

		[HttpPost]
		public ActionResult Deactive(int id, string x)
		{
			var Req = APIHandeling.getData("Request_Type/Deactive?id=" + id);
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
			var Req = APIHandeling.getData("Request_Type/Active?id=" + id);
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
			var Data = APIHandeling.Post("Request_Type/_Delete?id=" + id, new { });
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(res);
		}
	}
}