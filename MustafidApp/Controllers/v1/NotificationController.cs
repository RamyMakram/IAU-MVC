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
            var data = await _appContext.PhoneNumberNotifications.Where(q => q.PhoneNumber == Mobile_Phone).Skip(P_Size * P_Index).Take(P_Size).ToListAsync();

            var data_DTO = _mapper.Map<List<NotificationsDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data });
        }
    }
}
