using ExampleCQRS.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Repositories.Base.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity where TKey : struct
    {
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateRange(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TEntity> FindAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetAsNoTrackingAllAsync(CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetFilterAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        ValueTask<TEntity> GetByIdAsync(TKey id);
        IQueryable<TEntity> GetQueryable();
        IQueryable<TEntity> WhereIf<T>(IQueryable<TEntity> query, bool condition, Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        void RemoveAll(List<TEntity> entity);
        Task<long> GetCountAsync(CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetLastRecordAsync(CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetAsNoTrackingFilterAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task DeleteAllAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}
