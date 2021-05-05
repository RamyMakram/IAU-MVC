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
	public class EmailController : BaseController
	{
		// GET: Email
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Request/GetRequest_Data");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<IEnumerable<ReqestDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Preview(int id)
		{
			return View();
		}public ActionResult PDF()
		{
			return View();
		}
		public string RequestFiles(string ID)
		{
			var Data = APIHandeling.getDataRequestFile("/RequestFiles/" + ID + "/PDF.txt");
			var resJson = Data.Content.ReadAsStringAsync();
			return resJson.Result;
		}
	}
}