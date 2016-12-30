using NUnit.Framework;
using Ruc;

namespace RucTests
{
    [TestFixture]
    public class DniTest
    {
        [Test]
        public void DniTest1()
        {
            var dni = "42123390";
            var consult = new DniConsult();
            var res = consult.Get(dni);
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Length >= 3);
            StringAssert.StartsWith("DENIS", res[0]);
            Assert.IsTrue(res[1] == "DEL AGUILA");
            Assert.IsTrue(res[2] == "TEPA");
            TestContext.WriteLine($"DNI: {dni}");
            TestContext.WriteLine($"NOMBRES: {string.Join("-", res)}");
        }


        [Test]
        public void DniTest2()
        {
            var dni = "06477277";
            var consult = new DniConsult();
            var res = consult.Get(dni);
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Length >= 3);
            StringAssert.StartsWith("PEDRO PABLO", res[0]);
            Assert.IsTrue(res[2] == "GODARD");
            TestContext.WriteLine($"DNI: {dni}");
            TestContext.WriteLine($"NOMBRES: {string.Join("-", res)}");
        }

        [Test]
        public void DniErrorTest()
        {
            var consult = new DniConsult();
            var res = consult.Get("4212339");
            StringAssert.Contains("El DNI", string.Join(" ", res));
        }
    }
}
