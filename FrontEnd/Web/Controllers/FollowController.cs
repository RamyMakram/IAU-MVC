using IAU.DTO.Entity;
using System.Net.Http;
using System.Web.Mvc;
using Web.App_Start;

namespace Web.Controllers
{
    public class FollowController : BaseController
    {
        // GET: Follow
        public ActionResult Index()
        {

            ViewBag.CookieLang = Request.Cookies["lang"].Value;
            return View(new object());
        }
        [HttpPost]
        public JsonResult FollowRequest(string requestCode)
        {
            var res = APIHandeling.getData("/Request/FollowRequest?requestCode=" + requestCode);
            var lst = res.Content.ReadAsAsync<object>().Result;
            return Json(new { Result = "OK", Options = lst }, JsonRequestBehavior.AllowGet);
        }
    }
}