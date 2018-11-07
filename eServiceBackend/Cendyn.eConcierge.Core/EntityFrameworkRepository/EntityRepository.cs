using Cendyn.eConcierge.Core.EntityFrameworkRepository.Interfaces;
using RepositoryT.Extensions;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Cendyn.eConcierge.Core.EntityFrameworkRepository
{
   
    public abstract class EntityRepository<T, TContext> : EfRepositoryBase<TContext>, IEfRepository<T>
    where T : class
    where TContext : class, IDbContext, IDisposable
    {
        protected EntityRepository(IServiceLocator serviceLocator) :
            base(serviceLocator)
        {
        }

        public virtual void Add(T entity)
        {
            Set<T>().Add(entity);
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Set<T>().Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            Set<T>().Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            Set<T>().Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IDbSet<T> dbSet = Set<T>();
            IEnumerable<T> objects = dbSet.Where(@where).AsEnumerable();

            foreach (T obj in objects)
            {
                dbSet.Remove(obj);
            }
        }

        public virtual void Delete(int id)
        {
            Delete((object)id);
        }

        public virtual void Delete(long id)
        {
            Delete((object)id);
        }

        public virtual void Delete(string id)
        {
            Delete((object)id);
        }

        public virtual T Get(Expression<Func<T, bool>> @where)
        {
            return Set<T>().Where(@where).FirstOrDefault();
        }

        private void Delete(object id)
        {
            IDbSet<T> dbSet = Set<T>();
            T item = dbSet.Find(id);
            dbSet.Remove(item);
        }

        public virtual T GetById(int id)
        {
            return GetById((object)id);
        }

        public virtual T GetById(long id)
        {
            return GetById((object)id);
        }

        public virtual T GetById(string id)
        {
            return GetById((object)id);
        }

        private T GetById(object id)
        {
            IDbSet<T> dbSet = Set<T>();
            if (typeof(T).GetInterfaces().Contains(typeof(ISupportSoftDelete)))
            {
                ParameterExpression itemParameter = Expression.Parameter(typeof(T), "item");
                Expression<Func<T, bool>> whereExpression = Expression.Lambda<Func<T, bool>>
                    (
                        Expression.And(
                            Expression.Equal(Expression.Property(itemParameter, "ID"), Expression.Constant(id)),
                            Expression.Equal(Expression.Property(itemParameter, "IsDeleted"), Expression.Constant(false))
                            ),
                        new[] { itemParameter }
                    );
                return dbSet.Where(whereExpression).SingleOrDefault();
            }

            return dbSet.Find(id);
        }

        public virtual T GetAnyById(int id)
        {
            return GetAnyById((object)id);
        }

        public virtual T GetAnyById(long id)
        {
            return GetAnyById((object)id);
        }

        public virtual T GetAnyById(string id)
        {
            return GetAnyById((object)id);
        }

        private T GetAnyById(object id)
        {
            IDbSet<T> dbSet = Set<T>();
            if (typeof(T).GetInterfaces().Contains(typeof(ISupportSoftDelete)))
            {
                ParameterExpression itemParameter = Expression.Parameter(typeof(T), "item");
                Expression<Func<T, bool>> whereExpression = Expression.Lambda<Func<T, bool>>
                    (
                        Expression.Equal(Expression.Property(itemParameter, "ID"), Expression.Constant(id)),
                        new[] { itemParameter }
                    );
                return dbSet.Where(whereExpression).SingleOrDefault();
            }
            return dbSet.Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Set<T>();
        }

        public virtual IQueryable<T> GetAllActive()
        {
            IDbSet<T> allActive = Set<T>();
            if (typeof(ISupportSoftDelete).IsAssignableFrom(typeof(T)))
            {

                ParameterExpression itemParameter = Expression.Parameter(typeof(T), "item");

                Expression isDeletedFilter = Expression.Equal(
                    Expression.Property(itemParameter, "IsDeleted"),
                    Expression.Constant(false)
                    );

                Expression isActiveFilter = Expression.Equal(
                    Expression.Property(itemParameter, "IsActive"),
                    Expression.Constant(true)
                    );

                Expression combinedExpression = Expression.AndAlso(isDeletedFilter, isActiveFilter);

                Expression<Func<T, bool>> whereExpression = Expression.Lambda<Func<T, bool>>
                    (
                        combinedExpression, itemParameter);
                return allActive.Where(whereExpression);
            }
            return allActive;
        }

        public virtual IQueryable<T> GetAllDeleted()
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<T> IncludeSubSets(params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(Set<T>(), (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual List<TDynamicEntity> GetDynamic<TTable, TDynamicEntity>(Expression<Func<TTable, object>> selector, Func<object, TDynamicEntity> maker) where TTable : class
        {
            return DataContext.Set<TTable>().Select(selector.Compile()).Select(maker).ToList();
        }

        public virtual List<TDynamicEntity> GetDynamic<TTable, TDynamicEntity>(Func<TTable, object> selector, Func<object, TDynamicEntity> maker) where TTable : class
        {
            return DataContext.Set<TTable>().Select(selector).Select(maker).ToList();
        }

        public virtual IEnumerable<T> GetPaged(Expression<Func<T, bool>> predicate, Action<IOrderable<T>> order, int pageIndex, int pageSize)
        {
            int skip = pageIndex * pageSize;
            Orderable<T> orderable = new Orderable<T>(Set<T>().Where(predicate));
            order(orderable);

            return orderable.AsQueryable()
                .Skip(skip)
                .Take(pageSize);
        }

        public virtual IEnumerable<T> GetPagedActive(Expression<Func<T, bool>> predicate, Action<IOrderable<T>> order, int pageIndex, int pageSize)
        {
            return GetPagedActiveOrDeleted(predicate, order, pageIndex, pageSize, false);
        }

        public virtual IEnumerable<T> GetPagedDeleted(Expression<Func<T, bool>> predicate, Action<IOrderable<T>> order, int pageIndex, int pageSize)
        {
            return GetPagedActiveOrDeleted(predicate, order, pageIndex, pageSize, true);
        }

        private IEnumerable<T> GetPagedActiveOrDeleted(Expression<Func<T, bool>> predicate, Action<IOrderable<T>> orderAction, int pageIndex, int pageSize, bool isActiveOrDeleted)
        {
            int skip = pageIndex * pageSize;
            ParameterExpression itemParameter = Expression.Parameter(typeof(T), "item");
            Expression<Func<T, bool>> whereExpression = Expression.Lambda<Func<T, bool>>
                (
                    Expression.And(
                        predicate,
                        Expression.Equal(Expression.Property(itemParameter, "IsDeleted"), Expression.Constant(isActiveOrDeleted))
                        ), itemParameter);
            Orderable<T> orderable = new Orderable<T>(Set<T>().Where(whereExpression));

            orderAction(orderable);

            return orderable.AsQueryable()
                .Skip(skip)
                .Take(pageSize);
        }


        public List<TResult> ExecuteStoredProcedure<TResult>(string procedureName, Dictionary<string, object> parameters)
        {
            StringBuilder builder = new StringBuilder(procedureName);
            object[] sqlParameterCollection = new object[parameters.Count];

            int i = 0;
            foreach (var parameter in parameters)
            {
                sqlParameterCollection[i] = new SqlParameter(parameter.Key, parameter.Value);
                i++;
            }

            builder.AppendFormat(" {0}", string.Join(",", parameters.ToList().Select(x => x.Key)));

            return ObjectContextWrapper().ExecuteStoreQuery<TResult>(builder.ToString(), sqlParameterCollection).ToList<TResult>();
        }
    }
}
