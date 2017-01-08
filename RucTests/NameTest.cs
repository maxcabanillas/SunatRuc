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
            var json = cs.Get("Gladys Torres Magallanes");
            Assert.IsNotEmpty(json);
            StringAssert.Contains("success", json);
        }
        [Test]
        public void GetFailTest()
        {
            var cs = new NombreConsult();
            var json = cs.Get("Gladys Katherine Torres Magallanes");
            Assert.IsNotEmpty(json); 
            StringAssert.Contains("not_found", json);
        }
    }
}
