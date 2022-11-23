﻿using ExampleCQRS.Application.Commands.Books;
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
    public class AddOrEditBookHandler : IRequestHandler<AddOrEditBookCommand, Book>
    {
        public readonly IRepository<Book, Guid> Repository;
        public readonly IBookElasticsearchService _bookElasticsearchService;
        public AddOrEditBookHandler(IRepository<Book, Guid> repository, IBookElasticsearchService bookElasticsearchService)
        {
            Repository = repository;
            _bookElasticsearchService = bookElasticsearchService;
        }
        public async Task<Book> Handle(AddOrEditBookCommand request, CancellationToken cancellationToken)
        {
           
            if (request.model.Id == Guid.Empty)
            {
                var ret = await Repository.InsertAsync(request.model, true);
                await _bookElasticsearchService.InsertDocument("book", ret);
                return ret;
            }
            else
            {
                var ret = await Repository.UpdateAsync(request.model, true);
                await _bookElasticsearchService.InsertDocument("book", ret);
                return ret;
            }
           


        }
    }
}
