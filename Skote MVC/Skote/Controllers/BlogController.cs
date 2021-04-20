using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skote.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Grid()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }
    }
}