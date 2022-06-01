﻿using IAUAdmin.DTO.Helper;
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
    public class StatisticsController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();
        public async Task<IHttpActionResult> Get()
        {
            var MostafidCount = p.Service_Type.Where(q => !q.Deleted).Select(q => new { AR = q.Service_Type_Name_AR, EN = q.Service_Type_Name_EN, IMG = q.Image_Path, Count = q.Request_Data.Count() }).ToList();
            var Stat = new { Units = p.Units.Count(q => !q.Deleted), Requests = p.Request_Data.Count(), MainServices = p.Main_Services.Count(q => !q.Deleted), SubServices = p.Sub_Services.Count(q => !q.Deleted) };
            var Locations = p.Units_Location.Select(q => new { AR = q.Units_Location_Name_AR, EN = q.Units_Location_Name_EN, Count = (int?)q.Units.Sum(w => w.Request_Data.Count) ?? 0 }).ToList();
            return Ok(new ResponseClass() { success = true, result = new { MostafidCount, Stat, Locations } });
        }
    }
}