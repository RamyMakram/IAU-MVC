using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext Context)
        {
            base.OnActionExecuting(Context);
            if (Request.Cookies["lang"] == null)
                Response.Cookies.Set(new HttpCookie("lang", "ar"));
            if (Request.Path == "/" || Request.Path.ToLower() == "/home")
            {
                if (string.IsNullOrEmpty(Request.QueryString["u"]) && string.IsNullOrEmpty(Request.QueryString["naf"]))
                {
                    HandelRedirect();
                }
                else if (Request.Path.ToLower().Contains("followup"))
                    HandelRedirect();
            }

            void HandelRedirect()
            {
                if (!(TempData.Peek("Authorized") as bool?) ?? true)
                    Context.Result = RedirectToAction("Login", "Home");
            }
        }
    }
}