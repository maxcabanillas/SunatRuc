using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Graph.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Web.Graph.Controllers.Tests
{
    [TestClass()]
    public class NombreControllerTests
    {
        [TestMethod()]
        public void GetTest()
        {
            var controller = new NombreController();
            var res = controller.Get("GLADYS TORRES MAGALLANES");
            Assert.IsTrue(res.StatusCode == HttpStatusCode.OK);
            var json = res.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(json);
            StringAssert.Contains(json, "success");
        }

        [TestMethod]
        public void GetFailTest()
        {
            var controller = new NombreController();
            var res = controller.Get("GLADYS KETHERINE TORRES MAGALLANES");
            Assert.IsTrue(res.StatusCode == HttpStatusCode.OK);
            var json = res.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(json);
            StringAssert.Contains(json, "not_found");
        }

        [TestMethod()]
        public void PostTest()
        {
            var controller = new NombreController();
            var res = controller.Post("GLADYS TORRES MAGALLANES");
            Assert.IsTrue(res.StatusCode == HttpStatusCode.OK);
            var json = res.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(json);
            StringAssert.Contains(json, "success");
        }
    }
}