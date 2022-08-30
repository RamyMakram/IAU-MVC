using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MustafidApp.Helpers;
using MustafidAppDTO.DTO;
using MustafidAppModels.Context;
using MustafidAppModels.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MustafidApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private MustafidAppContext _appContext;
        private IMapper _mapper;
        public HomeController(MustafidAppContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns All Valid Service Type
        /// </summary>
        /// <returns>
        /// List of Service Type Object
        /// </returns>

        [HttpGet]
        public async Task<IActionResult> GetServiceType()
        {
            var data = await _appContext.ValidTos.Where(q =>
                !q.Deleted &&
                q.MainService.IsAction.Value &&
                q.MainService.ServiceType.IsAction.Value &&
                !q.MainService.ServiceType.Deleted &&
                q.ApplicantType.IsAction.Value &&
                q.MainService.UnitMainServices.Count(s =>
                    s.Unit.IsAction.Value &&
                    !s.Unit.Deleted &&
                    !s.MainService.Deleted &&
                    s.MainService.IsAction.Value &&
                    s.MainService.SubServices.Count(t =>
                        t.IsAction.Value &&
                        !t.Deleted
                        ) != 0
                    ) != 0
            ).Select(q => q.MainService.ServiceType).Distinct().ToListAsync();

            var data_DTO = _mapper.Map<List<ServiceTypeDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }


        /// <summary>
        /// Returns All Valid Request Type base on given Service Type ID
        /// </summary>
        /// <param name="S_ID">S_ID is Service Type ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRequestType(int S_ID)
        {
            var data = await _appContext.RequestTypes.Where(q =>
                !q.Deleted &&
                q.IsAction.Value &&
                q.UnitsRequestTypes.Count(w =>
                    !w.Units.Deleted &&
                    (//check if unit service type "main and sub" contain service type
                        w.Units.ServiceTypeId == S_ID ||
                w.Units.UnitServiceTypes.Any(s =>
                            s.ServiceTypeId == S_ID
                        )
                    )
                    && w.Units.UnitMainServices.Count(r =>//check unit main service 
                        !r.MainService.Deleted &&
                        r.MainService.IsAction.Value &&
                        r.MainService.SubServices.Count(g =>
                            g.IsAction.Value &&
                            !g.Deleted
                            ) != 0
                        ) != 0

                ) != 0
            ).ToListAsync();

            var data_DTO = _mapper.Map<List<ServiceTypeDTO>>(data);
            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }
    }
}
