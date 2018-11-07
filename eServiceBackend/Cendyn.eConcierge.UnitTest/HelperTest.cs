using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using RepositoryT.Infrastructure;
using Cendyn.eConcierge.EntityModel;
using System.Linq;
using Castle.Facilities.Logging;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Core.Helper;

namespace Cendyn.eConcierge.UnitTest
{
    [TestClass]
    public class HelperTest
    {

        [TestInitialize]
        public void Init_Test()
        {
        }

        [TestMethod]
        public void Test_Simple_Text_Encode()
        {
            var originalText = "JupiterBeachResort123";
            var encodedText = "073118111106115102113067100098098105081102114112113117048051050";

            var result = SimpleTextEncodeHelper.EncodeText(originalText);

            Assert.AreEqual(encodedText, result);
        }
    }
}
