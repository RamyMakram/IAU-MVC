﻿using Microsoft.AspNetCore.Authorization;
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
using Microsoft.Extensions.Hosting;
using MustafidApp.Helpers.SaveRequest;
using System.Threading;
using Microsoft.IdentityModel.Tokens;
using System.Text.Encodings.Web;
using System.Web;

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
        private readonly IHostingEnvironment environment;

        public RequestController(MustafidAppContext appContext, IMapper mapper, IConfiguration configuration, IHostingEnvironment _environment)
        {
            _appContext = appContext;
            _mapper = mapper;
            _configuration = configuration;
            environment = _environment;
        }


        /// <summary>
        /// Return All User Reuqets's code
        /// </summary>
        /// <param name="P_Index">Page Index</param>
        /// <param name="P_Size">Page Size</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRequests(int P_Size, int P_Index)
        {
            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;
            var data = await _appContext.RequestData
                .Include(q => q.PersonelData)
                .Include(q => q.RequestState)
                .Include(q => q.RequestTransactions).ThenInclude(q => q.ToUnit).Where(q => q.PersonelData.Mobile == Mobile_Phone).OrderByDescending(q => q.CreatedDate).Skip(P_Index * P_Size).Take(P_Size).ToListAsync();

            //if (data == null)
            //    return Ok(new ResponseClass() { Success = false, data = "NullData" });
            //if (data.PersonelData.Mobile != Mobile_Phone)
            //    return Ok(new ResponseClass() { Success = false, data = "NotAllowed" });

            var data_DTO = _mapper.Map<List<RequestDTO>>(data);
            int count = 0;
            foreach (var i in data_DTO)
            {
                var LastTrans = data[count].RequestTransactions.LastOrDefault();

                i.Req_Current_DateEnd = LastTrans?.ExpireDays;
                i.Req_Current_DateStart = LastTrans?.ForwardDate;
                i.Req_Current_Unit = _mapper.Map<UnitsDTO>(LastTrans?.ToUnit);
                count++;
            }

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ConfirmRequest(string Email)
        {
            int code = new Random().Next(1000, 9999);

            string message_en = $@"Use this code {code} to complete your Request.";
            string message_ar = $@"برجاء استخدام هذا الكود {code} لتاكيد طلبك.";

            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;

            if (Mobile_Phone.Contains("966xxxxxxxxx"))
                code = 9999;

            var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());

            var res = HttpClientAdminBackend.getDataAdmin($"/Request/NotifyUser?Mobile={HttpUtility.UrlEncode(Mobile_Phone)}&message_en={HttpUtility.UrlEncode(message_en)}&message_ar={HttpUtility.UrlEncode(message_ar)}" + $"&Email=", _configuration);

            var resJson = await res.Content.ReadAsStringAsync();

            var Res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            if (Res.Success)
                return Ok(new ResponseClass() { Success = true, data = CypherCode });
            else
                return Ok(new ResponseClass() { Success = false });
        }
        /// <summary>
        /// Save Request
        /// </summary>
        /// <param name="code">Code That User Enter IT</param>
        /// <param name="C_Code">Return Of Api</param>
        /// <param name="request">Req Object As FormData</param>
        /// 
        /// 
        /// <remarks>
        ///     {        
        ///         "Req_U_ID": Unit ID حدد الخدمة,
        ///         "Req_SS_ID": Sub Service ID,
        ///         "Req_Notes": Requst Notes,
        ///         "Req_S_ID": Service Type ID,
        ///         "Req_R_ID": Request Type ID,
        ///         "Req_Is_Mos": always is 'true',
        ///         "EformID": [
        ///              1,
        ///              2,
        ///              3
        ///          ] as Json string,
        ///          "PD_Address_C_ID": الدولة,
        ///          "PD_APP_ID": Applicant Type ID,
        ///          "PD_C_ID": مكان الاقامة,
        ///          "PD_F_Name": ,
        ///          "PD_L_Name": "incididunt aliquip",
        ///          "PD_M_Name": "id irure ",
        ///          "PD_ID_Doc_ID": نوع الهوية,
        ///          "PD_ID_Number": رقم الهوية,
        ///          "PD_mail": ,
        ///          "PD_National_ID": الجنسية,
        ///          "PD_TitleNames_ID": Mrs.,
        ///          "PD_IAUNumber": If Affliated,
        ///          "PD_Address_City_ID": المنطقة,
        ///          "PD_Adress_R_ID": المدينة,
        ///          "PD_Address_City": string : المنطقة,
        ///          "PD_Adress_Region": string : المدينة,
        ///          "PD_Postal": ,
        ///         "Req_ApplicantData": {
        ///          
        ///          
        ///      }
        ///     }
        ///dd
        /// </remarks>
        /// <returns></returns>
        //[AllowAnonymous]
        [HttpPost]
        [RequestSizeLimit(100_000_000_000)]
        public async Task<IActionResult> SaveRequest([FromForm] int code, [FromForm] string C_Code, [FromForm] RequestDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());

                if (CypherCode == C_Code)
                {
                    var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;
                    var logreq = new ReqLogObject { Date = DateTime.Now };

                    var trans = _appContext.Database.BeginTransaction();

                    request.PD_Phone = Mobile_Phone;

                    logreq.Dto = request;
                    //if (request.Req_ApplicantData.PD_JSON_EFormAnswer != null && request.Req_ApplicantData.PD_JSON_EFormAnswer.Length != 0)
                    //    request.Req_ApplicantData.PD_EFormAnswer = JsonConvert.DeserializeObject<List<EformAnsDTO>>(request.Req_ApplicantData.PD_JSON_EFormAnswer);


                    var request_Data = _mapper.Map<RequestDatum>(request);
                    logreq.Real = request_Data;
                    var path = Path.Combine(environment.ContentRootPath, "Log", "Log.json");

                    if (!System.IO.File.Exists(path))
                        System.IO.File.Create(path);

                    System.IO.File.AppendAllText(path, JsonConvert.SerializeObject(logreq, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                    request_Data.PersonelData.ApplicantTypeId = request.PD_APP_ID;


                    var model = request_Data.PersonelData;
                    var personel_Data = _appContext.PersonelData.FirstOrDefault(q => (q.IdDocument == model.IdDocument && q.IdNumber == model.IdNumber) || q.Mobile == model.Mobile);
                    if (personel_Data == null)
                    {
                        _appContext.PersonelData.Add(model);
                        await _appContext.SaveChangesAsync();
                        request_Data.PersonelDataId = model.PersonelDataId;
                    }
                    else
                        request_Data.PersonelDataId = personel_Data.PersonelDataId;




                    request_Data.CreatedDate = DateTime.Now;

                    #region CheckDeleted
                    var r_type = _appContext.RequestTypes.Find(request_Data.RequestTypeId);
                    if (r_type == null)
                    {
                        trans.Rollback();
                        return Ok(new
                        {
                            result = "No RT",
                            success = false
                        });
                    }
                    if (r_type.Deleted)
                    {
                        trans.Rollback();
                        return Ok(new
                        {
                            result = "Del RT",
                            success = false
                        });
                    }
                    request_Data.SubServicesId = request_Data.SubServicesId == 0 ? null : request_Data.SubServicesId;
                    if (request_Data.SubServicesId != null)
                    {
                        var ss_id = _appContext.SubServices.Find(request_Data.SubServicesId);
                        if (ss_id == null)
                        {
                            trans.Rollback();
                            return Ok(new
                            {
                                result = "No SS",
                                success = false
                            });
                        }
                        if (ss_id.Deleted)
                        {
                            trans.Rollback();
                            return Ok(new
                            {
                                result = "Del SS",
                                success = false
                            });
                        }
                    }

                    if (request_Data.ServiceTypeId == null)
                    {
                        trans.Rollback();
                        return Ok(new
                        {
                            result = "No ST",
                            success = false
                        });
                    }

                    var st = _appContext.ServiceTypes.Find(request_Data.ServiceTypeId);
                    if (st == null || st.Deleted)
                    {
                        trans.Rollback();
                        return Ok(new
                        {
                            result = "DelOrNo ST",
                            success = false
                        });
                    }
                    #endregion

                    //request_Data.Request_State_ID = 1;
                    request_Data.IsTwasulOc = false;
                    request_Data.Readed = false;
                    request_Data.IsArchived = false;
                    request_Data.RequestStateId = 1;
                    _appContext.RequestData.Add(request_Data);
                    await _appContext.SaveChangesAsync();

                    if (string.IsNullOrEmpty(request.EformID.Trim()))
                    {
                        var EformnsID = JsonConvert.DeserializeObject<List<int>>(request.EformID);
                        foreach (var EFORM in EformnsID)
                        {
                            var ef = await _appContext.PersonEforms.FindAsync(EFORM);
                            ef.PersonId = model.PersonelDataId;
                            ef.RequestId = request_Data.RequestDataId;
                        }
                    }
                    await _appContext.SaveChangesAsync();




                    HttpClientHandler handler = new HttpClientHandler();
                    using (var client = new HttpClient(handler, false))
                    {
                        client.DefaultRequestHeaders.Add("crd", "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks");

                        using (var content = new MultipartFormDataContent())
                        {
                            //int length = Request.Form.Files.Count;
                            if (request.Req_RequiredDocs != null)
                                foreach (var i in request.Req_RequiredDocs)
                                {
                                    var file = i;
                                    if (i.FileName.Contains("-"))
                                    {
                                        byte[] Bytes;
                                        using (var ms = new MemoryStream())
                                        {
                                            file.CopyTo(ms);
                                            Bytes = ms.ToArray();
                                        }
                                        var fileContent = new ByteArrayContent(Bytes);
                                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName.Replace("-", "|") };
                                        content.Add(fileContent);
                                    }
                                }
                            if (request.Req_Files != null)
                                foreach (var i in request.Req_Files)
                                {
                                    var file = i;
                                    byte[] Bytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        file.CopyTo(ms);
                                        Bytes = ms.ToArray();
                                    }
                                    var fileContent = new ByteArrayContent(Bytes);
                                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                                    content.Add(fileContent);
                                }

                            //Order Columns Answares

                            //foreach (var i in request.Req_ApplicantData.PD_EFormAnswer.Where(q => q.EFAns_TableCol != null))
                            //{
                            //    i.EFAns_TableCol = i.EFAns_TableCol.OrderBy(q => q.TC_ID).ToList();
                            //}


                            var requestUri = _configuration["AdminPanel_BE_URL"] + $"api/Request/SaveApplicantDataMobilePhones?ReqID={request_Data.RequestDataId}&SubService={request_Data.SubServicesId}";
                            var result = await client.PostAsync(requestUri, content);
                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var d = result.Content.ReadAsStringAsync();
                                var lst = JsonConvert.DeserializeObject<ResponseClassServer>(d.Result);
                                if (lst.success)
                                {
                                    //Response.Cookies.Set(new HttpCookie("u") { Expires = DateTime.Now.AddYears(-30), Value = "" });
                                    var data = JsonConvert.DeserializeObject<List<RequestFile>>(JsonConvert.SerializeObject(lst.result));

                                    _appContext.RequestFiles.AddRange(data);
                                    await _appContext.SaveChangesAsync();

                                    trans.Commit();

                                    requestUri = _configuration["AdminPanel_BE_URL"] + "api/Request/NotifyRequest?ReqID=" + request_Data.RequestDataId;
                                    result = await client.PostAsync(requestUri, content);

                                    return Ok(new ResponseClass() { Success = true, data = "" });
                                }
                                else
                                {
                                    trans.Rollback();
                                    return Ok(new ResponseClass() { Success = false, data = JsonConvert.SerializeObject(lst.result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
                                }
                            }
                            else
                            {
                                trans.Rollback();
                                return Ok(new ResponseClass() { Success = false, data = "Backend+" + JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
                            }
                        }

                        //return Ok(new ResponseClass() { Success = true, data = "Ok" });
                    }
                }
                else
                {
                    return Ok(new ResponseClass() { Success = false, data = "CodeError" });
                }
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { Success = false, data = JsonConvert.SerializeObject(ee, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
            }

        }

    }
}
