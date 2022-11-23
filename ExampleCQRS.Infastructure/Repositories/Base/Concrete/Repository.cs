using ExampleCQRS.Domain.Entities.Interfaces;
using ExampleCQRS.Infastructure.Context;
using ExampleCQRS.Infastructure.Repositories.Base.Interfaces;
using ExampleCQRS.Infastructure.Repositories.Base.Threading;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Repositories.Base.Concrete
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
                                                                        where TKey : struct
    {
         
        private readonly CQRSDbContext Context;
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }
        public virtual DbSet<TEntity> DbSet => Context.Set<TEntity>();        
        public Repository(CQRSDbContext context)
        {
            this.Context = context;
            CancellationTokenProvider = NullCancellationTokenProvider.Instance;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                //var auditEntries = Context.OnBeforeSaveChanges();
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
                //if (auditEntries.Count() > 0)
                //{
                //    for (int i = 0; i < auditEntries.Count(); i++)
                //    {
                //        auditEntries[i].ActionType = "Insert";
                //    }
                //    await Context.OnAfterSaveChangesAsync(auditEntries);
                //}

            }

            return savedEntity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave)
            {
                //var auditEntries = Context.OnBeforeSaveChanges();
                //await Context.OnAfterSaveChangesAsync(auditEntries);
                //if (auditEntries.Count() > 0)
                //{
                //    for (int i = 0; i < auditEntries.Count(); i++)
                //    {
                //        auditEntries[i].ActionType = "Insert";
                //    }
                //    await Context.OnAfterSaveChangesAsync(auditEntries);
                //}
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }
        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            //Context.Attach(entity);

            var entry = Context.Entry(entity);
            entry.State = EntityState.Modified;
            //var updatedEntity = Context.Update(entity);

            if (autoSave)
            {
                //var auditEntries = Context.OnBeforeSaveChanges();
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
                //if (auditEntries.Count() > 0)
                //{
                //    for (int i = 0; i < auditEntries.Count(); i++)
                //    {
                //        auditEntries[i].ActionType = "Update";

                //    }
                //    await Context.OnAfterSaveChangesAsync(auditEntries);
                //}

            }
            return entity;
        }
        public async Task UpdateRange(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(entities);
            if (autoSave)
            {
                await Context.SaveChangesAsync();
            }

        }
        public async Task<TEntity> FindAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking<TEntity>().Where(predicate).SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<TEntity>> GetAsNoTrackingAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking<TEntity>().ToListAsync(GetCancellationToken(cancellationToken));
        }
        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<TEntity>> GetAllAsync(IQueryable<TEntity> source, CancellationToken cancellationToken = default)
        {
            return await source.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<TEntity>> GetFilterAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable()
                .Where(predicate)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return entities;
        }
        public async Task<List<TEntity>> GetAsNoTrackingFilterAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable()
                .Where(predicate)
                .AsNoTracking<TEntity>()
                .ToListAsync(GetCancellationToken(cancellationToken));

            return entities;
        }
        public async Task<List<TEntity>> GetLastRecordAsync(CancellationToken cancellationToken = default)
        {
            var entities = DbSet.AsEnumerable().TakeLast(1000).ToList();
            return entities;
        }
        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<TEntity> WhereIf<T>(IQueryable<TEntity> query, bool condition, Expression<Func<TEntity, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.ToList().AsQueryable();
        }

        public void RemoveAll(List<TEntity> entity)
        {
            DbSet.RemoveRange(entity);
        }
        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable()
                .Where(predicate)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var entity in entities)
            {                 
                DbSet.Remove(entity);
            }

            if (autoSave)
            {

                /*var auditEntries = Context.OnBeforeSaveChanges()*/;
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
                //if (auditEntries.Count() > 0)
                //{
                //    for (int i = 0; i < auditEntries.Count(); i++)
                //    {
                //        auditEntries[i].ActionType = "Delete";
                //        await Context.OnAfterSaveChangesAsync(auditEntries);
                //    }
                //}

            }
        }
        public async Task DeleteAllAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {

            DbSet.RemoveRange(entities);


            if (autoSave)
            {
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return DbSet.SingleOrDefaultAsync(predicate, GetCancellationToken(cancellationToken));
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return DbSet.FirstOrDefaultAsync(predicate, GetCancellationToken(cancellationToken));
        }

        private CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
        {
            return CancellationTokenProvider.FallbackToProvider(preferredValue);
        }
        public IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public ValueTask<TEntity> GetByIdAsync(TKey id)
        {
            return DbSet.FindAsync(id);
        }
    }
}
