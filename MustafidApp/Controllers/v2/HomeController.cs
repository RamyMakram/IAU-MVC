using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MustafidApp.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "data from api v2";
        }
    }
}
