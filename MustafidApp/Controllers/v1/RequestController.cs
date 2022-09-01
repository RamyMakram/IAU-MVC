using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MustafidAppDTO.DTO;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MustafidAppModels.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MustafidApp.Helpers;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using MustafidAppModels.Models;
using System.Net.Http;
using System.IO;

namespace MustafidApp.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private MustafidAppContext _appContext;
        private IMapper _mapper;
        private readonly IConfiguration _configuration;

        public RequestController(MustafidAppContext appContext, IMapper mapper, IConfiguration configuration)
        {
            _appContext = appContext;
            _mapper = mapper;
            _configuration = configuration;
        }


        /// <summary>
        /// Return All User Reuqets's code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;
            var data = await _appContext.RequestData.Where(q => q.PersonelData.Mobile == Mobile_Phone && q.CodeGenerate != null).Select(q => q.CodeGenerate).ToListAsync();

            return Ok(new ResponseClass() { Success = true, data = data });
        }
        /// <summary>
        /// Return Object OF Request 
        /// </summary>
        /// <remarks>
        /// Req_Current_Unit Reperst CurrenUnit That Process request
        /// <br/>
        /// Req_Status Is Status Of Request: <br/>
        ///     ==1 تأكيد الطلب 
        ///     <br/>
        ///     ==2 ارسال إلى مزود الخدمة
        ///     <br/>
        ///     ==3 معالجة الطلب
        ///     <br/>
        ///     ==4 تقديم طلب الإرسال
        ///     <br/>
        ///     ==5 تم تسليم الطلب 
        /// <br/>
        /// Req_Current_DateEnd Date For Delivery
        /// <br/>
        /// Req_Is_Mos Is Twasel or not
        /// <br/>
        /// </remarks>
        /// <param name="Code">Request Code</param>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetRequestsInfo(string Code)
        {
            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;
            var data = await _appContext.RequestData
                .Include(q => q.PersonelData)
                .Include(q => q.RequestState)
                .Include(q => q.RequestTransactions).ThenInclude(q => q.ToUnit)
                .FirstOrDefaultAsync(q => q.CodeGenerate == Code);

            if (data == null)
                return Ok(new ResponseClass() { Success = false, data = "NullData" });
            if (data.PersonelData.Mobile != Mobile_Phone)
                return Ok(new ResponseClass() { Success = false, data = "NotAllowed" });

            var data_DTO = _mapper.Map<RequestDTO>(data);

            var LastTrans = data.RequestTransactions.LastOrDefault();

            data_DTO.Req_Current_DateEnd = LastTrans?.ExpireDays;
            data_DTO.Req_Current_Unit = _mapper.Map<UnitsDTO>(LastTrans.ToUnit);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Send Code To Confirm User
        /// </summary>
        /// <param name="Email">User Email</param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> ConfirmRequest(string Email)
        {
            int code = new Random().Next(1000, 9999);

            string message_en = $@"Use this code {code} to complete your Request.";
            string message_ar = $@"برجاء استخدام هذا الكود {code} لتاكيد طلبك.";

            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;

            var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());

            var res = HttpClientAdminBackend.getDataAdmin($"/Request/NotifyUser?Mobile={Mobile_Phone}&message_en={message_en}&message_ar={message_ar}&Email={Email}", _configuration);

            var resJson = await res.Content.ReadAsStringAsync();

            var Res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            if (Res.Success)
                return Ok(new ResponseClass() { Success = true, data = CypherCode });
            else
                return Ok(new ResponseClass() { Success = false });
        }

        [HttpPost]
        public async Task<IActionResult> SaveRequest(int code, string C_Code, RequestDTO request)
        {
            var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());

            if (CypherCode == C_Code)
            {
                //var request_Data = _mapper.Map<RequestDatum>(request);
                //HttpClientHandler handler = new HttpClientHandler();
                //using (var client = new HttpClient(handler, false))
                //{
                //    client.DefaultRequestHeaders.Add("crd", "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks");
                //    using (var content = new MultipartFormDataContent())
                //    {
                //        int length = Request.Form.Files.Count;

                //        for (int i = 0; i < length; i++)
                //        {
                //            var file = Request.Form.Files[i];
                //            byte[] Bytes;
                //            using (var ms = new MemoryStream())
                //            {
                //                file.CopyTo(ms);
                //                Bytes = ms.ToArray();
                //            }
                //            var fileContent = new ByteArrayContent(Bytes);
                //            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                //            content.Add(fileContent);
                //        }
                //        var stringContent = new StringContent(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<ApplicantRequest_Data_DTO>(request_Data).Personel_Data.E_Forms_Answer));
                //        stringContent.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
                //        content.Add(stringContent, "json");
                //        stringContent = new StringContent(JsonConvert.SerializeObject(request_Data));
                //        stringContent.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
                //        content.Add(stringContent, "json");

                //        var requestUri = APIHandeling.AdminURL + "/api/Request/saveApplicantData";
                //        var result = client.PostAsync(requestUri, content).Result;
                //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                //        {
                //            var d = result.Content.ReadAsStringAsync();
                //            var lst = JsonConvert.DeserializeObject<ResponseClass>(d.Result);
                //            if (lst.success)
                //            {
                //                Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-30), Value = "" });
                //                return JsonConvert.SerializeObject(new ResponseClass() { success = true });
                //            }
                //            else
                //                return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = lst.result });
                //        }
                //        return JsonConvert.SerializeObject(new ResponseClass() { success = false });
                //    }
                //}

                return Ok(new ResponseClass() { Success = true/*, data = token */});
            }
            else
                return Ok(new ResponseClass() { Success = false });

        }

    }
}
