using ExampleCQRS.Domain.Entities.Concrete;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Application.Queries.Books
{
    public record GetBookQueryById(Guid id) : IRequest<Book>;

}
