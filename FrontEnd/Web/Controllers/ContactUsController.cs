using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ContactUsController : BaseController
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            var db = new TasahelEntities();

            var domain = Request.Url.Authority;

            var AboutData = db.SubDomains.Where(q => q.Domain == domain && q.Domain1.Enabled).Select(q => q.Domain1.DomainInfo).FirstOrDefault();
            ViewBag.AboutData = AboutData;

            return View();
        }
    }
}