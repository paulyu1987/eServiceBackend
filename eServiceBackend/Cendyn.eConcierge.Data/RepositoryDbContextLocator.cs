using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryT.Infrastructure;
using Castle.Windsor;
using Castle.MicroKernel;

namespace Cendyn.eConcierge.Data
{
    public class RepositoryDbContextLocator : IServiceLocator
    {
        private readonly IKernel _container;
        private readonly string _connectionString;

        public RepositoryDbContextLocator(IKernel container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = _container.ResolveAll(enumerableServiceType);

            return (IEnumerable<object>)instance;
        }
    }
}
