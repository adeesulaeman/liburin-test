using Microsoft.EntityFrameworkCore;
using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Base.Repository
{
    public abstract class BaseReadOnlyRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;
        public BaseReadOnlyRepository(DbContext context)
        {
            Context = context;
        }
        public virtual T GetById(long id) =>
            Context.Set<T>().Find(id);

        public virtual IEnumerable<T> GetEntities() =>
            Context.Set<T>().AsEnumerable();

        public virtual IEnumerable<T> GetEntities(Expression<Func<T, bool>> predicate) =>
            Context.Set<T>().Where(predicate)
                            .AsEnumerable();


        public Task<T> GetByIdAsync(long id)
        {
            return Task.Run(() => Context.Set<T>().Find(id));
        }

        public Task<IEnumerable<T>> GetEntitiesAsync()
        {
            return Task.Run(() => Context.Set<T>().AsEnumerable());
        }

        public Task<IEnumerable<T>> GetEntitiesAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() => Context.Set<T>().Where(predicate)
                            .AsEnumerable()
                            );
        }

        public virtual Task<T> GetWithRelationsAsync(long id) =>
            GetByIdAsync(id);

        public virtual Task<IEnumerable<T>> GetWithRelationsAsync() =>
            GetEntitiesAsync();
        public virtual Task<IEnumerable<T>> GetWithRelationsAsync(Expression<Func<T, bool>> searchPredicate) =>
            GetEntitiesAsync(searchPredicate);
    }
}
