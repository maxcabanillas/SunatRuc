using GraphQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Graph.Controllers;

namespace Web.Graph.Tests.Controllers
{
    [TestClass]
    public class RucControllerTest
    {

        [TestMethod]
        public void GetById()
        {
            // Disponer
            var controller = new RucController();

            // Actuar
            var result = controller.Get(
            @"query {
	            empresa (ruc: ""10480048356""){
		            ruc,
		            nombre,
		            profesion
	            }
            }") as ExecutionResult;

            // Declarar
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void Post()
        {
            // Disponer
            RucController controller = new RucController();

            // Actuar
            var result = controller.Post(
            @"query {
	            empresa (ruc: ""10480048356""){
		            ruc,
		            nombre
	            }
            }") as ExecutionResult;

            // Declarar
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
        }
    }
}
