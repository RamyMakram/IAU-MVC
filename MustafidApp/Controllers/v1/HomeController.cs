using Microsoft.AspNetCore.Mvc;

namespace MustafidApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "data from api v1";
        }
    }
}
