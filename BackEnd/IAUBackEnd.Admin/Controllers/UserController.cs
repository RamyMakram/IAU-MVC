//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IAUBackEnd.Admin.Models;
using System.Data.Entity;
using IAUAdmin.DTO.Helper;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Data.Entity.Core.Objects;

namespace IAUBackEnd.Admin.Controllers
{
    public class UserController : ApiController
    {
        private Admin.Models.MostafidDBEntities db = new MostafidDBEntities();
        [HttpGet]
        public async Task<IHttpActionResult> Login(string email, string pass)
        {
            try
            {
                var data = db.Users.FirstOrDefault(q => q.IS_Active == "1" && q.User_Email == email && q.User_Password == pass && !q.Deleted);
                if (data == null)
                    return Ok(new ResponseClass
                    {
                        success = false,
                    });
                var date = Helper.GetDate();
                var Datetime = Helper.GetHashString("" + data.User_ID + data.User_Name + date.ToString());
                data.TEMP_Login = Datetime;
                data.LoginDate = date.AddDays(1);
                db.SaveChanges();
                WebSocketManager.SendLogout(data.User_ID.ToString());
                return Ok(new ResponseClass
                {
                    success = true,
                    result = new { Token = Datetime }
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

        [HttpGet]
        public async Task<IHttpActionResult> VerfiyToken(string token)
        {
            try
            {
                var date = Helper.GetDate().AddDays(1);
                var data = db.Users.Include(q => q.Units).FirstOrDefault(q => q.IS_Active == "1" && q.TEMP_Login == token && q.LoginDate <= date && !q.Deleted);
                if (data == null)
                    return Ok(new ResponseClass
                    {
                        success = false,
                    });
                return Ok(new ResponseClass
                {
                    success = true,
                    result = new { data.User_ID, data.UnitID, EN_Top = "Hello " + data.Units.Units_Name_EN + " ،" + data.User_Name.Split('|')[0], AR_Top = "مرحبا " + data.Units.Units_Name_AR + " ،" + data.User_Name.Split('|')[1] }
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

        [HttpGet]
        public async Task<IHttpActionResult> VerfiyUser(int id, string token)
        {
            try
            {
                var date = Helper.GetDate().AddDays(1);
                var data = db.Users.Include(q => q.Job.Job_Permissions.Select(s => s.Privilage)).Include(Q => Q.Units).FirstOrDefault(q => q.User_ID == id && q.IS_Active == "1" && q.TEMP_Login == token && q.LoginDate <= date && !q.Deleted);
                if (data == null)
                    return Ok(new ResponseClass
                    {
                        success = false
                    });
                else
                {
                    var perm = data.Job.Job_Permissions.Where(q => !q.Deleted).Select(q => q.Privilage.Name_EN).ToArray();
                    return Ok(new ResponseClass
                    {
                        success = true,
                        result = new { perm, data.Units.IS_Mostafid, data.UnitID }
                    });
                }
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

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var data = db.Users.Where(qt => !qt.Deleted)
                    .Include(q => q.Job)
                    .Include(q => q.Units)
                    .Select(q => new
                    {
                        q.User_ID,
                        q.User_Name,
                        q.User_Mobile,
                        q.User_Password,
                        q.User_Email,
                        q.IS_Active,
                        q.Job_ID,
                        q.Job.User_Permissions_Type_Name_AR,
                        q.Job.User_Permissions_Type_Name_EN,
                        Unit = q.Units
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
        [HttpGet]
        public async Task<IHttpActionResult> GetDeleted()
        {
            try
            {
                var data = db.Users.Where(qt => qt.Deleted)
                    .Include(q => q.Job)
                    .Include(q => q.Units)
                    .Select(q => new
                    {
                        q.User_ID,
                        q.User_Name,
                        q.User_Mobile,
                        q.User_Password,
                        q.User_Email,
                        q.IS_Active,
                        q.Job_ID,
                        q.Job.User_Permissions_Type_Name_AR,
                        q.Job.User_Permissions_Type_Name_EN,
                        Unit = q.Units,
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

        [HttpGet]
        public async Task<IHttpActionResult> GetAllByUnit(int UID)
        {
            try
            {
                var data = db.Users.Where(q => q.UnitID == UID && !q.Deleted)
                    .Include(q => q.Job)
                    .Select(q => new
                    {
                        q.User_ID,
                        q.User_Name,
                        q.User_Email,
                        q.Job
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

        [HttpGet]
        public async Task<IHttpActionResult> GetDetails(int uid)
        {
            try
            {
                var data = db.Users
                    .Where(qt => qt.User_ID == uid && !qt.Deleted)
                    .Select(q => new
                    {
                        q.User_ID,
                        q.User_Name,
                        q.User_Email,
                        q.User_Mobile,
                        q.User_Password,
                        q.IS_Active,
                        q.Job_ID,
                        q.Job.User_Permissions_Type_Name_AR,
                        q.Job.User_Permissions_Type_Name_EN,
                        q.UnitID
                    })
                    .FirstOrDefault();
                if (data == null)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Null"
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

        [HttpPost]
        public async Task<IHttpActionResult> UpdateData([FromBody] Users users)
        {
            var trans = db.Database.BeginTransaction();

            try
            {
                if (db.Job.Find(users.Job_ID).Deleted)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Deleted Job"
                    });
                if (db.Units.Find(users.UnitID).Deleted)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Del U"
                    });
                var data = db.Users.FirstOrDefault(qt => qt.User_ID == users.User_ID && !qt.Deleted);
                if (data == null)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Null Ref"
                    });
                var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                data.User_Mobile = users.User_Mobile;
                data.User_Name = users.User_Name;
                data.User_Password = users.User_Password;
                data.Job_ID = users.Job_ID;
                data.UnitID = users.UnitID;

                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.User, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.User_ID, notes: null);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true,
                        result = data
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

        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int uid)
        {
            var trans = db.Database.BeginTransaction();

            try
            {
                var data = db.Users.FirstOrDefault(qt => qt.User_ID == uid && !qt.Deleted);
                if (data == null)
                    return Ok(new ResponseClass
                    {
                        success = false
                    });
                var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                data.IS_Active = "0";
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.User, Method: "Deactive", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.User_ID, notes: null);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true,
                        result = data
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

        [HttpGet]
        public async Task<IHttpActionResult> Active(int uid)
        {
            var trans = db.Database.BeginTransaction();

            try
            {
                var data = db.Users.FirstOrDefault(qt => qt.User_ID == uid && !qt.Deleted);
                if (data == null)
                    return Ok(new ResponseClass
                    {
                        success = false
                    });
                var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                data.IS_Active = "1";

                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.User, Method: "Active", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.User_ID, notes: null);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true,
                        result = data
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
        public async Task<IHttpActionResult> Create([FromBody] Users users)
        {
            var trans = db.Database.BeginTransaction();

            try
            {
                if (db.Job.Find(users.Job_ID).Deleted)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Deleted Job"
                    });
                if (db.Units.Find(users.UnitID).Deleted)
                    return Ok(new ResponseClass
                    {
                        success = false,
                        result = "Del U"
                    });

                users.Deleted = false;
                var data = db.Users.Add(users);
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.User, Method: "Create", Oldval: null, Newval: data, es: out _, syslog: out _, ID: data.User_ID, notes: null);
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
        public async Task<IHttpActionResult> _Delete(int id)
        {
            var user = db.Users.FirstOrDefault(q => q.User_ID == id && !q.Deleted);
            if (user == null)
                return Ok(new ResponseClass() { success = false, result = "User Is Null" });

            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(user, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            user.Deleted = true;
            user.DeletedAt = DateTime.Now;

            //p.Users.Remove(user);


            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.User, Method: "Delete", Oldval: OldVals, Newval: user, es: out _, syslog: out _, ID: user.User_ID, notes: null);
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
        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            var user = db.Users.FirstOrDefault(q => q.User_ID == id && q.Deleted);
            if (user == null)
                return Ok(new ResponseClass() { success = false, result = "User Is Null" });
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(user, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            user.Deleted = false;

            //p.Users.Remove(user);


            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.User, Method: "Create", Oldval: null, Newval: user, es: out _, syslog: out _, ID: user.User_ID, notes: null);
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
    }
}
