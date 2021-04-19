using IAUAdmin.DTO.Helper;
using System.Web.Mvc;
using System.Web.Security;

namespace AdminPanel.Controllers
{
	public class LoginController : Controller
	{
		// GET: Login
		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Login(CustomUserLogin user = null)
		{

			//bool isFoundOnActive = false;
			//#region Old Code

			//int IsVaild = User_Session.LogIn(user);

			//switch (IsVaild)
			//{
			//    case (int)GlobalEnum.User_Login.Valid:
			//        {
			//            isFoundOnActive = true;
			//            break;
			//        }
			//    case (int)GlobalEnum.User_Login.InvalidCredential:
			//        {
			//            ViewBag.msg = GlobalEnum.GetEnumDescription<GlobalEnum.Error>(GlobalEnum.User_Login.InvalidCredential);
			//            break;
			//        }

			//    case (int)GlobalEnum.User_Login.LoginFromAnotherDevice:
			//        {
			//            ViewBag.msg = GlobalEnum.GetEnumDescription<GlobalEnum.Error>(GlobalEnum.User_Login.LoginFromAnotherDevice);
			//            break;
			//        }
			//}
			//#endregion
			//return (isFoundOnActive == true) ? View("Index") : View();
			return RedirectToAction("Index", "Home", new { area = "AdminData" });

		}

		public ActionResult LogOut()
		{
			User_Session current = User_Session.GetInstance;
			current.LogOut();
			// Session.Abandon();
			FormsAuthentication.SignOut();
			return RedirectToAction("Login", "Home", new { area = "" });
		}
	}
}