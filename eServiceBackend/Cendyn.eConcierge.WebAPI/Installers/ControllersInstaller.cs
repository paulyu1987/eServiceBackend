using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Cendyn.eConcierge.Service.Interface;
using AppConfigFacility;

namespace Cendyn.eConcierge.WebApi.Installers
{
    using Plumbing;
    using Cendyn.eConcierge.UICommon;
    using System.Web.Http;

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.
                    FromThisAssembly().
                    BasedOn<ApiController>().
                    If(c => c.Name.EndsWith("Controller")).
                    LifestyleTransient());

            container.Register(Component.For<IAppConfig>().FromAppConfig());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            //Create and Register the resolver
            var resolver = new WindsorDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
        }
    }
}