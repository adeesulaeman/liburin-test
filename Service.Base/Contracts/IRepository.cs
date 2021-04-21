using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Service.Base.Contracts
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : IEntity
    {
        string Actor { get; }
        void SetActor(string actor);
        IDbContextTransaction CreateTransaction(int isolationLevel = (int)IsolationLevel.ReadCommitted);
        Task CommitTransaction(IDbContextTransaction transaction);
        Task RollbackTransaction(IDbContextTransaction transaction);

        T Create(T entity);
        Task<T> CreateAsync(T entity);

        T Update(T entity);
        Task<T> UpdateAsync(T entity);

        T Delete(long id);
        Task<T> DeleteAsync(long id);

        T Restore(long id);
        Task<T> RestoreAsync(long id);
    }
}
