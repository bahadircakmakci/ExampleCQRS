using ExampleCQRS.Domain.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Application.Elasticsearch.BookElasticServices
{
    public class BookElasticsearchService : IBookElasticsearchService
    {

        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;

        public BookElasticsearchService(IConfiguration configuration)
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
                    .BookMapping()
                    .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))                   
                    );

            return false;

        }

        public async Task InsertDocument(string indexName, Book book)
        {

            var response = await _client.CreateAsync(book, q => q.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<Book>(book.Id, a => a.Index(indexName).Doc(book));
            }
        }

        public async Task RemoveDocument(string indexName, Guid id)
        {
            var response = await _client.DeleteAsync<Book>(id, d => d.Index(indexName));             
        }

        public async Task InsertDocuments(string indexName, List<Book> books)
        {
            await _client.IndexManyAsync(books, index: indexName);
        }


        public async Task<Book> GetDocument(string indexName, Guid id)
        {
            var response = await _client.GetAsync<Book>(id, q => q.Index(indexName));

            return response.Source;

        }

        public async Task<List<Book>> GetDocuments(string indexName)
        {
            var response = await _client.SearchAsync<Book>(q => q.Index(indexName));
            return response.Documents.ToList();
        }
    }

    public interface IBookElasticsearchService
    {
        Task<bool> ChekIndex(string indexName);
        Task InsertDocument(string indexName, Book book);
        Task InsertDocuments(string indexName, List<Book> books);
        Task<Book> GetDocument(string indexName, Guid id);
        Task RemoveDocument(string indexName, Guid id);
        Task<List<Book>> GetDocuments(string indexName);
    }
}
