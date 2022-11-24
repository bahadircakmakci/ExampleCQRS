using ExampleCQRS.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Repositories.Base.Interfaces
{
    public interface IElastichSearchRepository<TEntity, Tkey> where TEntity : class, IEntity where Tkey : struct
    {
        Task<bool> ChekIndex(string indexName);
        Task InsertOrUpdateDocument(string indexName, TEntity entity);       
        Task InsertDocuments(string indexName, List<TEntity> entity);
        Task<TEntity> GetDocument(string indexName, Guid id);
        Task<List<TEntity>> GetDocuments(string indexName);
        Task RemoveDocument(string indexName, Guid id);
    }
}
