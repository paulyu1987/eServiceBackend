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

namespace Cendyn.eConcierge.UnitTest
{
    [TestClass]
    public class ServiceTest
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
        public void Test_Reservation_Service_Get_Cancelled_Reservation()
        {
            var resService = _container.Resolve<IReservationService>();

            var data = resService.GetCancelledReservations();

            Assert.IsNotNull(data);
        }
    }
}
