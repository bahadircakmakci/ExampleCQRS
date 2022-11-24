using ExampleCQRS.Application.Commands.Books;
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
        private readonly IElastichSearchRepository<Book, Guid> _elasticRepository;
        public DeleteBookHandler(IRepository<Book, Guid> repository, IElastichSearchRepository<Book, Guid> elasticRepository)
        {
            Repository = repository;
            _elasticRepository = elasticRepository;
        }


        async Task<Guid> IRequestHandler<DeleteBookCommand, Guid>.Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await _elasticRepository.RemoveDocument("book", request.id);
            await Repository.DeleteAsync(x => x.Id == request.id, true);            
            return request.id;
        }
    }
}
