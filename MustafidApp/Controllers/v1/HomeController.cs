using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MustafidAppDTO.DTO;
using MustafidAppModels.Context;
using MustafidAppModels.Models;
using System.Collections.Generic;
using System.Linq;

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
        public object GetServiceType()
        {
            var data = _mapper.Map<List<ServiceTypeDTO>>(_appContext.ServiceTypes.Where(q => !q.Deleted && q.IsAction.Value).ToList());
            return data;
        }
    }
}
