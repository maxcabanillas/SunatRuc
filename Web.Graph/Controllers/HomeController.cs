using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Graph.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Pagina Principal";

            return View();
        }

        public ActionResult Example()
        {
            ViewBag.Title = "Ejemplo";
            return View();
        }

        public ActionResult Demo()
        {
            ViewBag.Title = "Demo";
            return View();
        }
    }
}
