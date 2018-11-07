using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Cendyn.eConcierge.Service;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Service.Implement;
using Cendyn.eConcierge.EntityModel;
using DynaCache;

namespace Cendyn.eConcierge.Service
{
    public class ServiceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Add Cache Component
            container.Register(Component.For(typeof(IDynaCacheService)).ImplementedBy(typeof(MemoryCacheService)).LifestyleSingleton());

            //Auto registration all the business services
            container.Register(Classes.FromThisAssembly()
                .BasedOn<ServiceBase>()
                .If(t => t.Name.EndsWith("Service"))
                .WithService.DefaultInterfaces()
                .Configure(c => c.LifestyleSingleton()));

            //Register Email Generate Service, it should be Transient
            container.Register(Component.For<IEmailGenerateService>()
                .ImplementedBy<RequestConfirmEmailService>().Named("RequestConfirmEmailService")
                .LifestyleTransient());

            //Overwrite the servies's registration to use cacheable implementation
            container.Register(Component.For<ISettingService>()
                .ImplementedBy(Cacheable.CreateType<SettingService>())
                .IsDefault().Named("CachedSettingService").LifestyleSingleton());

            //Register HotelInfo DTO
            container.Register(Component.For<DomainInformationModel>()
                .ImplementedBy<DomainInformationModel>().LifeStyle.HybridPerWebRequestPerThread());
            container.Register(Component.For<HotelInformationModel>()
                .ImplementedBy<HotelInformationModel>().LifeStyle.HybridPerWebRequestPerThread());

            //Register Room Available Generate Service, it should be Transient
            container.Register(Component.For<IRequestAvailableRoomService>()
                .ImplementedBy<RequestAvailableRoomService>().Named("RequestAvailableRoomService")
                .LifestyleTransient());

            //Register Room Available Generate Service, it should be Transient
            container.Register(Component.For<IPlannerEventService>()
                .ImplementedBy<PlannerEventService>().Named("PlannerEventService")
                .LifestyleTransient());

            //Register Room Available Generate Service, it should be Transient
            container.Register(Component.For<ICenResService>()
                .ImplementedBy<CenResService>().Named("CenResService")
                .LifestyleTransient());

        }
    }
}
