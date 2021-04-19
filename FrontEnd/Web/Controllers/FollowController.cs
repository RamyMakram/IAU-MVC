using IAU.DTO.Entity;
using System.Net.Http;
using System.Web.Mvc;
using Web.App_Start;

namespace Web.Controllers
{
    public class FollowController : Controller
    {
        // GET: Follow
        public ActionResult Index()
        {
            return View(new FollowRequest_DTO());
        }
        [HttpPost]
        public JsonResult FollowRequest(string requestCode)
        {
            var res = APIHandeling.getData("/Request/FollowRequest?requestCode=" + requestCode, Request.Headers.Get("lang") ?? "1");
            var lst = res.Content.ReadAsAsync<FollowRequest_DTO>().Result;
            return Json(new { Result = "OK", Options = lst }, JsonRequestBehavior.AllowGet);
        }
    }
}