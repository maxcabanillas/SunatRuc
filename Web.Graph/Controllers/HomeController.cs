using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Graph.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Pagina Principal";

            return View();
        }

        /// <summary>
        /// Example action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Example()
        {
            ViewBag.Title = "Ejemplos";
            return View();
        }

        /// <summary>
        /// Demo action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Demo()
        {
            ViewBag.Title = "Demo";
            return View();
        }
    }
}
