using IAUBackEnd.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using IAUAdmin.DTO.Helper;
using System.Web.Http.Cors;

namespace IAUBackEnd.Admin.Controllers
{
    public class JobController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();
        public async Task<IHttpActionResult> GetDeleted()
        {
            try
            {
                var data = p.Job.Where(q => q.Deleted)
                    .Select(q => new
                    {
                        q.User_Permissions_Type_ID,
                        q.User_Permissions_Type_Name_AR,
                        q.User_Permissions_Type_Name_EN,
                        q.DeletedAt
                    });
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
        public async Task<IHttpActionResult> GetAllJobs()
        {
            try
            {
                var data = p.Job.Where(q => !q.Deleted)
                    .Select(q => new
                    {
                        q.User_Permissions_Type_ID,
                        q.User_Permissions_Type_Name_AR,
                        q.User_Permissions_Type_Name_EN,
                        q.Job_Permissions,
                        q.IsModear
                    });
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
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public async Task<IHttpActionResult> GetJob(int jid)
        {
            try
            {
                var Job = p.Job
                    .Where(q => q.User_Permissions_Type_ID == jid && !q.Deleted)
                    .Select(q => new
                    {
                        q.User_Permissions_Type_ID,
                        q.User_Permissions_Type_Name_AR,
                        q.User_Permissions_Type_Name_EN,
                        q.IsModear
                    })
                    .FirstOrDefault();
                if (Job == null)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Invaild Data Entred"
                    });
                var Permissions = (from c in p.Privilage
                                   join o in p.Job_Permissions.Where(q => q.Job_ID == jid && !q.Deleted)
                                      on c.ID equals o.PrivilageID into sr
                                   from x in sr.DefaultIfEmpty()
                                   select new
                                   {
                                       c.ID,
                                       c.Name,
                                       c.Name_EN,
                                       c.CreatedOn,
                                       c.SubOF,
                                       c.DetailedFrom,
                                       Active = x == null ? false : true
                                   });
                var perm = Permissions.Where(w => w.DetailedFrom == null).Select(d => new
                {
                    d.ID,
                    d.Name,
                    d.Name_EN,
                    d.SubOF,
                    d.Active,
                    Sub =
                    Permissions.Where(w => w.SubOF == d.ID).Select(w => new { w.ID, w.Name, w.Name_EN, w.SubOF, w.Active }),
                    Details = Permissions.Where(r => r.DetailedFrom == d.ID)
                });
                return Ok(new ResponseClass
                {
                    success = true,
                    result = new { Job.User_Permissions_Type_Name_AR, Job.User_Permissions_Type_ID, Job.User_Permissions_Type_Name_EN, Job.IsModear, Permissions = perm }
                    //result = ss
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
        public async Task<IHttpActionResult> EditJob([FromBody] Job job)
        {
            try
            {
                var data = p.Job.FirstOrDefault(q => q.User_Permissions_Type_ID == job.User_Permissions_Type_ID);
                if (job.IsModear && ValidateIsModearExist(data.User_Permissions_Type_ID))/*IF Job IS Modear And There is modear*/
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Modear Error"
                    });
                data.User_Permissions_Type_Name_AR = job.User_Permissions_Type_Name_AR;
                data.User_Permissions_Type_Name_EN = job.User_Permissions_Type_Name_EN;
                data.IsModear = job.IsModear;
                if (p.SaveChanges() > 0)
                    return Ok(new ResponseClass
                    {
                        success = true,
                        result = data
                    });
                else
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Error"
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
        /// <summary>
        /// Check if There is job with Modear flag
        /// if Send ID Check if There is another job with Modear flag
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns>
        /// true: if Theris
        /// false: if not
        /// </returns>
        private bool ValidateIsModearExist(int? JobID)
        {
            return p.Job.Any(s => s.IsModear && (JobID.HasValue ? s.User_Permissions_Type_ID != JobID : true)/*Check IF There is another Job with Modear Flag*/);
        }
        [HttpGet]
        public async Task<IHttpActionResult> CheckModear(int? JID)
        {
            try
            {
                return Ok(new ResponseClass { success = true, result = ValidateIsModearExist(JID) });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass { success = false, result = ee });
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateJob([FromBody] Job user_Permissions_Type)
        {
            try
            {
                if (user_Permissions_Type.IsModear && ValidateIsModearExist(null))/*IF Job IS Modear And There is modear*/
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Modear Error"
                    });
                user_Permissions_Type.Deleted = false;
                var data = p.Job.Add(user_Permissions_Type);
                if (p.SaveChanges() > 0)
                    return Ok(new ResponseClass
                    {
                        success = true,
                        result = data
                    });
                else
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Error"
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
        public async Task<IHttpActionResult> _Delete(int id)
        {
            var job = p.Job.Include(q => q.Users).FirstOrDefault(q => q.User_Permissions_Type_ID == id && !q.Deleted);
            if (job == null)
                return Ok(new ResponseClass() { success = false, result = "Job Location Is Null" });

            var perm = p.Job_Permissions.Where(q => q.Job_ID == id);
            foreach (var i in perm)
            {
                i.Deleted = true;
                i.DeletedAt = DateTime.Now;
            }
            //p.Job_Permissions.RemoveRange(p.Job_Permissions.Where(q => q.Job_ID == id));

            job.Deleted = true;
            job.DeletedAt = DateTime.Now;

            //p.Job.Remove(job);
            await p.SaveChangesAsync();
            return Ok(new ResponseClass() { success = true });
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            var job = p.Job.Include(q => q.Users).FirstOrDefault(q => q.User_Permissions_Type_ID == id && q.Deleted);
            if (job == null)
                return Ok(new ResponseClass() { success = false, result = "Job Location Is Null" });
            if (job.Users.Count == 0)
            {
                var perm = p.Job_Permissions.Where(q => q.Job_ID == id);
                foreach (var i in perm)
                {
                    i.Deleted = false;
                }
                //p.Job_Permissions.RemoveRange(p.Job_Permissions.Where(q => q.Job_ID == id));

                job.Deleted = false;

                //p.Job.Remove(job);
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            return Ok(new ResponseClass() { success = false, result = "CantRemove" });
        }
    }
}
