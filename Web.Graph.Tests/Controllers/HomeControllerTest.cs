using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Graph;
using Web.Graph.Controllers;

namespace Web.Graph.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Disponer
            HomeController controller = new HomeController();

            // Actuar
            ViewResult result = controller.Index() as ViewResult;

            // Declarar
            Assert.IsNotNull(result);
            Assert.AreEqual("Pagina Principal", result.ViewBag.Title);
        }
    }
}
