using ExampleCQRS.Application.Elasticsearch.BookElasticServices;
using ExampleCQRS.Application.Queries.Books;
using ExampleCQRS.Domain.Entities.Concrete;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Application.Handlers.Books
{
    public class GetBookHandler : IRequestHandler<GetBookQueryById, Book>
    {
        private readonly IBookElasticsearchService _bookelasticsearchService;
        public GetBookHandler(IBookElasticsearchService bookelasticsearchService)
        {
           _bookelasticsearchService = bookelasticsearchService;
        }
        public async Task<Book> Handle(GetBookQueryById request, CancellationToken cancellationToken)
        {            
            var res = await _bookelasticsearchService.GetDocument("book", request.id);
            return res;
        }
    }
}
