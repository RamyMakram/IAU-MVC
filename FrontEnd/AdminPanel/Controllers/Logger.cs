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
	public class LoggerController : BaseController
	{
		// GET: User
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Logs/GetAll");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<LogDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}

		public ActionResult TransDetails(int id)
		{
			var Data = APIHandeling.getData("Logs/Details?Did=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<LogDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}