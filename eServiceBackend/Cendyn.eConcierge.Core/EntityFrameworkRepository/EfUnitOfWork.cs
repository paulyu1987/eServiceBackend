using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Core.EntityFrameworkRepository
{
    public class EfUnitOfWork<TContext> :UnitOfWorkBase<TContext> where TContext : class, IDisposable, IObjectContextAdapter
    {
        public EfUnitOfWork(IServiceLocator serviceLocator):base(serviceLocator)
        {
        }

        protected ObjectContext ObjectContext() 
        {
            return DataContext.ObjectContext;
        }

        protected ObjectStateManager ObjectStateManager() 
        { 
            return ObjectContext().ObjectStateManager;
        }

        public override void Commit()
        {
            var dbContext = DataContext as DbContext;
            if (dbContext != null)
            {
                dbContext.SaveChanges();
            }
        }
    }
}
