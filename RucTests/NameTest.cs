using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Ruc;

namespace RucTests
{
    [TestFixture]
    public class NameTest
    {
        [Test]
        public void GetTest()
        {
            var cs = new NombreConsult();
            var json = cs.Get("ENRIQUE SAMUEL CUCHO");
            Assert.IsNotEmpty(json);
            TestContext.WriteLine(json);

            var obj = JObject.Parse(json);
            StringAssert.Contains("success", (string)obj["status"]);
            var value = (JArray)obj["value"];
            foreach (var val in value)
            {
                TestContext.WriteLine("DNI : {0}, NAME: {1}", (string)val["dni"], (string)val["name"]);
            }
        }
        [Test]
        public void GetNotFoundTest()
        {
            var cs = new NombreConsult();
            var json = cs.Get("Gladys Katherine Torres Magallanes");
            Assert.IsNotEmpty(json);
            var obj = JObject.Parse(json);
            StringAssert.Contains("not_found", (string)obj["status"]);
            StringAssert.Contains("No se encontraron coincidencias", (string)obj["status_text"]);
        }
    }
}
