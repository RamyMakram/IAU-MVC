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
using System.Data.Entity;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
    public class PriviligesController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();
        public async Task<IHttpActionResult> GetAllAndSubPrivilges()
        {
            try
            {
                var Permissions = db.Privilage.Where(w => w.DetailedFrom == null).Select(d => new
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
                var data = db.Privilage.Where(q => q.SubOF == jid);
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
            var trans = db.Database.BeginTransaction();
            try
            {
                var jobid = pri.First().Job_ID;
                if (!pri.All(q => q.Job_ID == jobid))//If there is invalid job id
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Invalid Data"
                    });
                var Job = db.Job.Include(q => q.Job_Permissions).FirstOrDefault(q => q.User_Permissions_Type_ID == jobid);
                var OldVals = JsonConvert.SerializeObject(Job, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                if (Job.Deleted)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Deleted Job"
                    });


                var ExistPermIDs = Job.Job_Permissions.Where(q => !q.Deleted).Select(s => s.PrivilageID).ToArray();//Get Exist Permisions ID

                var AddedPermisions = pri.Where(q => !ExistPermIDs.Contains(q.PrivilageID)).ToList();//Get Added Permisions Object

                foreach (var i in AddedPermisions)
                {
                    var privilge = Job.Job_Permissions.FirstOrDefault(s => s.PrivilageID == i.PrivilageID);
                    if (privilge != null)
                        privilge.Deleted = false;
                    else
                        Job.Job_Permissions.Add(i);
                }


                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Job, Method: "Update", Oldval: OldVals, Newval: db.Job.Include(q => q.Job_Permissions).FirstOrDefault(q => q.User_Permissions_Type_ID == jobid), es: out _, syslog: out _, ID: pri.First().Job_ID, notes: "Add Privilges To Job");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
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
            var trans = db.Database.BeginTransaction();

            try
            {
                var jobid = pri.First().Job_ID;
                if (!pri.All(q => q.Job_ID == jobid))//If there is invalid job id
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Invalid Data"
                    });
                var Job = db.Job.Include(q => q.Job_Permissions).FirstOrDefault(q => q.User_Permissions_Type_ID == jobid);
                var OldVals = JsonConvert.SerializeObject(Job, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                if (Job.Deleted)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Deleted Job"
                    });
                var DeletedPermIDs = pri.Select(s => s.PrivilageID).ToArray();//Get Deletd Permisions ID
                var DeletdPerm = Job.Job_Permissions.Where(q => DeletedPermIDs.Contains(q.PrivilageID));

                foreach (var data in DeletdPerm)
                {
                    data.Deleted = true;
                    data.DeletedAt = DateTime.Now;
                    //p.Job_Permissions.Remove(data);
                }
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Job, Method: "Update", Oldval: OldVals, Newval: Job, es: out _, syslog: out _, ID: pri.First().Job_ID, notes: "Remove Privilges From Job");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
                return Ok(new ResponseClass
                {
                    success = false,
                    result = ee
                });
            }
        }
    }
}
