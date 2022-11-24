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
    public class GetBookHandler : IRequestHandler<GetBookQueryById, Book>
    {
        private readonly IElastichSearchRepository<Book, Guid> _elasticRepository;
        public GetBookHandler(IElastichSearchRepository<Book, Guid> elasticRepository)
        {
            _elasticRepository = elasticRepository;
        }
        public async Task<Book> Handle(GetBookQueryById request, CancellationToken cancellationToken)
        {            
            var res = await _elasticRepository.GetDocument("book", request.id);
            return res;
        }
    }
}
