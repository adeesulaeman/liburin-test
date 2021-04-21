using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Base.Contracts
{
    public interface IReadOnlyRepository<T> where T : IEntity
    {
        IEnumerable<T> GetEntities();
        Task<IEnumerable<T>> GetEntitiesAsync();

        IEnumerable<T> GetEntities(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetEntitiesAsync(Expression<Func<T, bool>> predicate);

        T GetById(long id);
        Task<T> GetByIdAsync(long id);

        Task<T> GetWithRelationsAsync(long id);

        Task<IEnumerable<T>> GetWithRelationsAsync();
        Task<IEnumerable<T>> GetWithRelationsAsync(Expression<Func<T, bool>> searchpredicate);
    }
}
