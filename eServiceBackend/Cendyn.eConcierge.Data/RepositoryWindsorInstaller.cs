using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using RepositoryT.Infrastructure;
using Cendyn.eConcierge.Core.EntityFrameworkRepository;
using Cendyn.eConcierge.Core.Helper;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Data
{
    public class RepositoryWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Register Service Locator 
            container.Register(Component.For<IServiceLocator>()
                .ImplementedBy<RepositoryDbContextLocator>().LifestyleSingleton());

            container.Register(Component.For<IConnectionStringBuilder>()
                .ImplementedBy<ConnectionStringBuilder>().LifeStyle.HybridPerWebRequestPerThread()
                .DynamicParameters((kernal, parameters) => parameters["domainName"] = kernal.Resolve<DomainInformationModel>().DomainName));

            //Register DBContextFactory
            container.Register(Component.For<IDataContextFactory<ConciergeEntities>>()
                .UsingFactoryMethod((kernel, creationContext) => {

                    var builder = kernel.Resolve<IConnectionStringBuilder>();

                    var conString = builder.GetConnectionString();

                    if (string.IsNullOrWhiteSpace(conString))
                    {
                        throw new System.ArgumentException("Please check the settings in CendynAdmin, cannot find connection string based on domain:" + kernel.Resolve<DomainInformationModel>().DomainName);
                    }

                    var factory = new DefaultDataContextFactory<ConciergeEntities>(conString);

                    return factory;
                }).LifeStyle.HybridPerWebRequestPerThread());

            //Register UnitOfWork
            container.Register(Component.For<IUnitOfWork>()
                .ImplementedBy<EfUnitOfWork<ConciergeEntities>>().LifestyleSingleton());

            //Register All Repositories
            container.Register(Classes.FromThisAssembly().BasedOn(typeof(IRepository<>))
                .WithService.DefaultInterfaces().LifestyleSingleton());
        }
    }
}
