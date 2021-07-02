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
	public class SendedRequestsController : BaseController
	{
		// GET: Email
		public ActionResult Home()
		{
			var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";

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


			Data = APIHandeling.getData("Request/GetSendedRequests_Data?UserID=" + Request.Cookies["u"].Value);
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<IEnumerable<ReqestDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Preview(int id)
		{
			var Data = APIHandeling.getData($"Request/GetSendedRequest_Data?id={id}&UserID={Request.Cookies["u"].Value}");
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
		[HttpPost, ValidateInput(false)]
		public ActionResult Reminder(int req, string Comment)
		{
			var Data = APIHandeling.Post($"Request/AddReminder?UserID={Request.Cookies["u"].Value}&RequestID={req}", Comment);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return RedirectToAction("Preview", new { id = req });
			else
				return RedirectToAction("Home");
		}
		[HttpPost]
		public JsonResult Filter(int? ST, int? RT, int? MT, DateTime? DT, DateTime? DF)
		{
			var Data = APIHandeling.getData($"Request/GetFilterSendedRequests_Data?RT={RT}&ST={ST}&MT={MT}&DF={DF}&DT={DT}&UserID=" + Request.Cookies["u"].Value);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return Json(res.result.ToString());
			else
				return Json("");
		}
	}
}