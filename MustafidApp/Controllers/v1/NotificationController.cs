using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MustafidApp.Helpers;
using MustafidAppDTO.DTO;
using MustafidAppModels.Context;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MustafidApp.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private MustafidAppContext _appContext;
        private IMapper _mapper;
        public NotificationController(MustafidAppContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }
        /// <summary>
        /// Returns All Notification OF User
        /// </summary>
        /// <param name="P_Index">Page Index</param>
        /// <param name="P_Size">Page Size</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetNotifcations(int P_Index, int P_Size)
        {
            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;
            var data = await _appContext.PhoneNumberNotifications.Where(q => q.PhoneNumber == Mobile_Phone).Skip(P_Size * P_Index).Take(P_Size).OrderByDescending(q => q.NotiDate).ToListAsync();

            var data_DTO = _mapper.Map<List<NotificationsDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data });
        }
        /// <summary>
        /// Mark All As Readed
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ReadAll()
        {
            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;
            _appContext.Database.ExecuteSqlRaw(@$"Update PhoneNumberNotification set Readed=1 where PhoneNumber='{Mobile_Phone}'");
            //var data = await _appContext.PhoneNumberNotifications.Where(q => q.PhoneNumber == Mobile_Phone && !q.Readed).ToListAsync();

            //var data_DTO = _mapper.Map<List<NotificationsDTO>>(data);

            return Ok(new ResponseClass() { Success = true/*, data = data */});
        }
        /// <summary>
        /// Returns Count Notification OF User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetNotifcationsCount()
        {
            var Mobile_Phone = User.FindFirst(q => q.Type == ClaimTypes.MobilePhone).Value;
            var data = await _appContext.PhoneNumberNotifications.CountAsync(q => q.PhoneNumber == Mobile_Phone && !q.Readed);

            return Ok(new ResponseClass() { Success = true, data = data });
        }
    }
}
