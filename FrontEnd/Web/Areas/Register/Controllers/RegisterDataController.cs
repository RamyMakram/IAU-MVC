using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using Web.App_Start;
using Newtonsoft.Json;
using IAU.DTO.Helper;
using IAU.DTO.Entity;

namespace Web.Areas.Register.Controllers
{
    public class RegisterDataController : Controller
    {
        // GET: Register/RegisterData
        //  string apiName = "MainService";
        public ActionResult Index()
        {
            var res = APIHandeling.getData("Main_Service/GetMainServices");
            ResponseClass lst = res.Content.ReadAsAsync<ResponseClass>().Result;
            var main_Service_DTO = JsonConvert.DeserializeObject<List<SelectList_DTO>>(lst.result.ToString());
            return View(main_Service_DTO);
        }
    }
}