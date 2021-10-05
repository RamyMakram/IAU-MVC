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
	public class DelayedTrasactionController : BaseController
	{
		public ActionResult Preview(int id)
		{
			var Data = APIHandeling.getData($"DelayedRequest/GetTranscation?id={id}");
			var resJson = Data.Content.ReadAsStringAsync();
			var Request_res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (Request_res.success)
			{
				Data = APIHandeling.getData($"Request/GetRequestTranscation?id={id}&UserID={Request.Cookies["u"].Value}");
				resJson = Data.Content.ReadAsStringAsync();
				var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
				ViewBag.RequestTransaction = JsonConvert.DeserializeObject<ICollection<Request_TransactionDTO>>(res.result.ToString());
				var RequestData = JsonConvert.DeserializeObject<DelayedTransDTO>(Request_res.result.ToString());
				return View("Preview", RequestData);
			}
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}