using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Core.EntityFrameworkRepository.Interfaces;
using RepositoryT.Infrastructure;

namespace Cendyn.eConcierge.Core.EntityFrameworkRepository
{
    public class DefaultDataContextFactory<TContext> : IDataContextFactory<TContext> where TContext : class,IDbContext, IDisposable
    {
        private TContext _dataContext;

        private string _connectionString;

        public DefaultDataContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
            
        #region IDatabaseFactory Members

        public TContext GetContext()
        {
            if (_dataContext == null)
            {
                Create();
            }
           
            return _dataContext;
        }
        public void Create()
        {
            _dataContext = (TContext)Activator.CreateInstance(typeof(TContext),_connectionString);
        }

        public void Release()
        {
            Dispose();
        }
        #endregion

        public void Dispose()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
