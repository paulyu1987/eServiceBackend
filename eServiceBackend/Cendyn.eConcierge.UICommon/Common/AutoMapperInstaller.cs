using AutoMapper;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Cendyn.eConcierge.UICommon.Common
{
    public class AutoMapperInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {

            container.Register(
                Types.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory,"Cendyn.*"))
                .BasedOn<Profile>()
                .WithService.Base()
                .Configure(c => c.Named(c.Implementation.FullName))
                .LifestyleTransient());

            var profiles = container.ResolveAll<Profile>();

            if (profiles.Length > 0)
            {
                Mapper.Initialize(config => AddProfiles(config, profiles));
            }

            container.Register(Component.For<IMapper>().UsingFactoryMethod(() => { return Mapper.Instance; }).LifestyleSingleton());
        }

        private void AddProfiles(IConfiguration configuration, IEnumerable<Profile> profiles)
        {
            profiles.ToList().ForEach(configuration.AddProfile);
        }

        private string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;

                var uri = new UriBuilder(codeBase);

                var path = Uri.UnescapeDataString(uri.Path);

                return Path.GetDirectoryName(path);
            }
        }
    }
}
