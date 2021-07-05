﻿using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
	public class ArchivedRequestsController : BaseController
	{
		// GET: ArchivedRequests
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Service_Type/GetActive");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
			ViewBag.ServiceType = Services;

			Data = APIHandeling.getData("Request_Type/GetActive");
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var ReqTypes = JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString());
			ViewBag.ReqTypes = ReqTypes;


			Data = APIHandeling.getData("Request/GetArchivedRequests_Data?UserID=" + Request.Cookies["u"].Value);
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<IEnumerable<ReqestDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Preview(int id)
		{
			var Data = APIHandeling.getData($"Request/GetArchivedRequest_Data?id={id}&UserID={Request.Cookies["u"].Value}");
			var resJson = Data.Content.ReadAsStringAsync();
			var Request_res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (Request_res.success)
			{
				Data = APIHandeling.getData($"Request/GetRequestTranscation?id={id}&UserID={Request.Cookies["u"].Value}");
				resJson = Data.Content.ReadAsStringAsync();
				var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
				ViewBag.RequestTransaction = JsonConvert.DeserializeObject<ICollection<Request_TransactionDTO>>(res.result.ToString());
				var RequestData = JsonConvert.DeserializeObject<ReqestDTO>(Request_res.result.ToString());
				return View("Preview", RequestData);
			}
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public JsonResult Filter(int? ST, int? RT, int? MT, DateTime? DT, DateTime? DF)
		{
			var Data = APIHandeling.getData($"Request/GetFilterArchivedRequests_Data?RT={RT}&ST={ST}&MT={MT}&DF={DF}&DT={DT}&UserID=" + Request.Cookies["u"].Value);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(res.result.ToString());
			else
				return Json("");
		}
	}
}