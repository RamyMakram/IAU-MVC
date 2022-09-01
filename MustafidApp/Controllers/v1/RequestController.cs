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
        public RequestController(MustafidAppContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
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
    }
}
