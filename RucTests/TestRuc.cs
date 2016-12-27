using System;
using System.Linq;
using NUnit.Framework;
using Ruc;
using Ruc.Auth;

namespace RucTests
{
    //Change path to C:\Users\Administrador\Documents\History\SunatRuc\RucTests\bin\Debug\tessdata, Resharper not copy folder.
    [TestFixture]
    public class TestRuc
    {
        [Test]
        public void TestOneRuc()
        {
            var ruc = "20100070970";
            var q = new RucConsult();
            var res = q.GetInfo(ruc);
            Assert.IsTrue(res.Count == 18);
            StringAssert.AreEqualIgnoringCase(res["Número de RUC:"].Split('-')[0].Trim(), ruc);
        }

        /// <summary>
        /// Testeo multiples ruc pero menor igual a 10
        /// </summary>
        [Test]
        public void TestMultipleRuc1()
        {
            var ruc = "10480048356";
            var q = new RucMultipleConsult();
            var res = q.GetInfo(ruc).ToArray();
            StringAssert.AreEqualIgnoringCase(res[0][0], ruc, "No coinciden los rucs");
        }

        [Test]
        public void TestMultipleRuc2()
        {
            var rucs = Enumerable.Range(0, 11).Select(r => "10000307888");
            var q = new RucMultipleConsult();
            q.GetInfo(rucs.ToArray());
        }

        [Test]
        public void LoginSucessTest()
        {
            var ruc = "20100451931";
            var user = "AZABRGAB";
            var pass = "3Q13CzHfP";
            var resp = RucLogin.Validate(ruc, user, pass);
            Assert.IsTrue(resp);
        }

        [Test]
        public void LoginErrorTest()
        {
            var ruc = "20100451931";
            var user = "AZABRGAB";
            var pass = "3Q1dsaffP";
            var resp = RucLogin.Validate(ruc, user, pass);
            Assert.IsFalse(resp);
        }

        [Test]
        public void LoginInvalidRucTest()
        {
            var ruc = "20455";
            var user = "213";
            var pass = "321";
            Assert.Throws<ArgumentException>(() => RucLogin.Validate(ruc, user, pass));
        }
    }
}
