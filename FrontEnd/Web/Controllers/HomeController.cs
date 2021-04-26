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
using Web.pdf;

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
			ApplicantRequest_Data_DTO request_Data = new ApplicantRequest_Data_DTO();
			var res = APIHandeling.getData("/ServiceType/GetAllServiceType", lang);
			var resJson = res.Content.ReadAsStringAsync();
			var lst = JsonConvert.DeserializeObject<Root>(resJson.Result);
			request_Data.serviceTypeList = lst.success ? lst.result.ServiceType : null;

			// //requestTypeList
			//requestTypeList
			res = APIHandeling.getData("/RequestType/GetAllRequestType", lang);
			resJson = res.Content.ReadAsStringAsync();
			lst = JsonConvert.DeserializeObject<Root>(resJson.Result);
			request_Data.requestTypeList = lst.success ? lst.result.RequestType : null;


			return View(request_Data);
		}

		[HttpGet]
		public JsonResult loadDocumentData()
		{
			try
			{
				var res = APIHandeling.getData("/Document/loadpage", Request.Cookies["lang"].Value);
				var resJson = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<Root>(resJson.Result).result;
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", Message = ex.Message });
			}
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
				var res = APIHandeling.getData("/Request/SendSMS?Mobile=" + to + "&message=" + message, Request.Cookies["lang"].Value);
				var resJson = res.Content.ReadAsStringAsync().Result;
				Response.Cookies.Add(new HttpCookie("n", Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(code.ToString())))));
				return Json(resJson, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public JsonResult loadApplicantData()
		{
			try
			{
				var res = APIHandeling.getData("/ApplicantData/loadApplicantData", Request.Cookies["lang"].Value);
				var resJson = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<Root>(resJson.Result).result;
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{

				return Json(new { Result = "ERROR", Message = ex.Message });
			}
		}


		[HttpGet]
		public JsonResult loadMainServiceData(int provideId)
		{
			try
			{
				var res = APIHandeling.getData("/Main_Service/GetMainServices?provideId=" + provideId, Request.Cookies["lang"].Value);
				var resJson = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<Root>(resJson.Result).result;
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{

				return Json(new { Result = "ERROR", Message = ex.Message });
			}
		}

		[HttpGet]
		public JsonResult loadSub_Services(int main_ID)
		{
			try
			{
				var res = APIHandeling.getData("/Sub_Services/GetSubServices?main_ID=" + main_ID, Request.Cookies["lang"].Value);
				var resJson = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<Root>(resJson.Result).result;
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", Message = ex.Message });
			}
		}


		[HttpPost]
		public async Task<object> saveApplicantDataAndRequest(string request_Data, string base64File, string code)
		{
			try
			{
				if (request_Data == null || base64File == null || base64File == "" || code == "")
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
					var res = APIHandeling.PostRequest("/Request/saveApplicantData", new RequestData_DTO() { PDFSignature = base64File, Request = data, Files = Files });
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


		[HttpPost]
		public string GenratePdfFile(ApplicantRequest_Data_DTO request_Data)
		{
			GeneratePDF pdf = new GeneratePDF();
			string base64File = pdf.GenratePDF(request_Data);
			return base64File;
		}
		[HttpGet]
		public ActionResult ChangeLang(string lang)
		{
			Response.Cookies.Set(new HttpCookie("lang", lang == "ar" ? lang : "en"));
			return RedirectToAction("Index","Home");
		}
	}
}