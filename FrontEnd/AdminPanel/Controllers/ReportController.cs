﻿using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        public async Task<ActionResult> Home()
        {
            return View();
        }

        public async Task<ActionResult> _UnitsByLevel(int? levl)
        {
            if (levl.HasValue)
            {
                var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";
                var Data = APIHandeling.getData("UnitLevels/GetUnitLevelForUnit");
                var resJson = Data.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                var Levels = JsonConvert.DeserializeObject<ICollection<UnitLevelDTO>>(res.result.ToString());
                ViewBag.Levels = Levels.ToList().ConvertAll(q => { return new SelectListItem() { Value = q.ID.ToString(), Text = (isar ? q.Name_AR : q.Name_EN), Selected = false }; });
                ViewBag.SelectedLevel = Levels.First(q => q.ID == levl);

                Data = APIHandeling.getData("Units/GetUnitsByLevel?lvlid=" + levl);
                resJson = Data.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

                return View(JsonConvert.DeserializeObject<ICollection<UnitsDTO>>(res.result.ToString()));
            }
            else
            {
                var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";

                var Data = APIHandeling.getData("UnitLevels/GetUnitLevelForUnit");
                var resJson = Data.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                var Levels = JsonConvert.DeserializeObject<ICollection<UnitLevelDTO>>(res.result.ToString());
                ViewBag.Levels = Levels.ToList().ConvertAll(q => { return new SelectListItem() { Value = q.ID.ToString(), Text = (isar ? q.Name_AR : q.Name_EN), Selected = false }; });

                return View();
            }
        }
        
        public async Task<ActionResult> _UnitsByLocation(int? locid)
        {
            if (locid.HasValue)
            {
                var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";
                var Data = APIHandeling.getData("UnitsLocation/GetActive");
                var resJson = Data.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                var Locations = JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString());
                if (isar)
                    ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_AR");
                else
                    ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_EN");
                
                ViewBag.SelectedLevel = Locations.First(q => q.Units_Location_ID == locid);

                Data = APIHandeling.getData("Units/GetUnitsByLocation?locid=" + locid);
                resJson = Data.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

                return View(JsonConvert.DeserializeObject<ICollection<UnitsDTO>>(res.result.ToString()));
            }
            else
            {
                var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";

                var Data = APIHandeling.getData("UnitsLocation/GetActive");
                var resJson = Data.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                var Locations = JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString());
                if (isar)
                    ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_AR");
                else
                    ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_EN");


                return View();
            }
        }

        public async Task<ActionResult> _Request()
        {
            var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";
            var Data = APIHandeling.getData("Service_Type/GetActive");
            var resJson = await Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
            ViewBag.ServiceType = Services;

            Data = APIHandeling.getData("Request_Type/GetActive");
            resJson = await Data.Content.ReadAsStringAsync();
            res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            var ReqTypes = JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString());
            ViewBag.ReqTypes = ReqTypes;

            Data = APIHandeling.getData("Units/GetActive");
            resJson = await Data.Content.ReadAsStringAsync();
            res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            var Units = JsonConvert.DeserializeObject<ICollection<UnitsDTO>>(res.result.ToString());
            if (isar)
                ViewBag.Units = new SelectList(Units, "Units_ID", "Units_Name_AR");
            else
                ViewBag.Units = new SelectList(Units, "Units_ID", "Units_Name_EN");
            Data = APIHandeling.getData("UnitsLocation/GetActive");
            resJson = await Data.Content.ReadAsStringAsync();
            res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            var Locations = JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString());
            if (isar)
                ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_AR");
            else
                ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_EN");
            bool count = true;

            if (isar)
                ViewBag.Source = new List<string> { "مستفيد", "تواصل" }.ConvertAll(e => { count = !count; return new SelectListItem() { Text = e, Value = count.ToString() }; });
            else
                ViewBag.Source = new List<string> { "Mostafid", "Twasol" }.ConvertAll(e => { count = !count; return new SelectListItem() { Text = e, Value = count.ToString() }; });
            Data = APIHandeling.getData("Request_State/GetActive");
            resJson = await Data.Content.ReadAsStringAsync();
            res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            ViewBag.Status = JsonConvert.DeserializeObject<ICollection<RequestStatusDTO>>(res.result.ToString());
            return View();
        }
        public async Task<ActionResult> FilterRequest(int? ST, int? RT, int? MT, int? location, int? Unit, int? ReqStatus, bool? ReqSource, DateTime? DF, DateTime? DT, string Columns)
        {
            var Data = APIHandeling.Post($"Request/ReportRequests?RT={RT}&ST={ST}&MT={MT}&DF={DF}&DT={DT}&location={location}&Unit={Unit}&ReqStatus={ReqStatus}&ReqSource={ReqSource}&Columns={Columns}", "");
            var resJson = await Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            if (res.success)
                return View("Filter", JsonConvert.DeserializeObject<ICollection<RequestReportDTO>>(res.result.ToString()));
            else
                return RedirectToAction("Home");
        }
    }
}