using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using System.Data.Entity;

namespace IAUBackEnd.Admin.Controllers
{
    public class DelayedRequestController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();
        public async Task<IHttpActionResult> GetActive(int uid, bool ar)
        {
            var unit = await p.Users.Include(q => q.Units).FirstOrDefaultAsync(q => q.User_ID == uid);
            if (unit == null)
                return Ok(new ResponseClass() { success = false });

            var date = Helper.GetDate();
            var query = p.DelayedTransaction.Where(q => EntityFunctions.DiffMonths(date, q.AddedDate) == 0 && q.DelayedOnUnitID != null);
            var isMos = unit.Units.IS_Mostafid;
            if (!isMos)
                query = query.Where(q => q.DelayedOnUnitID == unit.UnitID);/*Get My Delayed Transcations*/

            var data = query.OrderByDescending(q => q.AddedDate).Select(q => new { ReqID = q.RequestID, RequestStatusAR = q.Request_State.StateName_AR, RequestStatusEN = q.Request_State.StateName_EN, q.Readed, q.ID, UName = (isMos ? (ar ? q.Units.Units_Name_AR : q.Units.Units_Name_EN) : null) });
            return Ok(new ResponseClass() { success = true, result = data });
        }
        public async Task<IHttpActionResult> GetTranscation(int id)
        {
            var data = p.DelayedTransaction.Include(q => q.Request_Data.Request_State).Include(q => q.Request_Data.RequestTransaction).Include(q => q.Request_Data.Request_File).Include(q => q.Request_Data.Personel_Data.Country).Include(q => q.Request_Data.Personel_Data.ID_Document1).Include(q => q.Request_Data.Personel_Data.Country1).Include(q => q.Request_Data.Personel_Data.City).Include(q => q.Request_Data.Personel_Data.Region).Include(q => q.Request_Data.Personel_Data.Country2).Include(q => q.Request_Data.Personel_Data.Applicant_Type).Include(q => q.Request_Data.Personel_Data).Include(q => q.Request_Data.Service_Type).Include(q => q.Request_Data.Units).Include(q => q.Request_Data.Request_Type).Include(q => q.Request_Data.Request_File.Select(w => w.Required_Documents)).FirstOrDefault(q => q.ID == id);

            if (!data.Readed)
            {
                data.Readed = true;
                await p.SaveChangesAsync();
            }
            return Ok(new ResponseClass() { success = true, result = data });
        }
    }
}
