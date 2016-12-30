using GraphQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Graph.Controllers;

namespace Web.Graph.Tests.Controllers
{
    [TestClass]
    public class DniControllerTests
    {
        [TestMethod]
        public void GetTest()
        {
            // Disponer
            var controller = new DniController();

            // Actuar
            var result = controller.Get(
            @"query {
	            persona (dni: ""42123390""){
		            primer_nombre
		            segundo_nombre
		            apellido_paterno
		            apellido_materno
	            }
            }") as ExecutionResult;

            // Declarar
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void PostTest()
        {
            // Disponer
            var controller = new DniController();

            // Actuar
            var result = controller.Post(
            @"query {
	            persona (dni: ""06477277""){
		            primer_nombre
		            segundo_nombre
		            apellido_paterno
		            apellido_materno
	            }
            }") as ExecutionResult;

            // Declarar
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
        }
    }
}