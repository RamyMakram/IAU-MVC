using IAUAdmin.DTO.Helper;
using IAUAdmin.DTO.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class ProfileController : BaseController
    {
        public ActionResult Home()
        {
            var Data = APIHandeling.getData("Units/GetUnitSeginature?id=" + int.Parse(TempData.Peek("UnitID").ToString()));
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            var data = new Unit_Signature();
            if (res.result != null)
                data = JsonConvert.DeserializeObject<Unit_Signature>(res.result.ToString());
            Data = APIHandeling.getData("User/GetDetails?uid=" + int.Parse(Request.Cookies["u"].Value.ToString()));
            resJson = Data.Content.ReadAsStringAsync();
            res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            ViewBag.User = JsonConvert.DeserializeObject<UserDTO>(res.result.ToString());
            return View(data);
        }

        [HttpPost]
        public ActionResult Home(Unit_Signature signature)
        {
            if (signature != null && signature.Files.Count != 0)
            {
                signature.UnitID = int.Parse(TempData.Peek("UnitID").ToString());
                HttpPostedFileBase file = signature.Files[0];
                byte[] Bytes = new byte[file.InputStream.Length + 1];
                file.InputStream.Read(Bytes, 0, Bytes.Length);
                signature.Base64 = Convert.ToBase64String(Bytes);
                signature.Files = null;
                var Data = APIHandeling.Post("Units/SaveUnitSeginature", signature);
                var resJson = Data.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                if (res.success)
                    return RedirectToAction("Home", "Home");

                return View(signature);
            }
            return View(signature);
        }
    }
}