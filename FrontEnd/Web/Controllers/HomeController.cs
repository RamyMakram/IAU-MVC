using IAU.DTO.Entity;
using IAU.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;

namespace Web.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index(string u)
		{
			var lang = Request.Cookies["lang"].Value;
			if (u == null || u == "")
				Response.Cookies.Add(new HttpCookie("us", null));
			else
				Response.Cookies.Add(new HttpCookie("us", u));
			var res = APIHandeling.getData("/_Home/LoadMain");
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (response.success)
			{
				ViewBag.CookieLang = lang;
				return View(JsonConvert.DeserializeObject<_HomeDTO>(response.result.ToString()));
			}

			return RedirectToAction("Error");
		}


		[HttpGet]
		public ActionResult RedirectTo()
		{
			try
			{
				return new RedirectResult("https://iau-bsc.iau.edu.sa/");
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", Message = ex.Message });
			}
		}


		[HttpGet]
		public JsonResult SendVerification(string to)
		{
			try
			{
				int code = new Random().Next(1000, 9999);
				Debug.WriteLine(code);
				//Console.WriteLine(code);
				code = 1111;
				string message = $@"Use this code {code} to complete your request.";
				var res = APIHandeling.getData("/Request/SendSMS?Mobile=" + to + "&message=" + message);
				var resJson = res.Content.ReadAsStringAsync().Result;
				Response.Cookies.Add(new HttpCookie("n", Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(code.ToString())))));
				return Json(resJson, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public async Task<object> saveApplicantDataAndRequest(string request_Data, string code)
		{
			try
			{
				if (request_Data == null || code == "")
					return null;
				var shaCode = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(code)));
				if (shaCode == Request.Cookies["n"].Value)
				{
					var data = JsonConvert.DeserializeObject<ApplicantRequest_Data_DTO>(request_Data);
					var Files = new List<CustomeFile>();
					HttpFileCollectionBase files = Request.Files;
					int length = Request.Files.Count;

					for (int i = 0; i < length; i++)
					{
						HttpPostedFileBase file = files[i];
						byte[] Bytes = new byte[file.InputStream.Length + 1];
						file.InputStream.Read(Bytes, 0, Bytes.Length);
						Files.Add(new CustomeFile() { bytes = Bytes, filename = file.FileName });
					}
					var res = APIHandeling.PostRequest("/Request/saveApplicantData", new RequestData_DTO() { Request = data, Files = Files });
					var resJson = res.Content.ReadAsStringAsync();
					var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
					if (lst.success)
						return JsonConvert.SerializeObject(new ResponseClass() { success = true });
					else
						return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = lst.result });
				}
				else
					return JsonConvert.SerializeObject(new ResponseClass() { result = "ErrorInCode", success = false });

			}
			catch (Exception rr)
			{
				return JsonConvert.SerializeObject(new ResponseClass() { result = rr, success = false });
			}
		}

		[HttpGet]
		public ActionResult ChangeLang(string lang)
		{
			Response.Cookies.Set(new HttpCookie("lang", lang == "ar" ? lang : "en"));
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public JsonResult GetApplicantData(int ServiceID)
		{
			var res = APIHandeling.getData("/AppType/GetActive?SID=" + ServiceID);
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult GetCityRegion(int CID)
		{
			var res = APIHandeling.getData("/Region_City/GetActive?CountryID=" + CID);
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}
	}
}