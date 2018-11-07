using Cendyn.eConcierge.Core.EntityFrameworkRepository.Interfaces;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Core.EntityFrameworkRepository
{
    public class EfRepositoryBase<TContext> : RepositoryBase<TContext> where TContext : class, IDbContext, IDisposable
    {
        public EfRepositoryBase(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        protected IDbSet<T> Set<T>() where T : class
        {
            return DataContext.Set<T>();
        }

        protected ObjectContext ObjectContextWrapper() 
        {
            return DataContext.ObjectContext;
        }

    }
}
