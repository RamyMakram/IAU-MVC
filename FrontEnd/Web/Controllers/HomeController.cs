using IAU.DTO.Entity;
using IAU.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
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


		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult SendVerification(string to, string email)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			try
			{
				int code = new Random().Next(1000, 9999);
				Debug.WriteLine(code);
				string message = $@"Use this code {code} to complete your request.";
				var res = APIHandeling.getDataAdmin("/Request/NotifyUser?Mobile=" + to + "&message=" + message + "&Email=" + email);
				var resJson = res.Content.ReadAsStringAsync().Result;
				string HashedCode = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(code.ToString())));
				Response.Cookies.Add(new HttpCookie("n", HashedCode));
				return Json(resJson, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken()]
		public async Task<object> saveApplicantDataAndRequest(string request_Data, string code)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			try
			{
				if (request_Data == null || code == "")
					return null;
				var shaCode = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(code)));
				if (Request.Cookies["n"] != null && shaCode == Request.Cookies["n"].Value)
				{
					var Files = new List<CustomeFile>();
					List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
					HttpClientHandler handler = new HttpClientHandler();
					using (var client = new HttpClient(handler, false))
					{
						client.DefaultRequestHeaders.Add("crd", "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks");
						using (var content = new MultipartFormDataContent())
						{
							int length = Request.Files.Count;

							for (int i = 0; i < length; i++)
							{
								HttpPostedFileBase file = Request.Files[i];
								byte[] Bytes = new byte[file.InputStream.Length + 1];
								file.InputStream.Read(Bytes, 0, Bytes.Length);
								var fileContent = new ByteArrayContent(Bytes);
								fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
								content.Add(fileContent);
							}
							var stringContent = new StringContent(request_Data);
							stringContent.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
							content.Add(stringContent, "json");

							var requestUri = APIHandeling.AdminURL + "/api/Request/saveApplicantData";
							var result = client.PostAsync(requestUri, content).Result;
							if (result.StatusCode == System.Net.HttpStatusCode.OK)
							{
								var d = result.Content.ReadAsStringAsync();
								var lst = JsonConvert.DeserializeObject<ResponseClass>(d.Result);
								if (lst.success)
								{
									Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-30), Value = "" });
									return JsonConvert.SerializeObject(new ResponseClass() { success = true });
								}
								else
									return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = lst.result });
							}
							return JsonConvert.SerializeObject(new ResponseClass() { success = false });
						}
					}
					//var res = APIHandeling.PostRequest(", new RequestData_DTO() { Request = data, Files = files });
					//var resJson = res.Content.ReadAsStringAsync();
					//var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
					//if (lst.success)
					//	return JsonConvert.SerializeObject(new ResponseClass() { success = true });
					//else
					//	return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = lst.result });
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
		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult GetRequest(int ServiceID)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			var res = APIHandeling.getData("/RequestType/GetActive?SID=" + ServiceID);
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult GetApplicantData(int ServiceID, int RequestType)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			var res = APIHandeling.getData("/AppType/GetActive?SID=" + ServiceID + "&ReqType=" + RequestType);
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult GetCityRegion(int CID)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			if (CID != 24)
				return Json(JsonConvert.SerializeObject(new CountryAndRegion() { Regions = new List<RegionDTO>(), City = new List<CityDTO>() }), JsonRequestBehavior.AllowGet);

			var res = APIHandeling.getData("/Region_City/GetActive?CountryID=" + CID);
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var isar = Request.Cookies["lang"].Value == "ar";
			var data = JsonConvert.DeserializeObject<CountryAndRegion>(response.result.ToString());
			data.City = data.City.OrderBy(q => isar ? q.City_Name_AR : q.City_Name_EN).ToList();
			data.Regions = data.Regions.OrderBy(q => isar ? q.Region_Name_AR : q.Region_Name_EN).ToList();
			return Json(JsonConvert.SerializeObject(data), JsonRequestBehavior.DenyGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult GetProviders(int RID, int SID, int AID)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			var res = APIHandeling.getData($"/Units/GetActive?ReqID={RID}&SerID={SID}&AppType={AID}");
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult GetMainServices(int ID, int SID, int AID)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			var res = APIHandeling.getData($"/MainService/GetActive?UID={ID}&ServiceID={SID}&AppType={AID}");
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult GetSub(int ID)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);
			var res = APIHandeling.getData($"/SubServices/GetActive?MainService={ID}");
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken()]
		public JsonResult GetEforms(int ID)
		{
			if (!HttpContext.Request.IsAjaxRequest())
				return Json("401", JsonRequestBehavior.AllowGet);

			var res = APIHandeling.getData($"/Eforms/GetActive?SubService={ID}");
			var resJson = res.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			return Json(response.result.ToString(), JsonRequestBehavior.AllowGet);
		}
	}
}