using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IAUBackEnd.Admin.Controllers
{
    public class SocketController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Send(string name)
		{
            WebSocketManager.SendTo(name,"Hello From Server");
            return Ok();
		}
    }
}
