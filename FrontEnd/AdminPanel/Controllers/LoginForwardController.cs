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
    public class LoginForwardController : Controller
    {
        // GET: LoginForward
        public ActionResult Login(string t)
        {
            if (t == null)
                return RedirectToAction("Home", "Home");
            var res = APIHandeling.getData("User/VerfiyToken?token=" + t);
            var Res = res.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<ResponseClass>(Res.Result);
            if (lst.success)
            {
                var data = JObject.Parse(lst.result.ToString());
                Response.Cookies.Add(new HttpCookie("en_top_name", HttpUtility.UrlEncode(data["EN_Top"].Value<string>())) { Expires = Helper.GetDate().AddDays(1) });
                Response.Cookies.Add(new HttpCookie("ar_top_name", HttpUtility.UrlEncode(data["AR_Top"].Value<string>())) { Expires = Helper.GetDate().AddDays(1) });
                Response.Cookies.Add(new HttpCookie("u", data["User_ID"].Value<string>()) { Expires = Helper.GetDate().AddDays(1) });
                Response.Cookies.Add(new HttpCookie("token", t) { Expires = Helper.GetDate().AddDays(1) });
                Response.Cookies.Add(new HttpCookie("lang", "ar"));
                return RedirectToAction("Home", "Home");
            }
            else
                return RedirectToAction("NotFound", "Error");
        }
    }
}