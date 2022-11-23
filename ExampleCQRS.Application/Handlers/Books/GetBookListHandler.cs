using ExampleCQRS.Application.Elasticsearch.BookElasticServices;
using ExampleCQRS.Application.Queries.Books;
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
    public class GetBookListHandler : IRequestHandler<GetBookQueryList, List<Book>>
    {
        private readonly IBookElasticsearchService _bookelasticsearchService;
        private readonly IRepository<Book,Guid> Repository;
        public GetBookListHandler(IBookElasticsearchService bookelasticsearchService, IRepository<Book, Guid> repository)
        {
            _bookelasticsearchService=bookelasticsearchService;
            Repository = repository;
        }

        public async Task<List<Book>> Handle(GetBookQueryList request, CancellationToken cancellationToken)
        {
             var isCheck= await _bookelasticsearchService.ChekIndex("book");
            if (isCheck == false)
            {
                var firstRecord = await Repository.GetAllAsync();
                await _bookelasticsearchService.InsertDocuments("book", firstRecord);
            }
            var res = await _bookelasticsearchService.GetDocuments("book");
            return res;
        }
    }
}
