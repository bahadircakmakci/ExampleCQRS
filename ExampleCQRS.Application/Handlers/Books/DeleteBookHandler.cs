using ExampleCQRS.Application.Commands.Books;
using ExampleCQRS.Application.Elasticsearch.BookElasticServices;
using ExampleCQRS.Domain.Entities.Concrete;
using ExampleCQRS.Infastructure.Repositories.Base.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Application.Handlers.Books
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Guid>
    {
        public readonly IRepository<Book, Guid> Repository;
        public readonly IBookElasticsearchService _bookElasticsearchService;
        public DeleteBookHandler(IRepository<Book, Guid> repository, IBookElasticsearchService bookElasticsearchService)
        {
            Repository = repository;
            _bookElasticsearchService = bookElasticsearchService;
        }


        async Task<Guid> IRequestHandler<DeleteBookCommand, Guid>.Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await _bookElasticsearchService.RemoveDocument("book", request.id);
            await Repository.DeleteAsync(x => x.Id == request.id, true);            
            return request.id;
        }
    }
}
