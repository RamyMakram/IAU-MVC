using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skote.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DashboardBlog()
        {
            return View();
        }

        public ActionResult DashboardCrypto()
        {
            return View();
        }

        public ActionResult DashboardSaas()
        {
            return View();
        }


    }
}