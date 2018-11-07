using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Cendyn.eConcierge.WebApi.Helper;

namespace Cendyn.eConcierge.WebApi.App_Start
{
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        readonly IWindsorContainer container;

        ContainerBootstrapper(IWindsorContainer container)
        {
            this.container = container;
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }

        public static ContainerBootstrapper Bootstrap()
        {
            //Register 
            var container = new WindsorContainer().
                Install(FromAssembly.InThisApplication());

            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));

            return new ContainerBootstrapper(container);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}