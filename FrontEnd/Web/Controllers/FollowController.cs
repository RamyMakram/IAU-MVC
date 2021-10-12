using IAU.DTO.Entity;
using IAU.DTO.Helper;
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
		[ValidateAntiForgeryToken]
		public JsonResult FollowRequest(string requestCode)
		{
			var res = APIHandeling.Post("/Request/FollowRequest", requestCode);
			var lst = res.Content.ReadAsStringAsync().Result;
			return Json(lst, JsonRequestBehavior.AllowGet);
		}
	}
}