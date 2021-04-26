using IAU.DTO.Entity;
using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static IAU.DTO.Enums.GlobalEnum;

namespace IAU_BackEnd.Controllers.Request
{
	public class RequestController : ApiController
	{

		[HttpPost]
		[Route("api/Request/saveApplicantData")]
		public IHttpActionResult SaveApplicantData(RequestData_DTO requestData)
		{
			try
			{
				List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();
				MostafidDatabaseEntities p = new MostafidDatabaseEntities();
				ApplicantRequest_Data_DTO model = requestData.Request;

				//save personal data
				Personel_Data personel_Data = new Personel_Data();
				personel_Data.ID_Document = model.ID_Document.Value;
				personel_Data.ID_Number = model.Document_Number;
				personel_Data.IAU_Affiliate_ID = model.Affiliated;
				personel_Data.IAU_ID_Number = model.IAUID;
				personel_Data.Applicant_Type_ID = model.Applicant_Type_ID;
				personel_Data.Title_Middle_Names_ID = model.title;
				personel_Data.First_Name = model.First_Name;
				personel_Data.Middle_Name = model.Middle_Name;
				personel_Data.Last_Name = model.Last_Name;
				personel_Data.Nationality_ID = model.Nationality_ID;
				personel_Data.Country_ID = model.Country_ID;
				personel_Data.City_Country_1 = model.City_Country_1;
				personel_Data.City_Country_2 = model.City_Country_2;
				personel_Data.Region_Postal_Code_1 = model.Region_Postal_Code_1;
				personel_Data.Region_Postal_Code_2 = model.Region_Postal_Code_2;
				personel_Data.Email = model.Email;
				personel_Data.Mobile = model.Mobile;
				p.Personel_Data.Add(personel_Data);
				p.SaveChanges();

				//save request data
				byte request_State_ID = (byte)RequestState.Created;

				Request_Data request_Data = new Request_Data();
				request_Data.Personel_Data_ID = personel_Data.Personel_Data_ID;
				request_Data.Provider_Academic_Services_ID = model.provider;
				request_Data.Sub_Services_ID = model.Sub_Services_ID;
				request_Data.Required_Fields_Notes = model.Required_Fields_Notes;
				request_Data.Request_Type_ID = model.Request_Type_ID;
				request_Data.Service_Type_ID = model.Service_Type_ID;
				request_Data.Code_Generate = generateCode();
				request_Data.CreatedDate = DateTime.Now;
				request_Data.Request_State_ID = request_State_ID;
				//IsTwasul=true, OC ON-CAMPUS بالمركز=false
				request_Data.IsTwasul_OC = bool.Parse(Device_Info[1]);

				if (model.Supporting_Documents_ID != 0)
				{
					request_Data.Supporting_Documents_ID = model.Supporting_Documents_ID;
				}
				var ss = p.Request_Data.Add(request_Data);
				p.SaveChanges();
				//files
				var path = HttpContext.Current.Server.MapPath("~");
				var requestpath = Path.Combine("RequestFiles", request_Data.Request_Data_ID.ToString());
				Directory.CreateDirectory(Path.Combine(path, requestpath));

				foreach (var file in requestData.Files)
				{
					var filename = Path.GetFileName(file.filename);
					var filepath = Path.Combine(requestpath, filename);
					File.WriteAllBytes(Path.Combine(path, filepath), file.bytes);
					p.Request_File.Add(new Request_File()
					{
						Request_ID = request_Data.Request_Data_ID,
						CreatedDate = DateTime.Now,
						File_Name = filename,
						File_Path = filepath.Replace("\\", "/")
					});
				}
				InsertRequestLog(request_State_ID, request_Data.Request_Data_ID, null);
				p.SaveChanges();

				string message = @"عزيزي المستفيد ، 
									تم استلام طلبكم بنجاح ، وسيتم افادتكم بالكود الخاص بالطلب خلال ٤٨ ساعه";
				_ = SendSMS(model.Mobile, message);
				return Ok(new
				{
					success = true
				});
			}
			catch (Exception e)
			{
				return Ok(new
				{
					success = false
				});
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request_State_ID"></param>
		/// <param name="request_ID"></param>
		/// <param name="Employee_ID">0 for creation a new request </param>
		private void InsertRequestLog(byte request_State_ID, int request_ID, int? Employee_ID)
		{
			MostafidDatabaseEntities p = new MostafidDatabaseEntities();
			p.Request_Log.Add(new Request_Log
			{
				Request_ID = request_ID,
				Request_State_ID = request_State_ID,
				CreatedDate = DateTime.Now,
				Employee_ID = Employee_ID,
			}); ;
			p.SaveChanges();
		}

		private string generateCode()
		{
			return DateTime.Now.ToString("yyyyMMddHHmm");
		}

		[HttpGet]
		[Route("api/Request/sendcode/{Mobile}/{code}")]
		public async Task<IHttpActionResult> Sendcode(string Mobile, string code)
		{
			try
			{
				string url = "http://iau-bsc.com/Follow/Index";
				string message = $@"عزيزي المستفيد ، 
									برجاء العلم بان الكود الخاص بطلبكم هو {code} ويمكنكم الاطلاع علي حالة الطلب باستخدام الرابط التالي {url}";
				var res = await SendSMS(Mobile, message);
				return Ok(new
				{
					success = true,
					result = new { res, message }
				});
			}
			catch (Exception e)
			{
				return Ok(new
				{
					success = false
				});
			}
		}

		[HttpGet]
		[Route("api/Request/SendSMS")]
		public async Task<IHttpActionResult> SendSMS(string Mobile, string message)
		{
			try
			{
				HttpClient h = new HttpClient();

				string url = $"http://basic.unifonic.com/wrapper/sendSMS.php?appsid=f9iRotRBsanfAB0xcE4NzJtgMYf5Bk&msg={message}&to={Mobile}&sender=IAU-BSC&baseEncode=False&encoding=UCS2";
				h.BaseAddress = new Uri(url);

				var res = h.GetAsync("").Result.Content.ReadAsStringAsync().Result;
				return Ok(new ResponseClass()
				{
					result = res,
					success = true
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass()
				{
					result = ee,
					success = false
				});
			}
		}

		[HttpGet]
		[Route("api/Request/SendMail")]
		public async Task<IHttpActionResult> SendMail(string Email, string message)
		{
			try
			{
				string to1 = "RamyMakramEyd@outlook.com";
				string from1 = "ramy@iau-bsc.com";
				string pass1 = "Ramy@IAU2212";

				MailMessage mail = new MailMessage();
				mail.From = new MailAddress(from1);
				mail.To.Add(new MailAddress(to1));
				mail.Body = "TestRamy";
				mail.Subject = "IAU BSC";

				SmtpClient client = new SmtpClient();
				client.Host = "mail.iau-bsc.com";
				client.Port = 25;
				client.UseDefaultCredentials = false;
				client.EnableSsl = false;

				NetworkCredential login = new NetworkCredential(from1, pass1);
				client.Credentials = login;
				client.Send(mail);
				return Ok(new ResponseClass()
				{
					success = true
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass()
				{
					result = ee,
					success = false
				});
			}
		}

		[HttpGet]
		[Route("api/Request/FollowRequest")]
		public HttpResponseMessage GetFollowRequest(string requestCode)
		{
			try
			{
				List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();
				string lang = Device_Info[2];

				MostafidDatabaseEntities p = new MostafidDatabaseEntities();
				var req = p.Request_Data.Where(r => r.Code_Generate == requestCode).FirstOrDefault();
				FollowRequest_DTO res = new FollowRequest_DTO();
				if (req != null)
				{
					res.requestid = req.Request_Data_ID;
					res.requestCode = requestCode;
					res.location = "location";
					res.statusId = req.Request_State_ID;
					res.status = (lang == "ar" ? req.Request_State.StateName_AR : req.Request_State.StateName_EN);
					res.deliverydate = req.CreatedDate;
				}
				return Request.CreateResponse(System.Net.HttpStatusCode.OK, res);

			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
			}
		}
	}
}
