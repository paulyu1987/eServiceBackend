using System;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(Cendyn.eConcierge.WebApi.App_Start.WindsorActivator), "PreStart")]
[assembly: ApplicationShutdownMethodAttribute(typeof(Cendyn.eConcierge.WebApi.App_Start.WindsorActivator), "Shutdown")]

namespace Cendyn.eConcierge.WebApi.App_Start
{
    public static class WindsorActivator
    {
        static ContainerBootstrapper bootstrapper;

        public static void PreStart()
        {
            bootstrapper = ContainerBootstrapper.Bootstrap();
        }

        public static void Shutdown()
        {
            if (bootstrapper != null)
                bootstrapper.Dispose();
        }
    }
}