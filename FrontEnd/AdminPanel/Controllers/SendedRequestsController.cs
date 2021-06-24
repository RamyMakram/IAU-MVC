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
	public class SendedRequestsController : Controller
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
			var isar = Request.Cookies["lang"].Value == "ar";



			var Data = APIHandeling.getData($"Request/GetRequest_Data?id={id}&UserID={Request.Cookies["u"].Value}");
			var resJson = Data.Content.ReadAsStringAsync();
			var Request_res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var IsMostafid = (TempData["IsMostafid"] as Nullable<bool>);
			if (Request_res.success)
			{
				Data = APIHandeling.getData($"Request/GetRequestTranscation?id={id}&UserID={Request.Cookies["u"].Value}");
				resJson = Data.Content.ReadAsStringAsync();
				var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
				ViewBag.RequestTransaction = JsonConvert.DeserializeObject<ICollection<Request_TransactionDTO>>(res.result.ToString());

				var RequestData = JsonConvert.DeserializeObject<ReqestDTO>(Request_res.result.ToString());

				if ((RequestData.TempCode == "" || RequestData.TempCode == null))
				{
					bool count = true;
					if (isar)
						ViewBag.Source = new List<string> { "مستفيد", "تواصل" }.ConvertAll(e => { count = !count; return new SelectListItem() { Text = e, Value = count.ToString() }; });
					else
						ViewBag.Source = new List<string> { "Mostafid", "Twasol" }.ConvertAll(e => { count = !count; return new SelectListItem() { Text = e, Value = count.ToString() }; });

					Data = APIHandeling.getData("UnitsLocation/GetActive");
					resJson = Data.Content.ReadAsStringAsync();
					res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
					var Locations = JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString());
					if (isar)
						ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_AR");
					else
						ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_EN");

					Data = APIHandeling.getData("Service_Type/GetActive");
					resJson = Data.Content.ReadAsStringAsync();
					res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
					var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
					if (isar)
						ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_AR");
					else
						ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_EN");

					Data = APIHandeling.getData("Request_State/GetActive");
					resJson = Data.Content.ReadAsStringAsync();
					res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
					ViewBag.Status = JsonConvert.DeserializeObject<ICollection<RequestStatusDTO>>(res.result.ToString());

					Data = APIHandeling.getData("Request_Type/GetActive");
					resJson = Data.Content.ReadAsStringAsync();
					res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
					var ReqTypes = JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString());
					if (isar)
						ViewBag.ReqTypes = new SelectList(ReqTypes, "Request_Type_ID", "Request_Type_Name_AR");
					else
						ViewBag.ReqTypes = new SelectList(ReqTypes, "Request_Type_ID", "Request_Type_Name_EN");

					Data = APIHandeling.getData("Units/GetActive");
					resJson = Data.Content.ReadAsStringAsync();
					res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
					var Units = JsonConvert.DeserializeObject<ICollection<UnitsDTO>>(res.result.ToString());
					if (isar)
						ViewBag.Units = new SelectList(Units, "Units_ID", "Units_Name_AR");
					else
						ViewBag.Units = new SelectList(Units, "Units_ID", "Units_Name_EN");
					return View("Preview", RequestData);

				}
				else
					return RedirectToAction("NotFound", "Error");
			}
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}