using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Service.Base.Repository
{
    public abstract class BaseRepository<T> : BaseReadOnlyRepository<T>, IRepository<T> where T : BaseEntity
    {
        public string Actor { get; private set; }

        public BaseRepository(DbContext Context) : base(Context)
        {
        }
        public IDbContextTransaction CreateTransaction(int isolationLevel = (int)IsolationLevel.ReadCommitted)
        {
            return Context.Database.BeginTransaction();
        }
        public Task CommitTransaction(IDbContextTransaction transaction)
        {
            return Task.Run(() => transaction.Commit());
        }
        public Task RollbackTransaction(IDbContextTransaction transaction)
        {
            return Task.Run(() => transaction.Rollback());
        }

        public abstract T OnUpdating(T local, T db);
        public abstract T OnCreating(T entity);


        public void SetActor(string actor)
        {
            Actor = actor;
        }

        public T StampCreated(T entity)
        {
            DateTime date = DateTime.Now;

            entity.IsActive = true;
            entity.IsDeleted = false;

            entity.CreatedBy = Actor;
            entity.CreatedDate = date;

            StampModified(entity);

            return entity;
        }
        public T StampModified(T entity)
        {
            DateTime date = DateTime.Now;

            entity.ModifiedBy = Actor;
            entity.ModifiedDate = date;

            return entity;
        }
        public T StampDeleted(T entity)
        {
            entity.IsDeleted = true;

            StampModified(entity);

            return entity;
        }
        public T Create(T entity)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var createEntity = OnCreating(entity);
                StampCreated(createEntity);

                Context.Set<IEntity>().Add(entity);
                var result = Context.SaveChanges();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }
        public async Task<T> CreateAsync(T entity)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var createEntity = OnCreating(entity);
                StampCreated(createEntity);

                Context.Set<IEntity>().Add(entity);
                var result = await Context.SaveChangesAsync();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }

        public T Delete(long id)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var entity = Context.Set<T>().Find(id);
                StampDeleted(entity);

                Context.Set<IEntity>().Update(entity);
                Context.SaveChanges();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }
        public async Task<T> DeleteAsync(long id)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var entity = Context.Set<T>().Find(id);
                StampDeleted(entity);

                Context.Set<IEntity>().Update(entity);
                var result = await Context.SaveChangesAsync();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }

        public T Update(T entity)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var dbEntity = Context.Set<T>().Find(entity.Id);

                var updated = OnUpdating(entity, dbEntity);
                StampModified(updated);

                Context.Set<IEntity>().Update(entity);
                var result = Context.SaveChanges();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }
        public async Task<T> UpdateAsync(T entity)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var dbEntity = Context.Set<T>().Find(entity.Id);

                var updated = OnUpdating(entity, dbEntity);
                StampModified(updated);

                Context.Set<IEntity>().Update(entity);
                var result = await Context.SaveChangesAsync();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }

        public T Restore(long id)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var entity = Context.Set<T>()
                                      .IgnoreQueryFilters()
                                      .FirstOrDefault(x => x.Id == id);

                entity.IsDeleted = false;
                StampModified(entity);

                var result = Context.SaveChanges();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }
        public async Task<T> RestoreAsync(long id)
        {
            var internalTransaction = Context.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? Context.Database.CurrentTransaction : Context.Database.BeginTransaction();

            try
            {
                var entity = Context.Set<T>()
                                      .IgnoreQueryFilters()
                                      .FirstOrDefault(x => x.Id == id);

                entity.IsDeleted = false;
                StampModified(entity);

                var result = await Context.SaveChangesAsync();

                if (internalTransaction)
                    transaction.Commit();

                return entity;
            }
            catch (Exception exception)
            {
                if (internalTransaction)
                    transaction.Rollback();

                throw exception;
            }
        }
    }
}
