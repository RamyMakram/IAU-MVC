using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class GlobalSettingsController : BaseController
    {
        // GET: GlobalSettings
        public ActionResult Home()
        {
            var Data = APIHandeling.getData("GeneralSetting/Init");
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            var ob_data = JObject.Parse(res.result.ToString());
            var Status = JsonConvert.DeserializeObject<List<RequestStatusDTO>>(ob_data.Value<JArray>("Request_State").ToString());
           
            ViewBag.SMS_Status = ob_data["Use_SMS"].Value<bool>();

            ViewBag.NewAndFlollowRequestLogin = ob_data["NewAndFlollowRequestLogin"].ToObject<string[]>();

            ViewBag.NewAndFlollowRequestLogin_Current = ob_data["NewAndFlollowRequestLogin_Current"].Value<string>();
            
            return View(Status);
        }

        [HttpPost]
        public ActionResult Home(bool? value)
        {
            if (value == null)
                return RedirectToAction("Home");
            var Data = APIHandeling.Post($"GeneralSetting/UpdateSMS?value={value}", new { });
            var resJson = Data.Content.ReadAsStringAsync();
            return RedirectToAction("Home");
        }

        [HttpPost]
        public ActionResult UpdateLoginWay(string MustafeedLoginWay)
        {
            if (MustafeedLoginWay == null)
                return RedirectToAction("Home");
            var Data = APIHandeling.Post($"GeneralSetting/UpdateNewAndFlollowRequestLogin?value={MustafeedLoginWay}", new { });
            var resJson = Data.Content.ReadAsStringAsync();
            return RedirectToAction("Home");
        }

        [HttpPost]
        public JsonResult SaveData(string data)
        {
            var Listdata = JsonConvert.DeserializeObject<ICollection<RequestStatusDTO>>(data);
            var Data = APIHandeling.Post("GeneralSetting/SaveDelayeValue", Listdata);
            var resJson = Data.Content.ReadAsStringAsync();
            return Json("");
        }
    }
}