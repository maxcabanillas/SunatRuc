using System.Linq;
using NUnit.Framework;
using Ruc;

namespace RucTests
{
    //Change path to C:\Users\Administrador\Documents\History\SunatRuc\RucTests\bin\Debug\tessdata, Resharper not copy folder.
    [TestFixture]
    public class TestRuc
    {
        [Test]
        public void TestOneRuc()
        {
            var q = new RucConsult();
            q.GetInfo("10000307888");
        }

        /// <summary>
        /// Testeo multiples ruc pero menor igual a 10
        /// </summary>
        [Test]
        public void TestMultipleRuc1()
        {
            var q = new RucMultipleConsult();
            q.GetInfo(new []{"10480048356", "10000307888" });
        }

        [Test]
        public void TestMultipleRuc2()
        {
            var rucs = Enumerable.Range(0, 20).Select(r => "10000307888");
            var q = new RucMultipleConsult();
            q.GetInfo(rucs);
        }
    }
}
