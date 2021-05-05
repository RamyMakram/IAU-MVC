using IAUBackEnd.Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace IAUBackEnd.Admin.Controllers
{
    public class SocketController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Send(string name)
		{
            MostafidDBEntities p = new MostafidDBEntities();
            Request_Data data = p.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == 33);

            var MostafidUsers = p.Users.Where(q => q.Units.IS_Mostafid).Select(q => q.User_ID).ToArray();
            string message = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            WebSocketManager.SendToMulti(MostafidUsers, message);
            return Ok();
        }
    }
}
