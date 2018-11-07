using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using RepositoryT.Infrastructure;
using Cendyn.eConcierge.EntityModel;
using System.Linq;
using Castle.Facilities.Logging;

namespace Cendyn.eConcierge.UnitTest
{
    [TestClass]
    public class DBRepositoryTest
    {
        IWindsorContainer _container;

        [TestInitialize]
        public void Init_Test()
        {
            _container = new WindsorContainer().Install(FromAssembly.InThisApplication());
            _container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.NLog)
                                                         .WithConfig("NLog.config"));
        }

        [TestMethod]
        public void Test_Db_Connection()
        {
            var fgRepo = _container.Resolve<IFGuestRepository>();

            var data = fgRepo.GetAll().FirstOrDefault();

            Assert.IsNotNull(data);
        }
    }
}
