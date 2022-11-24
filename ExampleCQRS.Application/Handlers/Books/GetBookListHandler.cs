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
        
        private readonly IRepository<Book,Guid> Repository;
        private readonly IElastichSearchRepository<Book, Guid> _elasticRepository;
        public GetBookListHandler(IRepository<Book, Guid> repository, IElastichSearchRepository<Book, Guid> elasticRepository)
        {         
            Repository = repository;
            _elasticRepository = elasticRepository;
        }

        public async Task<List<Book>> Handle(GetBookQueryList request, CancellationToken cancellationToken)
        {
             var isCheck= await _elasticRepository.ChekIndex("book");
            if (isCheck == false)
            {
                var firstRecord = await Repository.GetAllAsync();
                if (firstRecord.Count()>0)
                {
                    await _elasticRepository.InsertDocuments("book", firstRecord);
                }                
            }
            var res = await _elasticRepository.GetDocuments("book");
            return res;
        }
    }
}
