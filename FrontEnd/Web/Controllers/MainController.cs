using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            ViewBag.CookieLang = Request.Cookies["lang"].Value;
            var db = new TasahelEntities();

            var domain = Request.Url.Authority;

            var AboutData = db.SubDomains.Where(q => q.Domain == domain && q.Domain1.Enabled).Select(q => q.Domain1.About.ToList()).FirstOrDefault();
            ViewBag.AboutData = AboutData;

            var StyleData = db.SubDomains.Where(q => q.Domain == domain && q.Domain1.Enabled).Select(q => q.Domain1.DomainStyle).FirstOrDefault();
            TempData["StyleData"] = StyleData;

            return View();
        }
    }
}