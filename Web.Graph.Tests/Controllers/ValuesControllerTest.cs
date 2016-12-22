using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Graph.Controllers;

namespace Web.Graph.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        //[TestMethod]
        //public void Get()
        //{
        //    // Disponer
        //    ValuesController controller = new ValuesController();

        //    // Actuar
        //    IEnumerable<string> result = controller.Get();

        //    // Declarar
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(2, result.Count());
        //    Assert.AreEqual("value1", result.ElementAt(0));
        //    Assert.AreEqual("value2", result.ElementAt(1));
        //}

        [TestMethod]
        public void GetById()
        {
            // Disponer
            RucController controller = new RucController();

            // Actuar
            var result = controller.Get("");

            // Declarar
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Disponer
            RucController controller = new RucController();

            // Actuar
            controller.Post("value");

            // Declarar
        }
    }
}
