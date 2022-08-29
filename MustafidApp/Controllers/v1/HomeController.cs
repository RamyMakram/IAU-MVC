using Microsoft.AspNetCore.Mvc;
using MustafidAppModels.Context;
using MustafidAppModels.Models;
using System.Linq;

namespace MustafidApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private MustafidAppContext _appContext;
        public HomeController(MustafidAppContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        public string Get()
        {
            return _appContext.EForms.FirstOrDefault().NameEn;
        }
    }
}
