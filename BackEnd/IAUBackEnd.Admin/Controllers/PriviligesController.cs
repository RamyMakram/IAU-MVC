using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IAUBackEnd.Admin.Controllers
{
    public class PriviligesController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();
        public async Task<IHttpActionResult> GetAllAndSubPrivilges()
        {
            try
            {
                var Permissions = p.Privilage.Where(w => w.DetailedFrom == null).Select(d => new
                {
                    d.ID,
                    d.Name,
                    d.Name_EN,
                    d.SubOF,
                    Sub =
                    d.Privilage1.Select(w => new { w.ID, w.Name, w.Name_EN, w.SubOF }),
                    Details = d.Privilage11.Select(w => new { w.ID, w.Name, w.Name_EN, w.SubOF })
                });
                return Ok(new ResponseClass
                {
                    success = true,
                    result = new { Permissions }
                });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass
                {
                    success = false,
                    result = ee
                });
            }
        }
        public async Task<IHttpActionResult> GetSubPeivilges(int jid)
        {
            try
            {
                var data = p.Privilage.Where(q => q.SubOF == jid);
                return Ok(new ResponseClass
                {
                    success = true,
                    result = data
                });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass
                {
                    success = false,
                    result = ee
                });
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddPrivilgesToJob(IEnumerable<Job_Permissions> pri)
        {
            try
            {
                if (p.Job.Find(pri.First().Job_ID).Deleted)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Deleted Job"
                    });

                var data = p.Job_Permissions.AddRange(pri);
                if (p.SaveChanges() > 0)
                    return Ok(new ResponseClass
                    {
                        success = true,
                    });
                else
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Nosave"
                    });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass
                {
                    success = false,
                    result = ee
                });
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> DeletePrivilgesFromJob(IEnumerable<Job_Permissions> pri)
        {
            try
            {
                foreach (var i in pri)
                {
                    var data = p.Job_Permissions.FirstOrDefault(q => q.Job_ID == i.Job_ID && q.PrivilageID == i.PrivilageID);
                    data.Deleted = true;
                    data.DeletedAt = DateTime.Now;
                    //p.Job_Permissions.Remove(data);
                }
                if (p.SaveChanges() > 0)
                    return Ok(new ResponseClass
                    {
                        success = true,
                    });
                else
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Nosave"
                    });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass
                {
                    success = false,
                    result = ee
                });
            }
        }
    }
}
