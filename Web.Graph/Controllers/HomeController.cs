using System.Web.Mvc;
using System.Web.UI;

namespace Web.Graph.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            ViewBag.Title = "Pagina Principal";

            return View();
        }

        /// <summary>
        /// Example action.
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client)]
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
