using ExampleCQRS.Domain.Entities.Concrete;
using ExampleCQRS.Domain.Entities.Interfaces;
using ExampleCQRS.Infastructure.Repositories.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Repositories.Base.Concrete
{
    public class ElastichSearchRepository<TEntity,Tkey>: IElastichSearchRepository<TEntity, Tkey> where TEntity : class, IEntity<Tkey>
                                                                        where Tkey : struct
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;

        public ElastichSearchRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }
        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticConnectionSettings:ElasticSearchHost").Value;
            string port = _configuration.GetSection("ElasticConnectionSettings:ElasticSearchPort").Value;
            string username = _configuration.GetSection("ElasticConnectionSettings:ElasticUsername").Value;
            string password = _configuration.GetSection("ElasticConnectionSettings:ElasticPassword").Value;

            var settings = new ConnectionSettings(new Uri(host + ":" + port + "/"));

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        public async Task<bool> ChekIndex(string indexName)
        {
            var anyy = await _client.Indices.ExistsAsync(indexName);
            if (anyy.Exists)
                return true;

            var response = await _client.Indices.CreateAsync(indexName,
                ci => ci
                    .Index(indexName)                   
                    .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                    );
            return false;

        }        
        public async Task InsertOrUpdateDocument(string indexName, TEntity entity)
        {
            var response = await _client.CreateAsync(entity, q => q.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<TEntity>(entity, a => a.Index(indexName).Doc(entity));
            }
        }

        public async Task RemoveDocument(string indexName, Guid id)
        {
            var response = await _client.DeleteAsync<TEntity>(id, d => d.Index(indexName.ToLower()));
        }

        public async Task InsertDocuments(string indexName, List<TEntity> entity)
        {
            await _client.IndexManyAsync(entity, index: indexName.ToLower());
        }


        public async Task<TEntity> GetDocument(string indexName, Guid id)
        {
            var response = await _client.GetAsync<TEntity>(id, q => q.Index(indexName.ToLower()));

            return response.Source;

        }

        public async Task<List<TEntity>> GetDocuments(string indexName)
        {
            var response = await _client.SearchAsync<TEntity>(q => q.Index(indexName.ToLower()));
            return response.Documents.ToList();
        }

    }
}
