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
	public class GlobalSettingsController : BaseController
	{
		// GET: GlobalSettings
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("GeneralSetting/GetStatusDelayeValue");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Status = JsonConvert.DeserializeObject<ICollection<RequestStatusDTO>>(res.result.ToString());
			return View(Status);
		}

		[HttpPost]
		public JsonResult SaveData(string data)
		{
			var Listdata = JsonConvert.DeserializeObject<ICollection<RequestStatusDTO>>(data);
			var Data = APIHandeling.Post("GeneralSetting/SaveDelayeValue", Listdata);
			var resJson = Data.Content.ReadAsStringAsync();
			return Json("");
		}
	}
}