using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext Context)
        {
            base.OnActionExecuting(Context);
            var db = new TasahelEntities();

            var domain = Request.Url.Authority;

            var StyleData = db.SubDomains.Where(q => q.Domain == domain && q.Domain1.Enabled).Select(q => q.Domain1.DomainStyle).FirstOrDefault();
            TempData["StyleData"] = StyleData;

            if (Request.Cookies["lang"] == null)
                Response.Cookies.Set(new HttpCookie("lang", "ar"));
        }
    }
}